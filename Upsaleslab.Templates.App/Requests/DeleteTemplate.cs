using System;
using System.ComponentModel.DataAnnotations;

namespace Upsaleslab.Templates.App.Requests
{
    public class DeleteTemplate
    {
        [Required]
        public Guid CorrelationId { get; set; }
    }
}