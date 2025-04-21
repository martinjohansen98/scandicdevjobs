using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ScandicDevJobApi.Data;
using ScandicDevJobApi.Models.Enums.Company;
using ScandicDevJobApi.Models.Enums.JobListing;
using ScandicDevJobApi.Models.Enums.Tag;
using ScandicDevJobApi.Models;
using System.Text.Json.Serialization;


namespace DatabaseSeeder
{
    public static class DatabaseSeeder
    {
        public static void Seed(AppDbContext context)
        {
            var users = new List<User>
            {
                new User { FirstName = "Alice", LastName = "Testtt", Email = "alice@example.com"},
                new User { FirstName = "Bob", LastName = "Testtt", Email = "bob@example.com" },
                new User { FirstName = "Charlie", LastName = "Testtt", Email = "charlie@example.com" },
                new User { FirstName = "David", LastName = "Testtt", Email = "david@example.com" },
                new User { FirstName = "Eve", LastName = "Testtt", Email = "eve@example.com" },
            };
            if (!context.Users.Any())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
            var techCorp = new Company
            {
                Name = "TechCorp",
                Email = "contact@techcorp.com",
                IsVerified = true,
                CompanySize = CompanySize.TenToFifty,
                CompanyLogoGuid = new Guid(),
                Website = "https://techcorp.com",
                ContactEmail = "support@techcorp.com",
                ContactPhone = "+1 234 567 890",
                LinkedIn = "https://linkedin.com/company/techcorp",
                Twitter = "https://twitter.com/techcorp",
                Facebook = "https://facebook.com/techcorp"
            };
            var devSoft = new Company
            {
                Name = "DevSoft",
                Email = "hr@devsoft.com",
                IsVerified = false,
                CompanySize = CompanySize.FiftyToHundred,
                CompanyLogoGuid = new Guid(),
                Website = "https://devsoft.com",
                ContactEmail = "info@devsoft.com",
                ContactPhone = "+1 987 654 321",
                LinkedIn = "https://linkedin.com/company/devsoft",
                Twitter = "https://twitter.com/devsoft",
                Facebook = "https://facebook.com/devsoft"
            };
            var innovateX = new Company
            {
                Name = "InnovateX",
                Email = "hello@innovatex.com",
                IsVerified = true,
                CompanySize = CompanySize.HundredToFiveHundred,
                CompanyLogoGuid = new Guid(),
                Website = "https://innovatex.com",
                ContactEmail = "contact@innovatex.com",
                ContactPhone = "+1 555 123 456",
                LinkedIn = "https://linkedin.com/company/innovatex",
                Twitter = "https://twitter.com/innovatex",
                Facebook = "https://facebook.com/innovatex"
            };
            var cloudNine = new Company
            {
                Name = "CloudNine",
                Email = "support@cloudnine.com",
                IsVerified = false,
                CompanySize = CompanySize.OneToTen,
                CompanyLogoGuid = new Guid(),
                Website = "https://cloudnine.com",
                ContactEmail = "hello@cloudnine.com",
                ContactPhone = "+1 555 654 321",
                LinkedIn = "https://linkedin.com/company/cloudnine",
                Twitter = "https://twitter.com/cloudnine",
                Facebook = "https://facebook.com/cloudnine"
            };
            var cyberTech = new Company
            {
                Name = "CyberTech",
                Email = "info@cybertech.com",
                IsVerified = true,
                CompanySize = CompanySize.FiveHundredPlus,
                CompanyLogoGuid = new Guid(),
                Website = "https://cybertech.com",
                ContactEmail = "contact@cybertech.com",
                ContactPhone = "+1 800 123 456",
                LinkedIn = "https://linkedin.com/company/cybertech",
                Twitter = "https://twitter.com/cybertech",
                Facebook = "https://facebook.com/cybertech"
            };
            var companies = new List<Company>
            {
                techCorp,
                devSoft,
                innovateX,
                cloudNine,
                cyberTech               
            };
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(companies);
                context.SaveChanges();
            }


            var tags = new List<Tag>
            {
                new Tag { Name = "C#", TagCategory = TagCategory.ProgramingLanguage },
                new Tag { Name = "React", TagCategory = TagCategory.FrontEndFramework  },
                new Tag { Name = "SQL", TagCategory = TagCategory.ProgramingLanguage },
                new Tag { Name = "AWS", TagCategory = TagCategory.Technologies },
                new Tag { Name = "Python", TagCategory = TagCategory.ProgramingLanguage },
            };
            if (!context.Tags.Any())
            {
                context.Tags.AddRange(tags);
                context.SaveChanges();
            }

