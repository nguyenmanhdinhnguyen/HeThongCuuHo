
using HeThongCuuHo.Data;
using HeThongCuuHo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace HeThongCuuHo.Controllers
{
    [Authorize(Roles = "KhachHang")]
    public class YeuCauCuuHoController : Controller
    {
        private readonly DBHeThongCuuHoContext _context;

        public YeuCauCuuHoController(DBHeThongCuuHoContext context)
        {
            _context = context;
        }

        private async Task<int?> LayMaKhachHang()
        {
            var maTKClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(maTKClaim, out int maTK))
                return null;

            var khachHang = await _context.KhachHang
                .AsNoTracking()
                .FirstOrDefaultAsync(kh => kh.MaTK == maTK);

            return khachHang?.MaKH;
        }

        private async Task NapDanhSachPhuongTien(
            TaoYeuCauCuuHoViewModel model, int maKH)
        {
            model.DanhSachPhuongTien = await (
                from pt in _context.PhuongTien
                join loai in _context.LoaiPhuongTien
                    on pt.MaLoaiPT equals loai.MaLoaiPT
                where pt.MaKH == maKH
                select new SelectListItem
                {
                    Value = pt.MaPT.ToString(),
                    Text = pt.BienSo + " - " +
                           (pt.HangXe ?? "") + " " +
                           (pt.DongXe ?? "") + " (" +
                           loai.TenLoai + ")"
                }
            ).ToListAsync();
        }

        // Tính khoảng cách giữa hai tọa độ bằng công thức Haversine
        private static double TinhKhoangCach(
            double viDo1, double kinhDo1,
            double viDo2, double kinhDo2)
        {
            const double banKinhTraiDat = 6371;

            double dViDo = (viDo2 - viDo1) * Math.PI / 180;
            double dKinhDo = (kinhDo2 - kinhDo1) * Math.PI / 180;

            double a =
                Math.Sin(dViDo / 2) * Math.Sin(dViDo / 2) +
                Math.Cos(viDo1 * Math.PI / 180) *
                Math.Cos(viDo2 * Math.PI / 180) *
                Math.Sin(dKinhDo / 2) * Math.Sin(dKinhDo / 2);

            double c = 2 * Math.Atan2(
                Math.Sqrt(a),
                Math.Sqrt(1 - a)
            );

            return banKinhTraiDat * c;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var maKH = await LayMaKhachHang();

            if (maKH == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var model = new TaoYeuCauCuuHoViewModel();

            await NapDanhSachPhuongTien(model, maKH.Value);

            if (model.DanhSachPhuongTien.Count == 0)
            {
                TempData["ThongBao"] =
                    "Bạn chưa có phương tiện nào. Vui lòng thêm phương tiện trước.";

                return RedirectToAction("Index", "KhachHang");
            }

            return View("~/Views/YeuCauCuuHo/Create.cshtml", model);
        }

        // API lấy danh sách cửa hàng gần vị trí khách hàng
        [HttpGet]
        public async Task<IActionResult> LayCuaHangGanDay(
            double viDo, double kinhDo)
        {
            if (viDo < -90 || viDo > 90 ||
                kinhDo < -180 || kinhDo > 180)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Tọa độ không hợp lệ."
                });
            }

            var danhSachCuaHang = await _context.CuaHang
                .AsNoTracking()
                .Where(ch =>
                    ch.TrangThai &&
                    ch.ViDo != null &&
                    ch.KinhDo != null)
                .Select(ch => new
                {
                    ch.MaCH,
                    ch.TenCH,
                    ch.DiaChi,
                    ch.SoDienThoai,
                    ch.ViDo,
                    ch.KinhDo
                })
                .ToListAsync();

            var ketQua = danhSachCuaHang
                .Select(ch => new
                {
                    maCH = ch.MaCH,
                    tenCH = ch.TenCH,
                    diaChi = ch.DiaChi,
                    soDienThoai = ch.SoDienThoai,
                    viDo = ch.ViDo,
                    kinhDo = ch.KinhDo,
                    khoangCach = Math.Round(
                        TinhKhoangCach(
                            viDo,
                            kinhDo,
                            Convert.ToDouble(ch.ViDo),
                            Convert.ToDouble(ch.KinhDo)
                        ), 2)
                })
                .OrderBy(ch => ch.khoangCach)
                .ToList();

            return Json(new
            {
                success = true,
                danhSach = ketQua
            });
        }

        // Lấy danh sách loại phương tiện
        [HttpGet]
        public async Task<IActionResult> LayLoaiPhuongTien()
        {
            var danhSach = await _context.LoaiPhuongTien
                .AsNoTracking()
                .Select(x => new
                {
                    maLoaiPT = x.MaLoaiPT,
                    tenLoai = x.TenLoai
                })
                .ToListAsync();

            return Json(danhSach);
        }

        // Thêm phương tiện mới cho khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemPhuongTien(
            int maLoaiPT,
            string bienSo,
            string? hangXe,
            string? dongXe,
            string? mauSac)
        {
            var maKH = await LayMaKhachHang();

            if (maKH == null)
                return Unauthorized(new
                {
                    success = false,
                    message = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại."
                });

            bienSo = (bienSo ?? "").Trim().ToUpper();

            hangXe = hangXe?.Trim();
            dongXe = dongXe?.Trim();
            mauSac = mauSac?.Trim();

            if (maLoaiPT <= 0 ||
                !await _context.LoaiPhuongTien.AnyAsync(
                    x => x.MaLoaiPT == maLoaiPT))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Vui lòng chọn loại phương tiện hợp lệ."
                });
            }

            if (string.IsNullOrWhiteSpace(bienSo))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Vui lòng nhập biển số xe."
                });
            }

            if (bienSo.Length > 20)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Biển số xe không được vượt quá 20 ký tự."
                });
            }

            if (hangXe?.Length > 100 ||
                dongXe?.Length > 100 ||
                mauSac?.Length > 50)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin hãng xe, dòng xe hoặc màu sắc quá dài."
                });
            }

            bool daTonTai = await _context.PhuongTien.AnyAsync(pt =>
                pt.MaKH == maKH.Value &&
                pt.BienSo == bienSo);

            if (daTonTai)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Biển số xe này đã được đăng ký trong tài khoản của bạn."
                });
            }

            var phuongTien = new PhuongTien
            {
                MaKH = maKH.Value,
                MaLoaiPT = maLoaiPT,
                BienSo = bienSo,
                HangXe = string.IsNullOrWhiteSpace(hangXe) ? null : hangXe,
                DongXe = string.IsNullOrWhiteSpace(dongXe) ? null : dongXe,
                MauSac = string.IsNullOrWhiteSpace(mauSac) ? null : mauSac,
                TrangThai = "Chờ xử lý"
            };

            _context.PhuongTien.Add(phuongTien);
            await _context.SaveChangesAsync();

            var tenLoai = await _context.LoaiPhuongTien
                .Where(x => x.MaLoaiPT == maLoaiPT)
                .Select(x => x.TenLoai)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                success = true,
                message = "Thêm phương tiện thành công!",
                maPT = phuongTien.MaPT,
                text = bienSo + " - " +
                       (hangXe ?? "") + " " +
                       (dongXe ?? "") + " (" + tenLoai + ")"
            });
        }

        // Gửi yêu cầu cứu hộ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            TaoYeuCauCuuHoViewModel model)
        {
            var maKH = await LayMaKhachHang();

            if (maKH == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var phuongTienHopLe = await _context.PhuongTien
                .AnyAsync(pt =>
                    pt.MaPT == model.MaPT &&
                    pt.MaKH == maKH.Value);

            if (!phuongTienHopLe)
            {
                ModelState.AddModelError(
                    nameof(model.MaPT),
                    "Phương tiện không hợp lệ."
                );
            }

            // Kiểm tra cửa hàng được chọn
            var cuaHangHopLe = model.MaCH.HasValue &&
                await _context.CuaHang.AnyAsync(ch =>
                    ch.MaCH == model.MaCH.Value &&
                    ch.TrangThai &&
                    ch.ViDo != null &&
                    ch.KinhDo != null);

            if (!cuaHangHopLe)
            {
                ModelState.AddModelError(
                    nameof(model.MaCH),
                    "Cửa hàng không hợp lệ hoặc đã ngừng hoạt động. Vui lòng chọn lại."
                );
            }

            if (model.HinhThucHoTro != "SuaTaiCho" &&
                model.HinhThucHoTro != "CuuHoVeTiem")
            {
                ModelState.AddModelError(
                    nameof(model.HinhThucHoTro),
                    "Hình thức hỗ trợ không hợp lệ."
                );
            }

            decimal? viDo = null;
            decimal? kinhDo = null;

            if (!string.IsNullOrWhiteSpace(model.ViDo))
            {
                if (decimal.TryParse(
                    model.ViDo,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out decimal parsedViDo) &&
                    parsedViDo >= -90 && parsedViDo <= 90)
                {
                    viDo = parsedViDo;
                }
                else
                {
                    ModelState.AddModelError(
                        nameof(model.ViDo),
                        "Vĩ độ không hợp lệ."
                    );
                }
            }

            if (!string.IsNullOrWhiteSpace(model.KinhDo))
            {
                if (decimal.TryParse(
                    model.KinhDo,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out decimal parsedKinhDo) &&
                    parsedKinhDo >= -180 && parsedKinhDo <= 180)
                {
                    kinhDo = parsedKinhDo;
                }
                else
                {
                    ModelState.AddModelError(
                        nameof(model.KinhDo),
                        "Kinh độ không hợp lệ."
                    );
                }
            }

            if (viDo == null || kinhDo == null)
            {
                ModelState.AddModelError(
                    nameof(model.DiaDiem),
                    "Vui lòng chọn vị trí hợp lệ trên bản đồ."
                );
            }

            if (!ModelState.IsValid)
            {
                await NapDanhSachPhuongTien(model, maKH.Value);

                return View(
                    "~/Views/YeuCauCuuHo/Create.cshtml",
                    model
                );
            }

            var yeuCau = new YeuCauCuuHo
            {
                MaKH = maKH.Value,
                MaCH = model.MaCH,
                MaPT = model.MaPT,
                MoTaSuCo = model.MoTaSuCo,
                DiaDiem = model.DiaDiem.Trim(),
                ViDo = viDo,
                KinhDo = kinhDo,
                ThoiGianYeuCau = DateTime.Now,
                MucDoUuTien = model.MucDoUuTien,
                TrangThai = "Chờ tiếp nhận",
                HinhThucHoTro = model.HinhThucHoTro
            };

            _context.YeuCauCuuHo.Add(yeuCau);
            await _context.SaveChangesAsync();

            TempData["ThongBao"] =
                "Gửi yêu cầu cứu hộ thành công! Mã yêu cầu: " +
                yeuCau.MaYC;

            return RedirectToAction(nameof(Index));
        }

        // Lịch sử yêu cầu của khách hàng
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var maKH = await LayMaKhachHang();

            if (maKH == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var danhSach = await (
                from yc in _context.YeuCauCuuHo
                join pt in _context.PhuongTien
                    on yc.MaPT equals pt.MaPT
                where yc.MaKH == maKH.Value
                orderby yc.ThoiGianYeuCau descending
                select new YeuCauCuuHoLichSuViewModel
                {
                    MaYC = yc.MaYC,
                    BienSo = pt.BienSo,
                    TenPhuongTien =
                        (pt.HangXe ?? "") + " " + (pt.DongXe ?? ""),
                    DiaDiem = yc.DiaDiem,
                    MoTaSuCo = yc.MoTaSuCo,
                    HinhThucHoTro = yc.HinhThucHoTro,
                    MucDoUuTien = yc.MucDoUuTien,
                    TrangThai = yc.TrangThai,
                    ThoiGianYeuCau = yc.ThoiGianYeuCau
                }
            ).ToListAsync();

            return View(danhSach);
        }
    }
}
