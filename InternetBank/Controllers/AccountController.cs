using InternetBank.Models;
using InternetBank.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternetBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Services.AuthenticationService _authService;
        public AccountController(Services.AuthenticationService authService)
        {
            _authService = authService;
        }

        // GET: api/<ValuesController>
/*        [Route("/api/account/login/{person}")]
        [HttpGet]
        public async Task<bool> Get(Person person)
        {
            var result = await _authService.UserLoginAsync(person.Email, person.Password);
            return result.Success;
        }*/

        [Route("/api/account/login/{email}/{password}")]
        [HttpGet]
        public async Task<IActionResult> Get(string email, string password)
        {
            var result = await _authService.UserLoginAsync(email, password);
            return Redirect("/");
        }

        // POST api/<ValuesController>
        /*        [Route("/api/account/login")]
                [HttpPost]
                public async Task<bool> Post([FromBody] Person person)
                {
                    var result = await _authService.UserLoginAsync(person.Email, person.Password);
                    return result.Success;
                }*/
    }
}
