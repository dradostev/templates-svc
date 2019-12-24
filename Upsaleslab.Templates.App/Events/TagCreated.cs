using System.Collections.Generic;

namespace Upsaleslab.Templates.App.Events
{
    public class TagCreated
    {
        public string Name { get; set; }
        
        public Dictionary<string, string> Title { get; set; }
    }
}