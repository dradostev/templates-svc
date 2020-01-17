using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Upsaleslab.Templates.App.Requests
{
    public class CreateTag
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public Dictionary<string, string> Title { get; set; }

        [Required]
        public Guid CorrelationId { get; set; }
    }
}