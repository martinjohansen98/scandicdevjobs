namespace ScandicDevJobApi.Models
{
    public class JobListing
    {
        // General
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public User Owner { get; set; }
        public List<Tag>? Tags { get; set; }
        public string? Category { get; set; }
        public string? CompanyLogoWebAddress { get; set; }

        // Salary & money
        public string Currency { get; set; }
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }

        // Location
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }        

    }
}
