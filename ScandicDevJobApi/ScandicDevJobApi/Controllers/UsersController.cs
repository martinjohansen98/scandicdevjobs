using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ScandicDevJobApi.Data;
using ScandicDevJobApi.Dtos;
using ScandicDevJobApi.Models;
using ScandicDevJobApi.Models.Enums.User;

namespace ScandicDevJobApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _jwtSecret;

        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret configuration is missing");
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("User already exists");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = (UserRole)dto.Role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            SetJwtCookie(user);
            return Ok(new { user.Id, user.Email, Role = user.Role.ToString() });
        }

        // POST: api/Users/company/register
        [HttpPost("company/register")]
        public async Task<ActionResult<User>> CompanyRegister(CompanyRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email already in use");

            // hash
            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // create user
            var user = new User
            {
                Email = dto.Email,
                Password = hashed,
                Role = UserRole.Company
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // create company profile
            var company = new Company
            {
                OwnerId = user.Id,
                Name = dto.CompanyName,
                Description = dto.Description,
                Website = dto.Website,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                IsVerified = false
            };
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            SetJwtCookie(user);
            return Ok(new { user.Id, user.Email, Role = user.Role.ToString() });
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized("Invalid credentials");

            SetJwtCookie(user);
            return Ok(new { user.Id, user.Email, Role = user.Role.ToString() });
        }

        [HttpGet("me")]
        public async Task<ActionResult<User>> Me()
        {
            // grab the JWT from the cookie
            if (!Request.Cookies.TryGetValue("jwt", out var token) || string.IsNullOrEmpty(token))
                return Unauthorized();

            // 2) validate it
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };

            // decode it using the async method
            var handler = new JsonWebTokenHandler();
            var result = await handler.ValidateTokenAsync(token, validationParams);
            if (!result.IsValid)
                return Unauthorized();

            JsonWebToken jwt;
            try
            {
                jwt = handler.ReadJsonWebToken(token);
            }
            catch
            {
                return Unauthorized();
            }

            // get userId
            if (!result.Claims.TryGetValue(ClaimTypes.NameIdentifier, out var idClaim) || idClaim is not string userIdString || !int.TryParse(userIdString, out var userId))
                return Unauthorized();

            // load the user
            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                Role = user.Role.ToString()
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"          // ← this must match how you set it
            });
            return NoContent();
        }

        private void SetJwtCookie(User user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_jwtSecret);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("role", user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };
            var jwt = new JsonWebTokenHandler().CreateToken(descriptor);
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = descriptor.Expires
            });
        }
    }
}
