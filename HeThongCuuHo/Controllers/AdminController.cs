
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HeThongCuuHo.Data;

namespace HeThongCuuHo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DBHeThongCuuHoContext _context;

        public AdminController(DBHeThongCuuHoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SoTaiKhoan = await _context.TaiKhoan.CountAsync();
            ViewBag.SoKhachHang = await _context.KhachHang.CountAsync();
            ViewBag.SoCuaHang = await _context.CuaHang.CountAsync();
            ViewBag.SoNhanVien = await _context.NhanVien.CountAsync();
            ViewBag.SoPhuongTien = await _context.PhuongTien.CountAsync();
            ViewBag.SoDichVu = await _context.DichVu.CountAsync();

            ViewBag.CuaHangs = await _context.CuaHang
                .Where(ch => ch.ViDo != null && ch.KinhDo != null)
                .Select(ch => new
                {
                    tenCH = ch.TenCH,
                    diaChi = ch.DiaChi,
                    soDienThoai = ch.SoDienThoai,
                    viDo = ch.ViDo,
                    kinhDo = ch.KinhDo
                })
                .ToListAsync();

            return View();
        }
    }
}
