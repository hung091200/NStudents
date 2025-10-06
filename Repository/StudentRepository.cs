using Microsoft.EntityFrameworkCore;
using NStudents.Data;
using NStudents.Models.Entity;
using NStudents.Repository.Interface;

namespace NStudents.Repository
{
    public class StudentRepository : GenericRepository<Students>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Students>> getAllStudent()
        {
            return await _context.Students
                .Include(s => s.Classes)
                .ThenInclude(c => c.majors)
                .ToListAsync();
        }

        public async Task<Students?> GetStudentWithClassAndMajorAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Classes)
                .ThenInclude(c => c.majors)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Implement missing interface member
        public async Task<Students?> getAllStudent(int id)
        {
            return await _context.Students
                .Include(s => s.Classes)
                .ThenInclude(c => c.majors)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
