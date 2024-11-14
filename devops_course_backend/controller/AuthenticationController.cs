using devops_course_backend.dal;
using devops_course_backend.dto;
using devops_course_backend.services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace devops_course_backend.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthenticationController(CustomerService customerService, UserContext context) : Controller
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
        [HttpDelete]
        public IActionResult DeleteAccount()
        {
            Customer customer = customerService.CreateCustomerFromAuthRequest();
            var customerToRemove = context.Customers.FirstOrDefault(c => c.Email == customer.Email);
            if (customerToRemove == null)
            {
                return BadRequest("Customer does not exists");
            }
            context.Customers.Remove(customerToRemove);
            context.SaveChanges();
            return Ok(new { message = "Customer has been removed succesfully " });
        }
    }
}
