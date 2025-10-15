using Microsoft.EntityFrameworkCore;
using ScandicDevJobApi.Data;
using ScandicDevJobApi.filters;
using ScandicDevJobApi.Models.Azure;
using ScandicDevJobApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Determine the environment
var env = builder.Environment; // This gives access to Development, Staging, Production

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection"))); // Use UseSqlServer for SQL Server

builder.Services.AddCors(options => // Add CORS policy

{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()                     
            .AllowAnyHeader()                     
            .AllowCredentials();                  
    });
});

// 2️⃣ Add your DbContext and controllers
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddControllers();

// Swagger for development
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.OperationFilter<FileUploadOperationFilter>(); // register file upload filter
});

// Azure Blob Storage
builder.Services.Configure<AzureBlobStorageSettings>(
    builder.Configuration.GetSection("Azure:BlobStorage")
);
builder.Services.AddSingleton<AzureBlobStorageService>();

// Geocoding service
builder.Services.AddHttpClient<IGeocodingService, NominatimGeocodingService>(client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("ScandicDevJobs/1.0 (contact:)");
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

// Enable CORS before other middleware
app.UseCors("AllowFrontend");

app.UseCors("AllowFrontend");

app.UseCors("AllowFrontend");

app.UseCors("AllowFrontend");

app.UseCors("AllowFrontend");

app.UseCors("AllowFrontend");

app.UseCors("AllowFrontend");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
