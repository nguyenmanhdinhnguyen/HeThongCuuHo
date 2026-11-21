
using HeThongCuuHo.Data;
using HeThongCuuHo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeThongCuuHo.Controllers
{
    [Authorize(Roles = "KhachHang")]
    public class KhachHangController : Controller
    {
        private readonly DBHeThongCuuHoContext _context;

        public KhachHangController(DBHeThongCuuHoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var maTKClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(maTKClaim, out int maTK))
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var khachHang = await _context.KhachHang
                .FirstOrDefaultAsync(kh => kh.MaTK == maTK);

            if (khachHang == null)
            {
                return NotFound("Không tìm thấy thông tin khách hàng.");
            }

            var danhSachXe = await (
                from pt in _context.PhuongTien
                join loai in _context.LoaiPhuongTien
                    on pt.MaLoaiPT equals loai.MaLoaiPT
                where pt.MaKH == khachHang.MaKH
                select new PhuongTienDashboardItem
                {
                    MaPT = pt.MaPT,
                    BienSo = pt.BienSo,
                    HangXe = pt.HangXe,
                    DongXe = pt.DongXe,
                    TenLoai = loai.TenLoai,
                    TrangThai = pt.TrangThai
                }
            ).ToListAsync();

            var model = new KhachHangDashboardViewModel
            {
                HoTen = khachHang.HoTen,
                SoDienThoai = khachHang.SoDienThoai,
                Email = khachHang.Email,
                TongPhuongTien = danhSachXe.Count,
                DanhSachPhuongTien = danhSachXe
            };

            return View(model);
        }
    }
}
