namespace NStudents.Models.Entity
{
    public class Classes
    {
        public int Id { get; set; }
        public string ClassName { get; set; }

        // FK đến Khoa
        public int MajorsId { get; set; }
        public Majors majors { get; set; }

        // 1 lớp có nhiều sinh viên
        public ICollection<Students> Students{ get; set; }
    }
}
