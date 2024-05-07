using System.ComponentModel.DataAnnotations.Schema;

namespace ChamCong_BackEnd.Server.Models
{
    [Table("Time")]
    public class Time
    {
        public int Id { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public TimeOnly? LunchBreak { get; set; }
    }
}
