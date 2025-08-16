using ScandicDevJobApi.Models.Enums.Company;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ScandicDevJobApi.Models
{
    public class Company
    {
        public int Id { get; set; }
        public int? OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User? Owner { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsVerified { get; set; }
        public Guid? CompanyLogoGuid { get; set; }
        public CompanySize? CompanySize { get; set; }
        public string? Website { get; set; } // Example: "company.com"
        public string? ContactEmail { get; set; } // Example: "info@company.com"
        public string? ContactPhone { get; set; } // Example: "+46 123 456 789"


        // Social Media
        public string? LinkedIn { get; set; } // Example: "linkedin.com/company/example"
        public string? Twitter { get; set; } // Example: "twitter.com/example"
        public string? Facebook { get; set; } // Example: "facebook.com/example"

        // Jobs Associated with Company
        [JsonIgnore]
        public List<JobListing>? JobListings { get; set; } // Jobs posted by this company

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
