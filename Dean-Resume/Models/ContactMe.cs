using System.ComponentModel.DataAnnotations;

namespace Dean_Resume.Models
{
    public class ContactMe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [MaxLength(80)]
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
