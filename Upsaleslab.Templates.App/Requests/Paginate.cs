namespace Upsaleslab.Projects.App.Models
{
    public class Paginate
    {
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;
        public string Category { get; set; }
    }
}