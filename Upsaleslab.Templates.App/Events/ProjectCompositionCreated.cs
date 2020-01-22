using System;

namespace Upsaleslab.Templates.App.Events
{
    public class ProjectCompositionCreated
    {
        public Guid TemplateId { set; get; }
        public Guid ProjectId { set; get; }
        public Guid CompositionId { set; get; }
        public string Ratio { set; get; }
        public string Key { set; get; }
        public string Title { set; get; }
    }
}