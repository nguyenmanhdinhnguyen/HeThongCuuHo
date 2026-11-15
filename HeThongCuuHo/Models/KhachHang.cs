
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }

        public int? MaTK { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? DiaChi { get; set; }
    }
}
