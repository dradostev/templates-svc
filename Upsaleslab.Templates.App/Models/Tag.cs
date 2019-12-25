using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Models
{
    public class Tag
    {
        public Guid Id { get; private set; }
        
        public string Name { get; private set; }
        
        public Dictionary<string, string> Title { get; private set; }

        public Guid CorrelationId { get; private set; }
        
        public long Created { get; private set; }
        
        public long Updated { get; private set; }

        [JsonIgnore]
        public long Deleted { get; private set; }

        public static (Tag, Event<TagCreated>) On(CreateTag request, Guid userId)
        {
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Title = request.Title,
                CorrelationId = request.CorrelationId,
                Created = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Updated = 0,
                Deleted = 0
            };
            return (tag, new Event<TagCreated>
            {
                Type = "tag-created",
                Version = 1,
                CorrelationId = request.CorrelationId,
                UserId = userId,
                OccurredOn = tag.Created,
                Payload = new TagCreated
                {
                    TagId = tag.Id
                }
            });
        }

        public Event<TagUpdated> On(UpdateTag request, Guid userId)
        {
            Name = request.Name;
            Title = request.Title;
            CorrelationId = request.CorrelationId;
            Updated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            return new Event<TagUpdated>
            {
                Type = "tag-updated",
                Version = 1,
                CorrelationId = request.CorrelationId,
                UserId = userId,
                OccurredOn = Updated,
                Payload = new TagUpdated
                {
                    TagId = Id
                }
            };
        }

        public Event<TagDeleted> On(DeleteTag request, Guid userId)
        {
            CorrelationId = request.CorrelationId;
            Deleted = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            return new Event<TagDeleted>
            {
                Type = "tag-deleted",
                Version = 1,
                CorrelationId = request.CorrelationId,
                UserId = userId,
                OccurredOn = Deleted,
                Payload = new TagDeleted
                {
                    TagId = Id
                }
            };
        }
    }
}