using System;
using System.Text.Json.Serialization;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Models
{
    public class Category
    {
        public Guid Id { get; private set; }
        
        public string Name { get; private set; }

        public Guid CorrelationId { get; private set; }
        
        public long Created { get; private set; }
        
        public long Updated { get; private set; }

        [JsonIgnore]
        public long Deleted { get; private set; }

        public static (Category, Event<CategoryCreated>) On(CreateCategory request, Guid userId)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CorrelationId = request.CorrelationId,
                Created = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Updated = 0,
                Deleted = 0
            };
            return (category, new Event<CategoryCreated>
            {
                Type = "category-created",
                Version = 1,
                CorrelationId = request.CorrelationId,
                UserId = userId,
                OccurredOn = category.Created,
                Payload = new CategoryCreated
                {
                    Name = category.Name
                }
            });
        }

        public Event<CategoryUpdated> On(UpdateCategory request, Guid userId)
        {
            Name = request.Name;
            CorrelationId = request.CorrelationId;
            Updated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return new Event<CategoryUpdated>
            {
                Type = "category-updated",
                Version = 1,
                CorrelationId = request.CorrelationId,
                UserId = userId,
                OccurredOn = Updated,
                Payload = new CategoryUpdated
                {
                    Name = Name
                }
            };
        }

        public Event<CategoryDeleted> On(DeleteCategory request, Guid userId)
        {
            CorrelationId = request.CorrelationId;
            Deleted = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            return new Event<CategoryDeleted>
            {
                Type = "category-updated",
                Version = 1,
                CorrelationId = request.CorrelationId,
                UserId = userId,
                OccurredOn = Deleted,
                Payload = new CategoryDeleted
                {
                    CategoryId = Id
                }
            };
        }
    }
}