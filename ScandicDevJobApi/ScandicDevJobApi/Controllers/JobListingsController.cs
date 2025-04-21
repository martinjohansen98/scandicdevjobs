using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScandicDevJobApi.Data;
using ScandicDevJobApi.Models;

namespace ScandicDevJobApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoblistingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        //private readonly IMapper _mapper;

        public JoblistingsController(AppDbContext context)/*  IMapper mapper*/
        {
            _context = context;
            //_mapper = mapper;
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
    }
}
