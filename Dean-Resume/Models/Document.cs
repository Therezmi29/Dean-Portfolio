using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dean_Resume.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
    }
}
