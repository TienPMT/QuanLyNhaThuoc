using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("NHANVIEN")]
    public class NhanVien
    {
        [Key]
        [StringLength(10)]
        public string MaNhanVien { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(10)]
        public string GioiTinh { get; set; }

        public DateTime? NgaySinh { get; set; }

        [StringLength(15)]
        public string SDT { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        // Navigation Properties (Liên kết)
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}