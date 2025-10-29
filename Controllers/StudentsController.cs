using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NStudents.Models.DTO;
using NStudents.Models.Entity;
using NStudents.Repository.Interface;

namespace NStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (!_cache.TryGetValue("students_all", out IEnumerable<Students> students))
            {
                students = await _unitOfWork.Students.getAllStudent();

                foreach (var student in students)
                {
                    if (student.NgayTotNghiep != null && student.NgayTotNghiep < DateTime.UtcNow)
                        student.TrangThai = "Đã tốt nghiệp";
                    else
                        student.TrangThai = "Chưa tốt nghiệp";
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _cache.Set("students_all", students, cacheOptions);
            }

            var result = _mapper.Map<IEnumerable<StudentDto>>(students);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string cacheKey = $"student_{id}";

            if (!_cache.TryGetValue(cacheKey, out Students student))
            {
                student = await _unitOfWork.Students.getAllStudent(id);
                if (student == null)
                    return NotFound();

                _cache.Set(cacheKey, student, TimeSpan.FromMinutes(10));
            }

            var result = _mapper.Map<StudentDto>(student);
            return Ok(result);
        }

        [HttpPost]
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
    }
}
