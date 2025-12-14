using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QLNhaThuoc.Database;

namespace QLNhaThuoc.Helper
{
    public class PDFHelper
    {
        // Font Unicode để hỗ trợ tiếng Việt
        private static BaseFont baseFont;
        private static Font fontTitle;
        private static Font fontHeader;
        private static Font fontNormal;
        private static Font fontBold;
        private static Font fontSmall;

        static PDFHelper()
        {
            // Sử dụng font Arial Unicode MS cho tiếng Việt
            try
            {
                // Thử tìm font Times New Roman trước (hỗ trợ tiếng Việt tốt hơn)
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                
                if (!File.Exists(fontPath))
                {
                    // Nếu không có, dùng Arial
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                }
                
                if (!File.Exists(fontPath))
                {
                    // Nếu không có, dùng Arial Unicode MS (hỗ trợ Unicode đầy đủ)
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialuni.ttf");
                }
                
                if (File.Exists(fontPath))
                {
                    // IDENTITY_H encoding hỗ trợ Unicode đầy đủ cho tiếng Việt
                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else
                {
                    // Fallback: sử dụng font mặc định (không hỗ trợ tiếng Việt có dấu)
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                }

                fontTitle = new Font(baseFont, 18, Font.BOLD);
                fontHeader = new Font(baseFont, 14, Font.BOLD);
                fontNormal = new Font(baseFont, 11, Font.NORMAL);
                fontBold = new Font(baseFont, 11, Font.BOLD);
                fontSmall = new Font(baseFont, 9, Font.NORMAL);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, sử dụng font mặc định
                MessageBox.Show($"Cảnh báo: Không thể tải font Unicode. PDF có thể không hiển thị đúng tiếng Việt.\nLỗi: {ex.Message}", 
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                fontTitle = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
                fontHeader = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
                fontNormal = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);
                fontBold = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD);
                fontSmall = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL);
            }
        }

