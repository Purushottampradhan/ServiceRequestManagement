using System.ComponentModel.DataAnnotations;

namespace ServiceRequestAPI.DTOs
{
    public class UpdateServiceRequestDto
    {
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}