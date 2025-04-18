using Microsoft.EntityFrameworkCore;
using ScandicDevJobApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Determine the environment
var env = builder.Environment; // This gives access to Development, Staging, Production

// 1️⃣ Add CORS policy that allows credentials
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // Your Svelte dev URL
            .AllowAnyMethod()                     // Allow GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader()                     // Allow all headers
            .AllowCredentials();                  // <-- MUST for cookies/credentials :contentReference[oaicite:0]{index=0}
    });
});

// 2️⃣ Add your DbContext and controllers
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddControllers();

// Swagger for development
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3️⃣ Enable CORS **before** other middleware (authorization, endpoints)
app.UseCors("AllowFrontend");            // <-- applies the policy globally :contentReference[oaicite:1]{index=1}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
