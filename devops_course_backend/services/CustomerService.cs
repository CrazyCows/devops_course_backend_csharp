using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using devops_course_backend.dto;
using Microsoft.AspNetCore.Http;

namespace devops_course_backend.services
{
    public class CustomerService(IHttpContextAccessor contextAccessor)
    {

        public Customer CreateCustomerFromAuthRequest()
        {
            var userId = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new ArgumentException("Customer email is required.");
            }

            Customer customer = new Customer
            {
                Email = userEmail,
                Name = userName,
                CreatedAt = DateTime.UtcNow
            };
            return customer;
        }
    }
}
