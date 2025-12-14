using System;
using System.Linq;
using System.Windows.Forms;
using QLNhaThuoc.Database;
using QLNhaThuoc.Helper;

namespace QLNhaThuoc.Form
{
    public partial class FormDangNhap : System.Windows.Forms.Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reset form đăng nhập về trạng thái ban đầu
        /// </summary>
        public void ResetForm()
        {
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            txtTaiKhoan.Focus();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new DbThuocContext())
                {
                    // Tìm tài khoản và thông tin nhân viên
                    var user = db.TaiKhoans.FirstOrDefault(u => u.TenTaiKhoan == taiKhoan && u.MatKhau == matKhau);

                    if (user != null)
                    {
                        // Lấy thông tin nhân viên từ MaNhanVien
                        var nhanVien = db.NhanViens.Find(user.MaNhanVien);
                        
                        if (nhanVien != null)
                        {
                            // Lưu thông tin vào UserSession
                            UserSession.SetUserInfo(
                                nhanVien.MaNhanVien, 
                                nhanVien.HoTen, 
                                user.VaiTro
                            );

                            MessageBox.Show($"Đăng nhập thành công!\nXin chào: {nhanVien.HoTen}\nVai trò: {user.VaiTro}", 
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Mở form trang chủ
                            var mainForm = new FormMain();
                            mainForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Ngăn không cho phát ra tiếng "ding" (lỗi) của Windows
                e.SuppressKeyPress = true;

                btnDangNhap.PerformClick();
            }
        }

        private void txtTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Ngăn không cho phát ra tiếng "ding"
                e.SuppressKeyPress = true;

                // Chuyển focus sang ô mật khẩu
                txtMatKhau.Focus();
            }
        }
    }
}