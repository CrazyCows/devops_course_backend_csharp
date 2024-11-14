using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devops_course_backend.dto
{
    public class Location
    {
        private DateTime createdAt = DateTime.UtcNow;

        [Key]
        public int? Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }

        public override string ToString()
        {
            return $"Location(Id: {Id}, Latitude: {Latitude}, Longitude: {Longitude}, CreatedAt: {CreatedAt})";
        }
    }

    public class LocationPair
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public required Location SignInLocation { get; set; }

        public Location? SignOutLocation { get; set; }
        public DateTime SignInTime { get; set; }
        public DateTime? SignOutTime { get; set; }

        public int CustomerId { get; set; }
        [Required]
        public required Customer Customer { get; set; }

        public override string ToString()
        {
            return $"LocationPair(Id: {Id}, SignInLocation: {SignInLocation}, SignOutLocation: {SignOutLocation}, " +
                   $"SignInTime: {SignInTime}, SignOutTime: {SignOutTime}, CustomerId: {CustomerId}, Customer: {Customer})";
        }
    }

}
