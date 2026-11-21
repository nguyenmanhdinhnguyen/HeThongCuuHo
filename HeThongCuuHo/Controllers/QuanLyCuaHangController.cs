
using HeThongCuuHo.Data;
using HeThongCuuHo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeThongCuuHo.Controllers
{
    [Authorize(Roles = "ChuCuaHang")]
    public class QuanLyCuaHangController : Controller
    {
        private readonly DBHeThongCuuHoContext _context;

        public QuanLyCuaHangController(DBHeThongCuuHoContext context)
        {
            _context = context;
        }

        // TRANG CHỦ CHỦ CỬA HÀNG
        public async Task<IActionResult> Index()
        {
            var maTKClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(maTKClaim, out int maTK))
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var thongTin = await (
                from chu in _context.ChuCuaHang
                join cuaHang in _context.CuaHang
                    on chu.MaCH equals cuaHang.MaCH
                where chu.MaTK == maTK
                select new ChuCuaHangDashboardViewModel
                {
                    TenCuaHang = cuaHang.TenCH,
                    DiaChi = cuaHang.DiaChi,
                    SoDienThoai = cuaHang.SoDienThoai,
                    TrangThai = cuaHang.TrangThai
                }
            ).FirstOrDefaultAsync();

            if (thongTin == null)
            {
                return NotFound("Không tìm thấy cửa hàng của tài khoản này.");
            }

            return View(thongTin);
        }

        // QUẢN LÝ YÊU CẦU CỨU HỘ VÀ ĐƠN SỬA CHỮA
        public async Task<IActionResult> YeuCauCuuHo()
        {
            var maTKClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(maTKClaim, out int maTK))
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            // Lấy cửa hàng của tài khoản đang đăng nhập
            var chuCuaHang = await _context.ChuCuaHang
                .FirstOrDefaultAsync(x => x.MaTK == maTK);

            if (chuCuaHang == null)
            {
                return NotFound("Không tìm thấy thông tin chủ cửa hàng.");
            }

            int maCH = chuCuaHang.MaCH;

            // Lấy thông tin cửa hàng
            var cuaHang = await _context.CuaHang
                .FirstOrDefaultAsync(x => x.MaCH == maCH);

            if (cuaHang == null)
            {
                return NotFound("Không tìm thấy cửa hàng.");
            }

            var model = new QuanLyYeuCauCuaHangViewModel
            {
                TenCuaHang = cuaHang.TenCH
            };

            // ==========================================
            // 1. LẤY YÊU CẦU CỨU HỘ
            // CHỈ LẤY YÊU CẦU KHÁCH ĐÃ CHỌN CỬA HÀNG NÀY
            // ==========================================

            model.DanhSachCuuHo = await (
                from yc in _context.YeuCauCuuHo
                join kh in _context.KhachHang
                    on yc.MaKH equals kh.MaKH
                join pt in _context.PhuongTien
                    on yc.MaPT equals pt.MaPT
                join loai in _context.LoaiPhuongTien
                    on pt.MaLoaiPT equals loai.MaLoaiPT

                where yc.MaCH == maCH

                orderby yc.ThoiGianYeuCau descending

                select new YeuCauCuuHoCuaHangItem
                {
                    MaYC = yc.MaYC,
                    TenKhachHang = kh.HoTen,
                    SoDienThoai = kh.SoDienThoai,
                    BienSo = pt.BienSo,
                    TenPhuongTien = loai.TenLoai,
                    DiaDiem = yc.DiaDiem,
                    MoTaSuCo = yc.MoTaSuCo,
                    HinhThucHoTro = yc.HinhThucHoTro,
                    MucDoUuTien = yc.MucDoUuTien,
                    TrangThai = yc.TrangThai,
                    ThoiGianYeuCau = yc.ThoiGianYeuCau
                }
            ).ToListAsync();

            // ==========================================
            // 2. LẤY ĐƠN SỬA CHỮA
            // CHỈ LẤY ĐƠN THUỘC CỬA HÀNG ĐANG ĐĂNG NHẬP
            // ==========================================

            model.DanhSachSuaChua = await (
                from don in _context.DonSuaChua
                join kh in _context.KhachHang
                    on don.MaKH equals kh.MaKH
                join pt in _context.PhuongTien
                    on don.MaPT equals pt.MaPT
                join loai in _context.LoaiPhuongTien
                    on pt.MaLoaiPT equals loai.MaLoaiPT

                where don.MaCH == maCH

                orderby don.NgayTiepNhan descending

                select new DonSuaChuaCuaHangItem
                {
                    MaDon = don.MaDon,
                    TenKhachHang = kh.HoTen,
                    SoDienThoai = kh.SoDienThoai,
                    BienSo = pt.BienSo,
                    TenPhuongTien = loai.TenLoai,
                    NgayTiepNhan = don.NgayTiepNhan,
                    NgayHoanThanh = don.NgayHoanThanh,
                    TongTien = don.TongTien,
                    TrangThai = don.TrangThai
                }
            ).ToListAsync();

            return View(model);
        }
    }
}
