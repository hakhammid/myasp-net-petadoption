using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace petmanagement.Models
{
    public class AdoptionRequest
    {
        [Key]
        public int RequestID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int PetID { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        public string Message { get; set; } = string.Empty;

        public DateTime DateRequested { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("PetID")]
        public virtual Pet Pet { get; set; } = null!;
    }
}