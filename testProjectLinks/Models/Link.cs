using System.ComponentModel.DataAnnotations;

namespace testProjectLinks.Models
{
    public class Link
    {
        public int Id { get; set; }
        public int LinkVisits { get; set; }
        public DateTime CreationTime { get; set; }

        [Required]
        public string? LinkStr { get; set; }
        public string? NewLinkStr { get; set; }

    }
}
