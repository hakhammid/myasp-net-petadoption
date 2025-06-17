using System.ComponentModel.DataAnnotations;

namespace petmanagement.Models.ViewModels
{
    public class AdoptionRequestViewModel
    {
        [Required]
        public int PetID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        public string Message { get; set; } = string.Empty;

        // For display purposes
        public string? PetName { get; set; }
        public string? PetImagePath { get; set; }
    }
}