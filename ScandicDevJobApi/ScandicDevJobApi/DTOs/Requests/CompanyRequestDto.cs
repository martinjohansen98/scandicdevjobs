using ScandicDevJobApi.Models;
using ScandicDevJobApi.Models.Enums.Company;
using System.Buffers.Text;

public record CompanyRequestDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; } // Signup / login email
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
    public string? BlobStorageLogoName { get; set; }
    //public string? CompanyLogoUrl => BlobStorageLogoName != null
    //    ? $"{BaseUrl}{BlobStorageLogoName}"
    //    : null;
    public CompanySize? CompanySize { get; set; }
    public string? Website { get; set; } // Example: "company.com"
    public string? ContactEmail { get; set; } // Example: "info@company.com"
    public string? ContactPhone { get; set; } // Example: "+46 123 456 789"


    // Social Media
    public string? LinkedIn { get; set; } // Example: "linkedin.com/company/example"
    public string? Twitter { get; set; } // Example: "twitter.com/example"
    public string? Facebook { get; set; } // Example: "facebook.com/example"

    // Jobs Associated with Company
    //[JsonIgnore]
    //public List<JobListing>? JobListings { get; set; } // Jobs posted by this company

}

