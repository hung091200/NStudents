using NStudents.Models.Entity;

namespace NStudents.Repository.Interface
{
    public interface IStudentRepository : IGenericRepository<Students>
    {
        Task<IEnumerable<Students>> getAllStudent();
        Task<Students?> getAllStudent(int id);
    }
}
