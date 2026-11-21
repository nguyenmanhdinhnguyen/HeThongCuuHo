
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongCuuHo.Models
{
    [Table("YeuCauCuuHo")]
    public class YeuCauCuuHo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaYC { get; set; }

        public int MaKH { get; set; }
        public int? MaCH { get; set; }

        public int MaPT { get; set; }

        [StringLength(500)]
        public string? MoTaSuCo { get; set; }

        [Required]
        [StringLength(255)]
        public string DiaDiem { get; set; } = string.Empty;

        public decimal? ViDo { get; set; }

        public decimal? KinhDo { get; set; }

        public DateTime? ThoiGianYeuCau { get; set; }

        [StringLength(30)]
        public string? MucDoUuTien { get; set; } = "Bình thường";

        [StringLength(50)]
        public string? TrangThai { get; set; } = "Chờ tiếp nhận";

        [StringLength(30)]
        public string? HinhThucHoTro { get; set; }
    }
}
