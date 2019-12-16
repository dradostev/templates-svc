using System;
using System.Collections.Generic;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Events
{
    public class TemplateCreated
    {
        public Guid TemplateId { get; set; }
        
        public string Title { get; set; }

        public string Category { get; set; }

        public string AspectRatio { get; set; }

        public List<Field> Payload { get; set; }
    }
}