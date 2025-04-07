using ScandicDevJobApi.Models.Enums.Company;
using ScandicDevJobApi.Models.Enums.JobListing;

namespace ScandicDevJobApi.Models
{
    public class JobListing
    {
        // General
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public User? Owner { get; set; }
        public List<Tag>? Tags { get; set; }
        public string? Category { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public WorkMode WorkMode { get; set; }
        public bool IsPublished { get; set; } // Determines if the job is publicly visible
        public int NumberOfApplicants { get; set; } = 0;

        // Company
        public Company? Company { get; set; }
        public string? ContactEmail { get; set; }
        public string? ApplicationUrl { get; set; }

        // Salary & money
        public string? Currency { get; set; }
        public int? SalaryRangeMin { get; set; }
        public int? SalaryRangeMax { get; set; }

        // Location
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        //Dates
        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedDate { get; set; }
        public DateTimeOffset? JobListingExpiryDate { get; set; }
        public DateTimeOffset? ApplicationDeadline { get; set; }


    }
}
