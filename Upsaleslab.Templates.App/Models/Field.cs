#nullable enable
using Constraints = System.Collections.Generic.Dictionary<string, string>;
using Localized = System.Collections.Generic.Dictionary<string, string>;

namespace Upsaleslab.Templates.App.Models
{
    public class Field
    {
        public string Key { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public Constraints? Constraints { get; set; }
        
        public Localized? Title { get; set; }
    }
}