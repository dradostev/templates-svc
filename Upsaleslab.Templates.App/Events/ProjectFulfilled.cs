using System;
using System.Collections.Generic;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Events
{
    public class ProjectFulfilled
    {
        public Guid ProjectId { get; set; }

        public Guid TemplateId { get; set; }

        public string Key { get; set; }

        public int Order { get; set; }

        public string Type { get; set; }

        public Preview Preview { get; set; }
        
        public List<Field> Resources { get; set; }
        
        public List<Field> Settings { get; set; }
    }
}