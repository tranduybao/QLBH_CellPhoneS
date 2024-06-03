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
    public partial class frmChiNhanh : Form
    {
        string sql;
        DataTable tblData;
        public frmChiNhanh()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmChiNhanh_Load(object sender, EventArgs e)
        {
            tblData = Extend.Funcions.GetDataToTable("SELECT * FROM ChiNhanh");
            dgvData.DataSource = tblData;
            txtMaChiNhanh.Text = "";
            txtMaChiNhanh.Enabled = true;
            btnThem.Enabled = true;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
        }
        private void ResetValues()
        {
            
            txtTenChiNhanh.Text = "";
            txtDiaChi.Text = "";
            mtbDienThoai.Text = "";
           
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaChiNhanh.Text = Extend.Funcions.TaoMaMoi("MaChiNhanh", "ChiNhanh", "CN");
            txtMaChiNhanh.Enabled = false;
            ResetValues();
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            
            if (txtMaChiNhanh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã chi nhánh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaChiNhanh.Focus();
                return;
            }
            if (txtTenChiNhanh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên chi nhánh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenChiNhanh.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenChiNhanh.Focus();
                return;
            }
            if (mtbDienThoai.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenChiNhanh.Focus();
                return;
            }
            sql = "SELECT MaChiNhanh FROM ChiNhanh WHERE MaChiNhanh=N'" + txtMaChiNhanh.Text.Trim() + "'";
            if (Extend.Funcions.CheckKey(sql))
            {
               MessageBox.Show("Đã tồn tại chi nhánh này, không thể tạo mới !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                sql = "INSERT INTO ChiNhanh(MaChiNhanh,TenChiNhanh, DiaChi, DienThoai) VALUES (N'" + txtMaChiNhanh.Text.Trim() + "',N'" + txtTenChiNhanh.Text.Trim() + "',N'" + txtDiaChi.Text.Trim() + "',N'" + mtbDienThoai.Text.Trim()+"')";
                Extend.Funcions.RunSQL(sql);
            }
            txtMaChiNhanh.Text = "";
            txtMaChiNhanh.Enabled = true;
            ResetValues();
            frmChiNhanh_Load(this, EventArgs.Empty);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            sql = "UPDATE ChiNhanh SET TenChiNhanh=N'" + txtTenChiNhanh.Text.Trim() +
                    "',DiaChi=N'" + txtDiaChi.Text.Trim() + 
                    "',DienThoai=N'" +mtbDienThoai.Text.Trim()+ "' WHERE MaChiNhanh='"+txtMaChiNhanh.Text+"'";
            Extend.Funcions.RunSQL(sql);
            ResetValues();
            frmChiNhanh_Load(this, EventArgs.Empty);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

                sql = "DELETE ChiNhanh WHERE MaChiNhanh=N'" + txtMaChiNhanh.Text + "'";
                Extend.Funcions.RunSQL(sql);

                ResetValues();
                frmChiNhanh_Load(this, EventArgs.Empty);

                btnXoa.Enabled = false;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            frmChiNhanh_Load(this, EventArgs.Empty);
            txtMaChiNhanh.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string soDienThoaiNumbers = new string(mtbDienThoai.Text.Where(char.IsDigit).ToArray());
            if ((txtMaChiNhanh.Text == "") && (txtTenChiNhanh.Text == "") && (txtDiaChi.Text == "") && (soDienThoaiNumbers == ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from ChiNhanh WHERE 1=1";
            if (txtMaChiNhanh.Text != "")
                sql += " AND MaChiNhanh LIKE N'%" + txtMaChiNhanh.Text + "%'";
            if (txtTenChiNhanh.Text != "")
                sql += " AND TenChiNhanh LIKE N'%" + txtTenChiNhanh.Text + "%'";
            if (txtDiaChi.Text != "")
                sql += " AND DiaChi LIKE N'%" + txtDiaChi.Text + "%'";
            if (soDienThoaiNumbers != "")
                sql += " AND REPLACE(REPLACE(REPLACE(DienThoai, '(', ''), ')', ''), '-', '') LIKE N'%" + soDienThoaiNumbers + "%'";
            tblData = Extend.Funcions.GetDataToTable(sql);
            if (tblData.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblData.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvData.DataSource = tblData;
            ResetValues();
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            frmChiNhanh_Load(this, EventArgs.Empty);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            txtMaChiNhanh.Text = dgvData.CurrentRow.Cells["MaChiNhanh"].Value.ToString();
            txtTenChiNhanh.Text = dgvData.CurrentRow.Cells["TenChiNhanh"].Value.ToString();
            txtDiaChi.Text = dgvData.CurrentRow.Cells["DiaChi"].Value.ToString();
            mtbDienThoai.Text = dgvData.CurrentRow.Cells["DienThoai"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
            txtMaChiNhanh.Enabled = false;
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
            exRange.Range["C2:E2"].Value = "DANH SÁCH CHI NHÁNH ";

            tblInPutData = tblData;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:E5"].Font.Bold = true;
            exRange.Range["A5:E5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:E5"].ColumnWidth = 18;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã Chi Nhánh";
            exRange.Range["C5:C5"].Value = "Tên Chi Nhánh";
            exRange.Range["D5:D5"].Value = "Địa Chỉ";
            exRange.Range["E5:E5"].Value = "Số Điện Thoại";
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
            exRange.Range["A1:C1"].Value = Extend.frmLogin.diaChiChiNhanh+", ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = Extend.frmLogin.tenChucVu;
            exRange.Range["A6:C6"].MergeCells = true;
            exRange.Range["A6:C6"].Font.Italic = true;
            exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A6:C6"].Value = Extend.frmLogin.tenNhanVienDangNhap;
            exSheet.Name = "Danh Sách Chi Nhánh";
            exApp.Visible = true;
        }
    }
}
