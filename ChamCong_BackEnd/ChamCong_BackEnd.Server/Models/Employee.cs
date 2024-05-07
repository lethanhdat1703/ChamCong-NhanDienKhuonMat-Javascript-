using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChamCong_BackEnd.Server.Models
{
    [Table("Employee")]
    public class Employee
    {
 
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public bool? Gender { get; set; }
        public int Phone { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? Base_Img { get; set; }

        public virtual ICollection<FaceModel> FaceModels { get; set; }
        public virtual ICollection<Timekeeping> Timekeepings { get; set; }
    }
}
