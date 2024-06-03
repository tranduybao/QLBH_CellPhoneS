using DoAn_QLBHCellPhoneS.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace DoAn_QLBHCellPhoneS.Manager
{
    public partial class frmNhapHang : Form
    {
        DataTable tblCTHDN; 
        CultureInfo culture = CultureInfo.CreateSpecificCulture("vi-VN");
        string sql;
        string giaCu,nhaCCCu,khoCu;



        public frmNhapHang()
        {
            InitializeComponent();
        }


        private void frmNhapHang_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnXuatDanhSach.Enabled = false;
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
            txtSoLuong.Text = "0";
            txtTongTien.Text = "0";
            txtDonGiaNhap.Text = "0";
            lblTienChu.Text = "Không Đồng";
            txtMaPhieuNhap.Text = "";
            Extend.Funcions.FillCombo("SELECT MaNCC,TenNCC FROM NhaCungCap", cboNCC, "MaNCC", "TenNCC");
            cboNCC.SelectedIndex = -1;
            Extend.Funcions.FillCombo("SELECT MaNhanVien, TenNhanVien FROM NhanVien", cboNhanVien, "MaNhanVien", "TenNhanVien");
            cboNhanVien.SelectedIndex = -1;
            Extend.Funcions.FillCombo("SELECT MaKho, TenKho FROM Kho", cboKho, "MaKho", "TenKho");
            cboKho.SelectedIndex = -1;
            btnXuatDanhSach.Enabled = true;
            sql = "SELECT * FROM CTPhieuNhap WHERE MaPhieuNhap = null";
            tblCTHDN = Extend.Funcions.GetDataToTable(sql);
        }

        private void CreateNewSP()
        {
            txtMaSPNhap.Text = Extend.Funcions.TaoMaMoi("MaSanPham", "SanPham", txtLoaiHang.Text + cboKho.SelectedValue.ToString() + cboNCC.SelectedValue.ToString());

            string sql = "INSERT INTO SanPham(MaSanPham,TenSanPham,MaLoaiHang,SoLuong,DonGiaNhap, DonGiaBan,Anh,GhiChu,MaKho,MaNCC) VALUES(N'"
                + txtMaSPNhap.Text.Trim() + "',N'" + txtTenSanPham.Text.Trim() +
                "',N'" + txtLoaiHang.Text +
                "'," + 0 + "," + txtDonGiaNhap.Text +
                "," + 0 + ",'" + null + "',N'" + null + "',N'" + cboKho.SelectedValue.ToString() + "',N'" + cboNCC.SelectedValue.ToString() + "')";

            Extend.Funcions.RunSQL(sql);
        }
      
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenSanPham = txtTenSanPham.Text.Trim();
            string maLoaiHang = txtLoaiHang.Text.Trim();
            double soLuong = double.Parse(txtSoLuong.Text);
            double giaSanPham = double.Parse(txtDonGiaNhap.Text.Trim());
            double giamGia = double.Parse(txtGiamGia.Text.Trim());
            double thanhTien = double.Parse(txtThanhTien.Text.Trim());
            
            if (giaSanPham.ToString()!=giaCu || cboNCC.SelectedValue.ToString() != nhaCCCu || cboKho.SelectedValue.ToString() != khoCu)
            {
                
                DialogResult result = MessageBox.Show("Thông tin sản phẩm cần nhập đã thay đổi, Xác nhận tạo mã sản phẩm mới ?", "Thông báo!", MessageBoxButtons.OKCancel);

                // Kiểm tra kết quả từ hộp thoại thông báo
                if (result == DialogResult.OK)
                {
                    // Nếu người dùng chọn OK, thực hiện các hành động liên quan đến việc tạo mới sản phẩm
                    CreateNewSP();
                    MessageBox.Show("Tạo thành công sản phẩm mới với mã " + txtMaSPNhap.Text, "Thông báo!", MessageBoxButtons.OKCancel);
                }
                else
                {
                    // Nếu người dùng chọn Cancel, không làm gì cả hoặc bạn có thể thực hiện các hành động khác tùy theo nhu cầu
                    return;
                }
                
            }
            string maSanPham = txtMaSPNhap.Text.Trim();
            
            dgvData.Rows.Add(maSanPham, tenSanPham,maLoaiHang, soLuong, giaSanPham, giamGia, thanhTien);

            tblCTHDN.Rows.Add(maSanPham, tenSanPham, maLoaiHang, soLuong, giaSanPham, giamGia, thanhTien);
            
            string tongTien, tienChu;
            Extend.Funcions.TinhTongTien(dgvData,out tongTien,out tienChu);
            txtTongTien.Text=tongTien;
            lblTienChu.Text=tienChu;
            

            // Định nghĩa các cột của DataTable dựa trên cột của DataGridView
            


        }

        private void txtMaSPNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    // Truy vấn dữ liệu sản phẩm từ SQL Server
                    sql = "SELECT * FROM SanPham WHERE MaSanPham = @MaSanPham";
                    using (SqlCommand command = new SqlCommand(sql, Extend.KetNoi.Con))
                    {
                        command.Parameters.AddWithValue("@MaSanPham", txtMaSPNhap.Text);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Kiểm tra nếu có kết quả
                            if (reader.Read())
                            {
                                // Hiển thị thông tin sản phẩm lên các TextBox
                                txtTenSanPham.Text = reader["TenSanPham"].ToString();
                                txtLoaiHang.Text = reader["MaLoaiHang"].ToString();
                                txtLuongTon.Text = reader["SoLuong"].ToString();
                                txtDonGiaNhap.Text = reader["DonGiaNhap"].ToString();
                                giaCu = reader["DonGiaNhap"].ToString();
                                cboNCC.SelectedValue = reader["MaNCC"].ToString();
                                nhaCCCu = reader["MaNCC"].ToString();
                                cboKho.SelectedValue = reader["MaKho"].ToString();
                                khoCu = reader["MaKho"].ToString();
                            }
                            else
                            {
                                // Hiển thị hộp thoại thông báo và kiểm tra xem người dùng đã chọn OK hay Cancel
                                DialogResult result = MessageBox.Show("Không tìm thấy sản phẩm với Mã Sản Phẩm này. Xác nhận tạo mới sản phẩm ?", "Thông báo!", MessageBoxButtons.OKCancel);

                                // Kiểm tra kết quả từ hộp thoại thông báo
                                if (result == DialogResult.OK)
                                {
                                    // Nếu người dùng chọn OK, thực hiện các hành động liên quan đến việc tạo mới sản phẩm
                                    frmSanPham vf = new frmSanPham();
                                    this.Hide();
                                    vf.ShowDialog();
                                    this.Show();
                                }
                                else
                                {
                                    // Nếu người dùng chọn Cancel, không làm gì cả hoặc bạn có thể thực hiện các hành động khác tùy theo nhu cầu
                                    return;
                                }
                            }
                        } // Khi khối using này kết thúc, reader sẽ tự động đóng
                    } // Khi khối using này kết thúc, command sẽ tự động được giải phóng
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi khi truy vấn dữ liệu
                    MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message);
                }
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnXoa.Enabled = true;
            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            DataGridViewRow currentRow = dgvData.CurrentRow;
            txtMaSPNhap.Text = currentRow.Cells["MaSanPham"].Value.ToString();
            txtTenSanPham.Text = currentRow.Cells["TenSanPham"].Value.ToString();
            txtLoaiHang.Text = currentRow.Cells["MaLoaiHang"].Value.ToString();
            txtSoLuong.Text = currentRow.Cells["SoLuong"].Value.ToString();
            txtDonGiaNhap.Text = currentRow.Cells["DonGiaNhap"].Value.ToString();
            txtGiamGia.Text = currentRow.Cells["GiamGia"].Value.ToString();
            txtThanhTien.Text = currentRow.Cells["ThanhTien"].Value.ToString();


        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Lấy chỉ số hàng đang được chọn trong DataGridView
            int rowIndex = dgvData.SelectedCells[0].RowIndex;

            // Xóa hàng tương ứng khỏi DataTable
            dgvData.Rows.RemoveAt(rowIndex);

            // Cập nhật lại DataGridView
            dgvData.DataSource = tblCTHDN;

            // Ẩn nút Xóa sau khi xóa hàng
            btnXoa.Enabled = false;

            string tongTien, tienChu;
            Extend.Funcions.TinhTongTien(dgvData, out tongTien, out tienChu);
            txtTongTien.Text = tongTien;
            lblTienChu.Text = tienChu;
        }

        private void btnHoanThanh_Click(object sender, EventArgs e)
        {
            
            txtMaPhieuNhap.Text = Extend.Funcions.CreateKey("HDN");
            
            if (cboNhanVien.Text == "" || cboNCC.Text==""||cboKho.Text=="")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin ");
                return;
            }
            sql = "INSERT INTO PhieuNhap (MaPhieuNhap, MaNhanVien,NgayNhap, MaNCC, TongTien,MaKho) VALUES (@1, @2, @3, @4, @5, @6)";
            Extend.Funcions.LuuHDBanVaoCSDL(sql, txtMaPhieuNhap.Text, cboNhanVien.SelectedValue.ToString(), cboNCC.SelectedValue.ToString(), txtTongTien.Text, cboKho.SelectedValue.ToString());
            sql = "INSERT INTO CTPhieuNhap (STT,MaPhieuNhap, MaSanPham, SoLuong, DonGiaNhap, GiamGia, ThanhTien) VALUES (@1, @2, @3, @4, @5, @6, @7)";
            int i = 0;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (!row.IsNewRow)
                {
                    string STT = txtMaPhieuNhap.Text+"STT"+i.ToString();
                    string maPhieuNhap = txtMaPhieuNhap.Text;
                    string maSanPham = row.Cells["MaSanPham"].Value.ToString();
                    string soLuong = row.Cells["SoLuong"].Value.ToString();
                    string giaSanPham = row.Cells["DonGiaNhap"].Value.ToString();
                    string giamGia = row.Cells["GiamGia"].Value.ToString();
                    string thanhTien = row.Cells["ThanhTien"].Value.ToString();

                    Extend.Funcions.LuuChiTietHDVaoCSDL(sql, STT ,maPhieuNhap, maSanPham, soLuong, giaSanPham, giamGia, thanhTien);
                    string soLuongMoi = (double.Parse(txtSoLuong.Text) + double.Parse(txtLuongTon.Text).ToString());
                    string query = "UPDATE SanPham SET SoLuong ='" + soLuongMoi + "' WHERE MaSanPham =N'" + row.Cells["MaSanPham"].Value.ToString() + "'";
                    Extend.Funcions.RunSQL(query); ;
                    i++;
                }
            }
            btnThem.Enabled = true;
            

        }

        private void txtDonGiaNhap_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaNhap.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaNhap.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaNhap.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaNhap.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaNhap.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaNhap.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmNhapHang_Load(this, EventArgs.Empty);    
        }

        private void btnXuatDanhSach_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;

            int hang = 0, cot = 0;
            DataTable tblInPutData;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "CellPhoneS";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "Địa Chỉ : " + Extend.frmLogin.diaChiChiNhanh;
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại : " + Extend.frmLogin.SDTChiNhanh; ;
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "DANH SÁCH HÀNG NHẬP KHO ";

            tblInPutData = tblCTHDN;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:H5"].Font.Bold = true;
            exRange.Range["A5:H5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:H5"].ColumnWidth = 18;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã Sản Phẩm";
            exRange.Range["C5:C5"].Value = "Tên Sản Phẩm";
            exRange.Range["D5:D5"].Value = "Loại Hàng";
            exRange.Range["E5:E5"].Value = "Số Lượng";
            exRange.Range["F5:F5"].Value = "Đơn Giá Nhập";
            exRange.Range["G5:G5"].Value = "Giảm Giá";
            exRange.Range["H5:H5"].Value = "Thành Tiền";
            for (hang = 0; hang < tblInPutData.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 6
                exSheet.Cells[1][hang + 6] = hang + 1;
                for (cot = 0; cot < tblInPutData.Columns.Count; cot++)
                //Điền thông tin hàng từ cột thứ 2, dòng 6
                {
                    exSheet.Cells[cot + 2][hang + 6] = tblInPutData.Rows[hang][cot].ToString();
                    if (cot == 3) exSheet.Cells[cot + 2][hang + 6] = tblInPutData.Rows[hang][cot].ToString();
                }
            }
            exRange = exSheet.Cells[cot-1][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền:";
            exRange = exSheet.Cells[cot][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = txtTongTien.Text;
            exRange = exSheet.Cells[1][hang + 15]; //Ô A1 
            exRange.Range["A1:G1"].MergeCells = true;
            exRange.Range["A1:G1"].Font.Bold = true;
            exRange.Range["A1:G1"].Font.Italic = true;
            exRange.Range["A1:G1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
            exRange.Range["A1:G1"].Value = "Bằng chữ: " + lblTienChu.Text;
            exRange = exSheet.Cells[5][hang + 17]; //Ô A1 
            exRange.Range["A1:C1"].MergeCells = true;
            exRange.Range["A1:C1"].Font.Italic = true;
            exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            DateTime d = DateTime.Now;
            exRange.Range["A1:C1"].Value = Extend.frmLogin.diaChiChiNhanh + ", ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = Extend.frmLogin.tenChucVu;
            exRange.Range["A6:C6"].MergeCells = true;
            exRange.Range["A6:C6"].Font.Italic = true;
            exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A6:C6"].Value = Extend.frmLogin.tenNhanVienDangNhap;
            exSheet.Name = "Danh Sách Nhập Kho";
            exApp.Visible = true;
        }
    }
}
