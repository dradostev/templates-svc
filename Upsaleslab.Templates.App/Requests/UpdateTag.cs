using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Upsaleslab.Templates.App.Requests
{
    public class UpdateTag
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public Dictionary<string, string> Title { get; set; }

        [Required]
        public Guid CorrelationId { get; set; }
    }
}