using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QLNhaThuoc.Database;

namespace QLNhaThuoc.Form
{
    public partial class UC_TongQuan : UserControl
    {
        public UC_TongQuan()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    DateTime homNay = DateTime.Today; // 00:00:00 hôm nay

                    // --- 1. DOANH THU HÔM NAY ---
                    // Tính tổng tiền của các hóa đơn được tạo từ 00:00 hôm nay
                    decimal doanhThuHomNay = db.HoaDons
                        .Where(h => h.NgayLap >= homNay)
                        .Sum(h => (decimal?)h.TongTien) ?? 0;

                    lblDoanhThuVal.Text = doanhThuHomNay.ToString("N0") + " VNĐ";

                    // --- 2. SỐ ĐƠN HÀNG HÔM NAY ---
                    int donHangHomNay = db.HoaDons.Count(h => h.NgayLap >= homNay);
                    lblDonHangVal.Text = donHangHomNay.ToString();

                    // --- 3. TỔNG SẢN PHẨM ---
                    // Chỉ đếm số lượng sản phẩm, không tính số lượng
                    int tongSanPham = db.SanPhams.Count();
                    lblSanPhamVal.Text = tongSanPham.ToString();

                    // --- 4. TỔNG KHÁCH HÀNG ---
                    int tongKhach = db.KhachHangs.Count();
                    lblKhachHangVal.Text = tongKhach.ToString();

                    // --- 5. BIỂU ĐỒ DOANH THU 6 THÁNG GẦN NHẤT ---
                    LoadRevenueChart(db);

                    // --- 6. TOP THUỐC BÁN CHẠY NHẤT ---
                    LoadTopProducts(db);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Dashboard: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRevenueChart(DbThuocContext db)
        {
            try
            {
                chartDoanhThu.Series.Clear();
                var series = chartDoanhThu.Series.Add("DoanhThu");
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 3;
                series.Color = System.Drawing.Color.FromArgb(46, 139, 87); // SeaGreen
                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 8;
                series.IsValueShownAsLabel = false;

                // Lấy ngày đầu tiên của tháng 6 tháng trước
                DateTime sixMonthsAgo = new DateTime(DateTime.Today.AddMonths(-5).Year, 
                    DateTime.Today.AddMonths(-5).Month, 1);

                // Lấy doanh thu theo tháng
                var doanhThuTheoThang = db.HoaDons
                    .Where(h => h.NgayLap >= sixMonthsAgo)
                    .GroupBy(h => new { h.NgayLap.Value.Month, h.NgayLap.Value.Year })
                    .Select(g => new 
                    { 
                        Thang = g.Key.Month, 
                        Nam = g.Key.Year, 
                        TongTien = g.Sum(h => h.TongTien) 
                    })
                    .OrderBy(x => x.Nam).ThenBy(x => x.Thang)
                    .ToList();

                // Thêm dữ liệu vào biểu đồ
                if (doanhThuTheoThang.Count > 0)
                {
                    foreach (var item in doanhThuTheoThang)
                    {
                        series.Points.AddXY($"T{item.Thang}", item.TongTien);
                    }
                }
                else
                {
                    // Nếu không có dữ liệu, hiển thị 6 tháng gần nhất với giá trị 0
                    for (int i = 5; i >= 0; i--)
                    {
                        DateTime month = DateTime.Today.AddMonths(-i);
                        series.Points.AddXY($"T{month.Month}", 0);
                    }
                }

                // Cấu hình biểu đồ
                chartDoanhThu.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                chartDoanhThu.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                chartDoanhThu.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chartDoanhThu.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                chartDoanhThu.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải biểu đồ: " + ex.Message);
            }
        }

        private void LoadTopProducts(DbThuocContext db)
        {
            try
            {
                // Lấy Top 5 sản phẩm có tổng số lượng bán cao nhất
                var topSanPhams = db.ChiTietHoaDons
                   .GroupBy(ct => ct.MaSanPham)
                   .Select(g => new 
                   { 
                       MaSP = g.Key, 
                       SoLuongBan = g.Sum(ct => ct.SoLuong) 
                   })
                   .OrderByDescending(x => x.SoLuongBan)
                   .Take(5)
                   .ToList();

                if (topSanPhams.Count > 0)
                {
                    string textHienThi = "";
                    int top = 1;

                    foreach (var item in topSanPhams)
                    {
                        // Lấy thông tin sản phẩm để hiển thị tên và đơn vị
                        var sp = db.SanPhams.Find(item.MaSP);
                        string tenThuoc = sp?.TenSanPham ?? "Không rõ";
                        string donViTinh = sp?.DonViTinh ?? "Đơn vị";

                        // Format: 1. Paracetamol 500mg - 150 Viên
                        textHienThi += $"{top}. {tenThuoc}\n";
                        top++;
                    }

                    lblTopProducts.Text = textHienThi;
                }
                else
                {
                    lblTopProducts.Text = "Chưa có dữ liệu bán hàng.";
                }
            }
            catch (Exception ex)
            {
                lblTopProducts.Text = "Lỗi tải dữ liệu: " + ex.Message;
            }
        }

        // Phương thức public để refresh dữ liệu từ bên ngoài
        public void RefreshData()
        {
            LoadDashboardData();
        }
    }
}
