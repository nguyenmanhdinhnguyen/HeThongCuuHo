
using Microsoft.EntityFrameworkCore;
using HeThongCuuHo.Models;

namespace HeThongCuuHo.Data
{
    public class DBHeThongCuuHoContext : DbContext
    {
        public DBHeThongCuuHoContext(DbContextOptions<DBHeThongCuuHoContext> options)
            : base(options)
        {
        }

        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<CuaHang> CuaHang { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<LoaiPhuongTien> LoaiPhuongTien { get; set; }
        public DbSet<PhuongTien> PhuongTien { get; set; }
        public DbSet<DichVu> DichVu { get; set; }
        public DbSet<TaiXeCuuHo> TaiXeCuuHo { get; set; }
        public DbSet<ViTriNhanVien> ViTriNhanVien { get; set; }
        public DbSet<YeuCauCuuHo> YeuCauCuuHo { get; set; }
        public DbSet<PhanCongCuuHo> PhanCongCuuHo { get; set; }
        public DbSet<DonSuaChua> DonSuaChua { get; set; }
        public DbSet<ChiTietSuaChua> ChiTietSuaChua { get; set; }
        public DbSet<LinhKien> LinhKien { get; set; }
        public DbSet<KhoLinhKien> KhoLinhKien { get; set; }
        public DbSet<SuDungLinhKien> SuDungLinhKien { get; set; }
        public DbSet<DonVanChuyen> DonVanChuyen { get; set; }
        public DbSet<LichSuTrangThai> LichSuTrangThai { get; set; }
        public DbSet<ThanhToan> ThanhToan { get; set; }
        public DbSet<DanhGia> DanhGia { get; set; }
    }
}
