using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NStudents.Data;
using NStudents.Models.Dto;
using NStudents.Models.DTO;
using NStudents.Models.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Kiểm tra username đã tồn tại chưa
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Tài khoản đã tồn tại.");

            // Tạo Student mới với đầy đủ thông tin
            var student = new Students
            {
                HoTen = dto.HoTen,
                NgaySinh = dto.NgaySinh,
                GioiTinh = dto.GioiTinh,
                Email = dto.Email,
                SoDienThoai = dto.SoDienThoai,
                DiaChi = dto.DiaChi,
                NgayNhapHoc = dto.NgayNhapHoc,
                ClassesId = dto.ClassesId,
                TrangThai = "Đang học"
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Hash password
            string hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Tạo User liên kết với Student vừa tạo
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hashed,
                Role = "Student",
                StudentId = student.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Đăng ký thành công",
                StudentId = student.Id,
                Username = user.Username
            });
        }



        [HttpPost("registerExistedStudent")]
        public async Task<IActionResult> RegisterUserForStudent(RegisterUserForExistingStudentDto dto)
        {
            // kiểm tra student tồn tại
            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
                return NotFound("Sinh viên không tồn tại.");

            // kiểm tra sinh viên đã có user chưa
            if (await _context.Users.AnyAsync(u => u.StudentId == dto.StudentId))
                return BadRequest("Sinh viên này đã có tài khoản.");

            // kiểm tra username trùng
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Tên tài khoản đã tồn tại.");

            // hash password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

   
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                Role = "Student",
                StudentId = dto.StudentId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Tạo tài khoản cho sinh viên thành công.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Sai tài khoản hoặc mật khẩu.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("StudentId", user.StudentId?.ToString() ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = jwt });
        }
    }
}
