using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devops_course_backend.dto;


namespace devops_course_backend.dal
{
    public class UserContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationPair> LocationPairs { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Define one-to-many relationship: Customer has many LocationPairs
            modelBuilder.Entity<LocationPair>()
                .HasOne(lp => lp.Customer)
                .WithMany(c => c.LocationPairs)
                .HasForeignKey(lp => lp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Ensure SignInLocation and SignOutLocation reference Location table
            modelBuilder.Entity<LocationPair>()
                .HasOne(lp => lp.SignInLocation)
                .WithMany()
                .HasForeignKey("SignInLocationId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LocationPair>()
                .HasOne(lp => lp.SignOutLocation)
                .WithMany()
                .HasForeignKey("SignOutLocationId")
                .OnDelete(DeleteBehavior.SetNull);
        }

    }

}