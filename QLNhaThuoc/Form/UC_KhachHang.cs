using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QLNhaThuoc.Database;
using QLNhaThuoc.Database.Entities;
using QLNhaThuoc.Helper;

namespace QLNhaThuoc.Form
{
    public partial class UC_KhachHang : UserControl
    {
        private string currentMaKH = null; // Biến lưu mã KH đang chọn (null = chế độ thêm mới)
        private bool isEditMode = false; // Biến xác định chế độ sửa hay thêm mới

        public UC_KhachHang()
        {
            InitializeComponent();
            SetupEvents();
            SetupPlaceholder();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.DesignMode)
            {
                ConfigGrid();
                LoadData();
                SetFormReadOnly(true); // Mặc định form ở chế độ chỉ đọc
            }
        }

        private void ConfigGrid()
        {
            dgvKhachHang.AutoGenerateColumns = false;
            // Map tên cột Designer với tên cột trong Database
            if (dgvKhachHang.Columns["colMaKH"] != null) dgvKhachHang.Columns["colMaKH"].DataPropertyName = "MaKhachHang";
            if (dgvKhachHang.Columns["colHoTen"] != null) dgvKhachHang.Columns["colHoTen"].DataPropertyName = "TenKhachHang";
            if (dgvKhachHang.Columns["colGioiTinh"] != null) dgvKhachHang.Columns["colGioiTinh"].DataPropertyName = "GioiTinh";
            if (dgvKhachHang.Columns["colSDT"] != null) dgvKhachHang.Columns["colSDT"].DataPropertyName = "SDT";
            if (dgvKhachHang.Columns["colSoLanMua"] != null) dgvKhachHang.Columns["colSoLanMua"].DataPropertyName = "SoLanMua";
        }

        private void LoadData()
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    var listKH = db.KhachHangs
                        .Select(kh => new
                        {
                            kh.MaKhachHang,
                            kh.TenKhachHang,
                            kh.GioiTinh,
                            kh.SDT,
                            kh.SoLanMua
                        })
                        .OrderByDescending(x => x.MaKhachHang)
                        .ToList();

