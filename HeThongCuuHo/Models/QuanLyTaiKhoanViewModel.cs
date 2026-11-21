
namespace HeThongCuuHo.Models
{
    public class QuanLyTaiKhoanViewModel
    {
        public int MaTK { get; set; }

        public string TenDangNhap { get; set; } = string.Empty;

        public string VaiTro { get; set; } = string.Empty;

        public bool TrangThai { get; set; }

        public string? HoTen { get; set; }

        public string? SoDienThoai { get; set; }

        public string? Email { get; set; }

        public string? DiaChi { get; set; }

        public string? TenCuaHang { get; set; }
    }
}
