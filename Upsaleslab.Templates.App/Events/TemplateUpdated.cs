using System;
using System.Collections.Generic;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Events
{
    public class TemplateUpdated
    {
        public Guid TemplateId { get; set; }
        
        public string Title { get; set; }

        public string Category { get; set; }

        public (int, int) AspectRatio { get; set; }

        public List<Field> Payload { get; set; }
    }
}