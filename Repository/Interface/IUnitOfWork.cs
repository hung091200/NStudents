using NStudents.Models.Entity;
using NStudents.Repository.Interface;

namespace NStudents.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        IGenericRepository<Classes> Classes { get; }
        IGenericRepository<Majors> Majors { get; }
        Task<int> SaveAsync();
    }
}
