namespace NStudents.Models.Dto
{
    public class RegisterUserForExistingStudentDto
    {
        public int StudentId { get; set; } 
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
