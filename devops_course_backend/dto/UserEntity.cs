using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace devops_course_backend.dto
{
    public class Customer
    {
        [Key]
        public int? Id { get; set; }
        public string? Name { get; set; }
        [Required]
        required public string Email { get; set; }
        public DateTime? CreatedAt { get; set; }

        public ICollection<LocationPair>? LocationPairs { get; set; }


        public override string ToString()
        {
            return $"Customer(Id: {Id}, Name: {Name}, Email: {Email}, CreatedAt: {CreatedAt}, " +
                   $"LocationPairsCount: {LocationPairs?.Count ?? 0})";
        }
    }
    
}
