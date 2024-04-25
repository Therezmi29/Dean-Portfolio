namespace Dean_Resume.Models
{
    public class AddEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public IFormFile Picture { get; set; }
    }
}

