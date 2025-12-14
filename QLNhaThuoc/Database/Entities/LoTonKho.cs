using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("LOTONKHO")]
    public class LoTonKho
    {
        [Key]
        [StringLength(20)]
        public string MaLoTonKho { get; set; }

        [Required]
        [StringLength(10)]
        public string MaSanPham { get; set; }

        [StringLength(50)]
        public string SoLo { get; set; }

        public DateTime HSD { get; set; }

        public int? SoLuongTon { get; set; }

        public DateTime? NgayNhap { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual SanPham SanPham { get; set; }
    }
}