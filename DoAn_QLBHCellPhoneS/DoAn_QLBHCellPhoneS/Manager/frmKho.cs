using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMExcel = Microsoft.Office.Interop.Excel;


namespace DoAn_QLBHCellPhoneS.Manager
{
    public partial class frmKho : Form
    {
        string sql;
        DataTable tblData;
        public frmKho()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmKho_Load(object sender, EventArgs e)
        {
            tblData = Extend.Funcions.GetDataToTable("SELECT * FROM Kho");
            dgvData.DataSource = tblData;
            txtMaKho.Text = "";
            txtMaKho.Enabled = true;
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            
        }
        private void ResetValues()
        {
            
            txtTenKho.Text = "";
            txtDiaChi.Text = "";
            

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaKho.Text = Extend.Funcions.TaoMaMoi("MaKho", "Kho", "K");
            txtMaKho.Enabled = false;
            ResetValues();
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            
            if (txtMaKho.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKho.Focus();
                return;
            }
            if (txtTenKho.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKho.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKho.Focus();
                return;
            }
            
            sql = "SELECT MaKho FROM Kho WHERE MaKho=N'" + txtMaKho.Text.Trim() + "'";
            if (Extend.Funcions.CheckKey(sql))
            {
                MessageBox.Show("Đã tồn tại kho này, không thể tạo mới !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                sql = "INSERT INTO Kho(MaKho,TenKho, DiaChi) VALUES (N'" + txtMaKho.Text.Trim() + "',N'" + txtTenKho.Text.Trim() + "',N'" + txtDiaChi.Text.Trim() + "')";
                Extend.Funcions.RunSQL(sql);
            }
            ResetValues();
            txtMaKho.Text = "";
            txtMaKho.Enabled = true;
            frmKho_Load(this, EventArgs.Empty);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            

            sql = "UPDATE Kho SET TenKho=N'" + txtTenKho.Text.Trim() +
                    "',DiaChi=N'" + txtDiaChi.Text.Trim() +
                    "' WHERE MaKho='"+txtMaKho.Text+"'";
            Extend.Funcions.RunSQL(sql);
            ResetValues();
            frmKho_Load(this, EventArgs.Empty);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

                sql = "DELETE Kho WHERE MaKho=N'" + txtMaKho.Text + "'";
                Extend.Funcions.RunSQL(sql);

                ResetValues();
                frmKho_Load(this, EventArgs.Empty);

                btnXoa.Enabled = false;
                
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            frmKho_Load(this, EventArgs.Empty);
            txtMaKho.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if ((txtMaKho.Text == "") && (txtTenKho.Text == "") && (txtDiaChi.Text == "") )
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from Kho WHERE 1=1";
            if (txtMaKho.Text != "")
                sql += " AND MaKho LIKE N'%" + txtMaKho.Text + "%'";
            if (txtTenKho.Text != "")
                sql += " AND TenKho LIKE N'%" + txtTenKho.Text + "%'";
            if (txtDiaChi.Text != "")
                sql += " AND DiaChi LIKE N'%" + txtDiaChi.Text + "%'";
            
            tblData = Extend.Funcions.GetDataToTable(sql);
            if (tblData.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblData.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvData.DataSource = tblData;
            ResetValues();
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            frmKho_Load(this, EventArgs.Empty);
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            txtMaKho.Text = dgvData.CurrentRow.Cells["MaKho"].Value.ToString();
            txtTenKho.Text = dgvData.CurrentRow.Cells["TenKho"].Value.ToString();
            txtDiaChi.Text = dgvData.CurrentRow.Cells["DiaChi"].Value.ToString();
            
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
            txtMaKho.Enabled = false;

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
            exRange.Range["C2:E2"].Value = "DANH SÁCH KHO HÀNG ";

            tblInPutData = tblData;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:F5"].Font.Bold = true;
            exRange.Range["A5:F5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:F5"].ColumnWidth = 18;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã Kho";
            exRange.Range["C5:C5"].Value = "Tên Kho";
            exRange.Range["D5:D5"].Value = "Địa Chỉ";
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

            exRange = exSheet.Cells[5][hang + 10]; //Ô A1 
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
            exSheet.Name = "Danh Sách Kho Hàng";
            exApp.Visible = true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
