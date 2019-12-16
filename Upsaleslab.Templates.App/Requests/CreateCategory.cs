using System;
using System.ComponentModel.DataAnnotations;

namespace Upsaleslab.Templates.App.Requests
{
    public class CreateCategory
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CorrelationId { get; set; }
    }
}