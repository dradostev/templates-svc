using System;
using System.Collections.Generic;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Events
{
    public class TemplateUpdated
    {
        public Guid TemplateId { get; set; }
        
        public Dictionary<string, string> Title { get; set; }

        public string[] Tags { get; set; }

        public string[] AspectRatios { get; set; }

        public List<Slide> Slides { get; set; }
    }
}