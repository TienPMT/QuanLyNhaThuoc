using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("SANPHAM")]
    public class SanPham
    {
        [Key]
        [StringLength(10)]
        public string MaSanPham { get; set; }

        [Required]
        [StringLength(200)]
        public string TenSanPham { get; set; }

        public decimal DonGia { get; set; }

        [StringLength(255)]
        public string ThanhPhan { get; set; }

        public string ChiDinh { get; set; }
        public string ChongChiDinh { get; set; }

        [StringLength(100)]
        public string NSX { get; set; }

        [StringLength(100)]
        public string XuatXu { get; set; }

        public bool? KeToa { get; set; }

        public string MoTa { get; set; }

        [StringLength(255)]
        public string DoiTuongSuDung { get; set; }

        public string LuuY { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }
        [StringLength(50)]
        public string DonViTinh { get; set; }

        // Navigation Properties
        public virtual ICollection<LoTonKho> LoTonKhos { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}