using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChamCong_BackEnd.Server.Models
{
    [Table("FaceModel")]
    public class FaceModel
    {
        public int Id { get; set; }
        public string? Img { get; set; }
        public int? EmployeeId { get; set; }
    }
}
