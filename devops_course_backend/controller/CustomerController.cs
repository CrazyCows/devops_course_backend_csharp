using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using devops_course_backend.dal;
using devops_course_backend.dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using devops_course_backend.services;

namespace devops_course_backend.controller
{


    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController(CustomerService customerService, UserContext context) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateCustomer()
        {
            Customer customer = customerService.CreateCustomerFromAuthRequest();

            var CustomerExists = await context.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);

            if (CustomerExists == null)
            {
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateCustomer), new { email = customer.Email }, customer);
            }
            return Ok( new { message = "Customer already exists" } );


        }
    }

}
