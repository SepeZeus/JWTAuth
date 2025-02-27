using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            // Tarkista tässä credentials-olion arvot, esimerkiksi tietokantahakujen kautta
            if (credentials.Username == "testuser" && credentials.Password == "testpassword")
            {
                // Jos tunnistetiedot ovat oikein, generoi JWT-token ja palauta se
                var tokenService = new JWTTokenGenerator(); // Oletetaan, että TokenService on luokka, joka generoi tokenin
                var token = tokenService.GenerateToken(credentials.Username, false);
                return Ok(new { Token = token });
            }
            else if (credentials.Username == "admin" && credentials.Password == "adminpassword")
            {
                // Jos tunnistetiedot ovat oikein, generoi JWT-token ja palauta se
                var tokenService = new JWTTokenGenerator(); // Oletetaan, että TokenService on luokka, joka generoi tokenin
                var token = tokenService.GenerateToken(credentials.Username, true);
                return Ok(new { Token = token });
            }

            else
            {
                // Jos tunnistetiedot ovat väärin, palauta virheilmoitus
                return Unauthorized("Käyttäjätunnus tai salasana on väärin.");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetSecret")]
        public IActionResult GetSecretData()
        {
            return Ok("Hehehe super secret data");
        }
    }
}
