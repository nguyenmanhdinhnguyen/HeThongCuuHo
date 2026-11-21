using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongCuuHo.Models
{
    [Table("DonSuaChua")]
    public class DonSuaChua
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDon { get; set; }

        public int? MaYC { get; set; }

        public int MaKH { get; set; }

        public int MaCH { get; set; }

        public int MaPT { get; set; }

        public DateTime? NgayTiepNhan { get; set; }

        public DateTime? NgayHoanThanh { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TongTien { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }
    }
}