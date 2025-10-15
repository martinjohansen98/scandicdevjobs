using ScandicDevJobApi.Models.Enums.Company;
using ScandicDevJobApi.Models.Enums.JobListing;

namespace ScandicDevJobApi.Dtos
{
    public record JobListingCreateDto(
        string Title,
        string Description,
        List<string> Tags,
        string Category,
        EmploymentType EmploymentType,
        WorkMode WorkMode,
        bool IsPublished,
        string ContactEmail,
        string ApplicationUrl,
        string Currency,
        int? SalaryRangeMin,
        int? SalaryRangeMax,
        string Address,
        string City,
        string Country,
        DateTimeOffset? JobListingExpiryDate,
        DateTimeOffset? ApplicationDeadline,
        Guid? LogoFileId
    );
}
