using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Requests
{
    public class UpdateTemplate
    {
        [Required]
        public Guid CorrelationId { get; set; }
        
        [Required]
        public Dictionary<string, string> Title { get; set; }
        
        [Required]
        public Dictionary<string, string> Description { get; set; }

        [Required]
        public string[] Tags { get; set; }

        [Required]
        public string[] AspectRatios { get; set; }

        [Required]
        public List<Slide> Slides { get; set; }
        
        [Required]
        public Dictionary<string, Preview> Preview { get; set; }
    }
}