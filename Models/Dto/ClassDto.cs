namespace NStudents.Models.DTO
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = null!;
        public int MajorsId { get; set; }
        public string MajorName { get; set; } = null!;
    }

    public class ClassCreateDto
    {
        public string ClassName { get; set; } = null!;
        public int MajorsId { get; set; }
    }

    public class ClassUpdateDto
    {
        public string ClassName { get; set; } = null!;
        public int MajorsId { get; set; }
    }
}
