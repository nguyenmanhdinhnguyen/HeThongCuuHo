
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; }

        public int? MaTK { get; set; }

        public int? MaCH { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        [StringLength(50)]
        public string? ChucVu { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}
