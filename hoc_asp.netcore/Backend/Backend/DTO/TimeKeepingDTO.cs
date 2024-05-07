using Backend.Models;

namespace Backend.DTO
{
    public class TimeKeepingDTO
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? Status { get; set; } 
        public int? EmployeeId { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
