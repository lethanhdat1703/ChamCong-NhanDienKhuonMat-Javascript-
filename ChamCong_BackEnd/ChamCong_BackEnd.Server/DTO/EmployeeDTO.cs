namespace ChamCong_BackEnd.Server.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Phone { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Base_Img { get; set; }
        public List<FaceModelDTO> Faces { get; set; }
        public List<TimeKeepingDTO> TimeKeeping { get; set; }
    }
}
