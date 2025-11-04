using Microsoft.EntityFrameworkCore;
using ServiceRequestAPI.Models;

namespace ServiceRequestAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add any additional configurations here
            modelBuilder.Entity<ServiceRequest>()
                .HasIndex(s => s.Status);
            
            modelBuilder.Entity<ServiceRequest>()
                .HasIndex(s => s.CreatedDate);
        }
    }
}