using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Upsaleslab.Templates.App.Models;
using Localized = System.Collections.Generic.Dictionary<string, string>;

namespace Upsaleslab.Templates.App.Requests
{
    public class UpdateTemplate
    {
        [Required]
        public Guid CorrelationId { get; set; }
        
        [Required, MinLength(2)]
        public string Key { get; set; }

        [Required, MinLength(2)]
        public string Type { get; set; }

        [Required]
        public Localized Title { get; set; }
        
        [Required]
        public Localized Description { get; set; }
        
        [Required]
        public string[] Tags { get; set; }
        
        [Required]
        public Dictionary<string, AspectRatio> Ratios { get; set; }
    }
}