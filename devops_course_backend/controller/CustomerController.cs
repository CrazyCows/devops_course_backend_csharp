using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using devops_course_backend.dal;
using devops_course_backend.dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace devops_course_backend.controller
{


    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController(UserContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateCustomer()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail == null)
            {
                return BadRequest("Customer data is null.");
            }

            Customer customer = new Customer
            { 
                Email = userEmail, 
                Name = userEmail, 
                CreatedAt = DateTime.UtcNow 
            };
            customer.CreatedAt = DateTime.UtcNow;

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCustomer), new { email = customer.Email }, customer);
        }
    }

}
