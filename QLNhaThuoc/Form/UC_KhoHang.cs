using System;
using System.Linq;
using System.Windows.Forms;
using QLNhaThuoc.Database;
using QLNhaThuoc.Helper;

namespace QLNhaThuoc.Form
{
    public partial class UC_KhoHang : UserControl
    {
        public UC_KhoHang()
        {
            InitializeComponent();
            LoadKhoHang();
        }

        private void LoadKhoHang()
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    // Lấy dữ liệu từ bảng LoTonKho và kết hợp với SanPham
                    var danhSachKho = db.LoTonKhos
                        .Select(l => new
                        {
                            TenThuoc = l.SanPham.TenSanPham,
                            l.SoLo,
                            HanDung = l.HSD,
                            SoLuong = l.SoLuongTon
                        })
                        .OrderBy(x => x.TenThuoc)
                        .ToList();

                    // Thêm STT sau khi đã ToList()
                    var danhSachKhoVoiSTT = danhSachKho.Select((l, index) => new
                    {
                        STT = index + 1,
                        l.TenThuoc,
                        l.SoLo,
                        l.HanDung,
                        l.SoLuong
                    }).ToList();

                    dgvKhoHang.DataSource = danhSachKhoVoiSTT;

                    // Format cột hạn dùng
                    if (colHanDung != null)
                        colHanDung.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu kho: " + ex.Message);
            }
        }

        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Tìm kiếm")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = "Tìm kiếm";
                txtTimKiem.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            
            // Bỏ qua nếu đang là placeholder text
            if (keyword == "tìm kiếm")
            {
                LoadKhoHang();
                return;
            }
            
            if (string.IsNullOrEmpty(keyword))
            {
                LoadKhoHang();
                return;
            }

            try
            {
                using (var db = new DbThuocContext())
                {
                    var danhSachKho = db.LoTonKhos
                        .Where(l => l.SanPham.TenSanPham.ToLower().Contains(keyword) || 
                                    l.SoLo.ToLower().Contains(keyword))
                        .Select(l => new
                        {
                            TenThuoc = l.SanPham.TenSanPham,
                            l.SoLo,
                            HanDung = l.HSD,
                            SoLuong = l.SoLuongTon
                        })
                        .OrderBy(x => x.TenThuoc)
                        .ToList();

                    // Thêm STT sau khi đã ToList()
                    var danhSachKhoVoiSTT = danhSachKho.Select((l, index) => new
                    {
                        STT = index + 1,
                        l.TenThuoc,
                        l.SoLo,
                        l.HanDung,
                        l.SoLuong
                    }).ToList();

                    dgvKhoHang.DataSource = danhSachKhoVoiSTT;

                    if (colHanDung != null)
                        colHanDung.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
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
            ExcelExporter.ExportToExcel(dgvKhoHang, "KhoHang", "BÁO CÁO KHO HÀNG");
        }
    }
}