using NStudents.Models.Entity;

namespace NStudents.Repository.Interface
{
    public interface IStudentRepository : IGenericRepository<Students>
    {
        Task<IEnumerable<Students>> GetStudentsWithClassAndMajorAsync();
        Task<Students?> GetStudentWithClassAndMajorAsync(int id);
    }
}
