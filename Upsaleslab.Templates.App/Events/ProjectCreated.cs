using System;

namespace Upsaleslab.Templates.App.Events
{
    public class ProjectCreated
    {
        public Guid UserId { set; get; }
        
        public string Title { set; get; }
        
        public Guid ProjectId { set; get; }
        
        public Guid TemplateId { set; get; }
        
        public string Ratio { set; get; }
    }
}