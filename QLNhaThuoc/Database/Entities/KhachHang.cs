using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("KHACHHANG")]
    public class KhachHang
    {
        [Key]
        [StringLength(10)]
        public string MaKhachHang { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhachHang { get; set; }

        [StringLength(10)]
        public string GioiTinh { get; set; }

        [StringLength(15)]
        public string SDT { get; set; }

        public int? SoLanMua { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}