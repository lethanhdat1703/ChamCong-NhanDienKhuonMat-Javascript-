using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Unknown")]
    public class Unknown
    {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public DateTime CaptureTime { get; set; }
    }
}
