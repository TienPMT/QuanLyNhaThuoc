using System;

namespace QLNhaThuoc.Helper
{
    /// <summary>
    /// Lưu trữ thông tin phiên làm việc của người dùng đã đăng nhập
    /// </summary>
    public static class UserSession
    {
        public static string MaNhanVien { get; set; }
        public static string TenNhanVien { get; set; }
        public static string VaiTro { get; set; }
        public static DateTime? NgayDangNhap { get; set; }

        /// <summary>
        /// Kiểm tra xem có người dùng đã đăng nhập hay chưa
        /// </summary>
        public static bool IsLoggedIn => !string.IsNullOrEmpty(MaNhanVien);

        /// <summary>
        /// Xóa thông tin phiên làm việc (đăng xuất)
        /// </summary>
        public static void Clear()
        {
            MaNhanVien = null;
            TenNhanVien = null;
            VaiTro = null;
            NgayDangNhap = null;
        }

        /// <summary>
        /// Đặt thông tin phiên làm việc khi đăng nhập thành công
        /// </summary>
        public static void SetUserInfo(string maNV, string tenNV, string vaiTro)
        {
            MaNhanVien = maNV;
            TenNhanVien = tenNV;
            VaiTro = vaiTro;
            NgayDangNhap = DateTime.Now;
        }
    }
}
