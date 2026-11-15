
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
    }
}
