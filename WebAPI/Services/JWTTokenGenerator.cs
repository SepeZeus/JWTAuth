using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Services
{

    public class JWTTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JWTTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string username, bool isAdmin)
        {
            var secretKeyString = _configuration["JWTSecretKey"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var roleClaim = isAdmin ? new Claim(ClaimTypes.Role, "Admin") : new Claim(ClaimTypes.Role, "user");
            claims.Add(roleClaim);

            var tokeOptions = new JwtSecurityToken(
                issuer: "MyTestAuthServer",
                audience: "MyTestApiUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token vanhenee 30 minuutin kuluttua
                signingCredentials: signinCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
