using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace DoAn_QLBHCellPhoneS.Manager
{
    public partial class frmAccount : Form
    {
        string sql;
        DataTable tblData;
        public frmAccount()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void frmAccount_Load(object sender, EventArgs e)
        {
            tblData = Extend.Funcions.GetDataToTable("SELECT * FROM Account");
            dgvData.DataSource = tblData;
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            int vitri = dgvData.CurrentCell.RowIndex;
            txtMaNhanVien.Text = dgvData.Rows[vitri].Cells[0].Value.ToString();
            txtPassWord.Text = dgvData.Rows[vitri].Cells[2].Value.ToString();
            txtUserName.Text = dgvData.Rows[vitri].Cells[1].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled=true;
            btnBoQua.Enabled=true;
            
        }

        private void f(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ResetValues()
        {
            txtMaNhanVien.Text = "";
            txtUserName.Text = "";
            txtPassWord.Text = "";
        }
        private bool IsAccountCountGreaterThanEmployeeCount()
        {
            string accountCountQuery = "SELECT COUNT(*) FROM Account";
            string employeeCountQuery = "SELECT COUNT(*) FROM NhanVien";

            

                using (SqlCommand accountCommand = new SqlCommand(accountCountQuery, Extend.KetNoi.Con))
                using (SqlCommand employeeCommand = new SqlCommand(employeeCountQuery, Extend.KetNoi.Con))
                {
                    int accountCount = (int)accountCommand.ExecuteScalar();
                    int employeeCount = (int)employeeCommand.ExecuteScalar();

                    return accountCount >= employeeCount;
                }
            
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsAccountCountGreaterThanEmployeeCount())
            {
                MessageBox.Show("Không thêm được vì số lượng Account lớn hơn số lượng nhân viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ResetValues();
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaNhanVien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                return;
            }
            if (txtUserName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }
            if (txtPassWord.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassWord.Focus();
                return;
            }
            
            sql = "SELECT MaNhanVien FROM NhanVien WHERE MaNhanVien=N'" + txtMaNhanVien.Text.Trim() + "'";
            if (Extend.Funcions.CheckKey(sql))
            {
                sql = "SELECT MaNhanVien FROM Account WHERE MaNhanVien=N'" + txtMaNhanVien.Text.Trim() + "'";
                if (!Extend.Funcions.CheckKey(sql))
                {
                    sql = "INSERT INTO Account(MaNhanVien,UserName,PassWord) VALUES (N'" + txtMaNhanVien.Text.Trim() + "',N'" + txtUserName.Text.Trim() + "',N'" + txtPassWord.Text.Trim() + "')";
                    Extend.Funcions.RunSQL(sql);
                }
                else
                {
                    MessageBox.Show("Nhân viên này đã có tài khoản, không thể tạo mới !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
            else
            {
                DialogResult result = MessageBox.Show("Nhân viên chưa tồn tại trong CSDL, xác nhận tạo mới !", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    frmNhanVien f = new frmNhanVien();
                    this.Hide();
                    f.ShowDialog();
                    this.Show();
                }
                
            }
            ResetValues();
            frmAccount_Load(this, EventArgs.Empty);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            sql = "UPDATE Account SET UserName=N'" + txtUserName.Text.Trim() +
                    "',PassWord=N'" + txtPassWord.Text.Trim() + "' WHERE MaNhanVien ='"+txtMaNhanVien.Text+"'";
            Extend.Funcions.RunSQL(sql);
            ResetValues();
            frmAccount_Load(this, EventArgs.Empty);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                
                sql = "DELETE Account WHERE MaNhanVien=N'" + txtMaNhanVien.Text + "'";
                Extend.Funcions.RunSQL(sql);
                
                ResetValues();
                frmAccount_Load(this, EventArgs.Empty);

                btnXoa.Enabled = false;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            frmAccount_Load(this, EventArgs.Empty);
            txtMaNhanVien.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if ((txtMaNhanVien.Text == "") && (txtUserName.Text == "") && (txtPassWord.Text == ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from Account WHERE 1=1";
            if (txtMaNhanVien.Text != "")
                sql += " AND MaNhanVien LIKE N'%" + txtMaNhanVien.Text + "%'";
            if (txtUserName.Text != "")
                sql += " AND UserName LIKE N'%" + txtUserName.Text + "%'";
            if (txtPassWord.Text != "")
                sql += " AND MaChiNhanh LIKE N'%" + txtPassWord.Text + "%'";
            tblData = Extend.Funcions.GetDataToTable(sql);
            if (tblData.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblData.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvData.DataSource = tblData;
            ResetValues();
            
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            frmAccount_Load(this, EventArgs.Empty);
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
            exRange.Range["C2:E2"].Value = "DANH SÁCH TÀI KHOẢN ";

            tblInPutData = tblData;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:E5"].Font.Bold = true;
            exRange.Range["A5:E5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:E5"].ColumnWidth = 18;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã Nhân Viên";
            exRange.Range["C5:C5"].Value = "Tài Khoản";
            exRange.Range["D5:D5"].Value = "Mật Khẩu";
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
            exSheet.Name = "Danh Sách Tài Khoản";
            exApp.Visible = true;
        }
    }
}
