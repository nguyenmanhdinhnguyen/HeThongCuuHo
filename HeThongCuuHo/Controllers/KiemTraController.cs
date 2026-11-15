
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeThongCuuHo.Data;

namespace HeThongCuuHo.Controllers
{
    public class KiemTraController : Controller
    {
        private readonly DBHeThongCuuHoContext _context;

        public KiemTraController(DBHeThongCuuHoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                bool ketNoi = await _context.Database.CanConnectAsync();

                if (ketNoi)
                {
                    int soTaiKhoan = await _context.TaiKhoan.CountAsync();

                    return Content(
                        $"Kết nối SQL Server thành công! Có {soTaiKhoan} tài khoản.");
                }

                return Content("Không thể kết nối SQL Server!");
            }
            catch (Exception ex)
            {
                return Content("Lỗi kết nối: " + ex.Message);
            }
        }
    }
}
