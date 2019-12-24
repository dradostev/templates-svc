using Microsoft.AspNetCore.Mvc;

namespace Upsaleslab.Projects.App.Models
{
    public class Paginate
    {
        public int Limit { get; set; } = 10;
        
        public int Offset { get; set; } = 0;
        
        [FromQuery(Name = "tags[]")]
        public string[] Tags { get; set; }
        
        [FromQuery(Name = "aspectRatios[]")]
        public string[] AspectRatios { get; set; }
    }
}