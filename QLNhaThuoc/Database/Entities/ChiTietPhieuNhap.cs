using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("CHITIETPHIEUNHAP")]
    public class ChiTietPhieuNhap
    {
        // Khóa chính gồm 2 cột, phải dùng Key + Column Order
        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string MaPhieuNhap { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string MaSanPham { get; set; }

        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        [ForeignKey("MaPhieuNhap")]
        public virtual PhieuNhap PhieuNhap { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual SanPham SanPham { get; set; }
    }
}