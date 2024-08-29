using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace testProjectLinks.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string? Login {  get; set; }
    }
}
