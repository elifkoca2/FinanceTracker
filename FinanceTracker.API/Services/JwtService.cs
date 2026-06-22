using FinanceTracker.API.Models;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceTracker.API.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(ApplicationUser user)
        {
            //Ayarları okuma 
            var secretKey = _configuration["Jwt:SecretKey"]!;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            //Dijital imza 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Claims 
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.GivenName, user.FirstName)
        };

            //Token nesnesini oluşturup stringe çevirme 
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: GetExpirationDate(),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public DateTime GetExpirationDate()
        {
            var minutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"]!);
            return DateTime.UtcNow.AddMinutes(minutes);
        }
    }
}
