using System;

namespace Upsaleslab.Templates.App.Events
{
    public class Event<T>
    {
        public string Type { get; set; }
        
        public int Version { get; set; }
        
        public Guid CorrelationId { get; set; }

        public Guid UserId { get; set; }

        public long OccurredOn { get; set; }
        
        public T Payload { get; set; }
    }
}