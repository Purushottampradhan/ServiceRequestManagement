using System.ComponentModel.DataAnnotations;

namespace ServiceRequestAPI.Models
{
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Open"; // Open, In Progress, Closed

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        public string UpdatedBy { get; set; }
    }
}