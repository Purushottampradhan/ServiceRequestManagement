using System.ComponentModel.DataAnnotations;

namespace ServiceRequestAPI.DTOs
{
    public class CreateServiceRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }
    }
}