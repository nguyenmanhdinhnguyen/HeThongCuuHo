using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HeThongCuuHo.Data;

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
    }
}