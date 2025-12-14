using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QLNhaThuoc.Database;

namespace QLNhaThuoc.Form
{
    public partial class UC_BaoCao : UserControl
    {
        public UC_BaoCao()
        {
            InitializeComponent();
            SetupComboBox();
        }

        private void SetupComboBox()
        {
            // Khởi tạo các mốc thời gian
            cboThoiGian.Items.Add("Hôm nay");
            cboThoiGian.Items.Add("Hôm qua");
            cboThoiGian.Items.Add("7 ngày qua");
            cboThoiGian.Items.Add("Tháng này");
            cboThoiGian.Items.Add("Tháng trước");
            cboThoiGian.Items.Add("Năm nay");
            cboThoiGian.Items.Add("Tất cả thời gian");

            cboThoiGian.SelectedIndex = 3; // Mặc định chọn "Tháng này"

            // Gán sự kiện khi thay đổi lựa chọn
            cboThoiGian.SelectedIndexChanged += CboThoiGian_SelectedIndexChanged;
        }

        // Sự kiện khi người dùng chọn mốc thời gian khác
        private void CboThoiGian_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateDateRangeAndLoadData();
        }

        // Hàm tính toán ngày và gọi tải dữ liệu
        private void CalculateDateRangeAndLoadData()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today.AddDays(1).AddSeconds(-1); // Hết ngày hôm nay (23:59:59)

            string selected = cboThoiGian.SelectedItem.ToString();

            switch (selected)
            {
                case "Hôm nay":
                    startDate = today;
                    break;
                case "Hôm qua":
                    startDate = today.AddDays(-1);
                    endDate = today.AddSeconds(-1);
                    break;
                case "7 ngày qua":
                    startDate = today.AddDays(-7);
                    break;
                case "Tháng này":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    break;
                case "Tháng trước":
                    startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                    endDate = new DateTime(today.Year, today.Month, 1).AddSeconds(-1);
                    break;
                case "Năm nay":
                    startDate = new DateTime(today.Year, 1, 1);
                    break;
                case "Tất cả thời gian":
                    startDate = new DateTime(2000, 1, 1); // Lấy từ xa xưa
                    break;
            }

            // Gọi hàm tải dữ liệu với khoảng thời gian đã tính
            LoadDataTuDatabase(startDate, endDate);
        }

        // Hàm nạp dữ liệu chính (Có tham số ngày)
        private void LoadDataTuDatabase(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    // Lọc hóa đơn theo khoảng thời gian đã chọn
                    var listHoaDon = db.HoaDons
                        .Where(hd => hd.NgayLap >= startDate && hd.NgayLap <= endDate)
                        .ToList(); // Tải về bộ nhớ để xử lý

                    // --- 1. TÍNH TOÁN 4 Ô CARD ---
                    decimal tongDoanhThu = listHoaDon.Sum(hd => (decimal?)hd.TongTien) ?? 0;
                    int tongDonHang = listHoaDon.Count;
                    int soKhachHang = listHoaDon.Where(hd => hd.MaKhachHang != null).Select(hd => hd.MaKhachHang).Distinct().Count();
                    decimal tongLoiNhuan = tongDoanhThu * 0.2m; // Giả định 20%

                    lblVal1.Text = FormatTien(tongDoanhThu);
                    lblVal2.Text = FormatTien(tongLoiNhuan);
                    lblVal3.Text = tongDonHang.ToString("N0");
                    lblVal4.Text = soKhachHang.ToString("N0");

                    // --- 2. VẼ BIỂU ĐỒ ---
                    if (chartThongKe != null)
                    {
                        chartThongKe.Series["Doanh thu"].Points.Clear();
                        chartThongKe.Series["Lợi nhuận"].Points.Clear();

                        // Logic vẽ biểu đồ thông minh:
                        // - Nếu khoảng thời gian > 31 ngày (xem Năm/Tất cả) -> Gom nhóm theo THÁNG
                        // - Nếu khoảng thời gian <= 31 ngày (xem Tháng/Tuần) -> Gom nhóm theo NGÀY

                        bool groupByMonth = (endDate - startDate).TotalDays > 32;

                        var dataBieuDo = listHoaDon
                            .GroupBy(hd => groupByMonth
                                ? new { Year = hd.NgayLap.Value.Year, Month = hd.NgayLap.Value.Month, Day = 1 } // Gom theo tháng
                                : new { Year = hd.NgayLap.Value.Year, Month = hd.NgayLap.Value.Month, Day = hd.NgayLap.Value.Day }) // Gom theo ngày
                            .Select(g => new
                            {
                                ThoiGian = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                                DoanhThu = g.Sum(x => x.TongTien) ?? 0
                            })
                            .OrderBy(x => x.ThoiGian)
                            .ToList();

                        foreach (var item in dataBieuDo)
                        {
                            // Nhãn trục X: Nếu gom tháng hiện "T1/2024", gom ngày hiện "15/06"
                            string labelX = groupByMonth
                                ? $"T{item.ThoiGian.Month}/{item.ThoiGian.Year}"
                                : $"{item.ThoiGian.Day}/{item.ThoiGian.Month}";

                            decimal dt = item.DoanhThu;
                            decimal ln = dt * 0.2m;

                            chartThongKe.Series["Doanh thu"].Points.AddXY(labelX, dt);
                            chartThongKe.Series["Lợi nhuận"].Points.AddXY(labelX, ln);
                        }

                        // Định dạng trục Y
                        chartThongKe.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private string FormatTien(decimal soTien)
        {
            if (soTien == 0) return "0 VNĐ";
            return string.Format("{0:N0} VNĐ", soTien);
        }

        // Tự động load khi mở form (tránh lỗi Designer)
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.DesignMode)
            {
                CalculateDateRangeAndLoadData();
            }
        }
    }
}