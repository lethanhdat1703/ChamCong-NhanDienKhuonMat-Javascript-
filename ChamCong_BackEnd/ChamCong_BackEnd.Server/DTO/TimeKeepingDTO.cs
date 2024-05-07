namespace ChamCong_BackEnd.Server.DTO
{
    public class TimeKeepingDTO
    {
        public int Id { get; set; }
        public DateTime CheckIin { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; }
        public int? EmployeeId { get; set; }
    }
}
