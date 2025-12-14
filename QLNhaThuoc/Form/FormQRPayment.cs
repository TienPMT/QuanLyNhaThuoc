using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QLNhaThuoc.Form
{
    public partial class FormQRPayment : System.Windows.Forms.Form
    {
        public bool IsConfirmed { get; private set; }
        private bool isQRLoaded = false;

        public FormQRPayment(decimal tongTien)
        {
            InitializeComponent();
            lblSoTien.Text = $"Số tiền: {tongTien:N0} VNĐ";

            // Load QR code và kiểm tra
            isQRLoaded = LoadQRImage();

            // Nếu không load được QR, disable nút xác nhận
            if (!isQRLoaded)
            {
                btnXacNhan.Enabled = false;
                btnXacNhan.BackColor = Color.Gray;
                lblHuongDan.Text = "KHÔNG TÌM THẤY MÃ QR!\n\n" +
                    "Vui lòng đặt file QR_THANHTOAN.png vào thư mục Img\n" +
                    "và thử lại.";
                lblHuongDan.ForeColor = Color.Red;
            }
        }

        private bool LoadQRImage()
        {
            try
            {
                // Tạo thư mục Img nếu chưa tồn tại
                string imgFolder = Path.Combine(Application.StartupPath, "Img");
                if (!Directory.Exists(imgFolder))
                {
                    Directory.CreateDirectory(imgFolder);
                }

                // Thử nhiều đường dẫn khác nhau
                string[] possiblePaths = new string[]
                {
                    Path.Combine(Application.StartupPath, "Img", "QR_THANHTOAN.png"),
                    Path.Combine(Application.StartupPath, "img", "QR_THANHTOAN.png"),
                    Path.Combine(Application.StartupPath, "Images", "QR_THANHTOAN.png"),
                    Path.Combine(Application.StartupPath, "QR_THANHTOAN.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Img", "QR_THANHTOAN.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "QR_THANHTOAN.png"),
                    // Thêm đường dẫn từ project root (khi debug)
                    Path.Combine(Application.StartupPath, "..", "..", "Img", "QR_THANHTOAN.png")
                };

                string foundPath = null;
                foreach (string qrPath in possiblePaths)
                {
                    string normalizedPath = Path.GetFullPath(qrPath);
                    if (File.Exists(normalizedPath))
                    {
                        foundPath = normalizedPath;
                        break;
                    }
                }

                if (foundPath != null)
                {
                    // Load ảnh vào memory trước để tránh lock file
                    using (var tempImage = Image.FromFile(foundPath))
                    {
                        picQRCode.Image = new Bitmap(tempImage);
                    }
                    picQRCode.SizeMode = PictureBoxSizeMode.Zoom;
                    return true;
                }

                // Không tìm thấy file ở bất kỳ đường dẫn nào
                ShowQRNotFoundMessage(imgFolder);
                CreatePlaceholderImage();
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tải mã QR:\n{ex.Message}\n\nChi tiết: {ex.StackTrace}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                CreatePlaceholderImage();
                return false;
            }
        }

        private void ShowQRNotFoundMessage(string imgFolder)
        {
            string message =
                "═══════════════════════════════════════\n" +
                "  KHÔNG TÌM THẤY FILE QR_THANHTOAN.png\n" +
                "═══════════════════════════════════════\n\n" +
                "HƯỚNG DẪN CÀI ĐẶT:\n\n" +
                "1. Tạo mã QR từ ứng dụng ngân hàng\n" +
                "   (hoặc sử dụng VietQR: https://vietqr.io/)\n\n" +
                "2. Lưu ảnh mã QR với tên: QR_THANHTOAN.png\n" +
                "   (viết HOA toàn bộ)\n\n" +
                "3. Đặt file vào thư mục:\n" +
                $"   {imgFolder}\n\n" +
                "4. Khởi động lại chương trình\n\n" +
                "═══════════════════════════════════════\n" +
                "Lưu ý: Tên file phải CHÍNH XÁC (viết hoa)\n" +
                "Định dạng: PNG (khuyến nghị)";

            MessageBox.Show(
                message,
                "Thiếu file mã QR thanh toán",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void CreatePlaceholderImage()
        {
            // Tạo ảnh placeholder với text
            Bitmap placeholder = new Bitmap(300, 300);
            using (Graphics g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.WhiteSmoke);

                // Vẽ viền
                using (Pen borderPen = new Pen(Color.LightGray, 2))
                {
                    g.DrawRectangle(borderPen, 1, 1, 298, 298);
                }

                // Vẽ icon X lớn
                using (Pen xPen = new Pen(Color.LightCoral, 4))
                {
                    g.DrawLine(xPen, 100, 100, 200, 200);
                    g.DrawLine(xPen, 200, 100, 100, 200);
                }

                // Vẽ text
                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.DarkRed))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    g.DrawString(
                        "KHÔNG TÌM THẤY\nMÃ QR",
                        font,
                        brush,
                        new RectangleF(0, 220, 300, 80),
                        sf
                    );
                }
            }

            picQRCode.Image = placeholder;
            picQRCode.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (!isQRLoaded)
            {
                MessageBox.Show(
                    "Không thể xác nhận thanh toán do thiếu mã QR!\n\n" +
                    "Vui lòng cài đặt file QR_THANHTOAN.png và thử lại.",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var result = MessageBox.Show(
                "Bạn đã chuyển khoản thành công?\n\n" +
                "Chỉ nhấn Có nếu giao dịch đã hoàn tất!",
                "Xác nhận thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                IsConfirmed = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn hủy thanh toán?",
                "Xác nhận hủy",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                IsConfirmed = false;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Giải phóng image khi đóng form
            if (picQRCode.Image != null)
            {
                var img = picQRCode.Image;
                picQRCode.Image = null;
                img.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}