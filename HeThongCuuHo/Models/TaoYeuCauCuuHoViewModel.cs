
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class TaoYeuCauCuuHoViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn phương tiện.")]
        public int MaPT { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn cửa hàng cứu hộ.")]
        public int? MaCH { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? MoTaSuCo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa điểm cứu hộ.")]
        [StringLength(255)]
        public string DiaDiem { get; set; } = string.Empty;

        public string? ViDo { get; set; }

        public string? KinhDo { get; set; }

        public string MucDoUuTien { get; set; } = "Bình thường";

        [Required]
        public string HinhThucHoTro { get; set; } = "CuuHoVeTiem";

        public List<SelectListItem> DanhSachPhuongTien { get; set; } = new();
    }

    public class YeuCauCuuHoLichSuViewModel
    {
        public int MaYC { get; set; }
        public string BienSo { get; set; } = string.Empty;
        public string TenPhuongTien { get; set; } = string.Empty;
        public string DiaDiem { get; set; } = string.Empty;
        public string? MoTaSuCo { get; set; }
        public string? HinhThucHoTro { get; set; }
        public string? MucDoUuTien { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? ThoiGianYeuCau { get; set; }
    }
}
