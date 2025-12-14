using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using QLNhaThuoc.Database;
using QLNhaThuoc.Database.Entities;
using QLNhaThuoc.Helper;

namespace QLNhaThuoc.Form
{
    public partial class UC_BanThuoc : UserControl
    {
        public UC_BanThuoc()
        {
            InitializeComponent();
            LoadInitialData();
            SetupEvents();
        }

        private void LoadInitialData()
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    // 1. Load Autocomplete cho tên thuốc
                    var danhSachThuoc = db.SanPhams
                        .Where(s => s.TrangThai == "Đang kinh doanh")
                        .Select(s => s.TenSanPham)
                        .ToArray();

                    if (txtTimKiem != null)
                    {
                        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
                        source.AddRange(danhSachThuoc);
                        txtTimKiem.AutoCompleteCustomSource = source;
                        txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    }

                    // 2. Sinh mã hóa đơn tự động
                    GenerateNewMaHD(db);

                    // 3. Set ngày lập = hôm nay và ReadOnly
                    if (dtpNgayLap != null)
                    {
                        dtpNgayLap.Value = DateTime.Now;
                        dtpNgayLap.Enabled = false; // ReadOnly
                    }

                    // 4. Load ComboBox hình thức thanh toán
                    if (cboHinhThuc != null)
                    {
                        cboHinhThuc.Items.Clear();
                        cboHinhThuc.Items.Add(new { Text = "Tiền mặt", Value = 0 });
                        cboHinhThuc.Items.Add(new { Text = "Chuyển khoản", Value = 1 });
                        cboHinhThuc.DisplayMember = "Text";
                        cboHinhThuc.ValueMember = "Value";
                        cboHinhThuc.SelectedIndex = 0;
                    }

                    // 5. Hiển thị tên nhân viên đã đăng nhập
                    if (txtTenNhanVien != null)
                    {
                        if (UserSession.IsLoggedIn)
                        {
                            txtTenNhanVien.Text = UserSession.TenNhanVien;
                            txtTenNhanVien.ReadOnly = true;
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin nhân viên đăng nhập!", "Cảnh báo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi");
            }
        }

        private void GenerateNewMaHD(DbThuocContext db)
        {
            try
            {
                var lastBill = db.HoaDons
                    .OrderByDescending(h => h.MaHoaDon)
                    .FirstOrDefault();

                string newCode = "HD001";

                if (lastBill != null)
                {
                    string numberPart = lastBill.MaHoaDon.Substring(2);
                    if (int.TryParse(numberPart, out int number))
                    {
                        newCode = "HD" + (number + 1).ToString("D3");
                    }
                }

                if (txtMaHD != null)
                {
                    txtMaHD.Text = newCode;
                    txtMaHD.ReadOnly = true;
                }
            }
            catch
            {
                txtMaHD.Text = "HD" + DateTime.Now.ToString("yyMMddHHmmss");
            }
        }

        private void SetupEvents()
        {
            // 1. Sự kiện tìm kiếm SĐT khách hàng
            if (txtSDT != null)
            {
                txtSDT.TextChanged += TxtSDT_TextChanged;
            }

            // 2. Thêm thuốc vào giỏ
            if (btnThemThuoc != null)
            {
                btnThemThuoc.Click += (s, e) => ThemThuocVaoGio();
            }

            if (txtTimKiem != null)
            {
                txtTimKiem.KeyDown += (s, e) => {
                    if (e.KeyCode == Keys.Enter)
                    {
                        ThemThuocVaoGio();
                        e.SuppressKeyPress = true;
                    }
                };

                // Placeholder
                txtTimKiem.Enter += (s, e) => {
                    if (txtTimKiem.ForeColor == Color.Gray)
                    {
                        txtTimKiem.Text = "";
                        txtTimKiem.ForeColor = Color.Black;
                    }
                };
                txtTimKiem.Leave += (s, e) => {
                    if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
                    {
                        txtTimKiem.Text = "Nhập tên thuốc...";
                        txtTimKiem.ForeColor = Color.Gray;
                    }
                };
            }

            // 3. Xử lý Grid
            if (dgvGioHang != null)
            {
                dgvGioHang.CellValueChanged += (s, e) => {
                    if (e.RowIndex >= 0 && e.ColumnIndex == colSL.Index)
                    {
                        TinhLaiThanhTienRow(e.RowIndex);
                        CapNhatTongTien();
                    }
                };

                dgvGioHang.CellContentClick += (s, e) => {
                    // Kiểm tra nếu click vào cột Xóa
                    if (e.RowIndex >= 0 && e.ColumnIndex == colXoa.Index)
                    {
                        // Kiểm tra nếu là dòng trống (new row) hoặc không có dữ liệu
                        DataGridViewRow row = dgvGioHang.Rows[e.RowIndex];
                        if (row.IsNewRow)
                        {
                            // Không xóa dòng trống
                            return;
                        }

                        // Kiểm tra nếu dòng có dữ liệu
                        if (row.Cells[colTenThuoc.Index].Value == null || 
                            string.IsNullOrWhiteSpace(row.Cells[colTenThuoc.Index].Value.ToString()))
                        {
                            // Không xóa dòng không có tên thuốc
                            return;
                        }

                        // Xác nhận xóa
                        var result = MessageBox.Show(
                            $"Bạn có chắc muốn xóa thuốc '{row.Cells[colTenThuoc.Index].Value}' khỏi giỏ hàng?",
                            "Xác nhận xóa",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            dgvGioHang.Rows.RemoveAt(e.RowIndex);
                            CapNhatSTT();
                            CapNhatTongTien();
                        }
                    }
                };

                dgvGioHang.EditingControlShowing += (s, e) => {
                    if (dgvGioHang.CurrentCell.ColumnIndex == colSL.Index && e.Control is TextBox tb)
                    {
                        tb.KeyPress -= CheckNumericOnly;
                        tb.KeyPress += CheckNumericOnly;
                    }
                };
            }

            // 4. Giảm giá thay đổi
            if (txtGiamGia != null)
            {
                txtGiamGia.TextChanged += (s, e) => CapNhatTongTien();
            }

            // 5. Nút Hủy
            if (btnHuy != null)
            {
                btnHuy.Click += (s, e) => ResetForm();
            }

            // 6. Nút Tạo hóa đơn
            if (btnTaoHD != null)
            {
                btnTaoHD.Click += (s, e) => XuLyThanhToan();
            }
        }

