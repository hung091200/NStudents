using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NStudents.Data;
using NStudents.Models.DTO;
using NStudents.Models.Entity;

namespace NStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MajorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MajorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MajorDto>>> GetMajors()
        {
            return await _context.Majors
                .Select(m => new MajorDto
                {
                    Id = m.Id,
                    MajorName = m.MajorName
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MajorDto>> GetMajor(int id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null) return NotFound();

            return new MajorDto { Id = major.Id, MajorName = major.MajorName };
        }

        [HttpPost]
        public async Task<ActionResult<MajorDto>> CreateMajor(MajorCreateDto dto)
        {
            var major = new Majors { MajorName = dto.MajorName };
            _context.Majors.Add(major);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMajor), new { id = major.Id },
                new MajorDto { Id = major.Id, MajorName = major.MajorName });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMajor(int id, MajorUpdateDto dto)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null) return NotFound();

            major.MajorName = dto.MajorName;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMajor(int id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null) return NotFound();

            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
