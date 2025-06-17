using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace petmanagement.Models
{
    public class Pet
    {
        [Key]
        public int PetID { get; set; }

        [Required(ErrorMessage = "Pet name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Breed is required")]
        [StringLength(50, ErrorMessage = "Breed cannot exceed 50 characters")]
        public string Breed { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required")]
        [Range(0, 30, ErrorMessage = "Age must be between 0 and 30 years")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public bool IsAdopted { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
    }
}