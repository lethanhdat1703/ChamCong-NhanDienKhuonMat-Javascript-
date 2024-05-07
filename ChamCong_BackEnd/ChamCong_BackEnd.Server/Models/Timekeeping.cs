using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChamCong_BackEnd.Server.Models
{
    [Table("TimeKeeping")]
    public class Timekeeping
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; }
        public int EmployeeId { get; set; }

    }

}
