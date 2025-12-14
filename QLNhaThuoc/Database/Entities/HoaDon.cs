using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("HOADON")]
    public class HoaDon
    {
        [Key]
        [StringLength(15)]
        public string MaHoaDon { get; set; }

        public DateTime? NgayLap { get; set; }
        public decimal? TongTien { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNhanVien { get; set; }

        [StringLength(10)]
        public string MaKhachHang { get; set; } // Nullable

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; }

        [ForeignKey("MaKhachHang")]
        public virtual KhachHang KhachHang { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}