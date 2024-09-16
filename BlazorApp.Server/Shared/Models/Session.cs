using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        [Required]
        [StringLength(30)]
        public string? Credential { get; set; }

        [Required]
        public string CookieString { get; set; }

        [Required]
        public DateTimeOffset? ExpireTime { get; set; }
    }
}
