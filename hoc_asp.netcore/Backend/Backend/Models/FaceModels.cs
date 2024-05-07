using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("FaceModels")]
    public class FaceModels
    {
        public int Id { get; set; }
        public string Img { get; set; } = null!;
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
    }
}
