using System.Collections.Generic;

namespace Upsaleslab.Templates.App.Models
{
    public class Slide
    {
        public Dictionary<string, string> Title { get; private set; }

        public Dictionary<string, Preview> Preview { get; private set; }

        public List<Field> Payload { get; private set; }

        public Slide(Dictionary<string, string> title, Dictionary<string, Preview> preview, List<Field> payload)
        {
            Title = title;
            Preview = preview;
            Payload = payload;
        }
    }
}