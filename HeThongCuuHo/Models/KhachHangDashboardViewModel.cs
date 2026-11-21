
namespace HeThongCuuHo.Models
{
    public class KhachHangDashboardViewModel
    {
        public string HoTen { get; set; } = string.Empty;

        public string SoDienThoai { get; set; } = string.Empty;

        public string? Email { get; set; }

        public int TongPhuongTien { get; set; }

        public List<PhuongTienDashboardItem> DanhSachPhuongTien { get; set; }
            = new List<PhuongTienDashboardItem>();
    }

    public class PhuongTienDashboardItem
    {
        public int MaPT { get; set; }

        public string BienSo { get; set; } = string.Empty;

        public string? HangXe { get; set; }

        public string? DongXe { get; set; }

        public string TenLoai { get; set; } = string.Empty;

        public string TrangThai { get; set; } = "Sẵn sàng";
    }
}
