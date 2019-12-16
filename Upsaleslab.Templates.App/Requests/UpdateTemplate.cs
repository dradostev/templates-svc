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
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public (int, int) AspectRatio { get; set; }

        [Required]
        public List<Field> Payload { get; set; }
    }
}