            var jobListings = new List<JobListing>
            {
                new JobListing
                {
                    Title = "Software Engineer",
                    Description = "Develop and maintain software applications.",
                    Owner = users[0],
                    Company = companies[0],
                    EmploymentType = EmploymentType.FullTime,
                    WorkMode = WorkMode.Remote,
                    IsPublished = true,
                    NumberOfApplicants = 10,
                    SalaryRangeMin = 70000,
                    SalaryRangeMax = 100000,
                    Currency = "USD",
                    Address = "123 Tech Street",
                    City = "Tech City",
                    Country = "Techland",
                    Latitude = 40.7128,
                    Longitude = -74.0060,
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow,
                    JobListingExpiryDate = DateTimeOffset.UtcNow.AddMonths(1),
                    ApplicationDeadline = DateTimeOffset.UtcNow.AddMonths(1),
                    Tags = new List<Tag> {tags[0], tags[1], tags[4], tags[3] }
                   
                },
                new JobListing
                {
                    Title = "Frontend Developer",
                    Description = "Design and implement user-facing features.",
                    Owner = users[1],
                    Company = companies[1],
                    EmploymentType = EmploymentType.PartTime,
                    WorkMode = WorkMode.Hybrid,
                    IsPublished = true,
                    NumberOfApplicants = 5,
                    SalaryRangeMin = 50000,
                    SalaryRangeMax = 75000,
                    Currency = "USD",
                    Address = "456 Dev Lane",
                    City = "Dev Town",
                    Country = "Devland",
                    Latitude = 34.0522,
                    Longitude = -118.2437,
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow,
                    JobListingExpiryDate = DateTimeOffset.UtcNow.AddMonths(1),
                    ApplicationDeadline = DateTimeOffset.UtcNow.AddMonths(1),
                    Tags = new List<Tag> {tags[0], tags[1]}

                },
                new JobListing
                {
                    Title = "Backend Developer",
                    Description = "Maintain the backend systems and infrastructure.",
                    Owner = users[2],
                    Company = companies[2],
                    EmploymentType = EmploymentType.FullTime,
                    WorkMode = WorkMode.OnSite,
                    IsPublished = true,
                    NumberOfApplicants = 8,
                    SalaryRangeMin = 80000,
                    SalaryRangeMax = 120000,
                    Currency = "USD",
                    Address = "789 Backend Blvd",
                    City = "Dev City",
                    Country = "Devland",
                    Latitude = 37.7749,
                    Longitude = -122.4194,
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow,
                    JobListingExpiryDate = DateTimeOffset.UtcNow.AddMonths(1),
                    ApplicationDeadline = DateTimeOffset.UtcNow.AddMonths(1),
                    Tags = new List<Tag> { tags[4], tags[3] }

                },
                new JobListing
                {
                    Title = "DevOps Engineer",
                    Description = "Work with development and operations teams to manage infrastructure.",
                    Owner = users[3],
                    Company = companies[3],
                    EmploymentType = EmploymentType.TimeContract,
                    WorkMode = WorkMode.Remote,
                    IsPublished = false,
                    NumberOfApplicants = 2,
                    SalaryRangeMin = 90000,
                    SalaryRangeMax = 110000,
                    Currency = "USD",
                    Address = "101 Cloud Nine Rd",
                    City = "Cloud City",
                    Country = "Cloudland",
                    Latitude = 51.5074,
                    Longitude = -0.1278,
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow,
                    JobListingExpiryDate = DateTimeOffset.UtcNow.AddMonths(1),
                    ApplicationDeadline = DateTimeOffset.UtcNow.AddMonths(1),
                    Tags = new List<Tag> {tags[0] }

                },
                new JobListing
                {
                    Title = "Data Scientist",
                    Description = "Analyze data to inform business decisions.",
                    Owner = users[4],
                    Company = companies[4],
                    EmploymentType = EmploymentType.FullTime,
                    WorkMode = WorkMode.Hybrid,
                    IsPublished = true,
                    NumberOfApplicants = 3,
                    SalaryRangeMin = 85000,
                    SalaryRangeMax = 115000,
                    Currency = "USD",
                    Address = "202 CyberTech Way",
                    City = "Cyber City",
                    Country = "Cyberland",
                    Latitude = 48.8566,
                    Longitude = 2.3522,
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow,
                    JobListingExpiryDate = DateTimeOffset.UtcNow.AddMonths(1),
                    ApplicationDeadline = DateTimeOffset.UtcNow.AddMonths(1),
                    Tags = new List<Tag> {tags[4] }

                }
            };
            if (!context.Joblistings.Any())
            {
                context.Joblistings.AddRange(jobListings);
                context.SaveChanges();
            }

        }
    }
}
 