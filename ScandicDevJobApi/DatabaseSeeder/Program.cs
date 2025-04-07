using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using ScandicDevJobApi; // Replace with your actual namespace
using ScandicDevJobApi.Data;

namespace DatabaseSeeder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build the host to read configuration
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                // Retrieve the connection string from appsettings.json
                var configuration = services.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DBConnection");

                // Configure DbContext with Npgsql (PostgreSQL)
                var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(connectionString)
                    .Options);

                // Apply migrations and seed the database
                context.Database.Migrate(); // Ensures the database is up to date
                DatabaseSeeder.Seed(context); // Call the seeding method
                Console.WriteLine("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Add configuration and services needed
                    services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build());
                });
    }
}
