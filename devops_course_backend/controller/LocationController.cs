using devops_course_backend.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using devops_course_backend.dal;
using System.Security.Claims;


namespace devops_course_backend.controller
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class LocationController(UserContext context) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> SignInOnLocation([FromBody] Location location)
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

            int CustomerId;
            if (location == null)
            {
                return BadRequest(new { error = "No customer or location is given" });
            }

            Customer? existingCustomer = await context.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);

            if (existingCustomer == null || existingCustomer.Id == null)
            {
                return NotFound(new { error = "Customer not found or ID is null." });
            }
            else
            {
                CustomerId = (int)existingCustomer.Id;
            }

            LocationPair? existingLocationPair = await context.LocationPairs
                .Where(c => c.CustomerId == CustomerId)
                .OrderByDescending(lp => lp.SignInTime)
                .Include(lp => lp.SignInLocation)
                .Include(lp => lp.SignOutLocation)
                .FirstOrDefaultAsync();

            if (existingLocationPair != null)
            {
                Console.Out.WriteLine(existingLocationPair.ToString());
            }
            if (existingLocationPair != null && existingLocationPair.SignOutLocation == null)
            {
                return BadRequest(new { error = "Customer has not signed out of previous location" });
            }

            LocationPair locationPair = new LocationPair
            {
                SignInLocation = location,
                SignInTime = DateTime.UtcNow,
                CustomerId = CustomerId, // Use existing customer ID
                Customer = existingCustomer // Ensure you are using the tracked customer
            };

            await context.LocationPairs.AddAsync(locationPair);
            await context.SaveChangesAsync();
            return Ok(new { message = "Customer sign in location has been saved" });
        }


        [HttpPost]
        public async Task<IActionResult> SignOutOnLocation([FromBody] Location location)
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

            if (location == null)
            {
                return BadRequest("No location is given");
            }

            var locationPair = await context.LocationPairs
                .Include(lp => lp.Customer)
                .FirstOrDefaultAsync(lp => lp.Customer.Email == customer.Email && lp.SignOutLocation == null);


            if (locationPair == null)
            {
                return BadRequest(new { error = "No sign in location found" });
            }
            locationPair.SignOutLocation = location;
            locationPair.SignOutTime = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return Ok(new { message = "Customer sign out lcation has been saved" });
        }
    }
}

