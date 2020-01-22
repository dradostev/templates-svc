using System;
using System.Collections.Generic;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Events
{
    public class ProjectCompositionFulfilled
    {
        public Guid TemplateId { set; get; }
        
        public Guid ProjectId { set; get; }
        
        public Guid CompositionId { set; get; }
        
        public string Key { set; get; }
        
        public Preview Preview { get; set; }
        
        public List<Field> Resources { get; set; }
        
        public List<Field> Settings { get; set; }
    }
}