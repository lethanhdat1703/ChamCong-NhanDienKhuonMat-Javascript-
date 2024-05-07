using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public enum GenDer
    {
        Male,
        Female,
        Other
    }
    [Table("Employee")]
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public GenDer Gender { get; set; } 
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Base_Img { get; set; } = null!;
        public virtual ICollection<FaceModels>? FaceModels { get; set; }
        public virtual ICollection<TimeKeeping>? TimeKeepings { get; set; }
    }
}
