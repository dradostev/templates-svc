using System;
using System.Collections.Generic;

namespace Upsaleslab.Templates.App.Models
{
    public class Meta
    {
        public Guid Id { set; get; }
        public string Key { set; get; }
        public string Type { set; get; }
        public string[] Ratios { set; get; }
        public Dictionary<string, Preview> Preview { set; get; }
        public Dictionary<string, string> Title { set; get; }
        public Dictionary<string, string> Description { set; get; }
        public string[] Tags { set; get; }
    }
}