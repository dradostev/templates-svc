using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Services
{
    public class EventListener : IEventListener
    {
        private readonly IMongoCollection<Template> _templates;
        private readonly IEventService _eventService;
        private readonly ILogger<IEventListener> _logger;

        public EventListener(IMongoClient mongo, IEventService eventService, ILogger<IEventListener> logger)
        {
            _templates = mongo.GetDatabase("TemplatesService").GetCollection<Template>("Templates");
            _eventService = eventService;
            _logger = logger;
        }
        
        public async Task<Result> On(Event<ProjectCreated> e)
        {
            _logger.LogInformation(
                $"Trying to fulfill project {e.Payload.ProjectId} with template {e.Payload.TemplateId}");
            
            var template = await _templates
                .Find(x => x.Id == e.Payload.TemplateId)
                .FirstOrDefaultAsync();

            if (template is null)
            {
                _logger.LogError($"Template {e.Payload.TemplateId} is not found");
                return Result.NotFound;
            }

            if (!template.Ratios.TryGetValue(e.Payload.Ratio, out var ratio))
            {
                _logger.LogError(
                    $"Aspect ratio ${e.Payload.Ratio} is not found in template {e.Payload.TemplateId}");
                return Result.NotFound;
            }

            await _eventService.PublishAsync(new Event<ProjectFulfilled>
            {
                CorrelationId = e.CorrelationId,
                OccurredOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Type = "project-fulfilled",
                Version = 1,
                UserId = e.UserId,
                Payload = new ProjectFulfilled
                {
                    ProjectId = e.Payload.ProjectId,
                    TemplateId = e.Payload.TemplateId,
                    Key = ratio.Project.Key,
                    Order = ratio.Project.Order,
                    Type = template.Type,
                    Preview = ratio.Project.Preview,
                    Resources = ratio.Project.Resources,
                    Settings = ratio.Project.Settings
                } 
            });
            
            _logger.LogInformation(
                $"Project {e.Payload.ProjectId} is succesfully fulfilled with template {e.Payload.TemplateId}");

            return Result.Successful;
        }

        public async Task<Result> On(Event<ProjectCompositionCreated> e)
        {
            _logger.LogInformation(
                $"Trying to fulfill composition {e.Payload.CompositionId} in project {e.Payload.ProjectId} with template {e.Payload.TemplateId}.");
            
            var template = await _templates
                .Find(x => x.Id == e.Payload.TemplateId)
                .FirstOrDefaultAsync();

            var aspectTemplate = template.Ratios[e.Payload.Ratio];
            var aspectComposition = aspectTemplate.Compositions.FirstOrDefault(c => c.Key == e.Payload.Key);
            if (aspectComposition == null)
            {
                _logger.LogError($"Can't find slide with key '{e.Payload.Key}' in template with Id: '{e.Payload.TemplateId}'.");
                return Result.NotFound;
            }
            
            await _eventService.PublishAsync(new Event<ProjectCompositionFulfilled>
            {
                CorrelationId = e.CorrelationId,
                OccurredOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Type = "project-composition-fulfilled",
                Version = 1,
                UserId = e.UserId,
                Payload = new ProjectCompositionFulfilled
                {
                    TemplateId = e.Payload.TemplateId,
                    ProjectId = e.Payload.ProjectId,
                    CompositionId = e.Payload.CompositionId,
                    Key = e.Payload.Key,
                    Preview = aspectComposition.Preview,
                    Resources = aspectComposition.Resources,
                    Settings = aspectComposition.Settings
                } 
            });
            
            _logger.LogInformation(
                $"Composition {e.Payload.CompositionId} from Project {e.Payload.ProjectId} is succesfully fulfilled with key {e.Payload.Key}");

            return Result.Successful;
        }
    }
}