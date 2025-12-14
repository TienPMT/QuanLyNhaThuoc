using System;
using System.Linq;
using System.Windows.Forms;
using QLNhaThuoc.Helper;

namespace QLNhaThuoc.Form
{
    public partial class FormMain : System.Windows.Forms.Form
    {
        private bool isLoggingOut = false; // Flag để kiểm soát đăng xuất

        public FormMain()
        {
            InitializeComponent();
            
            // Load logo image
            LoadLogoImage();
            
            SetupEvents();

            // Default: Load Dashboard (UC_TongQuan) when opening
            LoadDashboard();
            HighlightButton(btnTongQuan); // Highlight Overview button
        }

        private void LoadLogoImage()
        {
            try
            {
                // Get the application startup path
                string appPath = Application.StartupPath;
                string logoPath = System.IO.Path.Combine(appPath, "Img", "logo.jpg");
                
                // Alternative: try relative path from current directory
                if (!System.IO.File.Exists(logoPath))
                {
                    logoPath = System.IO.Path.Combine(appPath, "..", "..", "Img", "logo.jpg");
                    logoPath = System.IO.Path.GetFullPath(logoPath);
                }
                
                if (System.IO.File.Exists(logoPath))
                {
                    picLogo.Image = System.Drawing.Image.FromFile(logoPath);
                }
                else
                {
                    // If logo doesn't exist, display white background
                    picLogo.BackColor = System.Drawing.Color.White;
                    System.Diagnostics.Debug.WriteLine($"Logo not found at: {logoPath}");
                }
            }
            catch (Exception ex)
            {
                // If error loading image, just use white background
                picLogo.BackColor = System.Drawing.Color.White;
                System.Diagnostics.Debug.WriteLine($"Error loading logo: {ex.Message}");
            }
        }

        private void LoadDashboard()
        {
            try
            {
                // Remove any existing UserControls
                foreach (Control item in panelContent.Controls.OfType<UserControl>().ToList())
                {
                    panelContent.Controls.Remove(item);
                }

                // Create and add UC_TongQuan
                UC_TongQuan ucTongQuan = new UC_TongQuan();
                ucTongQuan.Dock = DockStyle.Fill;
                panelContent.Controls.Add(ucTongQuan);
                ucTongQuan.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải trang Tổng quan: " + ex.Message);
            }
        }

        private void SetupEvents()
        {
            // Event handler for logout button is already set in Designer
            // No need to add it again here
        }

        // =================================================================================
        // PART 1: UI NAVIGATION HANDLING
        // =================================================================================

        private void HienThiUserControl(UserControl uc)
        {
            // Remove old UserControls
            foreach (Control item in panelContent.Controls.OfType<UserControl>().ToList())
            {
                panelContent.Controls.Remove(item);
            }

            // Add new UserControl
            if (uc != null)
            {
                uc.Dock = DockStyle.Fill;
                panelContent.Controls.Add(uc);
                uc.BringToFront();
            }
        }

        private void HienThiDashboard()
        {
            LoadDashboard();
        }

        private void HighlightButton(Button btnActive)
        {
            // Reset colors to default yellow
            btnTongQuan.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            btnBanThuoc.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            btnKhoHang.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            btnKhachHang.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            btnNhapThuoc.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            btnBaoCao.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            if (this.Controls.Find("btnHoaDon", true).FirstOrDefault() is Button btnHD)
                btnHD.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);

            // Highlight active button with orange
            if (btnActive != null) btnActive.BackColor = System.Drawing.Color.FromArgb(255, 165, 0);
        }

        // =================================================================================
        // PART 2: MENU CLICK EVENTS
        // =================================================================================

        private void btnTongQuan_Click_1(object sender, EventArgs e)
        {
            HighlightButton(btnTongQuan);
            LoadDashboard();
        }

        private void btnBanThuoc_Click(object sender, EventArgs e)
        {
            HighlightButton(btnBanThuoc);
            UC_BanThuoc uc = new UC_BanThuoc();
            HienThiUserControl(uc);
        }

        private void btnKhoHang_Click(object sender, EventArgs e)
        {
            HighlightButton(btnKhoHang);
            UC_KhoHang uc = new UC_KhoHang();
            HienThiUserControl(uc);
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            HighlightButton(btnKhachHang);
            UC_KhachHang uc = new UC_KhachHang();
            HienThiUserControl(uc);
        }

        private void btnNhapThuoc_Click(object sender, EventArgs e)
        {
            HighlightButton(btnNhapThuoc);
            UC_NhapThuoc uc = new UC_NhapThuoc();
            HienThiUserControl(uc);
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            // Kiểm tra quyền truy cập - chỉ Quản lý mới được phép
            if (UserSession.VaiTro != "Quản lý")
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HighlightButton(btnBaoCao);
            UC_BaoCao uc = new UC_BaoCao();
            HienThiUserControl(uc);
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            UC_HoaDon uc = new UC_HoaDon();
            HienThiUserControl(uc);
        }

        // =================================================================================
        // PART 3: LOGOUT & CLOSE
        // =================================================================================
        
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            // Hỏi xác nhận trước khi đăng xuất
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận đăng xuất", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Xóa thông tin session
                UserSession.Clear();
                
                // Đặt flag để không hiển thị lại hộp thoại xác nhận
                isLoggingOut = true;
                
                // Tìm FormDangNhap đang ẩn và hiện lại nó
                FormDangNhap loginForm = null;
                foreach (System.Windows.Forms.Form form in Application.OpenForms)
                {
                    if (form is FormDangNhap)
                    {
                        loginForm = (FormDangNhap)form;
                        break;
                    }
                }
                
                // Nếu tìm thấy form đăng nhập cũ, reset và hiện lại nó
                if (loginForm != null)
                {
                    loginForm.ResetForm(); // Reset các trường nhập liệu
                    loginForm.Show();
                    loginForm.BringToFront();
                    loginForm.Focus();
                }
                else
                {
                    // Nếu không tìm thấy, tạo mới (trường hợp đặc biệt)
                    loginForm = new FormDangNhap();
                    loginForm.Show();
                }
                
                // Đóng form hiện tại
                this.Close();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đang đăng xuất, chỉ đóng form này và KHÔNG thoát ứng dụng
            if (isLoggingOut)
            {
                // Không làm gì thêm, chỉ để form đóng bình thường
                return;
            }

            // Hiển thị hộp thoại xác nhận thoát (chỉ khi KHÔNG phải đăng xuất)
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                // Hủy việc đóng form
                e.Cancel = true;
            }
            else
            {
                // Thoát hoàn toàn ứng dụng
                Application.Exit();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}