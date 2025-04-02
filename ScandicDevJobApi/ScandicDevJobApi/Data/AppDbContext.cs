using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ScandicDevJobApi.Models;

namespace ScandicDevJobApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<JobListing> Joblistings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // Add more DbSets for other models
    }
}
