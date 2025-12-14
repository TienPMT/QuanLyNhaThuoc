using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaThuoc.Database.Entities
{
    [Table("LICHSUDANGNHAP")]
    public class LichSuDangNhap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự tăng
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string TaiKhoan { get; set; }

        public DateTime? ThoiGian { get; set; }

        [ForeignKey("TaiKhoan")]
        public virtual TaiKhoan TaiKhoanNav { get; set; }
    }
}