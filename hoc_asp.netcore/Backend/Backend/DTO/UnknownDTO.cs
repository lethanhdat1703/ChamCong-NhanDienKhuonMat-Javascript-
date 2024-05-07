namespace Backend.DTO
{
    public class UnknownDTO
    {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public DateTime CaptureTime { get; set; } = DateTime.Now;
    }
}
