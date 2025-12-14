using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("PHIEUNHAP")]
    public class PhieuNhap
    {
        [Key]
        [StringLength(15)]
        public string MaPhieuNhap { get; set; }

        public DateTime? NgayLap { get; set; }
        public DateTime? NgayNhap { get; set; }
        public decimal? TongTien { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNhanVien { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNCC { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; }

        [ForeignKey("MaNCC")]
        public virtual NhaCungCap NhaCungCap { get; set; }

        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
    }
}