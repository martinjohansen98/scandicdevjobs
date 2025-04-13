using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace ScandicDevJobApi.Services
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly int _expirationDays;

        public JwtService(string secret, int expirationDays = 7)
        {
            _secret = secret;
            _expirationDays = expirationDays;
        }

        public string GenerateToken(int userId, string email)
        {
            var key = Encoding.ASCII.GetBytes(_secret);
            var signingKey = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddDays(_expirationDays),
                SigningCredentials = signingCredentials
            };

            // Use JsonWebTokenHandler instead of JwtSecurityTokenHandler
            var tokenHandler = new JsonWebTokenHandler();
            var tokenString = tokenHandler.CreateToken(tokenDescriptor);
            return tokenString;
        }
    }
}