using QLNhaThuoc.Database;
using QLNhaThuoc.Database.Entities;
using QLNhaThuoc.Helper;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace QLNhaThuoc.Form
{
    public partial class UC_ChiTietPhieuNhap : UserControl
    {
        private bool isEditMode = false;
        private string trangThaiCu = ""; // Lưu trạng thái cũ để so sánh

        // [MỚI] Event để thông báo khi dữ liệu được lưu thành công
        public event EventHandler DataSaved;

        public UC_ChiTietPhieuNhap()
        {
            InitializeComponent();
            LoadInitData();
            SetupEvents();
        }

        private void LoadInitData()
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    cboNhaCungCap.DataSource = db.NhaCungCaps.Select(x => new { x.MaNCC, x.TenNCC }).ToList();
                    cboNhaCungCap.DisplayMember = "TenNCC";
                    cboNhaCungCap.ValueMember = "MaNCC";
                    cboNhaCungCap.SelectedIndex = -1;

                    cboThuoc.DataSource = db.SanPhams.Select(x => new { x.MaSanPham, x.TenSanPham }).ToList();
                    cboThuoc.DisplayMember = "TenSanPham";
                    cboThuoc.ValueMember = "MaSanPham";
                    cboThuoc.SelectedIndex = -1;

                    // [MỚI] Load ComboBox Trạng Thái
                    cboTrangThai.Items.Clear();
                    cboTrangThai.Items.Add("Chờ duyệt");
                    cboTrangThai.Items.Add("Đã nhập kho");

                    // [CẬP NHẬT] Hiển thị thông tin nhân viên đăng nhập
                    if (UserSession.IsLoggedIn)
                    {
                        txtNhanVien.Text = UserSession.TenNhanVien;
                        txtNhanVien.ReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên đăng nhập!", "Cảnh báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu danh mục: " + ex.Message);
            }
        }

        public void SetMaPhieu(string maPhieu)
        {
            ResetForm();

            if (string.IsNullOrEmpty(maPhieu))
            {
                // --- THÊM MỚI ---
                isEditMode = false;
                lblTitle.Text = "THÊM MỚI PHIẾU NHẬP";
                txtMaPhieu.Text = TaoMaPhieuTuDong();
                dtpNgayNhap.Value = DateTime.Now;

                // [CẬP NHẬT] Phân quyền trạng thái dựa trên vai trò
                // Nhân viên không phải Quản lý: luôn là "Chờ duyệt" và không được sửa
                if (UserSession.VaiTro != "Quản lý")
                {
                    cboTrangThai.SelectedIndex = 0; // Chờ duyệt
                    cboTrangThai.Enabled = false;
                }
                else
                {
                    // Quản lý có thể chọn trạng thái
                    cboTrangThai.SelectedIndex = 0; // Mặc định là Chờ duyệt
                    cboTrangThai.Enabled = true;
                }
            }
            else
            {
                // --- SỬA ---
                isEditMode = true;
                lblTitle.Text = "CẬP NHẬT PHIẾU NHẬP";
                txtMaPhieu.Text = maPhieu;
                txtMaPhieu.ReadOnly = true;

                LoadDataToForm(maPhieu);
            }
        }

        private void LoadDataToForm(string maPhieu)
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    var pn = db.PhieuNhaps.Include("ChiTietPhieuNhaps")
                                          .FirstOrDefault(x => x.MaPhieuNhap == maPhieu);

                    if (pn != null)
                    {
                        dtpNgayNhap.Value = pn.NgayNhap ?? DateTime.Now;
                        
                        // [CẬP NHẬT] Hiển thị tên nhân viên từ database
                        var nhanVien = db.NhanViens.Find(pn.MaNhanVien);
                        if (nhanVien != null)
                        {
                            txtNhanVien.Text = nhanVien.HoTen;
                        }
                        
                        cboNhaCungCap.SelectedValue = pn.MaNCC;
                        
                        // [MỚI] Load trạng thái vào ComboBox
                        trangThaiCu = pn.TrangThai ?? "Chờ duyệt";
                        int indexTrangThai = cboTrangThai.Items.IndexOf(trangThaiCu);
                        if (indexTrangThai >= 0)
                        {
                            cboTrangThai.SelectedIndex = indexTrangThai;
                        }

                        // [MỚI] Kiểm tra quyền và trạng thái để enable/disable controls
                        if (trangThaiCu == "Đã nhập kho")
                        {
                            // Nếu đã nhập kho -> không cho sửa chi tiết dù là ai
                            DisableAllControls();
                            MessageBox.Show("Phiếu nhập đã được nhập kho, không thể chỉnh sửa!", 
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Nếu chưa nhập kho -> phân quyền
                            if (UserSession.VaiTro == "Quản lý")
                            {
                                cboTrangThai.Enabled = true;
                            }
                            else
                            {
                                cboTrangThai.Enabled = false; // Nhân viên không được đổi trạng thái
                            }
                        }

                        var details = (from ct in pn.ChiTietPhieuNhaps
                                       join sp in db.SanPhams on ct.MaSanPham equals sp.MaSanPham
                                       select new { ct.MaSanPham, sp.TenSanPham, ct.SoLuong, ct.DonGia }).ToList();

                        foreach (var item in details)
                        {
                            decimal thanhTien = (decimal)item.SoLuong * item.DonGia;
                            dgvChiTiet.Rows.Add(item.MaSanPham, item.TenSanPham, item.SoLuong, item.DonGia, thanhTien);
                        }
                        TinhTongTien();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu chi tiết: " + ex.Message);
            }
        }

        // [MỚI] Disable tất cả controls khi phiếu đã nhập kho
        private void DisableAllControls()
        {
            cboNhaCungCap.Enabled = false;
            cboThuoc.Enabled = false;
            txtSoLuong.Enabled = false;
            txtDonGia.Enabled = false;
            btnThemSP.Enabled = false;
            btnLuu.Enabled = false;
            cboTrangThai.Enabled = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.AllowUserToDeleteRows = false;
            
            // Ẩn cột xóa
            if (colXoa != null)
            {
                colXoa.Visible = false;
            }
        }

        private void SetupEvents()
        {
            btnQuayLai.Click += (s, e) => {
                if (this.Parent != null) this.Parent.Controls.Remove(this);
            };

            btnThemSP.Click += (s, e) => {
                ThemSanPhamVaoLuoi();
            };

            dgvChiTiet.CellContentClick += (s, e) => {
                if (e.RowIndex >= 0 && e.ColumnIndex == colXoa.Index)
                {
                    dgvChiTiet.Rows.RemoveAt(e.RowIndex);
                    TinhTongTien();
                }
            };

            // [MỚI] Sự kiện chọn dòng trong DataGridView để hiển thị lên các control
            dgvChiTiet.CellClick += (s, e) => {
                if (e.RowIndex >= 0 && e.RowIndex < dgvChiTiet.Rows.Count)
                {
                    HienThiThongTinChiTiet(e.RowIndex);
                }
            };

            btnLuu.Click += (s, e) => {
                LuuDuLieuVaoDB();
            };
        }

        // [MỚI] Hiển thị thông tin chi tiết từ dòng đã chọn
        private void HienThiThongTinChiTiet(int rowIndex)
        {
            try
            {
                DataGridViewRow row = dgvChiTiet.Rows[rowIndex];
                
                // Lấy thông tin từ dòng đã chọn
                string maSP = row.Cells[colMaSP.Index].Value?.ToString();
                string tenSP = row.Cells[colTenSP.Index].Value?.ToString();
                string soLuong = row.Cells[colSoLuong.Index].Value?.ToString();
                string donGia = row.Cells[colDonGia.Index].Value?.ToString();

                // Hiển thị lên các control
                if (!string.IsNullOrEmpty(maSP))
                {
                    cboThuoc.SelectedValue = maSP;
                }
                
                txtSoLuong.Text = soLuong ?? "";
                txtDonGia.Text = donGia ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị thông tin: " + ex.Message);
            }
        }

        private void ThemSanPhamVaoLuoi()
        {
            if (cboThuoc.SelectedValue == null) return;

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0"); return;
            }
            if (!decimal.TryParse(txtDonGia.Text, out decimal donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ"); return;
            }

            string maSP = cboThuoc.SelectedValue.ToString();
            string tenSP = cboThuoc.Text;
            decimal thanhTien = soLuong * donGia;

            bool daCo = false;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.Cells[colMaSP.Index].Value?.ToString() == maSP)
                {
                    int slCu = int.Parse(row.Cells[colSoLuong.Index].Value.ToString());
                    row.Cells[colSoLuong.Index].Value = slCu + soLuong;
                    row.Cells[colThanhTien.Index].Value = (slCu + soLuong) * donGia;
                    daCo = true;
                    break;
                }
            }

            if (!daCo)
            {
                dgvChiTiet.Rows.Add(maSP, tenSP, soLuong, donGia, thanhTien);
            }

            TinhTongTien();
            txtSoLuong.Text = "";
            txtDonGia.Text = "";
            cboThuoc.Focus();
        }

        private void TinhTongTien()
        {
            decimal tong = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.Cells[colThanhTien.Index].Value != null)
                    tong += decimal.Parse(row.Cells[colThanhTien.Index].Value.ToString());
            }
            txtTongTien.Text = tong.ToString("N0");
        }

        private void LuuDuLieuVaoDB()
        {
            // [CẬP NHẬT] Kiểm tra đăng nhập
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập trước khi thực hiện chức năng này!", "Cảnh báo");
                return;
            }

            if (cboNhaCungCap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp!"); return;
            }
            if (dgvChiTiet.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng nhập ít nhất 1 sản phẩm!"); return;
            }

            // [MỚI] Kiểm tra trạng thái
            string trangThaiMoi = cboTrangThai.SelectedItem?.ToString() ?? "Chờ duyệt";

            try
            {
                using (var db = new DbThuocContext())
                {
                    PhieuNhap pn;

                    if (isEditMode)
                    {
                        pn = db.PhieuNhaps.Find(txtMaPhieu.Text);
                        if (pn == null) { MessageBox.Show("Phiếu nhập không tồn tại!"); return; }

                        pn.NgayNhap = dtpNgayNhap.Value;
                        pn.MaNCC = cboNhaCungCap.SelectedValue.ToString();
                        pn.MaNhanVien = UserSession.MaNhanVien;
                        
                        // [CẬP NHẬT] Trạng thái: Quản lý có thể sửa, nhân viên giữ nguyên
                        if (UserSession.VaiTro == "Quản lý")
                        {
                            pn.TrangThai = trangThaiMoi;
                        }

                        var oldDetails = db.ChiTietPhieuNhaps.Where(x => x.MaPhieuNhap == pn.MaPhieuNhap);
                        db.ChiTietPhieuNhaps.RemoveRange(oldDetails);
                    }
                    else
                    {
                        pn = new PhieuNhap();
                        pn.MaPhieuNhap = txtMaPhieu.Text;
                        pn.NgayNhap = dtpNgayNhap.Value;
                        pn.MaNCC = cboNhaCungCap.SelectedValue.ToString();
                        pn.MaNhanVien = UserSession.MaNhanVien;

                        // [CẬP NHẬT] Trạng thái khi thêm mới
                        if (UserSession.VaiTro == "Quản lý")
                        {
                            pn.TrangThai = trangThaiMoi;
                        }
                        else
                        {
                            pn.TrangThai = "Chờ duyệt"; // Nhân viên luôn là Chờ duyệt
                        }

                        db.PhieuNhaps.Add(pn);
                    }

                    decimal tongTienDB = 0;
                    foreach (DataGridViewRow row in dgvChiTiet.Rows)
                    {
                        if (row.IsNewRow) continue;
                        var ct = new ChiTietPhieuNhap();
                        ct.MaPhieuNhap = pn.MaPhieuNhap;
                        ct.MaSanPham = row.Cells[colMaSP.Index].Value.ToString();
                        ct.SoLuong = int.Parse(row.Cells[colSoLuong.Index].Value.ToString());
                        ct.DonGia = decimal.Parse(row.Cells[colDonGia.Index].Value.ToString());

                        db.ChiTietPhieuNhaps.Add(ct);
                        tongTienDB += (ct.SoLuong * ct.DonGia);
                    }
                    pn.TongTien = tongTienDB;

                    // [MỚI] Nếu trạng thái là "Đã nhập kho", cập nhật tồn kho
                    if (pn.TrangThai == "Đã nhập kho")
                    {
                        CapNhatTonKho(db, pn.MaPhieuNhap);
                    }

                    db.SaveChanges();
                    MessageBox.Show("Lưu dữ liệu thành công!");

                    // [MỚI] Kích hoạt event để thông báo dữ liệu đã được lưu
                    OnDataSaved();
                    
                    if (this.Parent != null) this.Parent.Controls.Remove(this);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Lỗi khi lưu: " + msg);
            }
        }

        // [MỚI] Cập nhật tồn kho khi phiếu nhập được duyệt
        private void CapNhatTonKho(DbThuocContext db, string maPhieuNhap)
        {
            try
            {
                var chiTietPhieuNhap = db.ChiTietPhieuNhaps.Where(ct => ct.MaPhieuNhap == maPhieuNhap).ToList();

                foreach (var ct in chiTietPhieuNhap)
                {
                    // Kiểm tra xem đã có lô tồn kho cho sản phẩm này chưa
                    var loTonKho = db.LoTonKhos
                        .Where(l => l.MaSanPham == ct.MaSanPham)
                        .OrderByDescending(l => l.NgayNhap)
                        .FirstOrDefault();

                    if (loTonKho != null)
                    {
                        // Nếu đã có, tăng số lượng tồn
                        loTonKho.SoLuongTon = (loTonKho.SoLuongTon ?? 0) + ct.SoLuong;
                        loTonKho.NgayNhap = DateTime.Now;
                    }
                    else
                    {
                        // Nếu chưa có, tạo mới lô tồn kho
                        var loMoi = new LoTonKho
                        {
                            MaLoTonKho = TaoMaLoTonKho(db),
                            MaSanPham = ct.MaSanPham,
                            SoLo = "LO" + DateTime.Now.ToString("yyMMdd"),
                            HSD = DateTime.Now.AddYears(2), // Mặc định HSD 2 năm
                            SoLuongTon = ct.SoLuong,
                            NgayNhap = DateTime.Now
                        };
                        db.LoTonKhos.Add(loMoi);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật tồn kho: " + ex.Message);
            }
        }

        // [MỚI] Tạo mã lô tồn kho tự động
        private string TaoMaLoTonKho(DbThuocContext db)
        {
            try
            {
                var lastLo = db.LoTonKhos
                    .OrderByDescending(l => l.MaLoTonKho)
                    .FirstOrDefault();

                if (lastLo != null && lastLo.MaLoTonKho.Length > 2)
                {
                    string numPart = lastLo.MaLoTonKho.Substring(2);
                    if (int.TryParse(numPart, out int number))
                    {
                        return "LO" + (number + 1).ToString("D6");
                    }
                }

                return "LO000001";
            }
            catch
            {
                return "LO" + DateTime.Now.ToString("yyMMddHHmmss");
            }
        }

        // [MỚI] Phương thức để kích hoạt event DataSaved
        protected virtual void OnDataSaved()
        {
            DataSaved?.Invoke(this, EventArgs.Empty);
        }

        // -------------------------------------------------------------------------
        // HÀM SINH MÃ TỰ ĐỘNG (ĐÃ CẬP NHẬT: PN001, PN002...)
        // -------------------------------------------------------------------------
        private string TaoMaPhieuTuDong()
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    // Lấy phiếu có mã lớn nhất
                    var lastPN = db.PhieuNhaps
                                   .OrderByDescending(x => x.MaPhieuNhap)
                                   .FirstOrDefault();

                    if (lastPN != null)
                    {
                        // Cắt lấy phần số sau chữ "PN"
                        string numPart = lastPN.MaPhieuNhap.Substring(2);
                        if (int.TryParse(numPart, out int number))
                        {
                            // Cộng 1 và định dạng lại thành chuỗi (D3 = 001, 002...)
                            return "PN" + (number + 1).ToString("D3");
                        }
                    }

                    // Nếu chưa có hoặc lỗi format cũ -> Reset về PN001
                    return "PN001";
                }
            }
            catch
            {
                return "PN001"; // Fallback an toàn
            }
        }

        private void ResetForm()
        {
            txtMaPhieu.Clear();
            cboNhaCungCap.SelectedIndex = -1;
            dgvChiTiet.Rows.Clear();
            txtTongTien.Text = "0";
            txtSoLuong.Text = "";
            txtDonGia.Text = "";
            
            // [MỚI] Reset ComboBox trạng thái
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.Enabled = true;
            
            // Reset lại các controls
            cboNhaCungCap.Enabled = true;
            cboThuoc.Enabled = true;
            txtSoLuong.Enabled = true;
            txtDonGia.Enabled = true;
            btnThemSP.Enabled = true;
            btnLuu.Enabled = true;
            dgvChiTiet.ReadOnly = false;
            dgvChiTiet.AllowUserToDeleteRows = true;
            if (colXoa != null)
            {
                colXoa.Visible = true;
            }
            
            // [CẬP NHẬT] Hiển thị lại tên nhân viên đăng nhập
            if (UserSession.IsLoggedIn)
            {
                txtNhanVien.Text = UserSession.TenNhanVien;
            }
        }

    }
}