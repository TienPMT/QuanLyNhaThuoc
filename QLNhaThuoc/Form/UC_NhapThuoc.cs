using System;
using System.Linq;
using System.Windows.Forms;
using QLNhaThuoc.Database;
using QLNhaThuoc.Helper;

namespace QLNhaThuoc.Form
{
    public partial class UC_NhapThuoc : UserControl
    {
        public UC_NhapThuoc()
        {
            InitializeComponent();
            LoadData();
            SetupEvents();
        }

        private void LoadData()
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    var listPhieuNhap = db.PhieuNhaps
                        .Select(pn => new
                        {
                            pn.MaPhieuNhap,
                            pn.NgayNhap,
                            pn.TongTien,
                            pn.TrangThai,
                            TenNhanVien = pn.NhanVien.HoTen,
                            TenNCC = pn.NhaCungCap.TenNCC
                        })
                        .OrderByDescending(p => p.NgayNhap)
                        .ToList();

                    dgvKhoHang.DataSource = listPhieuNhap;

                    if (colTongTien != null) colTongTien.DefaultCellStyle.Format = "N0";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void SetupEvents()
        {
            // Sự kiện Click trái bình thường
            dgvKhoHang.CellClick += (s, e) => {
                if (e.RowIndex >= 0 && e.RowIndex < dgvKhoHang.Rows.Count)
                {
                    DoDuLieuVaoInput(e.RowIndex);
                }
            };
        }

        // [CẬP NHẬT] Sử dụng tên cột thay vì index để tránh lỗi khi thứ tự cột thay đổi
        private void DoDuLieuVaoInput(int rowIndex)
        {
            DataGridViewRow row = dgvKhoHang.Rows[rowIndex];
            
            // Sử dụng tên cột (Name) thay vì Index
            txtMaPhieu.Text = row.Cells["colMaPhieu"].Value?.ToString() ?? "";
            txtNhanVien.Text = row.Cells["colNhanVien"].Value?.ToString() ?? "";
            txtNhaCungCap.Text = row.Cells["colNhaCungCap"].Value?.ToString() ?? "";
            
            if (DateTime.TryParse(row.Cells["colNgayNhap"].Value?.ToString(), out DateTime ngay))
                dtpNgayNhap.Value = ngay;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            MoFormChiTiet(null);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            if (string.IsNullOrEmpty(maPhieu))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập trên lưới để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MoFormChiTiet(maPhieu);
        }

        private void MoFormChiTiet(string maPhieuInput)
        {
            var ucChiTiet = new UC_ChiTietPhieuNhap();
            
            // [MỚI] Đăng ký event để refresh dữ liệu khi lưu thành công
            ucChiTiet.DataSaved += (s, e) => {
                LoadData(); // Refresh lại danh sách phiếu nhập
            };
            
            ucChiTiet.SetMaPhieu(maPhieuInput);

            if (this.Parent != null)
            {
                this.Parent.Controls.Add(ucChiTiet);
                ucChiTiet.Dock = DockStyle.Fill;
                ucChiTiet.BringToFront();
            }
            else
            {
                MessageBox.Show("Lỗi: Không tìm thấy Form cha để hiển thị chi tiết.");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            if (string.IsNullOrEmpty(maPhieu))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn chắc chắn muốn xóa phiếu nhập [{maPhieu}]?\nThao tác này sẽ xóa cả chi tiết phiếu nhập!",
                                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var db = new DbThuocContext())
                    {
                        var pn = db.PhieuNhaps.Find(maPhieu);
                        if (pn != null)
                        {
                            var listChiTiet = db.ChiTietPhieuNhaps.Where(x => x.MaPhieuNhap == maPhieu);
                            db.ChiTietPhieuNhaps.RemoveRange(listChiTiet);
                            db.PhieuNhaps.Remove(pn);
                            db.SaveChanges();

                            MessageBox.Show("Xóa thành công!");
                            LoadData();
                            ClearInput();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phiếu nhập này trong CSDL.");
                            LoadData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.InnerException?.Message ?? ex.Message);
                }
            }
        }

        private void ClearInput()
        {
            txtMaPhieu.Clear();
            txtNhanVien.Clear();
            txtNhaCungCap.Clear();
            dtpNgayNhap.Value = DateTime.Now;
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (dgvKhoHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xuất trực tiếp ra Excel, không cần menu
            ExcelExporter.ExportToExcel(dgvKhoHang, "PhieuNhapThuoc", "BÁO CÁO PHIẾU NHẬP THUỐC");
        }
    }
}