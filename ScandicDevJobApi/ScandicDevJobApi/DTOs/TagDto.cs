using ScandicDevJobApi.Models.Enums.Tag;
using ScandicDevJobApi.Models;

public record TagDto
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public string? UnicodeIcon { get; set; }

    public int? TagLevel { get; set; }
    public TagCategory? TagCategory { get; set; }

}
