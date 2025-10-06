using NStudents.Data;
using NStudents.Models.Entity;
using NStudents.Repository.Interface;

namespace NStudents.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IStudentRepository Students { get; private set; }
        public IGenericRepository<Classes> Classes { get; private set; }
        public IGenericRepository<Majors> Majors { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Students = new StudentRepository(_context);
            Classes = new GenericRepository<Classes>(_context);
            Majors = new GenericRepository<Majors>(_context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
