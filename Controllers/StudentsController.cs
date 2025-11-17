using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NStudents.Models.DTO;
using NStudents.Models.Entity;
using NStudents.Repository.Interface;
using System.Security.Claims;

namespace NStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public StudentsController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Student")
            {
                // Lấy StudentId từ token
                var studentIdStr = User.FindFirstValue("StudentId");
                if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

                int studentId = int.Parse(studentIdStr);
                var student = await _unitOfWork.Students.getAllStudent(studentId);
                if (student == null) return NotFound();

                var result = _mapper.Map<StudentDto>(student);
                return Ok(result);
            }
            else if (role == "Admin")
            {
                // Admin xem tất cả
                if (!_cache.TryGetValue("students_all", out IEnumerable<Students> students))
                {
                    students = await _unitOfWork.Students.getAllStudent();
                    foreach (var student in students)
                    {
                        student.TrangThai = (student.NgayTotNghiep != null && student.NgayTotNghiep < DateTime.UtcNow)
                            ? "Đã tốt nghiệp"
                            : "Chưa tốt nghiệp";
                    }
                    _cache.Set("students_all", students, TimeSpan.FromMinutes(10));
                }

                var result = _mapper.Map<IEnumerable<StudentDto>>(students);
                return Ok(result);
            }
            return Forbid();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Student")
            {
                var studentIdStr = User.FindFirstValue("StudentId");
                if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

                int studentId = int.Parse(studentIdStr);
                if (id != studentId) return Forbid(); // không được xem student khác
            }

            string cacheKey = $"student_{id}";
            if (!_cache.TryGetValue(cacheKey, out Students student))
            {
                student = await _unitOfWork.Students.getAllStudent(id);
                if (student == null) return NotFound();

                _cache.Set(cacheKey, student, TimeSpan.FromMinutes(10));
            }

            var result = _mapper.Map<StudentDto>(student);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StudentCreateDto dto)
        {
            var entity = _mapper.Map<Students>(dto);
            await _unitOfWork.Students.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            _cache.Remove("students_all");

            var savedStudent = await _unitOfWork.Students.getAllStudent(entity.Id);
            var result = _mapper.Map<StudentDto>(savedStudent);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, StudentUpdateDto dto)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            _mapper.Map(dto, student);
            _unitOfWork.Students.Update(student);
            await _unitOfWork.SaveAsync();

            _cache.Remove("students_all");
            _cache.Remove($"student_{id}");

            var result = _mapper.Map<StudentDto>(student);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            _unitOfWork.Students.Delete(student);
            await _unitOfWork.SaveAsync();

            _cache.Remove("students_all");
            _cache.Remove($"student_{id}");

            return NoContent();
        }

        [HttpGet("me")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyInfo()
        {
            var studentIdStr = User.FindFirstValue("StudentId");
            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

            int studentId = int.Parse(studentIdStr);
            var student = await _unitOfWork.Students.getAllStudent(studentId);
            if (student == null) return NotFound();

            var result = _mapper.Map<StudentDto>(student);
            return Ok(result);
        }
    }
}
