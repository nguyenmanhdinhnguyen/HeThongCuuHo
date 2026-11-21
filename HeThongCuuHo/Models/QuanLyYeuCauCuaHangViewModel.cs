namespace HeThongCuuHo.Models
{
    public class QuanLyYeuCauCuaHangViewModel
    {
        public string TenCuaHang { get; set; } = "";

        public List<YeuCauCuuHoCuaHangItem> DanhSachCuuHo { get; set; } = new();

        public List<DonSuaChuaCuaHangItem> DanhSachSuaChua { get; set; } = new();
    }

    public class YeuCauCuuHoCuaHangItem
    {
        public int MaYC { get; set; }
        public string TenKhachHang { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public string BienSo { get; set; } = "";
        public string TenPhuongTien { get; set; } = "";
        public string DiaDiem { get; set; } = "";
        public string? MoTaSuCo { get; set; }
        public string? HinhThucHoTro { get; set; }
        public string? MucDoUuTien { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? ThoiGianYeuCau { get; set; }
    }

    public class DonSuaChuaCuaHangItem
    {
        public int MaDon { get; set; }
        public string TenKhachHang { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public string BienSo { get; set; } = "";
        public string TenPhuongTien { get; set; } = "";
        public DateTime? NgayTiepNhan { get; set; }
        public DateTime? NgayHoanThanh { get; set; }
        public decimal? TongTien { get; set; }
        public string? TrangThai { get; set; }
    }
}