        private void TxtSDT_TextChanged(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text.Trim();

            // Nếu nhập "000" -> Khách vãng lai
            if (sdt == "000")
            {
                txtTenKH.Text = "Khách vãng lai";
                txtTenKH.ReadOnly = true;
                return;
            }

            // Nếu nhập đủ 10 số -> Tìm khách hàng
            if (sdt.Length == 10)
            {
                try
                {
                    using (var db = new DbThuocContext())
                    {
                        var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == sdt);
                        if (khachHang != null)
                        {
                            txtTenKH.Text = khachHang.TenKhachHang;
                            txtTenKH.ReadOnly = true;
                        }
                        else
                        {
                            // Không tìm thấy -> Cho phép nhập tên mới
                            txtTenKH.Text = "";
                            txtTenKH.ReadOnly = false;
                            MessageBox.Show("Không tìm thấy khách hàng. Vui lòng nhập thông tin mới.", "Thông báo");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tìm khách hàng: " + ex.Message);
                }
            }
            else
            {
                // Chưa đủ 10 số
                txtTenKH.Clear();
                txtTenKH.ReadOnly = false;
            }
        }

        private void CheckNumericOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void ThemThuocVaoGio()
        {
            string tenThuoc = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(tenThuoc) || tenThuoc == "Nhập tên thuốc...")
            {
                MessageBox.Show("Vui lòng nhập tên thuốc!", "Thông báo");
                return;
            }

            try
            {
                using (var db = new DbThuocContext())
                {
                    var thuoc = db.SanPhams.FirstOrDefault(p => p.TenSanPham == tenThuoc);
                    if (thuoc == null)
                    {
                        MessageBox.Show("Không tìm thấy thuốc!", "Thông báo");
                        return;
                    }

                    // Kiểm tra trùng
                    foreach (DataGridViewRow row in dgvGioHang.Rows)
                    {
                        if (row.IsNewRow) continue;
                        if (row.Cells[colTenThuoc.Index].Value?.ToString() == thuoc.TenSanPham)
                        {
                            int slHienTai = Convert.ToInt32(row.Cells[colSL.Index].Value ?? 0);
                            row.Cells[colSL.Index].Value = slHienTai + 1;
                            TinhLaiThanhTienRow(row.Index);
                            CapNhatTongTien();
                            ResetTimKiem();
                            return;
                        }
                    }

                    // Thêm mới - Đếm số dòng thực tế (không tính dòng trống)
                    int soLuongDongThucTe = 0;
                    foreach (DataGridViewRow row in dgvGioHang.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            soLuongDongThucTe++;
                        }
                    }
                    int stt = soLuongDongThucTe + 1;
                    
                    decimal thanhTien = thuoc.DonGia;
                    dgvGioHang.Rows.Add(stt, thuoc.TenSanPham, thuoc.DonViTinh ?? "Hộp", thuoc.DonGia, 1, thanhTien);

                    CapNhatTongTien();
                    ResetTimKiem();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ResetTimKiem()
        {
            txtTimKiem.Clear();
            txtTimKiem.Focus();
        }

        private void TinhLaiThanhTienRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvGioHang.Rows.Count) return;

            var row = dgvGioHang.Rows[rowIndex];
            decimal donGia = Convert.ToDecimal(row.Cells[colDonGia.Index].Value ?? 0);
            int soLuong = Convert.ToInt32(row.Cells[colSL.Index].Value ?? 0);
            row.Cells[colThanhTien.Index].Value = donGia * soLuong;
        }

