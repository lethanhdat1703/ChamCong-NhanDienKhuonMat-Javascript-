namespace ChamCong_BackEnd.Server.DTO
{
    public class TimeDTO
    {

        public int Id { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public TimeOnly? LunchBreak { get; set; }
    }
}
