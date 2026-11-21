
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class PhuongTien
    {
        [Key]
        public int MaPT { get; set; }

        public int MaKH { get; set; }

        public int MaLoaiPT { get; set; }

        [Required]
        [StringLength(20)]
        public string BienSo { get; set; } = string.Empty;

        [StringLength(50)]
        public string? HangXe { get; set; }

        [StringLength(50)]
        public string? DongXe { get; set; }

        [StringLength(30)]
        public string? MauSac { get; set; }

        [Required]
        [StringLength(30)]
        public string TrangThai { get; set; } = "Sẵn sàng";
    }
}
