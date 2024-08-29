using System.ComponentModel.DataAnnotations;
namespace testProjectLinks.Models
{
    public class Link
    {
        public int Id { get; set; }
        public int LinkVisits { get; set; } = 0;
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string? LinkStr { get; set; }
        public string? NewLinkStr { get; set; }
    }
}
