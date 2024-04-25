using System.ComponentModel.DataAnnotations;

namespace Dean_Resume.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(300)]
        [EmailAddress]
        [Required(ErrorMessage = "Email required ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required ")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
