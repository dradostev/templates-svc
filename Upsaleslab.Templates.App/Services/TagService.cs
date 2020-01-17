using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;
using Tag = Upsaleslab.Templates.App.Models.Tag;

namespace Upsaleslab.Templates.App.Services
{
    public class TagService : ITagService
    {
        private readonly IEventService _eventService;
        
        private readonly ILogger<TagService> _logger;
        
        private readonly IMongoCollection<Tag> _tags;

        public TagService(IMongoClient mongo, IEventService eventService, ILogger<TagService> logger)
        {
            _eventService = eventService;
            _logger = logger;
            _tags = mongo.GetDatabase("TemplatesService").GetCollection<Tag>("Tags");
        }
        
        public async Task<(Result, Tag)> CreateAsync(CreateTag request, Guid userId)
        {
            _logger.LogInformation($"Trying to create tag {request.Key}");
            
            if (await _tags.CountDocumentsAsync(
                    x => x.CorrelationId == request.CorrelationId
                         || x.Key == request.Key) > 0)
            {
                return (Result.Conflict, null);
            }

            var (category, categoryCreated) = Tag.On(request, userId);

            await _tags.InsertOneAsync(category);

            await _eventService.PublishAsync(categoryCreated);
            
            _logger.LogInformation($"Tag {request.Key} created");

            return (Result.Successful, category);
        }

        public async Task<(Result, Tag)> UpdateAsync(Guid catId, UpdateTag request, Guid userId)
        {
            _logger.LogInformation($"Trying to update tag {request.Name}");
            
            var category = await _tags
                .Find(x => x.Id == catId)
                .FirstOrDefaultAsync();
            
            if (category is null) return (Result.NotFound, null);
            
            if (category.Deleted > 0) return (Result.Gone, null);

            if (category.CorrelationId == request.CorrelationId) return (Result.Conflict, null);

            var categoryUpdated = category.On(request, userId);

            await _tags.ReplaceOneAsync(x => x.Id == catId, category);

            await _eventService.PublishAsync(categoryUpdated);
            
            _logger.LogInformation($"Tag {request.Name} updated");

            return (Result.Successful, category);
        }

        public async Task<Result> DeleteAsync(Guid catId, DeleteTag request, Guid userId)
        {
            _logger.LogInformation($"Trying to delete tag {catId}");
            
            var category = await _tags
                .Find(x => x.Id == catId)
                .FirstOrDefaultAsync();
            
            if (category is null) return Result.NotFound;
            
            if (category.Deleted > 0) return Result.Gone;

            if (category.CorrelationId == request.CorrelationId) return Result.Conflict;

            var categoryDeleted = category.On(request, userId);
            
            await _tags.ReplaceOneAsync(x => x.Id == catId, category);

            await _eventService.PublishAsync(categoryDeleted);
            
            _logger.LogInformation($"Tag {category.Key} deleted");

            return Result.Successful;
        }

        public async Task<IEnumerable<Tag>> ListAsync() =>
            await _tags.Find(x => true).ToListAsync();
    }
}