                    dgvKhachHang.DataSource = listKH;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupPlaceholder()
        {
            // Placeholder cho ô tìm kiếm
            txtTimKiem.Enter += (s, e) =>
            {
                if (txtTimKiem.Text == "Tìm kiếm khách hàng")
                {
                    txtTimKiem.Text = "";
                    txtTimKiem.ForeColor = System.Drawing.Color.Black;
                }
            };

            txtTimKiem.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    txtTimKiem.Text = "Tìm kiếm khách hàng";
                    txtTimKiem.ForeColor = System.Drawing.Color.Gray;
                }
            };

            // Cho phép tìm kiếm khi nhấn Enter
            txtTimKiem.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnTimKiem_Click(s, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
        }

        private void SetupEvents()
        {
            // Click Grid -> Đổ dữ liệu lên form để xem thông tin
            dgvKhachHang.CellClick += dgvKhachHang_CellClick;

            // Gán sự kiện cho các nút
            if (btnThem != null) btnThem.Click += btnThem_Click;
            if (btnSua != null) btnSua.Click += btnSua_Click;
            if (btnXoa != null) btnXoa.Click += btnXoa_Click;
            if (btnLuu != null) btnLuu.Click += btnLuu_Click;
            if (btnHuy != null) btnHuy.Click += btnHuy_Click;
            if (btnTimKiem != null) btnTimKiem.Click += btnTimKiem_Click;
            if (btnXuat != null) btnXuat.Click += btnXuat_Click;
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvKhachHang.Rows.Count)
            {
                var item = dgvKhachHang.Rows[e.RowIndex].DataBoundItem;
                dynamic data = item;

                currentMaKH = data.MaKhachHang;
                txtHoTen.Text = data.TenKhachHang;
                txtSDT.Text = data.SDT;
                txtSoLanMua.Text = data.SoLanMua?.ToString() ?? "0";

                if (data.GioiTinh == "Nam") radNam.Checked = true;
                else radNu.Checked = true;

                // Nếu đang không ở chế độ thêm/sửa, giữ form ở chế độ chỉ đọc
                if (!isEditMode)
                {
                    SetFormReadOnly(true);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Chuyển sang chế độ thêm mới
            isEditMode = true;
            currentMaKH = null;
            ClearInputs();
            SetFormReadOnly(false);
            txtHoTen.Focus();
            grpThongTin.Text = "Thông tin khách hàng - THÊM MỚI";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentMaKH))
            {
                MessageBox.Show("Vui lòng chọn khách hàng trên bảng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển sang chế độ sửa
            isEditMode = true;
            SetFormReadOnly(false);
            txtHoTen.Focus();
            grpThongTin.Text = "Thông tin khách hàng - SỬA ĐỔI";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                MessageBox.Show("Vui lòng nhấn nút 'Thêm khách hàng mới' hoặc 'Sửa thông tin' trước khi lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // Validate số điện thoại
            if (!ValidatePhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập 10-11 số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            try
            {
                using (var db = new DbThuocContext())
                {
                    KhachHang kh;

                    // TRƯỜNG HỢP 1: THÊM MỚI
                    if (string.IsNullOrEmpty(currentMaKH))
                    {
                        kh = new KhachHang();
                        kh.MaKhachHang = GenerateNewMaKH(db);
                        kh.SoLanMua = 0;
                        db.KhachHangs.Add(kh);
                    }
                    // TRƯỜNG HỢP 2: CẬP NHẬT
                    else
                    {
                        kh = db.KhachHangs.Find(currentMaKH);
                        if (kh == null)
                        {
                            MessageBox.Show("Khách hàng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Gán thông tin
                    kh.TenKhachHang = txtHoTen.Text.Trim();
                    kh.SDT = txtSDT.Text.Trim();
                    kh.GioiTinh = radNam.Checked ? "Nam" : "Nữ";

                    db.SaveChanges();

                    MessageBox.Show(
                        string.IsNullOrEmpty(currentMaKH) ? "Thêm mới khách hàng thành công!" : "Cập nhật thông tin thành công!",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    LoadData();
                    ClearInputs();
                    SetFormReadOnly(true);
                    isEditMode = false;
                    grpThongTin.Text = "Thông tin khách hàng";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                var result = MessageBox.Show("Bạn có muốn hủy thao tác đang thực hiện?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    ClearInputs();
                    SetFormReadOnly(true);
                    isEditMode = false;
                    grpThongTin.Text = "Thông tin khách hàng";
                }
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            
            // Bỏ qua nếu là placeholder
            if (keyword == "Tìm kiếm khách hàng" || string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            keyword = keyword.ToLower();

            try
            {
                using (var db = new DbThuocContext())
                {
                    var ketQua = db.KhachHangs
                        .Where(kh => kh.TenKhachHang.ToLower().Contains(keyword) ||
                                     kh.SDT.Contains(keyword) ||
                                     kh.MaKhachHang.ToLower().Contains(keyword))
                        .Select(kh => new
                        {
                            kh.MaKhachHang,
                            kh.TenKhachHang,
                            kh.GioiTinh,
                            kh.SDT,
                            kh.SoLanMua
                        })
                        .ToList();

                    dgvKhachHang.DataSource = ketQua;

                    if (ketQua.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentMaKH))
            {
                MessageBox.Show("Vui lòng chọn khách hàng trên bảng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng '{txtHoTen.Text}'?\n\nLưu ý: Không thể xóa nếu khách hàng đã có lịch sử mua hàng!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var db = new DbThuocContext())
                    {
                        var kh = db.KhachHangs.Find(currentMaKH);
                        if (kh != null)
                        {
                            db.KhachHangs.Remove(kh);
                            db.SaveChanges();
                            MessageBox.Show("Đã xóa khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearInputs();
                            SetFormReadOnly(true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Không thể xóa khách hàng này!\n\nNguyên nhân: Khách hàng đã có lịch sử mua hàng trong hệ thống.\n\nChi tiết lỗi: " + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có dữ liệu không
                if (dgvKhachHang.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xuất file Excel (.xls) trực tiếp
                ExcelExporter.ExportToExcel(
                    dgvKhachHang,
                    "DanhSachKhachHang",
                    "DANH SÁCH KHÁCH HÀNG"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            currentMaKH = null;
            txtHoTen.Clear();
            txtSDT.Clear();
            txtSoLanMua.Text = "0";
            radNam.Checked = true;
            dgvKhachHang.ClearSelection();
        }

        private void SetFormReadOnly(bool isReadOnly)
        {
            txtHoTen.ReadOnly = isReadOnly;
            txtSDT.ReadOnly = isReadOnly;
            radNam.Enabled = !isReadOnly;
            radNu.Enabled = !isReadOnly;
            
            if (isReadOnly)
            {
                txtHoTen.BackColor = System.Drawing.Color.WhiteSmoke;
                txtSDT.BackColor = System.Drawing.Color.WhiteSmoke;
            }
            else
            {
                txtHoTen.BackColor = System.Drawing.Color.White;
                txtSDT.BackColor = System.Drawing.Color.White;
            }
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            // Kiểm tra số điện thoại Việt Nam (10-11 số)
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            
            phoneNumber = phoneNumber.Trim();
            
            // Chỉ chấp nhận số
            if (!phoneNumber.All(char.IsDigit)) return false;
            
            // Độ dài 10-11 số
            if (phoneNumber.Length < 10 || phoneNumber.Length > 11) return false;
            
            return true;
        }

        // Hàm sinh mã KH dạng KH001, KH002...
        private string GenerateNewMaKH(DbThuocContext db)
        {
            var maxKH = db.KhachHangs
                          .OrderByDescending(x => x.MaKhachHang)
                          .FirstOrDefault();

            if (maxKH != null)
            {
                string phanSo = maxKH.MaKhachHang.Substring(2);
                if (int.TryParse(phanSo, out int soCu))
                {
                    return "KH" + (soCu + 1).ToString("D3");
                }
            }
            return "KH001";
        }
    }
}