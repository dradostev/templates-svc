using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Upsaleslab.Projects.App.Models;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;
using Tag = Upsaleslab.Templates.App.Models.Tag;

namespace Upsaleslab.Templates.App.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IEventService _eventService;
        
        private readonly ITagService _tagService;

        private readonly ILogger<TemplateService> _logger;
        
        private readonly IMongoCollection<Template> _templates;
        
        private readonly IMongoCollection<Tag> _tags;

        public TemplateService(
            IMongoClient mongo, IEventService eventService, ITagService tagService, ILogger<TemplateService> logger)
        {
            _eventService = eventService;
            _tagService = tagService;
            _logger = logger;
            _templates = mongo.GetDatabase("TemplatesService").GetCollection<Template>("Templates");
            _tags = mongo.GetDatabase("TemplatesService").GetCollection<Tag>("Tags");
        }
        
        public async Task<(Result, Template?)> CreateAsync(CreateTemplate request, Guid userId)
        {
            _logger.LogInformation($"Trying to create template {request.Title}");
            if (await _templates.CountDocumentsAsync(x => x.CorrelationId == request.CorrelationId) > 0)
            {
                return (Result.Conflict, null);
            }

            foreach (var tagName in request.Tags)
            {
                if (await _tags.CountDocumentsAsync(x => x.Key == tagName) == 0)
                {
                    await _tagService.CreateAsync(new CreateTag
                    {
                        CorrelationId = request.CorrelationId,
                        Key = tagName,
                        Title = new Dictionary<string, string>{ {"en", tagName} }
                    }, userId);
                }
            }

            var (template, templateCreated) = Template.On(request, userId);

            await _templates.InsertOneAsync(template);

            await _eventService.PublishAsync(templateCreated);
            
            _logger.LogInformation($"Template {template.Id} created");

            return (Result.Successful, template);
        }

        public async Task<(Result, Template?)> UpdateAsync(Guid templateId, UpdateTemplate request, Guid userId)
        {
            var template = await _templates
                .Find(x => x.Id == templateId)
                .FirstOrDefaultAsync();
            
            if (template is null) return (Result.NotFound, null);
            
            if (template.Deleted > 0) return (Result.Gone, null);

            if (template.CorrelationId == request.CorrelationId) return (Result.Conflict, null);

            _logger.LogInformation($"Trying to update template {template.Id}");
            
            foreach (var tagName in request.Tags)
            {
                if (await _tags.CountDocumentsAsync(x => x.Key == tagName) == 0)
                {
                    await _tagService.CreateAsync(new CreateTag
                    {
                        CorrelationId = request.CorrelationId,
                        Key = tagName,
                        Title = new Dictionary<string, string>{ {"en", tagName} }
                    }, userId);
                }
            }

            var templateUpdated = template.On(request, userId);

            await _templates.ReplaceOneAsync(x => x.Id == templateId, template);

            await _eventService.PublishAsync(templateUpdated);
            
            _logger.LogInformation($"Template {template.Id} updated");

            return (Result.Successful, template);
        }

        public async Task<Result> DeleteAsync(Guid templateId, DeleteTemplate request, Guid userId)
        {
            var template = await _templates
                .Find(x => x.Id == templateId)
                .FirstOrDefaultAsync();

            if (template is null) return Result.NotFound;
            
            if (template.Deleted > 0) return Result.Gone;

            _logger.LogInformation($"Trying to delete template {template.Id}");

            var templateDeleted = template.On(request, userId);
            
            await _templates.ReplaceOneAsync(x => x.Id == templateId, template);

            await _eventService.PublishAsync(templateDeleted);
            
            _logger.LogInformation($"Template {template.Id} deleted");

            return Result.Successful;
        }

        public async Task<(Result, Template?)> FindAsync(Guid templateId)
        {
            var template = await _templates
                .Find(x => x.Id == templateId)
                .FirstOrDefaultAsync();

            if (template is null) return (Result.NotFound, null);

            if (template.Deleted > 0) return (Result.Gone, null);

            return (Result.Successful, template);
        }

        public async Task<IEnumerable<Meta>> ListAsync(Paginate request)
        {
            var builder = Builders<Template>.Filter;
            var filter = builder.Eq("Deleted", 0);
            
            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    filter &= builder.Where(x => x.Tags.Contains(tagName));
                }
            }

            if (request.AspectRatios != null && request.AspectRatios.Length > 0)
            {
                foreach (var aspectRatio in request.AspectRatios)
                {
                    filter &= builder.Where(x => x.Ratios.Keys.Contains(aspectRatio));
                }
            }

            var tpl = await _templates
                .Find(filter)
                .Skip(request.Offset)
                .Limit(request.Limit)
                .ToListAsync();

            return tpl.Select(t => t.ToMeta()).ToArray();
        }
    }
}