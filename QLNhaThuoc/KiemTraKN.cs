using System;
using System.Drawing; // Để dùng Point, Size
using System.Linq;    // Để dùng hàm Count()
using System.Windows.Forms;
using QLNhaThuoc.Database; // Namespace chứa DbThuocContext

namespace QLNhaThuoc
{
    // Chỉ định rõ Form này là của Windows, không phải cái thư mục của bạn
    public partial class KiemTraKN : System.Windows.Forms.Form
    {
        // Khai báo các control
        private Button btnCheckConnection;
        private Label lblStatus;

        public KiemTraKN()
        {
            InitializeComponent();
            SetupUI(); // Gọi hàm tạo giao diện
        }

        // Hàm tự tạo giao diện (Code tay thay vì kéo thả)
        private void SetupUI()
        {
            this.Text = "Kiểm tra kết nối Database";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. Tạo nút bấm
            btnCheckConnection = new Button();
            btnCheckConnection.Text = "Bấm để kiểm tra kết nối";
            btnCheckConnection.Size = new Size(200, 40);
            btnCheckConnection.Location = new Point(90, 40);
            btnCheckConnection.Click += BtnCheckConnection_Click; // Gán sự kiện click
            this.Controls.Add(btnCheckConnection);

            // 2. Tạo nhãn hiển thị trạng thái
            lblStatus = new Label();
            lblStatus.Text = "Trạng thái: Chưa kiểm tra";
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(90, 100);
            lblStatus.ForeColor = Color.Gray;
            this.Controls.Add(lblStatus);
        }

        // Sự kiện khi bấm nút
        private void BtnCheckConnection_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Đang kết nối...";
            lblStatus.ForeColor = Color.Blue;
            Application.DoEvents(); // Cập nhật giao diện ngay lập tức

            try
            {
                // Khởi tạo Context
                using (var db = new DbThuocContext())
                {
                    // Bước 1: Thử mở kết nối vật lý
                    // (Nếu sai chuỗi kết nối trong App.config sẽ lỗi ngay tại đây)
                    db.Database.Connection.Open();

                    // Bước 2: Thử truy vấn dữ liệu thực tế
                    // (Lấy số lượng nhân viên để đảm bảo Entity đã map đúng bảng)
                    int soLuongNV = db.NhanViens.Count();

                    // Nếu chạy xuống được đây là thành công
                    lblStatus.Text = "Kết nối THÀNH CÔNG!";
                    lblStatus.ForeColor = Color.Green;

                    MessageBox.Show($"Kết nối tốt!\nĐã tìm thấy {soLuongNV} nhân viên trong CSDL.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Nếu lỗi
                lblStatus.Text = "Kết nối THẤT BẠI!";
                lblStatus.ForeColor = Color.Red;

                MessageBox.Show($"Lỗi kết nối:\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}