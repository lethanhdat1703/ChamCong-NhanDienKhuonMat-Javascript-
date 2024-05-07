namespace Backend.Models
{
    public enum TimeKeepingStatus
    {
        Absent,
        ShortTime,
    }
    public class TimeKeeping
    {

        public int Id { get; set; }
        public DateTime CheckIn { get; set; } = DateTime.Now;
        public DateTime? CheckOut { get; set; } = DateTime.Now;
        public TimeKeepingStatus Status { get; set; }
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
      
    }
}
