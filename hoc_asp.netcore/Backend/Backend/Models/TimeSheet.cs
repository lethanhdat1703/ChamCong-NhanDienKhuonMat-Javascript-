using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("TimeSheet")]
    public class TimeSheet
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set;}
        public TimeOnly LunchBreak { get; set; }
    }
}
