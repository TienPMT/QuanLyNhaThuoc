namespace QLNhaThuoc.Form
{
    partial class UC_BanThuoc
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelTotal = new System.Windows.Forms.Panel();
            this.lblTongTienThuoc = new System.Windows.Forms.Label();
            this.txtTongTienThuoc = new System.Windows.Forms.TextBox();
            this.lblGiamGia = new System.Windows.Forms.Label();
            this.txtGiamGia = new System.Windows.Forms.TextBox();
            this.lblTongThanhToan = new System.Windows.Forms.Label();
            this.txtTongThanhToan = new System.Windows.Forms.TextBox();
            this.btnTaoHD = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.panelCart = new System.Windows.Forms.Panel();
            this.dgvGioHang = new System.Windows.Forms.DataGridView();
            this.colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonVi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXoa = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnThemThuoc = new System.Windows.Forms.Button();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.txtTenNhanVien = new System.Windows.Forms.TextBox();
            this.lblMaHD = new System.Windows.Forms.Label();
            this.txtMaHD = new System.Windows.Forms.TextBox();
            this.lblNgayLap = new System.Windows.Forms.Label();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.lblTenKH = new System.Windows.Forms.Label();
            this.txtTenKH = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.lblHinhThuc = new System.Windows.Forms.Label();
            this.cboHinhThuc = new System.Windows.Forms.ComboBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelTotal.SuspendLayout();
            this.panelCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).BeginInit();
            this.panelSearch.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTotal
            // 
            this.panelTotal.Controls.Add(this.lblTongTienThuoc);
            this.panelTotal.Controls.Add(this.txtTongTienThuoc);
            this.panelTotal.Controls.Add(this.lblGiamGia);
            this.panelTotal.Controls.Add(this.txtGiamGia);
            this.panelTotal.Controls.Add(this.lblTongThanhToan);
            this.panelTotal.Controls.Add(this.txtTongThanhToan);
            this.panelTotal.Controls.Add(this.btnTaoHD);
            this.panelTotal.Controls.Add(this.btnHuy);
            this.panelTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTotal.Location = new System.Drawing.Point(20, 530);
            this.panelTotal.Name = "panelTotal";
            this.panelTotal.Size = new System.Drawing.Size(840, 150);
            this.panelTotal.TabIndex = 3;
            // 
            // lblTongTienThuoc
            // 
            this.lblTongTienThuoc.Location = new System.Drawing.Point(400, 10);
            this.lblTongTienThuoc.Name = "lblTongTienThuoc";
            this.lblTongTienThuoc.Size = new System.Drawing.Size(100, 23);
            this.lblTongTienThuoc.TabIndex = 0;
            this.lblTongTienThuoc.Text = "Tổng tiền thuốc:";
            this.lblTongTienThuoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTongTienThuoc
            // 
            this.txtTongTienThuoc.Location = new System.Drawing.Point(550, 5);
            this.txtTongTienThuoc.Name = "txtTongTienThuoc";
            this.txtTongTienThuoc.ReadOnly = true;
            this.txtTongTienThuoc.Size = new System.Drawing.Size(200, 22);
            this.txtTongTienThuoc.TabIndex = 1;
            this.txtTongTienThuoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblGiamGia
            // 
            this.lblGiamGia.Location = new System.Drawing.Point(400, 40);
            this.lblGiamGia.Name = "lblGiamGia";
            this.lblGiamGia.Size = new System.Drawing.Size(100, 23);
            this.lblGiamGia.TabIndex = 2;
            this.lblGiamGia.Text = "Giảm giá:";
            this.lblGiamGia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGiamGia
            // 
            this.txtGiamGia.Location = new System.Drawing.Point(550, 35);
            this.txtGiamGia.Name = "txtGiamGia";
            this.txtGiamGia.Size = new System.Drawing.Size(200, 22);
            this.txtGiamGia.TabIndex = 3;
            this.txtGiamGia.Text = "0";
            this.txtGiamGia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTongThanhToan
            // 
            this.lblTongThanhToan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTongThanhToan.Location = new System.Drawing.Point(350, 70);
            this.lblTongThanhToan.Name = "lblTongThanhToan";
            this.lblTongThanhToan.Size = new System.Drawing.Size(150, 23);
            this.lblTongThanhToan.TabIndex = 4;
            this.lblTongThanhToan.Text = "Thực trả";
            this.lblTongThanhToan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTongThanhToan
            // 
            this.txtTongThanhToan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtTongThanhToan.Location = new System.Drawing.Point(550, 65);
            this.txtTongThanhToan.Name = "txtTongThanhToan";
            this.txtTongThanhToan.ReadOnly = true;
            this.txtTongThanhToan.Size = new System.Drawing.Size(200, 32);
            this.txtTongThanhToan.TabIndex = 5;
            this.txtTongThanhToan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnTaoHD
            // 
            this.btnTaoHD.BackColor = System.Drawing.Color.LightGray;
            this.btnTaoHD.Location = new System.Drawing.Point(300, 110);
            this.btnTaoHD.Name = "btnTaoHD";
            this.btnTaoHD.Size = new System.Drawing.Size(150, 35);
            this.btnTaoHD.TabIndex = 6;
            this.btnTaoHD.Text = "Thanh toán";
            this.btnTaoHD.UseVisualStyleBackColor = false;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.LightGray;
            this.btnHuy.Location = new System.Drawing.Point(500, 110);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(150, 35);
            this.btnHuy.TabIndex = 7;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // panelCart
            // 
            this.panelCart.Controls.Add(this.dgvGioHang);
            this.panelCart.Controls.Add(this.panelSearch);
            this.panelCart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCart.Location = new System.Drawing.Point(20, 240);
            this.panelCart.Name = "panelCart";
            this.panelCart.Size = new System.Drawing.Size(840, 290);
            this.panelCart.TabIndex = 2;
            // 
            // dgvGioHang
            // 
            this.dgvGioHang.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGioHang.BackgroundColor = System.Drawing.Color.White;
            this.dgvGioHang.ColumnHeadersHeight = 30;
            this.dgvGioHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSTT,
            this.colTenThuoc,
            this.colDonVi,
            this.colDonGia,
            this.colSL,
            this.colThanhTien,
            this.colXoa});
            this.dgvGioHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGioHang.Location = new System.Drawing.Point(0, 50);
            this.dgvGioHang.Name = "dgvGioHang";
            this.dgvGioHang.RowHeadersWidth = 51;
            this.dgvGioHang.Size = new System.Drawing.Size(840, 240);
            this.dgvGioHang.TabIndex = 0;
            // 
            // colSTT
            // 
            this.colSTT.FillWeight = 30F;
            this.colSTT.HeaderText = "STT";
            this.colSTT.MinimumWidth = 6;
            this.colSTT.Name = "colSTT";
            // 
            // colTenThuoc
            // 
            this.colTenThuoc.FillWeight = 150F;
            this.colTenThuoc.HeaderText = "Tên thuốc";
            this.colTenThuoc.MinimumWidth = 6;
            this.colTenThuoc.Name = "colTenThuoc";
            // 
            // colDonVi
            // 
            this.colDonVi.FillWeight = 50F;
            this.colDonVi.HeaderText = "Đơn vị";
            this.colDonVi.MinimumWidth = 6;
            this.colDonVi.Name = "colDonVi";
            // 
            // colDonGia
            // 
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.MinimumWidth = 6;
            this.colDonGia.Name = "colDonGia";
            // 
            // colSL
            // 
            this.colSL.FillWeight = 40F;
            this.colSL.HeaderText = "SL";
            this.colSL.MinimumWidth = 6;
            this.colSL.Name = "colSL";
            // 
            // colThanhTien
            // 
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.MinimumWidth = 6;
            this.colThanhTien.Name = "colThanhTien";
            // 
            // colXoa
            // 
            this.colXoa.FillWeight = 30F;
            this.colXoa.HeaderText = "Xóa";
            this.colXoa.MinimumWidth = 6;
            this.colXoa.Name = "colXoa";
            this.colXoa.Text = "🗑";
            this.colXoa.UseColumnTextForButtonValue = true;
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.txtTimKiem);
            this.panelSearch.Controls.Add(this.btnThemThuoc);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.panelSearch.Size = new System.Drawing.Size(840, 50);
            this.panelSearch.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtTimKiem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.Gray;
            this.txtTimKiem.Location = new System.Drawing.Point(0, 10);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(650, 32);
            this.txtTimKiem.TabIndex = 0;
            this.txtTimKiem.Text = "Nhập tên thuốc...";
            // 
            // btnThemThuoc
            // 
            this.btnThemThuoc.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnThemThuoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemThuoc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThemThuoc.ForeColor = System.Drawing.Color.White;
            this.btnThemThuoc.Location = new System.Drawing.Point(660, 8);
            this.btnThemThuoc.Name = "btnThemThuoc";
            this.btnThemThuoc.Size = new System.Drawing.Size(120, 32);
            this.btnThemThuoc.TabIndex = 1;
            this.btnThemThuoc.Text = "Thêm thuốc";
            this.btnThemThuoc.UseVisualStyleBackColor = false;
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.White;
            this.panelInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInfo.Controls.Add(this.txtTenNhanVien);
            this.panelInfo.Controls.Add(this.lblMaHD);
            this.panelInfo.Controls.Add(this.txtMaHD);
            this.panelInfo.Controls.Add(this.lblNgayLap);
            this.panelInfo.Controls.Add(this.dtpNgayLap);
            this.panelInfo.Controls.Add(this.lblTenKH);
            this.panelInfo.Controls.Add(this.txtTenKH);
            this.panelInfo.Controls.Add(this.lblSDT);
            this.panelInfo.Controls.Add(this.txtSDT);
            this.panelInfo.Controls.Add(this.lblNhanVien);
            this.panelInfo.Controls.Add(this.lblHinhThuc);
            this.panelInfo.Controls.Add(this.cboHinhThuc);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(20, 60);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(840, 180);
            this.panelInfo.TabIndex = 4;
            // 
            // txtTenNhanVien
            // 
            this.txtTenNhanVien.Location = new System.Drawing.Point(120, 121);
            this.txtTenNhanVien.Name = "txtTenNhanVien";
            this.txtTenNhanVien.ReadOnly = true;
            this.txtTenNhanVien.Size = new System.Drawing.Size(200, 22);
            this.txtTenNhanVien.TabIndex = 14;
            // 
            // lblMaHD
            // 
            this.lblMaHD.AutoSize = true;
            this.lblMaHD.Location = new System.Drawing.Point(30, 20);
            this.lblMaHD.Name = "lblMaHD";
            this.lblMaHD.Size = new System.Drawing.Size(79, 16);
            this.lblMaHD.TabIndex = 0;
            this.lblMaHD.Text = "Số hóa đơn:";
            // 
            // txtMaHD
            // 
            this.txtMaHD.Location = new System.Drawing.Point(120, 15);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.ReadOnly = true;
            this.txtMaHD.Size = new System.Drawing.Size(200, 22);
            this.txtMaHD.TabIndex = 1;
            // 
            // lblNgayLap
            // 
            this.lblNgayLap.AutoSize = true;
            this.lblNgayLap.Location = new System.Drawing.Point(450, 20);
            this.lblNgayLap.Name = "lblNgayLap";
            this.lblNgayLap.Size = new System.Drawing.Size(65, 16);
            this.lblNgayLap.TabIndex = 2;
            this.lblNgayLap.Text = "Ngày lập:";
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayLap.Location = new System.Drawing.Point(530, 15);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(200, 22);
            this.dtpNgayLap.TabIndex = 3;
            // 
            // lblTenKH
            // 
            this.lblTenKH.AutoSize = true;
            this.lblTenKH.Location = new System.Drawing.Point(30, 70);
            this.lblTenKH.Name = "lblTenKH";
            this.lblTenKH.Size = new System.Drawing.Size(55, 16);
            this.lblTenKH.TabIndex = 4;
            this.lblTenKH.Text = "Tên KH:";
            // 
            // txtTenKH
            // 
            this.txtTenKH.Location = new System.Drawing.Point(120, 65);
            this.txtTenKH.Name = "txtTenKH";
            this.txtTenKH.Size = new System.Drawing.Size(300, 22);
            this.txtTenKH.TabIndex = 5;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Location = new System.Drawing.Point(450, 70);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(37, 16);
            this.lblSDT.TabIndex = 6;
            this.lblSDT.Text = "SĐT:";
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(530, 65);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(200, 22);
            this.txtSDT.TabIndex = 7;
            this.txtSDT.TextChanged += new System.EventHandler(this.TxtSDT_TextChanged);
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Location = new System.Drawing.Point(30, 124);
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.Size = new System.Drawing.Size(70, 16);
            this.lblNhanVien.TabIndex = 10;
            this.lblNhanVien.Text = "Nhân viên:";
            // 
            // lblHinhThuc
            // 
            this.lblHinhThuc.AutoSize = true;
            this.lblHinhThuc.Location = new System.Drawing.Point(400, 124);
            this.lblHinhThuc.Name = "lblHinhThuc";
            this.lblHinhThuc.Size = new System.Drawing.Size(77, 16);
            this.lblHinhThuc.TabIndex = 12;
            this.lblHinhThuc.Text = "Thanh toán:";
            // 
            // cboHinhThuc
            // 
            this.cboHinhThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHinhThuc.Items.AddRange(new object[] {
            "Tiền mặt",
            "Chuyển khoản",
            "Thẻ"});
            this.cboHinhThuc.Location = new System.Drawing.Point(550, 119);
            this.cboHinhThuc.Name = "cboHinhThuc";
            this.cboHinhThuc.Size = new System.Drawing.Size(180, 24);
            this.cboHinhThuc.TabIndex = 13;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(840, 40);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "HÓA ĐƠN BÁN HÀNG";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // UC_BanThuoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(233)))));
            this.Controls.Add(this.panelCart);
            this.Controls.Add(this.panelTotal);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.lblTitle);
            this.Name = "UC_BanThuoc";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Size = new System.Drawing.Size(880, 700);
            this.panelTotal.ResumeLayout(false);
            this.panelTotal.PerformLayout();
            this.panelCart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).EndInit();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // Controls
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelCart;
        private System.Windows.Forms.Panel panelTotal;
        private System.Windows.Forms.Label lblTitle;

        // NEW CONTROLS FOR SEARCH
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnThemThuoc;

        // Info Controls
        private System.Windows.Forms.TextBox txtMaHD; private System.Windows.Forms.Label lblMaHD;
        private System.Windows.Forms.DateTimePicker dtpNgayLap; private System.Windows.Forms.Label lblNgayLap;
        private System.Windows.Forms.TextBox txtTenKH; private System.Windows.Forms.Label lblTenKH;
        private System.Windows.Forms.TextBox txtSDT; private System.Windows.Forms.Label lblSDT;
private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.ComboBox cboHinhThuc; private System.Windows.Forms.Label lblHinhThuc;

        // Cart Grid
        private System.Windows.Forms.DataGridView dgvGioHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT, colTenThuoc, colDonVi, colDonGia, colSL, colThanhTien;
        private System.Windows.Forms.DataGridViewButtonColumn colXoa;

        // Total Controls
        private System.Windows.Forms.Label lblTongTienThuoc; private System.Windows.Forms.TextBox txtTongTienThuoc;
        private System.Windows.Forms.Label lblGiamGia; private System.Windows.Forms.TextBox txtGiamGia;
        private System.Windows.Forms.Label lblTongThanhToan; private System.Windows.Forms.TextBox txtTongThanhToan;
        private System.Windows.Forms.Button btnTaoHD, btnHuy;
        private System.Windows.Forms.TextBox txtTenNhanVien;
    }
}