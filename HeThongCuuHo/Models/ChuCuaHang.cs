
using System.ComponentModel.DataAnnotations;

namespace HeThongCuuHo.Models
{
    public class ChuCuaHang
    {
        [Key]
        public int MaChuCH { get; set; }

        public int MaTK { get; set; }

        public int MaCH { get; set; }
    }
}
