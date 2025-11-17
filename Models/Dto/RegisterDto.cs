namespace NStudents.Models.DTO
{
    public class RegisterDto
    {
        public string HoTen { get; set; } = null!;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public DateTime NgayNhapHoc { get; set; }
        public int ClassesId { get; set; }

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
