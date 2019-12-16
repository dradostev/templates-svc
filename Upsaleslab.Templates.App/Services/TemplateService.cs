using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Upsaleslab.Projects.App.Models;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IEventService _eventService;
        
        private readonly ICategoriesService _categoriesService;

        private readonly ILogger<TemplateService> _logger;
        
        private readonly IMongoCollection<Template> _templates;
        
        private readonly IMongoCollection<Category> _categories;

        public TemplateService(
            IMongoClient mongo, IEventService eventService, ICategoriesService categoriesService, ILogger<TemplateService> logger)
        {
            _eventService = eventService;
            _categoriesService = categoriesService;
            _logger = logger;
            _templates = mongo.GetDatabase("TemplatesService").GetCollection<Template>("Templates");
            _categories = mongo.GetDatabase("TemplatesService").GetCollection<Category>("Categories");
        }
        
        public async Task<(Result, Template?)> CreateTemplateAsync(CreateTemplate request, Guid userId)
        {
            _logger.LogInformation($"Trying to create template {request.Title}");
            if (await _templates.CountDocumentsAsync(x => x.CorrelationId == request.CorrelationId) > 0)
            {
                return (Result.Conflict, null);
            }

            // if category doesn't exist create it
            if (await _categories.CountDocumentsAsync(x => x.Name == request.Category) == 0)
            {
                await _categoriesService.CreateCategoryAsync(new CreateCategory
                {
                    CorrelationId = request.CorrelationId,
                    Name = request.Category
                }, userId);
            }

            var (template, templateCreated) = Template.On(request, userId);

            await _templates.InsertOneAsync(template);

            await _eventService.PublishAsync(templateCreated);
            
            _logger.LogInformation($"Template {template.Id} created");

            return (Result.Successful, template);
        }

        public async Task<(Result, Template?)> UpdateTemplateAsync(Guid templateId, UpdateTemplate request, Guid userId)
        {
            var template = await _templates
                .Find(x => x.Id == templateId)
                .FirstOrDefaultAsync();
            
            if (template is null) return (Result.NotFound, null);
            
            if (template.Deleted > 0) return (Result.Gone, null);

            if (template.CorrelationId == request.CorrelationId) return (Result.Conflict, null);

            _logger.LogInformation($"Trying to update template {template.Id}");
            
            // if category doesn't exist create it
            if (await _categories.CountDocumentsAsync(x => x.Name == request.Category) == 0)
            {
                await _categoriesService.CreateCategoryAsync(new CreateCategory
                {
                    CorrelationId = request.CorrelationId,
                    Name = request.Category
                }, userId);
            }

            var templateUpdated = template.On(request, userId);

            await _templates.ReplaceOneAsync(x => x.Id == templateId, template);

            await _eventService.PublishAsync(templateUpdated);
            
            _logger.LogInformation($"Template {template.Id} updated");

            return (Result.Successful, template);
        }

        public async Task<Result> DeleteTemplateAsync(Guid templateId, DeleteTemplate request, Guid userId)
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

        public async Task<(Result, Template?)> FindTemplateAsync(Guid templateId)
        {
            var template = await _templates
                .Find(x => x.Id == templateId)
                .FirstOrDefaultAsync();

            if (template is null) return (Result.NotFound, null);

            if (template.Deleted > 0) return (Result.Gone, null);

            return (Result.Successful, template);
        }

        public async Task<IEnumerable<Template>> ListTemplatesAsync(Paginate request, string category = null)
        {
            if (category is null)
            {
                return await _templates
                    .Find(x => x.Deleted == 0)
                    .Skip(request.Offset)
                    .Limit(request.Limit)
                    .ToListAsync();
            }

            return await _templates
                .Find(x => x.Deleted == 0 && x.Category == category)
                .Skip(request.Offset)
                .Limit(request.Limit)
                .ToListAsync();
        }
    }
}