
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class CuaHang
    {
        [Key]
        public int MaCH { get; set; }

        [Required]
        [StringLength(150)]
        public string TenCH { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string DiaChi { get; set; } = string.Empty;

        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        public decimal? ViDo { get; set; }

        public decimal? KinhDo { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}