        public static bool XuatHoaDonPDF(string maHoaDon)
        {
            try
            {
                using (var db = new DbThuocContext())
                {
                    // Lấy thông tin hóa đơn
                    var hoaDon = db.HoaDons
                        .Where(h => h.MaHoaDon == maHoaDon)
                        .Select(h => new
                        {
                            h.MaHoaDon,
                            h.NgayLap,
                            h.TongTien,
                            TenNhanVien = h.NhanVien.HoTen,
                            TenKhachHang = h.KhachHang != null ? h.KhachHang.TenKhachHang : "Khách vãng lai",
                            SDT = h.KhachHang != null ? h.KhachHang.SDT : "000"
                        })
                        .FirstOrDefault();

                    if (hoaDon == null)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Lấy chi tiết hóa đơn
                    var chiTietList = db.ChiTietHoaDons
                        .Where(ct => ct.MaHoaDon == maHoaDon)
                        .Select(ct => new
                        {
                            TenSanPham = ct.SanPham.TenSanPham,
                            DonViTinh = ct.SanPham.DonViTinh,
                            ct.SoLuong,
                            DonGia = ct.ThanhTien / ct.SoLuong,
                            ct.ThanhTien
                        })
                        .ToList();

                    // Chọn nơi lưu file
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveDialog.FileName = $"HoaDon_{maHoaDon}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (saveDialog.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }

                    string filePath = saveDialog.FileName;

                    // Tạo document PDF
                    Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();

                    // 1. Tiêu đề nhà thuốc
                    Paragraph title = new Paragraph("NHÀ THUỐC ABC", fontTitle);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    Paragraph subtitle = new Paragraph("Địa chỉ: 123 Đường ABC, Quận XYZ, TP.HCM", fontSmall);
                    subtitle.Alignment = Element.ALIGN_CENTER;
                    document.Add(subtitle);

                    Paragraph phone = new Paragraph("Điện thoại: 0123-456-789", fontSmall);
                    phone.Alignment = Element.ALIGN_CENTER;
                    document.Add(phone);

                    document.Add(new Paragraph("\n"));

                    // 2. Tiêu đề hóa đơn
                    Paragraph invoiceTitle = new Paragraph("HÓA ĐƠN BÁN HÀNG", fontHeader);
                    invoiceTitle.Alignment = Element.ALIGN_CENTER;
                    document.Add(invoiceTitle);

                    document.Add(new Paragraph("\n"));

                    // 3. Thông tin hóa đơn
                    PdfPTable infoTable = new PdfPTable(2);
                    infoTable.WidthPercentage = 100;
                    infoTable.SetWidths(new float[] { 1f, 1f });

                    // Cột trái
                    PdfPCell leftCell = new PdfPCell();
                    leftCell.Border = Rectangle.NO_BORDER;
                    leftCell.AddElement(new Paragraph($"Số HĐ: {hoaDon.MaHoaDon}", fontNormal));
                    leftCell.AddElement(new Paragraph($"Ngày lập: {hoaDon.NgayLap:dd/MM/yyyy HH:mm}", fontNormal));
                    leftCell.AddElement(new Paragraph($"Nhân viên: {hoaDon.TenNhanVien}", fontNormal));

                    // Cột phải
                    PdfPCell rightCell = new PdfPCell();
                    rightCell.Border = Rectangle.NO_BORDER;
                    rightCell.AddElement(new Paragraph($"Khách hàng: {hoaDon.TenKhachHang}", fontNormal));
                    rightCell.AddElement(new Paragraph($"SĐT: {hoaDon.SDT}", fontNormal));

                    infoTable.AddCell(leftCell);
                    infoTable.AddCell(rightCell);
                    document.Add(infoTable);

                    document.Add(new Paragraph("\n"));

                    // 4. Bảng chi tiết sản phẩm
                    PdfPTable detailTable = new PdfPTable(5);
                    detailTable.WidthPercentage = 100;
                    detailTable.SetWidths(new float[] { 0.5f, 2.5f, 1f, 1f, 1.5f });

                    // Header
                    AddTableHeader(detailTable, "STT");
                    AddTableHeader(detailTable, "Tên sản phẩm");
                    AddTableHeader(detailTable, "SL");
                    AddTableHeader(detailTable, "Đơn giá");
                    AddTableHeader(detailTable, "Thành tiền");

                    // Dữ liệu
                    int stt = 1;
                    foreach (var item in chiTietList)
                    {
                        AddTableCell(detailTable, stt.ToString(), Element.ALIGN_CENTER);
                        AddTableCell(detailTable, item.TenSanPham, Element.ALIGN_LEFT);
                        AddTableCell(detailTable, item.SoLuong.ToString(), Element.ALIGN_CENTER);
                        AddTableCell(detailTable, $"{item.DonGia:N0}", Element.ALIGN_RIGHT);
                        AddTableCell(detailTable, $"{item.ThanhTien:N0}", Element.ALIGN_RIGHT);
                        stt++;
                    }

                    document.Add(detailTable);

                    document.Add(new Paragraph("\n"));

                    // 5. Tổng tiền
                    PdfPTable totalTable = new PdfPTable(2);
                    totalTable.WidthPercentage = 100;
                    totalTable.SetWidths(new float[] { 3f, 1.5f });

                    PdfPCell totalLabelCell = new PdfPCell(new Phrase("TỔNG CỘNG:", fontBold));
                    totalLabelCell.Border = Rectangle.NO_BORDER;
                    totalLabelCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalLabelCell.PaddingRight = 10;

                    PdfPCell totalValueCell = new PdfPCell(new Phrase($"{hoaDon.TongTien:N0} VNĐ", fontBold));
                    totalValueCell.Border = Rectangle.TOP_BORDER;
                    totalValueCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                    totalTable.AddCell(totalLabelCell);
                    totalTable.AddCell(totalValueCell);
                    document.Add(totalTable);

                    document.Add(new Paragraph("\n\n"));

                    // 6. Chữ ký
                    PdfPTable signTable = new PdfPTable(2);
                    signTable.WidthPercentage = 100;
                    signTable.SetWidths(new float[] { 1f, 1f });

                    PdfPCell customerSignCell = new PdfPCell();
                    customerSignCell.Border = Rectangle.NO_BORDER;
                    customerSignCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    customerSignCell.AddElement(new Paragraph("Khách hàng", fontBold));
                    customerSignCell.AddElement(new Paragraph("(Ký, ghi rõ họ tên)", fontSmall));
                    customerSignCell.PaddingTop = 10;

                    PdfPCell staffSignCell = new PdfPCell();
                    staffSignCell.Border = Rectangle.NO_BORDER;
                    staffSignCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    staffSignCell.AddElement(new Paragraph("Nhân viên bán hàng", fontBold));
                    staffSignCell.AddElement(new Paragraph("(Ký, ghi rõ họ tên)", fontSmall));
                    staffSignCell.PaddingTop = 10;

                    signTable.AddCell(customerSignCell);
                    signTable.AddCell(staffSignCell);
                    document.Add(signTable);

                    document.Add(new Paragraph("\n\n"));

                    // 7. Lời cảm ơn
                    Paragraph thanks = new Paragraph("Cảm ơn Quý khách! Hẹn gặp lại!", fontNormal);
                    thanks.Alignment = Element.ALIGN_CENTER;
                    document.Add(thanks);

                    // Đóng document
                    document.Close();
                    writer.Close();

                    // Mở file PDF sau khi tạo
                    var result = MessageBox.Show($"Xuất hóa đơn thành công!\nBạn có muốn mở file PDF?", 
                        "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(filePath);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static void AddTableHeader(PdfPTable table, string text)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, fontBold));
            cell.BackgroundColor = new BaseColor(200, 200, 200);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private static void AddTableCell(PdfPTable table, string text, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, fontNormal));
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            table.AddCell(cell);
        }
    }
}
