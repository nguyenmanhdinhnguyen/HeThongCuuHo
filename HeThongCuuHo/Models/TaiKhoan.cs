
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTK { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string VaiTro { get; set; } = string.Empty;

        public bool TrangThai { get; set; } = true;
    }
}
