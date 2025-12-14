using System.Data.Entity;
using QLNhaThuoc.Database.Entities; // Nhớ using namespace chứa entities

namespace QLNhaThuoc.Database
{
    public class DbThuocContext : DbContext
    {
        public DbThuocContext() : base("name=ChuoiKetNoiDb")
        {
        }

        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<LoTonKho> LoTonKhos { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<LichSuDangNhap> LichSuDangNhaps { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        // Cấu hình thêm cho cột TaiKhoan vì nó trùng tên bảng (nếu cần)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map property TenTaiKhoan vào cột TaiKhoan trong DB
            modelBuilder.Entity<TaiKhoan>()
                .Property(t => t.TenTaiKhoan)
                .HasColumnName("TaiKhoan");
        }
    }
}