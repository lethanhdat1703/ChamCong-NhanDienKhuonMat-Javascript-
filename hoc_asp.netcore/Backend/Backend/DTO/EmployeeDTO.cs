namespace Backend.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Base_Img { get; set; } = null!;

        public ICollection<FaceModelsDTO>? FaceModels { get; set; }
        public ICollection<TimeKeepingDTO>? TimeKeepings { get; set; }
    }
}
