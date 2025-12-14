using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QLNhaThuoc.Helper
{
    /// <summary>
    /// Helper class để xuất dữ liệu ra file Excel (CSV và XLS format)
    /// Không cần thư viện bên ngoài, tương thích với .NET Framework 4.7.2
    /// </summary>
    public static class ExcelExporter
    {
        /// <summary>
        /// Xuất DataGridView ra file CSV (có thể mở bằng Excel)
        /// </summary>
        public static void ExportToCSV(DataGridView dgv, string fileName = "Export")
        {
            try
            {
                // Hiển thị hộp thoại chọn vị trí lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.FileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Tạo StringBuilder để xây dựng nội dung CSV
                    StringBuilder sb = new StringBuilder();

                    // Thêm tiêu đề cột
                    int visibleColumnCount = 0;
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        if (dgv.Columns[i].Visible)
                        {
                            if (visibleColumnCount > 0) sb.Append(",");
                            sb.Append(dgv.Columns[i].HeaderText);
                            visibleColumnCount++;
                        }
                    }
                    sb.AppendLine();

                    // Thêm dữ liệu từng dòng
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            int colIndex = 0;
                            for (int i = 0; i < dgv.Columns.Count; i++)
                            {
                                if (dgv.Columns[i].Visible)
                                {
                                    if (colIndex > 0) sb.Append(",");
                                    
                                    var cellValue = row.Cells[i].Value?.ToString() ?? "";
                                    
                                    // Xử lý dấu phẩy (") trong text
                                    if (cellValue.Contains(",") || cellValue.Contains("\"") || cellValue.Contains("\n"))
                                    {
                                        cellValue = "\"" + cellValue.Replace("\"", "\"\"") + "\"";
                                    }
                                    
                                    sb.Append(cellValue);
                                    colIndex++;
                                }
                            }
                            sb.AppendLine();
                        }
                    }

                    // Ghi file với encoding UTF-8 (có BOM để Excel nhận dạng tiếng Việt)
                    File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                    // Hỏi người dùng có muốn mở file không
                    if (MessageBox.Show("Xuất file CSV thành công!\nBạn có muốn mở file không?", 
                        "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file CSV: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xuất DataGridView ra file Excel (.xls) sử dụng HTML format
        /// File này có thể mở trực tiếp bằng Excel với format đẹp hơn CSV
        /// </summary>
        public static void ExportToExcel(DataGridView dgv, string fileName = "Export", string title = "Báo cáo")
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.FileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xls";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    
                    // Thêm header HTML với encoding UTF-8 và Excel metadata
                    sb.AppendLine("<?xml version=\"1.0\"?>");
                    sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
                    sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                    sb.AppendLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
                    sb.AppendLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
                    sb.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                    sb.AppendLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
                    
                    // Styles
                    sb.AppendLine("<Styles>");
                    
                    // Style cho header
                    sb.AppendLine("<Style ss:ID=\"HeaderStyle\">");
                    sb.AppendLine("<Font ss:Bold=\"1\" ss:Size=\"11\" ss:Color=\"#FFFFFF\"/>");
                    sb.AppendLine("<Interior ss:Color=\"#4472C4\" ss:Pattern=\"Solid\"/>");
                    sb.AppendLine("<Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
                    sb.AppendLine("<Borders>");
                    sb.AppendLine("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                    sb.AppendLine("<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                    sb.AppendLine("<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                    sb.AppendLine("<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                    sb.AppendLine("</Borders>");
                    sb.AppendLine("</Style>");
                    
                    // Style cho title
                    sb.AppendLine("<Style ss:ID=\"TitleStyle\">");
                    sb.AppendLine("<Font ss:Bold=\"1\" ss:Size=\"16\" ss:Color=\"#1F4E78\"/>");
                    sb.AppendLine("<Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
                    sb.AppendLine("</Style>");
                    
                    // Style cho cells
                    sb.AppendLine("<Style ss:ID=\"CellStyle\">");
                    sb.AppendLine("<Alignment ss:Vertical=\"Center\"/>");
                    sb.AppendLine("<Borders>");
                    sb.AppendLine("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("</Borders>");
                    sb.AppendLine("</Style>");
                    
                    // Style cho cells chẵn (alternate row)
                    sb.AppendLine("<Style ss:ID=\"AlternateStyle\">");
                    sb.AppendLine("<Interior ss:Color=\"#F2F2F2\" ss:Pattern=\"Solid\"/>");
                    sb.AppendLine("<Alignment ss:Vertical=\"Center\"/>");
                    sb.AppendLine("<Borders>");
                    sb.AppendLine("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#D0D0D0\"/>");
                    sb.AppendLine("</Borders>");
                    sb.AppendLine("</Style>");
                    
                    sb.AppendLine("</Styles>");
                    
                    // Worksheet
                    sb.AppendLine($"<Worksheet ss:Name=\"{EscapeXml(title)}\">");
                    sb.AppendLine("<Table>");
                    
                    // Đếm số cột visible
                    int visibleColumnCount = 0;
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Visible) visibleColumnCount++;
                    }
                    
                    // Row 1: Title (merged cells)
                    sb.AppendLine("<Row ss:Height=\"24\">");
                    sb.AppendLine($"<Cell ss:MergeAcross=\"{visibleColumnCount - 1}\" ss:StyleID=\"TitleStyle\">");
                    sb.AppendLine($"<Data ss:Type=\"String\">{EscapeXml(title)}</Data>");
                    sb.AppendLine("</Cell>");
                    sb.AppendLine("</Row>");
                    
                    // Row 2: Export date
                    sb.AppendLine("<Row>");
                    sb.AppendLine($"<Cell ss:MergeAcross=\"{visibleColumnCount - 1}\">");
                    sb.AppendLine($"<Data ss:Type=\"String\">Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</Data>");
                    sb.AppendLine("</Cell>");
                    sb.AppendLine("</Row>");
                    
                    // Empty row
                    sb.AppendLine("<Row/>");
                    
                    // Row 4: Header
                    sb.AppendLine("<Row ss:Height=\"20\">");
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Visible)
                        {
                            sb.AppendLine("<Cell ss:StyleID=\"HeaderStyle\">");
                            sb.AppendLine($"<Data ss:Type=\"String\">{EscapeXml(col.HeaderText)}</Data>");
                            sb.AppendLine("</Cell>");
                        }
                    }
                    sb.AppendLine("</Row>");
                    
                    // Data rows
                    int rowIndex = 0;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string styleID = (rowIndex % 2 == 0) ? "CellStyle" : "AlternateStyle";
                            sb.AppendLine("<Row>");
                            
                            foreach (DataGridViewColumn col in dgv.Columns)
                            {
                                if (col.Visible)
                                {
                                    var cellValue = row.Cells[col.Index].Value?.ToString() ?? "";
                                    string dataType = "String";
                                    
                                    // Xác định kiểu dữ liệu
                                    if (row.Cells[col.Index].Value != null)
                                    {
                                        var valueType = row.Cells[col.Index].Value.GetType();
                                        if (valueType == typeof(int) || valueType == typeof(long) || 
                                            valueType == typeof(decimal) || valueType == typeof(double) || 
                                            valueType == typeof(float))
                                        {
                                            dataType = "Number";
                                        }
                                    }
                                    
                                    sb.AppendLine($"<Cell ss:StyleID=\"{styleID}\">");
                                    sb.AppendLine($"<Data ss:Type=\"{dataType}\">{EscapeXml(cellValue)}</Data>");
                                    sb.AppendLine("</Cell>");
                                }
                            }
                            
                            sb.AppendLine("</Row>");
                            rowIndex++;
                        }
                    }
                    
                    sb.AppendLine("</Table>");
                    sb.AppendLine("</Worksheet>");
                    sb.AppendLine("</Workbook>");

                    // Ghi file với UTF-8 encoding
                    File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                    if (MessageBox.Show("Xuất file Excel thành công!\nBạn có muốn mở file không?", 
                        "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file Excel: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Escape special characters for XML
        /// </summary>
        private static string EscapeXml(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            
            return text.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace("\"", "&quot;")
                       .Replace("'", "&apos;");
        }
    }
}
