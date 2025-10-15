using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using ScandicDevJobApi.Data;
using ScandicDevJobApi.Dtos;
using ScandicDevJobApi.Models;
using ScandicDevJobApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScandicDevJobApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoblistingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IGeocodingService _geocoder;

        public JoblistingsController(AppDbContext context, IGeocodingService geocoder)
        {
            _context = context;
            _geocoder = geocoder;
        }

        // GET: api/JobListings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobListing>>> GetJoblistings()
        {
            return await _context.Joblistings
                .Include(j => j.Company)
                .Include(j => j.Tags)
                .ToListAsync();
        }

        // GET: api/JobListings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobListing>> GetJobListing(int id)
        {
            var jobListing = await _context.Joblistings.FindAsync(id);

            if (jobListing == null)
            {
                return NotFound();
            }

            return jobListing;
        }

        // PUT: api/JobListings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobListing(int id, JobListing jobListing)
        {
            if (id != jobListing.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobListing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobListingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/JobListings
        [HttpPost]
        public async Task<ActionResult<JobListing>> PostJobListing(JobListing jobListing)
        {
            _context.Joblistings.Add(jobListing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobListing", new { id = jobListing.Id }, jobListing);
        }

        // DELETE: api/JobListings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobListing(int id)
        {
            var jobListing = await _context.Joblistings.FindAsync(id);
            if (jobListing == null)
            {
                return NotFound();
            }

            _context.Joblistings.Remove(jobListing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobListingExists(int id)
        {
            return _context.Joblistings.Any(e => e.Id == id);
        }

        [HttpPost("create")]
        public async Task<ActionResult<JobListing>> Create(JobListingCreateDto dto)
        {
            // get the current user‐ID from the cookie/JWT
            if (!Request.Cookies.TryGetValue("jwt", out var token))
                return Unauthorized();

            var handler = new JsonWebTokenHandler();
            var jwt = handler.ReadJsonWebToken(token);
            var idClaim = jwt.GetPayloadValue<string>(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out var ownerId))
                return Unauthorized();

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.OwnerId == ownerId);
            if (company == null)
                return BadRequest("You must have a verified company to post jobs.");

            var addressParts = new[] { dto.Address, dto.City, dto.Country }
                .Where(s => !string.IsNullOrWhiteSpace(s));
            var fullAddress = string.Join(", ", addressParts);

            var job = new JobListing
            {
                OwnerId = ownerId,
                CompanyId = company.Id,
                Title = dto.Title,
                Description = dto.Description,
                Tags = dto.Tags.Select(t => new Tag { Name = t }).ToList(),
                Category = dto.Category,
                EmploymentType = dto.EmploymentType,
                WorkMode = dto.WorkMode,
                IsPublished = dto.IsPublished,
                ContactEmail = dto.ContactEmail,
                ApplicationUrl = dto.ApplicationUrl,
                Currency = dto.Currency,
                SalaryRangeMin = dto.SalaryRangeMin,
                SalaryRangeMax = dto.SalaryRangeMax,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                JobListingExpiryDate = dto.JobListingExpiryDate?.ToUniversalTime(),
                ApplicationDeadline = dto.ApplicationDeadline?.ToUniversalTime(),
                CreatedDate = DateTimeOffset.UtcNow,
                LogoFileId = dto.LogoFileId
            };

            if (!string.IsNullOrWhiteSpace(fullAddress) &&
                (job.Latitude is null || job.Longitude is null))
            {
                var pt = await _geocoder.GeocodeAsync(fullAddress, HttpContext.RequestAborted);
                if (pt is not null)
                {
                    job.Latitude = pt.Value.lat;
                    job.Longitude = pt.Value.lon;
                }
            }

            _context.Joblistings.Add(job);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobListing), new { id = job.Id }, job);
        }
    }
}
