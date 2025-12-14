# 🏥 Phần mềm Quản lý Nhà thuốc

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-7.3-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.5.1-orange.svg)](https://docs.microsoft.com/en-us/ef/)

Phần mềm quản lý nhà thuốc với đầy đủ chức năng quản lý bán hàng, kho, nhập thuốc, khách hàng và báo cáo thống kê.

## 📋 Mục lục

- [Tính năng](#-tính-năng)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Yêu cầu hệ thống](#-yêu-cầu-hệ-thống)
- [Cài đặt](#-cài-đặt)
- [Cấu trúc dự án](#-cấu-trúc-dự-án)
- [Hướng dẫn sử dụng](#-hướng-dẫn-sử-dụng)
- [Phân quyền](#-phân-quyền)
- [Đóng góp](#-đóng-góp)
- [Tác giả](#-tác-giả)

## ✨ Tính năng

### 🎯 Chức năng chính

- **🏠 Tổng quan (Dashboard)**
  - Hiển thị thống kê tổng quan về doanh thu, đơn hàng, khách hàng
  - Biểu đồ trực quan về tình hình kinh doanh

- **💊 Bán thuốc**
  - Tạo hóa đơn bán hàng nhanh chóng
  - Quản lý giỏ hàng linh hoạt
  - Tìm kiếm sản phẩm theo tên
  - Áp dụng giảm giá cho khách hàng
  - Xuất hóa đơn PDF với font tiếng Việt chuẩn
  - Thanh toán và in hóa đơn

- **📦 Quản lý Kho hàng**
  - Xem danh sách sản phẩm trong kho
  - Thêm, sửa, xóa sản phẩm
  - Quản lý tồn kho theo lô (Lot)
  - Cảnh báo hàng tồn kho thấp
  - Theo dõi hạn sử dụng (HSD)
  - Xuất báo cáo Excel

- **📥 Nhập thuốc**
  - Tạo phiếu nhập thuốc
  - Quản lý nhà cung cấp
  - Nhập chi tiết sản phẩm theo lô
  - Phân quyền duyệt phiếu nhập (Quản lý)
  - Xuất báo cáo phiếu nhập

- **👥 Quản lý Khách hàng**
  - Thêm, sửa, xóa thông tin khách hàng
  - Tìm kiếm khách hàng theo tên, số điện thoại
  - Lưu lịch sử mua hàng

- **📄 Quản lý Hóa đơn**
  - Xem danh sách hóa đơn
  - Chi tiết hóa đơn
  - Xuất hóa đơn PDF
  - Tìm kiếm hóa đơn theo mã, ngày

- **📊 Báo cáo & Thống kê** *(Chỉ dành cho Quản lý)*
  - Báo cáo doanh thu theo thời gian
  - Thống kê lợi nhuận
  - Biểu đồ trực quan (Chart)
  - Lọc theo khoảng thời gian linh hoạt
  - Xuất báo cáo Excel

### 🔐 Bảo mật & Phân quyền

- **Đăng nhập & Đăng xuất**
  - Xác thực người dùng qua tài khoản
  - Lưu session người dùng
  - Ghi lại lịch sử đăng nhập

- **Phân quyền theo vai trò**
  - **Quản lý**: Toàn quyền truy cập tất cả chức năng
  - **Nhân viên**: Giới hạn truy cập chức năng Báo cáo

## 🛠 Công nghệ sử dụng

### Framework & Ngôn ngữ
- **.NET Framework 4.7.2**
- **C# 7.3**
- **Windows Forms** - Giao diện desktop

### Database & ORM
- **Entity Framework 6.5.1** - Code First approach
- **SQL Server** - Cơ sở dữ liệu

### Thư viện bên thứ 3
- **iTextSharp 5.5.13.4** - Tạo và xuất file PDF
- **BouncyCastle.Cryptography 2.4.0** - Mã hóa và bảo mật
- **System.Windows.Forms.DataVisualization** - Biểu đồ Chart

## 💻 Yêu cầu hệ thống

### Phần mềm cần thiết
- **Windows 7/8/10/11** (64-bit hoặc 32-bit)
- **.NET Framework 4.7.2 trở lên** ([Tải tại đây](https://dotnet.microsoft.com/download/dotnet-framework/net472))
- **SQL Server 2012 trở lên** hoặc **SQL Server Express** ([Tải tại đây](https://www.microsoft.com/sql-server/sql-server-downloads))
- **Visual Studio 2019/2022** (để phát triển)

### Cấu hình tối thiểu
- **RAM**: 2GB trở lên
- **CPU**: Dual Core 1.8GHz trở lên
- **Dung lượng ổ cứng**: 500MB trống

## 🚀 Cài đặt

### 1. Clone dự án

```bash
git clone https://github.com/[TienPMT]/QLNhaThuoc.git
cd QLNhaThuoc
```

### 2. Cài đặt SQL Server

- Tải và cài đặt SQL Server hoặc SQL Server Express
- Tạo database mới hoặc để Entity Framework tự động tạo

### 3. Cấu hình Connection String

Mở file `App.config` và cập nhật connection string:

```xml
<connectionStrings>
  <add name="DbThuocContext" 
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=QLNhaThuocDB;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Thay thế:**
- `YOUR_SERVER_NAME`: Tên SQL Server của bạn (Ví dụ: `localhost`, `.\SQLEXPRESS`)

### 4. Restore NuGet Packages

Trong Visual Studio:
```
Tools > NuGet Package Manager > Manage NuGet Packages for Solution
```
Nhấn **Restore** để tải về các package cần thiết.

Hoặc dùng lệnh:
```bash
nuget restore
```

### 5. Build và chạy

1. Mở solution `QLNhaThuoc.sln` trong Visual Studio
2. Build solution: `Ctrl + Shift + B`
3. Chạy ứng dụng: `F5`

### 6. Tạo database & Seed data

Khi chạy lần đầu, Entity Framework sẽ tự động:
- Tạo database nếu chưa tồn tại
- Tạo các bảng theo entities đã định nghĩa

**Tài khoản mặc định** (nếu có seed data):
```
Username: admin
Password: admin123
Vai trò: Quản lý
```

## 📁 Cấu trúc dự án

```
QLNhaThuoc/
│
├── Form/                          # Các form giao diện
│   ├── FormMain.cs               # Form chính
│   ├── FormDangNhap.cs           # Form đăng nhập
│   ├── UC_TongQuan.cs            # User Control Tổng quan
│   ├── UC_BanThuoc.cs            # User Control Bán thuốc
│   ├── UC_KhoHang.cs             # User Control Kho hàng
│   ├── UC_NhapThuoc.cs           # User Control Nhập thuốc
│   ├── UC_ChiTietPhieuNhap.cs    # User Control Chi tiết phiếu nhập
│   ├── UC_KhachHang.cs           # User Control Khách hàng
│   ├── UC_HoaDon.cs              # User Control Hóa đơn
│   └── UC_BaoCao.cs              # User Control Báo cáo
│
├── Database/                      # Database context & entities
│   ├── DbThuocContext.cs         # Entity Framework DbContext
│   └── Entities/                 # Các entity class
│       ├── SanPham.cs            # Sản phẩm
│       ├── HoaDon.cs             # Hóa đơn
│       ├── ChiTietHoaDon.cs      # Chi tiết hóa đơn
│       ├── PhieuNhap.cs          # Phiếu nhập
│       ├── ChiTietPhieuNhap.cs   # Chi tiết phiếu nhập
│       ├── KhachHang.cs          # Khách hàng
│       ├── NhanVien.cs           # Nhân viên
│       ├── TaiKhoan.cs           # Tài khoản
│       ├── NhaCungCap.cs         # Nhà cung cấp
│       ├── LoTonKho.cs           # Lô tồn kho
│       └── LichSuDangNhap.cs     # Lịch sử đăng nhập
│
├── Helper/                        # Các class tiện ích
│   ├── UserSession.cs            # Quản lý session người dùng
│   ├── PDFHelper.cs              # Xuất PDF hóa đơn
│   └── ExcelExporter.cs          # Xuất báo cáo Excel
│
├── Img/                          # Thư mục chứa hình ảnh
│   └── logo.jpg                  # Logo nhà thuốc
│
├── App.config                    # File cấu hình ứng dụng
├── packages.config               # NuGet packages
└── Program.cs                    # Entry point của ứng dụng
```

## 📖 Hướng dẫn sử dụng

### Đăng nhập
1. Khởi động ứng dụng
2. Nhập tài khoản và mật khẩu
3. Nhấn **Đăng nhập** hoặc **Enter**

### Bán thuốc
1. Chọn menu **Bán thuốc**
2. Tìm kiếm sản phẩm theo tên
3. Nhấn **Thêm** để thêm vào giỏ hàng
4. Nhập thông tin khách hàng (tùy chọn)
5. Áp dụng giảm giá nếu cần
6. Nhấn **Thanh toán** để hoàn tất
7. Chọn **Xuất PDF** để in hóa đơn

### Nhập thuốc
1. Chọn menu **Nhập thuốc**
2. Nhấn **Thêm mới** để tạo phiếu nhập
3. Chọn nhà cung cấp
4. Thêm từng sản phẩm với số lượng và đơn giá
5. Nhấn **Lưu** để hoàn tất
6. *Quản lý có thể sửa trạng thái phiếu nhập*

### Xem báo cáo (Chỉ Quản lý)
1. Chọn menu **Báo cáo**
2. Chọn khoảng thời gian cần xem
3. Xem các chỉ số: Doanh thu, Lợi nhuận, Đơn hàng, Khách hàng
4. Xem biểu đồ trực quan
5. Xuất Excel nếu cần

### Đăng xuất
1. Nhấn nút **Đăng xuất** ở góc phải
2. Xác nhận đăng xuất
3. Quay về màn hình đăng nhập


## 🔑 Phân quyền

| Chức năng | Quản lý | Nhân viên |
|-----------|---------|-----------|
| Tổng quan | ✅ | ✅ |
| Bán thuốc | ✅ | ✅ |
| Kho hàng | ✅ | ✅ |
| Nhập thuốc | ✅ | ✅ |
| Khách hàng | ✅ | ✅ |
| Hóa đơn | ✅ | ✅ |
| **Báo cáo** | ✅ | ❌ |
| Duyệt phiếu nhập | ✅ | ❌ |

## 🐛 Xử lý sự cố

### Lỗi kết nối database
```
Kiểm tra:
- SQL Server đã chạy chưa?
- Connection string trong App.config đúng chưa?
- Firewall có chặn kết nối không?
```

### Lỗi xuất PDF không hiển thị tiếng Việt
```
Giải pháp:
- Đảm bảo font Times New Roman hoặc Arial Unicode MS đã cài đặt
- Font sẽ được tự động nhúng vào PDF
```

### Lỗi build project
```
Giải pháp:
- Restore lại NuGet packages
- Clean solution rồi Rebuild
- Kiểm tra .NET Framework 4.7.2 đã cài đặt chưa
```

## 🤝 Đóng góp

Mọi đóng góp đều được chào đón! Vui lòng làm theo các bước sau:

1. Fork dự án
2. Tạo branch mới (`git checkout -b feature/AmazingFeature`)
3. Commit thay đổi (`git commit -m 'Add some AmazingFeature'`)
4. Push lên branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

### Coding Standards
- Đặt tên biến, hàm theo chuẩn **PascalCase** (C#)
- Comment bằng tiếng Việt hoặc tiếng Anh
- Tuân thủ cấu trúc dự án hiện tại

## 👨‍💻 Tác giả

**Tên tác giả**
- GitHub: [@TienPMT](https://github.com/TienPMT)
- Email: minhtienvvt7@gmail.com

## 🙏 Cảm ơn

- [Entity Framework](https://docs.microsoft.com/en-us/ef/) - ORM framework
- [iTextSharp](https://github.com/itext/itextsharp) - PDF generation library
- [Microsoft](https://www.microsoft.com/) - .NET Framework

---

⭐️ **Nếu bạn thấy dự án hữu ích, hãy cho một ngôi sao nhé!** ⭐️
