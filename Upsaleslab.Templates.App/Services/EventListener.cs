using System;
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
            var template = await _templates
                .Find(x => x.Id == e.Payload.TemplateId)
                .FirstOrDefaultAsync();

            if (template is null) return Result.NotFound;

            if (!template.Ratios.TryGetValue(e.Payload.Ratio, out var ratio)) return Result.NotFound;

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
                    Preview = ratio.Project.Preview,
                    Resources = ratio.Project.Resources,
                    Settings = ratio.Project.Settings
                } 
            });

            return Result.Successful;
        }
    }
}