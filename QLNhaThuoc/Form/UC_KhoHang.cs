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
                    // [CẬP NHẬT] Lấy tất cả sản phẩm, bao gồm cả sản phẩm có số lượng tồn = 0
                    // Group by MaSanPham để tính tổng số lượng tồn kho của từng sản phẩm
                    var danhSachKho = (from lo in db.LoTonKhos
                                       group lo by new { lo.MaSanPham, lo.SanPham.TenSanPham } into g
                                       select new
                                       {
                                           TenThuoc = g.Key.TenSanPham,
                                           SoLo = g.Count() + " lô",
                                           HanDung = (DateTime?)g.Min(x => x.HSD), // Lấy HSD gần nhất - cast to nullable
                                           SoLuong = g.Sum(x => x.SoLuongTon ?? 0)
                                       })
                                       .ToList();

                    // [MỚI] Thêm cả các sản phẩm chưa có trong kho (số lượng = 0)
                    var sanPhamChuaCoKho = (from sp in db.SanPhams
                                            where !db.LoTonKhos.Any(lo => lo.MaSanPham == sp.MaSanPham)
                                            select new
                                            {
                                                TenThuoc = sp.TenSanPham,
                                                SoLo = "0 lô",
                                                HanDung = (DateTime?)null,
                                                SoLuong = 0
                                            })
                                            .ToList();

                    // Gộp 2 danh sách
                    var danhSachKhoDay = danhSachKho
                        .Concat(sanPhamChuaCoKho)
                        .OrderBy(x => x.TenThuoc)
                        .ToList();

                    // Thêm STT sau khi đã ToList()
                    var danhSachKhoVoiSTT = danhSachKhoDay.Select((l, index) => new
                    {
                        STT = index + 1,
                        l.TenThuoc,
                        l.SoLo,
                        HanDung = l.HanDung.HasValue ? l.HanDung.Value.ToString("dd/MM/yyyy") : "N/A",
                        l.SoLuong
                    }).ToList();

                    dgvKhoHang.DataSource = danhSachKhoVoiSTT;
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
                    // [CẬP NHẬT] Tìm kiếm cũng bao gồm sản phẩm có số lượng = 0
                    var danhSachKho = (from lo in db.LoTonKhos
                                       where lo.SanPham.TenSanPham.ToLower().Contains(keyword) ||
                                             (lo.SoLo != null && lo.SoLo.ToLower().Contains(keyword))
                                       group lo by new { lo.MaSanPham, lo.SanPham.TenSanPham } into g
                                       select new
                                       {
                                           TenThuoc = g.Key.TenSanPham,
                                           SoLo = g.Count() + " lô",
                                           HanDung = (DateTime?)g.Min(x => x.HSD), // Cast to nullable
                                           SoLuong = g.Sum(x => x.SoLuongTon ?? 0)
                                       })
                                       .ToList();

                    // [MỚI] Tìm kiếm cả sản phẩm chưa có trong kho
                    var sanPhamChuaCoKho = (from sp in db.SanPhams
                                            where sp.TenSanPham.ToLower().Contains(keyword) &&
                                                  !db.LoTonKhos.Any(lo => lo.MaSanPham == sp.MaSanPham)
                                            select new
                                            {
                                                TenThuoc = sp.TenSanPham,
                                                SoLo = "0 lô",
                                                HanDung = (DateTime?)null,
                                                SoLuong = 0
                                            })
                                            .ToList();

                    // Gộp 2 danh sách
                    var danhSachKhoDay = danhSachKho
                        .Concat(sanPhamChuaCoKho)
                        .OrderBy(x => x.TenThuoc)
                        .ToList();

                    // Thêm STT sau khi đã ToList()
                    var danhSachKhoVoiSTT = danhSachKhoDay.Select((l, index) => new
                    {
                        STT = index + 1,
                        l.TenThuoc,
                        l.SoLo,
                        HanDung = l.HanDung.HasValue ? l.HanDung.Value.ToString("dd/MM/yyyy") : "N/A",
                        l.SoLuong
                    }).ToList();

                    dgvKhoHang.DataSource = danhSachKhoVoiSTT;
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