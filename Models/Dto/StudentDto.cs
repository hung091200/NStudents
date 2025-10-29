namespace NStudents.Models.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int ClassesId { get; set; }
        public string ClassName { get; set; } = null!;
        public string MajorName { get; set; } = null!;
        public DateTime NgayNhapHoc { get; internal set; }
        public DateTime? NgayTotNghiep { get; set; }
        public string TrangThai { get; internal set; } = null!;
        public string DiaChi { get; internal set; } = null!;
        public string SoDienThoai { get; internal set; } = null!;
        public string GioiTinh { get; internal set; } = null!;
        public DateTime NgaySinh { get; internal set; } 
    }

    public class StudentCreateDto
    {
        public string HoTen { get; set; } = null!;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public DateTime NgayNhapHoc { get; set; }
        public DateTime? NgayTotNghiep { get; set; }
        public string TrangThai { get; set; } = null!;
        public int ClassesId { get; set; }
    }

    public class StudentUpdateDto : StudentCreateDto { }
}
