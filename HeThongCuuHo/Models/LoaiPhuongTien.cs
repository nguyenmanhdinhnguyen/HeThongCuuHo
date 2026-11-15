
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class LoaiPhuongTien
    {
        [Key]
        public int MaLoaiPT { get; set; }

        [Required]
        [StringLength(50)]
        public string TenLoai { get; set; } = string.Empty;
    }
}
