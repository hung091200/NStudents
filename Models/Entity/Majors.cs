namespace NStudents.Models.Entity
{
    public class Majors
    {
        public int Id { get; set; }
        public string MajorName { get; set; }

        // 1 Major có nhiều Class
        public ICollection<Classes> Classes { get; set; } = new List<Classes>();
    }
}
