using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NStudents.Data;
using NStudents.Models.Entity;
using NStudents.Models.DTO;

namespace NStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            return await _context.Classes
                .Include(c => c.majors)
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    MajorsId = c.MajorsId,
                    MajorName = c.majors.MajorName
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDto>> GetClass(int id)
        {
            var cls = await _context.Classes.Include(c => c.majors)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cls == null) return NotFound();

            return new ClassDto
            {
                Id = cls.Id,
                ClassName = cls.ClassName,
                MajorsId = cls.MajorsId,
                MajorName = cls.majors.MajorName
            };
        }

        [HttpPost]
        public async Task<ActionResult<ClassDto>> CreateClass(ClassCreateDto dto)
        {
            var cls = new Classes
            {
                ClassName = dto.ClassName,
                MajorsId = dto.MajorsId
            };

            _context.Classes.Add(cls);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClass), new { id = cls.Id },
                new ClassDto
                {
                    Id = cls.Id,
                    ClassName = cls.ClassName,
                    MajorsId = cls.MajorsId
                });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, ClassUpdateDto dto)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return NotFound();

            cls.ClassName = dto.ClassName;
            cls.MajorsId = dto.MajorsId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return NotFound();

            _context.Classes.Remove(cls);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
