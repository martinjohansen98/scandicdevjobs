using ScandicDevJobApi.Models.Enums.Tag;
using System.Text.Json.Serialization;

namespace ScandicDevJobApi.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? UnicodeIcon { get; set; }

        public int? TagLevel { get; set; }
        public TagCategory? TagCategory { get; set; }

        [JsonIgnore]
        public List<JobListing>? Joblistings { get; set; }
    }
}
