using FranDanBackend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FranDanBackend
{
    public class JWTGenerator
    {
        private readonly IConfiguration _configuration;
        public JWTGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJWTToken(User user)
        {
            var secretKeyString = _configuration["JwtSecretKey"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Email, user.email.Address),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };
            var token = new JwtSecurityToken(
            issuer: "FranDanBackend",
            audience: "AppUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public static string createHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool verifyHash(string passwordToVerify, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(passwordToVerify, hash);
        }
    }
}
