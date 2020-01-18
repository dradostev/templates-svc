using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Requests;
using Localized = System.Collections.Generic.Dictionary<string, string>;

namespace Upsaleslab.Templates.App.Models
{
    public class Template
    {
        public Guid Id { get; private set; }
        
        public Guid CorrelationId { get; private set; }

        public string Key { get; private set; }

        public string Type { get; private set; }

        public Localized Title { get; private set; }
        
        public Localized Description { get; private set; }

        public string[] Tags { get; private set; }

        public Dictionary<string, AspectRatio> Ratios { get; private set; }

        public long Created { get; private set; }

        public long Updated { get; private set; }

        [JsonIgnore]
        public long Deleted { get; private set; }
        
        public static (Template, Event<TemplateCreated>) On(CreateTemplate request, Guid userId)
        {
            var template = new Template
            {
                Id = Guid.NewGuid(),
                Key = request.Key,
                Type = request.Type,
                CorrelationId = request.CorrelationId,
                Title = request.Title,
                Description = request.Description,
                Tags = request.Tags,
                Ratios = request.Ratios,
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
                    TemplateId = template.Id
                }
            });
        }
        
        public Event<TemplateUpdated> On(UpdateTemplate request, Guid userId)
        {
            CorrelationId = request.CorrelationId;
            Key = request.Key;
            Title = request.Title;
            Description = request.Description;
            Tags = request.Tags;
            Ratios = request.Ratios;
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
                    TemplateId = Id
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

        public Meta ToMeta()
        {
            return new Meta
            {
                Id = Id,
                Key = Key,
                Type = Type,
                Ratios = Ratios.Keys.ToArray(),
                Preview = Ratios.Keys.ToDictionary(k => k, k=> Ratios[k].Project.Preview),
                Title = Title,
                Description = Description
            };
        }
    }
}