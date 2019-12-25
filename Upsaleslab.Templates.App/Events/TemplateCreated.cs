using System;
using System.Collections.Generic;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Events
{
    public class TemplateCreated
    {
        public Guid TemplateId { get; set; }
    }
}