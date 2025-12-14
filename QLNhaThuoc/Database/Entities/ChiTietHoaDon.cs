using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("CHITIETHOADON")]
    public class ChiTietHoaDon
    {
        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string MaHoaDon { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string MaSanPham { get; set; }

        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }

        [ForeignKey("MaHoaDon")]
        public virtual HoaDon HoaDon { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual SanPham SanPham { get; set; }
    }
}