using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QLNhaThuoc.Database;

namespace QLNhaThuoc.Form
{
    public partial class UC_HoaDon : UserControl
    {
        public UC_HoaDon()
        {
            InitializeComponent();
            SetupInterface();
        }

        private void SetupInterface()
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;

            dgvHoaDon.AutoGenerateColumns = false;
            dgvChiTiet.AutoGenerateColumns = false;

            // Load toàn bộ danh sách khi mới mở
            LoadDanhSachHoaDon(filterByDate: false);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon(filterByDate: true);
        }

        private void LoadDanhSachHoaDon(bool filterByDate = false)
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    var query = db.HoaDons.AsQueryable();

                    if (filterByDate)
                    {
                        DateTime tuNgay = dtpTuNgay.Value.Date;
                        DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1);
                        query = query.Where(hd => hd.NgayLap >= tuNgay && hd.NgayLap < denNgay);
                    }

                    var listHD = query
                        .Select(hd => new
                        {
                            hd.MaHoaDon,
                            hd.NgayLap,
                            hd.TongTien,
                            NguoiLap = hd.MaNhanVien,
                            KhachHang = hd.MaKhachHang ?? "Khách lẻ"
                        })

                        // --- SỬA Ở ĐÂY: Sắp xếp theo Mã HĐ TĂNG DẦN ---
                        .OrderBy(hd => hd.MaHoaDon) // Sắp xếp từ HD001, HD002...
                                                    // Nếu muốn Sắp xếp theo Ngày mới nhất: .OrderByDescending(hd => hd.NgayLap) 
                                                    // ------------------------------------------------

                        .ToList();

                    dgvHoaDon.DataSource = listHD;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hóa đơn: " + ex.Message);
            }
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvHoaDon.Rows[e.RowIndex].IsNewRow)
                return;

            var cellValue = dgvHoaDon.Rows[e.RowIndex].Cells["colMaHD"].Value;

            if (cellValue != null)
            {
                string maHD = cellValue.ToString();
                LoadChiTietHoaDon(maHD);
            }
        }

        private void LoadChiTietHoaDon(string maHD)
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    var listChiTiet = db.ChiTietHoaDons
                        .Where(ct => ct.MaHoaDon == maHD)
                        .Select(ct => new
                        {
                            TenThuoc = db.SanPhams.FirstOrDefault(s => s.MaSanPham == ct.MaSanPham).TenSanPham,
                            ct.SoLuong,
                            ct.ThanhTien
                        })
                        .ToList();

                    dgvChiTiet.DataSource = listChiTiet;
                }
            }
            catch { }
        }
    }
}