        private void CapNhatSTT()
        {
            int stt = 1;
            foreach (DataGridViewRow row in dgvGioHang.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells[colSTT.Index].Value = stt++;
                }
            }
        }

        private void CapNhatTongTien()
        {
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvGioHang.Rows)
            {
                if (!row.IsNewRow)
                {
                    tongTien += Convert.ToDecimal(row.Cells[colThanhTien.Index].Value ?? 0);
                }
            }

            if (txtTongTienThuoc != null)
                txtTongTienThuoc.Text = tongTien.ToString("N0");

            decimal giamGia = 0;
            if (txtGiamGia != null)
                decimal.TryParse(txtGiamGia.Text, out giamGia);

            if (txtTongThanhToan != null)
                txtTongThanhToan.Text = (tongTien - giamGia).ToString("N0") + " VNĐ";
        }

        private void XuLyThanhToan()
        {
            // Validate
            if (dgvGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtSDT.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại khách hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTenKH.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra hình thức thanh toán
            var selectedItem = cboHinhThuc.SelectedItem;
            string hinhThuc = "";
            
            if (selectedItem != null)
            {
                // Lấy text từ anonymous type
                var itemType = selectedItem.GetType();
                var textProperty = itemType.GetProperty("Text");
                if (textProperty != null)
                {
                    hinhThuc = textProperty.GetValue(selectedItem, null)?.ToString() ?? "";
                }
            }

            // Nếu thanh toán bằng chuyển khoản -> Hiển thị QR
            if (hinhThuc == "Chuyển khoản")
            {
                XuLyThanhToanQR();
            }
            else
            {
                // Thanh toán tiền mặt hoặc thẻ -> Xử lý như cũ
                XuLyThanhToanTienMat();
            }
        }

        private void XuLyThanhToanQR()
        {
            // Lấy tổng tiền thanh toán
            decimal tongTien = 0;
            string tongThanhToanStr = txtTongThanhToan.Text.Replace("VNĐ", "").Replace(",", "").Trim();
            if (!decimal.TryParse(tongThanhToanStr, out tongTien))
            {
                MessageBox.Show("Lỗi tính tổng tiền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hiển thị form QR Payment
            using (var qrForm = new FormQRPayment(tongTien))
            {
                var result = qrForm.ShowDialog();

                if (result == DialogResult.OK && qrForm.IsConfirmed)
                {
                    // Người dùng đã xác nhận thanh toán
                    try
                    {
                        LuuHoaDonVaoDB();

                        MessageBox.Show("Thanh toán thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Hỏi xuất hóa đơn
                        var xuatHD = MessageBox.Show("Bạn có muốn xuất hóa đơn?", "Xuất hóa đơn",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (xuatHD == DialogResult.Yes)
                        {
                            XuatHoaDonPDF();
                        }

                        ResetForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Người dùng đã hủy
                    MessageBox.Show("Đã hủy thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void XuLyThanhToanTienMat()
        {
            // Tạo thông tin xác nhận
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== XÁC NHẬN THANH TOÁN ===\n");
            sb.AppendLine($"Số hóa đơn: {txtMaHD.Text}");
            sb.AppendLine($"Khách hàng: {txtTenKH.Text}");
            sb.AppendLine($"SĐT: {txtSDT.Text}");
            sb.AppendLine($"Nhân viên: {txtTenNhanVien.Text}");
            sb.AppendLine($"\n--- Danh sách thuốc ---");

            foreach (DataGridViewRow row in dgvGioHang.Rows)
            {
                if (!row.IsNewRow)
                {
                    string ten = row.Cells[colTenThuoc.Index].Value?.ToString();
                    string sl = row.Cells[colSL.Index].Value?.ToString();
                    string tt = row.Cells[colThanhTien.Index].Value?.ToString();
                    sb.AppendLine($"- {ten} x{sl}: {Convert.ToDecimal(tt):N0} VNĐ");
                }
            }

            sb.AppendLine($"\nTổng tiền thuốc: {txtTongTienThuoc.Text} VNĐ");
            sb.AppendLine($"Giảm giá: {txtGiamGia.Text} VNĐ");
            sb.AppendLine($"Thực trả: {txtTongThanhToan.Text}");

            var result = MessageBox.Show(sb.ToString(), "Xác nhận thanh toán",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    LuuHoaDonVaoDB();

                    MessageBox.Show("Thanh toán thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Hỏi xuất hóa đơn
                    var xuatHD = MessageBox.Show("Bạn có muốn xuất hóa đơn?", "Xuất hóa đơn",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (xuatHD == DialogResult.Yes)
                    {
                        XuatHoaDonPDF();
                    }

                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LuuHoaDonVaoDB()
        {
            using (var db = new DbThuocContext())
            {
                // 1. Xử lý khách hàng
                string sdt = txtSDT.Text.Trim();
                string maKH = null;

                if (sdt != "000")
                {
                    var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == sdt);
                    if (khachHang == null)
                    {
                        // Tạo khách hàng mới
                        string lastMaKH = db.KhachHangs.OrderByDescending(k => k.MaKhachHang).FirstOrDefault()?.MaKhachHang;
                        int newNum = 1;
                        if (lastMaKH != null && lastMaKH.Length > 2)
                        {
                            int.TryParse(lastMaKH.Substring(2), out newNum);
                            newNum++;
                        }
                        maKH = "KH" + newNum.ToString("D3");

                        khachHang = new KhachHang
                        {
                            MaKhachHang = maKH,
                            TenKhachHang = txtTenKH.Text.Trim(),
                            SDT = sdt,
                            GioiTinh = "Khác",
                            SoLanMua = 1
                        };
                        db.KhachHangs.Add(khachHang);
                    }
                    else
                    {
                        maKH = khachHang.MaKhachHang;
                        khachHang.SoLanMua = (khachHang.SoLanMua ?? 0) + 1;
                    }
                }

                // 2. Lưu hóa đơn
                decimal tongTien = decimal.Parse(txtTongTienThuoc.Text.Replace(",", "").Trim());
                decimal giamGia = 0;
                decimal.TryParse(txtGiamGia.Text, out giamGia);

                var hoaDon = new HoaDon
                {
                    MaHoaDon = txtMaHD.Text,
                    NgayLap = DateTime.Now,
                    MaKhachHang = maKH,
                    MaNhanVien = UserSession.MaNhanVien,
                    TongTien = tongTien - giamGia
                    // Bỏ HinhThucThanhToan vì không tồn tại trong entity
                };
                db.HoaDons.Add(hoaDon);

                // 3. Lưu chi tiết
                foreach (DataGridViewRow row in dgvGioHang.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string tenThuoc = row.Cells[colTenThuoc.Index].Value.ToString();
                        var sanPham = db.SanPhams.FirstOrDefault(sp => sp.TenSanPham == tenThuoc);

                        if (sanPham != null)
                        {
                            int soLuong = Convert.ToInt32(row.Cells[colSL.Index].Value);
                            decimal donGia = Convert.ToDecimal(row.Cells[colDonGia.Index].Value);
                            
                            var chiTiet = new ChiTietHoaDon
                            {
                                MaHoaDon = txtMaHD.Text,
                                MaSanPham = sanPham.MaSanPham,
                                SoLuong = soLuong,
                                ThanhTien = soLuong * donGia  // Tính ThanhTien thay vì lưu DonGia
                            };
                            db.ChiTietHoaDons.Add(chiTiet);

                            // Trừ tồn kho qua LoTonKho
                            // Tìm lô tồn kho có HSD xa nhất và còn hàng
                            var loTonKho = db.LoTonKhos
                                .Where(l => l.MaSanPham == sanPham.MaSanPham && l.SoLuongTon > 0)
                                .OrderBy(l => l.HSD)
                                .FirstOrDefault();
                            
                            if (loTonKho != null && loTonKho.SoLuongTon >= soLuong)
                            {
                                loTonKho.SoLuongTon -= soLuong;
                            }
                            // Note: Nếu không đủ tồn kho trong 1 lô, cần logic phức tạp hơn
                            // Hiện tại đơn giản hóa: trừ từ lô đầu tiên tìm được
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        private void XuatHoaDonPDF()
        {
            // Lấy mã hóa đơn vừa tạo
            string maHoaDon = txtMaHD.Text;
            
            // Gọi helper để xuất PDF
            PDFHelper.XuatHoaDonPDF(maHoaDon);
        }

        private void ResetForm()
        {
            dgvGioHang.Rows.Clear();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtGiamGia.Text = "0";
            txtTenKH.ReadOnly = false;

            using (var db = new DbThuocContext())
            {
                GenerateNewMaHD(db);
            }

            CapNhatTongTien();
        }
    }
}