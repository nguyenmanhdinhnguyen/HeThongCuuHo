using HeThongCuuHo.Data;
using HeThongCuuHo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeThongCuuHo.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly DBHeThongCuuHoContext _context;

        public TaiKhoanController(DBHeThongCuuHoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(
            string tenDangNhap, string matKhau)
        {
            var taiKhoan = await _context.TaiKhoan
                .FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap);

            if (taiKhoan == null ||
                taiKhoan.MatKhau != matKhau ||
                !taiKhoan.TrangThai)
            {
                ViewBag.Loi = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaTK.ToString()),
                new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap),
                new Claim(ClaimTypes.Role, taiKhoan.VaiTro)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("DangNhap");
        }

        [AllowAnonymous]
        public IActionResult TuChoi()
        {
            return Content("Bạn không có quyền truy cập chức năng này!");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(DangKyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool trungTen = await _context.TaiKhoan
                .AnyAsync(x => x.TenDangNhap == model.TenDangNhap);

            if (trungTen)
            {
                ModelState.AddModelError("TenDangNhap",
                    "Tên đăng nhập đã tồn tại!");
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var taiKhoan = new TaiKhoan
                {
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau,
                    VaiTro = "KhachHang",
                    TrangThai = true
                };

                _context.TaiKhoan.Add(taiKhoan);
                await _context.SaveChangesAsync();

                var khachHang = new KhachHang
                {
                    MaTK = taiKhoan.MaTK,
                    HoTen = model.HoTen,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email
                };

                _context.KhachHang.Add(khachHang);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction("DangNhap");
            }
            catch
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Đăng ký thất bại. Vui lòng thử lại!");
                return View(model);
            }
        }
    }
}

