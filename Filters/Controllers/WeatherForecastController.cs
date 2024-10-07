using Filters.ExceptionsMiddleware.Models;
using Filters.Service.FiltersService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Filters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [MySampleActionFilter("Controller")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
       

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpContext = httpContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
        [MySampleAsyncActionFilter("Action")]
        [MySampleActionFilter("Action")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Index")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new UnauthorizeException("You are not authorized to access this resource.", new Dictionary<string, string[]>());
            }

            return Ok("You are authorized to access this resource.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (email == "admin@gmail.com" && password == "Admin")
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim("Email", email),
                    new Claim("Password", password)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principle = new ClaimsPrincipal(identity);
                await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

                return Ok(new { message = "Login successful" }); ;
            }
            else
            {
                throw new Exception("email or password is wrong");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new { message = "Logout successful" });
        }

    }
}
