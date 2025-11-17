using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NStudents.Data;
using NStudents.Models.Dto;
using NStudents.Models.DTO;
using NStudents.Models.Entity;

namespace NStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Thêm admin mới
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin(AdminCreateDto dto)
        {
            // Kiểm tra username đã tồn tại chưa
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Tên tài khoản đã tồn tại.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Admin",
                StudentId = null // admin không gắn với student
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tạo admin thành công",
                Username = user.Username
            });
        }

        // Lấy danh sách admin (quản lý)
        [HttpGet("list")]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _context.Users
                .Where(u => u.Role == "Admin")
                .Select(u => new { u.UserId, u.Username })
                .ToListAsync();

            return Ok(admins);
        }
    }
}
