using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IEventService _eventService;
        
        private readonly ILogger<CategoriesService> _logger;
        
        private readonly IMongoCollection<Category> _categories;

        public CategoriesService(IMongoClient mongo, IEventService eventService, ILogger<CategoriesService> logger)
        {
            _eventService = eventService;
            _logger = logger;
            _categories = mongo.GetDatabase("TemplatesService").GetCollection<Category>("Categories");
        }
        
        public async Task<(Result, Category)> CreateCategoryAsync(CreateCategory request, Guid userId)
        {
            _logger.LogInformation($"Trying to create category {request.Name}");
            
            if (await _categories.CountDocumentsAsync(
                    x => x.CorrelationId == request.CorrelationId
                         || x.Name == request.Name) > 0)
            {
                return (Result.Conflict, null);
            }

            var (category, categoryCreated) = Category.On(request, userId);

            await _categories.InsertOneAsync(category);

            await _eventService.PublishAsync(categoryCreated);
            
            _logger.LogInformation($"Category {request.Name} created");

            return (Result.Successful, category);
        }

        public async Task<(Result, Category)> UpdateCategoryAsync(Guid catId, UpdateCategory request, Guid userId)
        {
            _logger.LogInformation($"Trying to update category {request.Name}");
            
            var category = await _categories
                .Find(x => x.Id == catId)
                .FirstOrDefaultAsync();
            
            if (category is null) return (Result.NotFound, null);
            
            if (category.Deleted > 0) return (Result.Gone, null);

            if (category.CorrelationId == request.CorrelationId) return (Result.Conflict, null);

            var categoryUpdated = category.On(request, userId);

            await _categories.ReplaceOneAsync(x => x.Id == catId, category);

            await _eventService.PublishAsync(categoryUpdated);
            
            _logger.LogInformation($"Category {request.Name} updated");

            return (Result.Successful, category);
        }

        public async Task<Result> DeleteCategoryAsync(Guid catId, DeleteCategory request, Guid userId)
        {
            _logger.LogInformation($"Trying to delete category {catId}");
            
            var category = await _categories
                .Find(x => x.Id == catId)
                .FirstOrDefaultAsync();
            
            if (category is null) return Result.NotFound;
            
            if (category.Deleted > 0) return Result.Gone;

            if (category.CorrelationId == request.CorrelationId) return Result.Conflict;

            var categoryDeleted = category.On(request, userId);
            
            await _categories.ReplaceOneAsync(x => x.Id == catId, category);

            await _eventService.PublishAsync(categoryDeleted);
            
            _logger.LogInformation($"Category {category.Name} deleted");

            return Result.Successful;
        }

        public async Task<IEnumerable<Category>> ListCategoriesAsync() =>
            await _categories.Find(x => true).ToListAsync();
    }
}