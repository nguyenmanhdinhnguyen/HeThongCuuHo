
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HeThongCuuHo.Data;
using HeThongCuuHo.Models;

namespace HeThongCuuHo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuanLyTaiKhoanController : Controller
    {
        private readonly DBHeThongCuuHoContext _context;

        public QuanLyTaiKhoanController(DBHeThongCuuHoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? tuKhoa, string? vaiTro)
        {
        

            var thongKe = _context.TaiKhoan
                .Where(tk => tk.VaiTro == "KhachHang"
                          || tk.VaiTro == "ChuCuaHang");

            ViewBag.TongTaiKhoan = await thongKe.CountAsync();

            ViewBag.SoKhachHang = await thongKe
                .CountAsync(tk => tk.VaiTro == "KhachHang");

            ViewBag.SoChuCuaHang = await thongKe
                .CountAsync(tk => tk.VaiTro == "ChuCuaHang");

            ViewBag.DangHoatDong = await thongKe
                .CountAsync(tk => tk.TrangThai);

            ViewBag.DaKhoa = await thongKe
                .CountAsync(tk => !tk.TrangThai);

     
            var query =
                from tk in _context.TaiKhoan
                where tk.VaiTro == "KhachHang"
                   || tk.VaiTro == "ChuCuaHang"

                join kh in _context.KhachHang
                    on tk.MaTK equals kh.MaTK into khGroup

                from kh in khGroup.DefaultIfEmpty()

                join chu in _context.ChuCuaHang
                    on tk.MaTK equals chu.MaTK into chuGroup

                from chu in chuGroup.DefaultIfEmpty()

                join ch in _context.CuaHang
                    on (chu == null ? (int?)null : chu.MaCH)
                    equals (int?)ch.MaCH into chGroup

                from ch in chGroup.DefaultIfEmpty()

                select new QuanLyTaiKhoanViewModel
                {
                    MaTK = tk.MaTK,
                    TenDangNhap = tk.TenDangNhap,
                    VaiTro = tk.VaiTro,
                    TrangThai = tk.TrangThai,

                    HoTen = kh != null ? kh.HoTen : null,
                    SoDienThoai = kh != null ? kh.SoDienThoai : null,
                    Email = kh != null ? kh.Email : null,
                    DiaChi = kh != null ? kh.DiaChi : null,

                    TenCuaHang = ch != null ? ch.TenCH : null
                };

       
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();

                query = query.Where(tk =>
                    tk.TenDangNhap.Contains(tuKhoa)
                    || (tk.HoTen != null && tk.HoTen.Contains(tuKhoa))
                    || (tk.SoDienThoai != null && tk.SoDienThoai.Contains(tuKhoa))
                    || (tk.TenCuaHang != null && tk.TenCuaHang.Contains(tuKhoa))
                );
            }


            if (vaiTro == "KhachHang" || vaiTro == "ChuCuaHang")
            {
                query = query.Where(tk => tk.VaiTro == vaiTro);
            }

        

            var danhSach = await query
                .OrderBy(tk => tk.VaiTro)
                .ThenBy(tk => tk.MaTK)
                .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.VaiTro = vaiTro;

            return View(danhSach);
        }
    }
}
