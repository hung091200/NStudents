using System.ComponentModel.DataAnnotations;

namespace NStudents.Models.Entity
{
    public class Students
    {
        public int Id { get; set; }
        public string HoTen { get; set; } = null!;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public DateTime NgayNhapHoc { get; set; }
        public DateTime? NgayTotNghiep { get; set; }

        public string TrangThai { get; set; } = null!;

        // FK đến Classes
        public int ClassesId { get; set; }
        public Classes? Classes { get; set; }
    }
}
