namespace NStudents.Models.DTO
{
    public class MajorDto
    {
        public int Id { get; set; }
        public string MajorName { get; set; } = null!;
    }

    public class MajorCreateDto
    {
        public string MajorName { get; set; } = null!;
    }

    public class MajorUpdateDto
    {
        public string MajorName { get; set; } = null!;
    }
}
