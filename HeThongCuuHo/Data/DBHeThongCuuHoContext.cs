
using Microsoft.EntityFrameworkCore;
using HeThongCuuHo.Models;

namespace HeThongCuuHo.Data
{
    public class DBHeThongCuuHoContext : DbContext
    {
        public DBHeThongCuuHoContext(
            DbContextOptions<DBHeThongCuuHoContext> options)
            : base(options)
        {
        }

        public DbSet<DonSuaChua> DonSuaChua { get; set; }
        public DbSet<PhanCongCuuHo> PhanCongCuuHo { get; set; }
        public DbSet<ChuCuaHang> ChuCuaHang { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<CuaHang> CuaHang { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<LoaiPhuongTien> LoaiPhuongTien { get; set; }
        public DbSet<PhuongTien> PhuongTien { get; set; }
        public DbSet<DichVu> DichVu { get; set; }
        public DbSet<YeuCauCuuHo> YeuCauCuuHo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<YeuCauCuuHo>(entity =>
            {
                entity.ToTable("YeuCauCuuHo");

                entity.HasKey(e => e.MaYC);

                entity.Property(e => e.MaYC)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MaCH)
                    .IsRequired(false);

                entity.Property(e => e.ThoiGianYeuCau)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.MucDoUuTien)
                    .HasDefaultValue("Bình thường");

                entity.Property(e => e.TrangThai)
                    .HasDefaultValue("Chờ tiếp nhận");
            });
        }
    }
}
