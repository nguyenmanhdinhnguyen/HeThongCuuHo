
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class DichVu
    {
        [Key]
        public int MaDV { get; set; }

        [Required]
        [StringLength(150)]
        public string TenDV { get; set; } = string.Empty;

        [StringLength(500)]
        public string? MoTa { get; set; }

        public decimal? GiaCoBan { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}
