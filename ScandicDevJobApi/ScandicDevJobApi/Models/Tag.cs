using ScandicDevJobApi.Models.Enums;

namespace ScandicDevJobApi.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? UnicodeIcon { get; set; }
        public TagCategory? TagCategory { get; set; }
    }
}
