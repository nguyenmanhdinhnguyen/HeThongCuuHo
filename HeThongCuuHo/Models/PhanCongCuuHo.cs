using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongCuuHo.Models
{
    [Table("PhanCongCuuHo")]
    public class PhanCongCuuHo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaPC { get; set; }

        public int MaYC { get; set; }

        public int MaTX { get; set; }

        public int? MaCH { get; set; }

        public DateTime? ThoiGianPhanCong { get; set; }

        public DateTime? ThoiGianChapNhan { get; set; }

        public DateTime? ThoiGianDen { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }
    }
}