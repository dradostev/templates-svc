using System.Collections.Generic;
using Localized = System.Collections.Generic.Dictionary<string, string>;

namespace Upsaleslab.Templates.App.Models
{
    public class Composition
    {
        public string Key { get; set; }

        public int Order { get; set; }

        public Localized Title { get; set; }

        public Localized Description { get; set; }

        public Preview Preview { get; set; }
        
        public List<Field> Resources { get; set; }
        
        public List<Field> Settings { get; set; }
    }
}