using System.ComponentModel.DataAnnotations;

namespace testProjectLinks.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Login {  get; set; }
        public string? Password { get; set; }
        
    }
}
