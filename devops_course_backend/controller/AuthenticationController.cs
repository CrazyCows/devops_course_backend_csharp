using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace devops_course_backend.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();

            if (!authenticateResult.Succeeded)
                return BadRequest("Authentication failed.");

            var claims = authenticateResult.Principal?.Identities.FirstOrDefault()?.Claims;
            var userName = claims?.FirstOrDefault(c => c.Type == "name")?.Value;

            return Ok(new { User = userName });
        }
    }
}
