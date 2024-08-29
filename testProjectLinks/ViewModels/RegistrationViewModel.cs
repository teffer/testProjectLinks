using System.ComponentModel.DataAnnotations;

namespace testProjectLinks.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email {  get; set; }
        [Required]
        public string? Login {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password {  get; set; }
        [Compare("Password",ErrorMessage ="Пароли не совпадают.")]
        [DataType(DataType.Password)]
        public string? ConfirmedPassword {  get; set; }
    }
}
