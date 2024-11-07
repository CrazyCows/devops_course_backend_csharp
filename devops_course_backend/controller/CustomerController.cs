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
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly UserContext _context;

        public CustomerController(CustomerService customerService, UserContext context)
        {
            _customerService = customerService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer()
        {
            Customer customer = _customerService.CreateCustomerFromAuthRequest();

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCustomer), new { email = customer.Email }, customer);
        }
    }

}
