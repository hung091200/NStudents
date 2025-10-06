using Microsoft.AspNetCore.Mvc;
using NStudents.Models.DTO;
using NStudents.Models.Entity;
using NStudents.Repository;
using NStudents.Repository.Interface;

namespace NStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _unitOfWork.Students.getAllStudent();

            var result = students.Select(s => new StudentDto
            {
                Id = s.Id,
                HoTen = s.HoTen,
                NgaySinh = s.NgaySinh,
                GioiTinh = s.GioiTinh,
                Email = s.Email,
                SoDienThoai = s.SoDienThoai,
                DiaChi = s.DiaChi,
                NgayNhapHoc = s.NgayNhapHoc,
                TrangThai = s.TrangThai,
                ClassesId = s.ClassesId,
                ClassName = s.Classes?.ClassName ?? string.Empty,
                MajorName = s.Classes?.majors?.MajorName ?? string.Empty
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _unitOfWork.Students.getAllStudent(id);
            if (student == null) return NotFound();

            return new StudentDto
            {
                Id = student.Id,
                HoTen = student.HoTen,
                NgaySinh = student.NgaySinh,
                GioiTinh = student.GioiTinh,
                Email = student.Email,
                SoDienThoai = student.SoDienThoai,
                DiaChi = student.DiaChi,
                NgayNhapHoc = student.NgayNhapHoc,
                TrangThai = student.TrangThai,
                ClassesId = student.ClassesId,
                ClassName = student.Classes?.ClassName ?? string.Empty,
                MajorName = student.Classes?.majors?.MajorName ?? string.Empty
            };
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> CreateStudent(StudentCreateDto dto)
        {
            var student = new Students
            {
                HoTen = dto.HoTen,
                NgaySinh = dto.NgaySinh,
                GioiTinh = dto.GioiTinh,
                Email = dto.Email,
                SoDienThoai = dto.SoDienThoai,
                DiaChi = dto.DiaChi,
                NgayNhapHoc = dto.NgayNhapHoc,
                TrangThai = dto.TrangThai,
                ClassesId = dto.ClassesId
            };

            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveAsync();

            var newStudent = await _unitOfWork.Students.getAllStudent(student.Id);
            if (newStudent == null) return NotFound();

            return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, new StudentDto
            {
                Id = newStudent.Id,
                HoTen = newStudent.HoTen,
                NgaySinh = newStudent.NgaySinh,
                GioiTinh = newStudent.GioiTinh,
                Email = newStudent.Email,
                SoDienThoai = newStudent.SoDienThoai,
                DiaChi = newStudent.DiaChi,
                NgayNhapHoc = newStudent.NgayNhapHoc,
                TrangThai = newStudent.TrangThai,
                ClassName = newStudent.Classes?.ClassName ?? string.Empty,
                MajorName = newStudent.Classes?.majors?.MajorName ?? string.Empty
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentUpdateDto dto)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return NotFound();

            student.HoTen = dto.HoTen;
            student.NgaySinh = dto.NgaySinh;
            student.GioiTinh = dto.GioiTinh;
            student.Email = dto.Email;
            student.SoDienThoai = dto.SoDienThoai;
            student.DiaChi = dto.DiaChi;
            student.NgayNhapHoc = dto.NgayNhapHoc;
            student.TrangThai = dto.TrangThai;
            student.ClassesId = dto.ClassesId;

            _unitOfWork.Students.Update(student);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return NotFound();

            _unitOfWork.Students.Delete(student);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
