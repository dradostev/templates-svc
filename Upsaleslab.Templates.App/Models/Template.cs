using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Models
{
    public class Template
    {
        public Guid Id { get; private set; }

        [JsonIgnore]
        public Guid CorrelationId { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Category { get; private set; }

        public (int, int) AspectRatio { get; private set; }

        public List<Field> Payload { get; private set; }

        public long Created { get; private set; }

        public long Updated { get; private set; }

        [JsonIgnore]
        public long Deleted { get; private set; }

        public static (Template, Event<TemplateCreated>) On(CreateTemplate request, Guid userId)
        {
            var template = new Template
            {
                Id = Guid.NewGuid(),
                CorrelationId = request.CorrelationId,
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                AspectRatio = request.AspectRatio,
                Payload = request.Payload,
                Created = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Updated = 0,
                Deleted = 0
            };

            return (template, new Event<TemplateCreated>
            {
                Type = "template-created",
                Version = 1,
                CorrelationId = request.CorrelationId,
                OccurredOn = template.Created,
                UserId = userId,
                Payload = new TemplateCreated
                {
                    Title = template.Title,
                    AspectRatio = template.AspectRatio,
                    Category = template.Category,
                    Payload = template.Payload
                }
            });
        }

        public Event<TemplateUpdated> On(UpdateTemplate request, Guid userId)
        {
            CorrelationId = request.CorrelationId;
            Title = request.Title;
            Description = request.Description;
            Category = request.Category;
            AspectRatio = request.AspectRatio;
            Payload = request.Payload;
            Updated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            return new Event<TemplateUpdated>
            {
                CorrelationId = CorrelationId,
                OccurredOn = Updated,
                Type = "template-updated",
                Version = 1,
                UserId = userId,
                Payload = new TemplateUpdated
                {
                    AspectRatio = AspectRatio,
                    Category = Category,
                    Payload = Payload,
                    Title = Title
                }
            };
        }

        public Event<TemplateDeleted> On(DeleteTemplate request, Guid userId)
        {
            CorrelationId = request.CorrelationId;
            Deleted = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return new Event<TemplateDeleted>
            {
                Type = "template-deleted",
                Version = 1,
                CorrelationId = request.CorrelationId,
                UserId = userId,
                OccurredOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Payload = new TemplateDeleted
                {
                    TemplateId = Id
                }
            };
        }
    }
}