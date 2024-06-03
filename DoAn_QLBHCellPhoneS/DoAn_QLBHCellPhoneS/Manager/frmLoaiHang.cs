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
    public partial class frmLoaiHang : Form
    {
        DataTable tblData;
        string sql;

        public frmLoaiHang()
        {
            InitializeComponent();
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * FROM LoaiHang";
            tblData = Extend.Funcions.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            dgvData.DataSource = tblData; //Nguồn dữ liệu            
            dgvData.Columns[0].HeaderText = "Mã loại hàng";
            dgvData.Columns[1].HeaderText = "Tên loại hàng";
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dgvData.AllowUserToAddRows = false; //Không cho người dùng thêm dữ liệu trực tiếp
            dgvData.EditMode = DataGridViewEditMode.EditProgrammatically; //Không cho sửa dữ liệu trực tiếp
        }
        private void frmLoaiHang_Load(object sender, EventArgs e)
        {
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
            txtMaLoaiHang.Enabled = true;
            txtMaLoaiHang.Text = "";
            LoadDataGridView(); //Hiển thị bảng LoaiHang
        }
        private void ResetValue()
        {
            txtMaLoaiHang.Text = "";
            txtTenLoaiHang.Text = "";
            txtMaLoaiHang.Enabled=true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            
            ResetValue(); //Xoá trắng các textbox
            txtMaLoaiHang.Focus();
            btnLuu.Enabled=true;
            btnBoQua.Enabled=true;
            btnThem.Enabled=false;
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            txtMaLoaiHang.Text = dgvData.CurrentRow.Cells["MaLoaiHang"].Value.ToString();
            txtTenLoaiHang.Text = dgvData.CurrentRow.Cells["TenLoaiHang"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
            txtMaLoaiHang.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql; //Lưu lệnh sql
            if (txtMaLoaiHang.Text.Trim().Length == 0) //Nếu chưa nhập mã chất liệu
            {
                MessageBox.Show("Bạn phải nhập mã chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaLoaiHang.Focus();
                return;
            }
            if (txtTenLoaiHang.Text.Trim().Length == 0) //Nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn phải nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenLoaiHang.Focus();
                return;
            }
            sql = "Select MaLoaiHang From LoaiHang where MaLoaiHang=N'" + txtMaLoaiHang.Text.Trim() + "'";
            if (Extend.Funcions.CheckKey(sql))
            {
                MessageBox.Show("Mã chất liệu này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaLoaiHang.Focus();
                return;
            }

            sql = "INSERT INTO LoaiHang VALUES(N'" +
                txtMaLoaiHang.Text + "',N'" + txtTenLoaiHang.Text + "')";
            Extend.Funcions.RunSQL(sql); //Thực hiện câu lệnh sql
            LoadDataGridView(); //Nạp lại DataGridView
            ResetValue();
            frmLoaiHang_Load(this, EventArgs.Empty);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql; //Lưu câu lệnh sql
            if (tblData.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaLoaiHang.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenLoaiHang.Text.Trim().Length == 0) //nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn chưa nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sql = "UPDATE LoaiHang SET TenLoaiHang=N'" +
                txtTenLoaiHang.Text.ToString() +
                "' WHERE MaLoaiHang=N'" + txtMaLoaiHang.Text + "'";
            Extend.Funcions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();

            btnSua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE LoaiHang WHERE MaLoaiHang=N'" + txtMaLoaiHang.Text + "'";
                Extend.Funcions.RunSQL(sql);
                LoadDataGridView();
                ResetValue();

                btnXoa.Enabled = false;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValue();
            frmLoaiHang_Load(this,EventArgs.Empty);
            
        }

        private void txtMaLoaiHang_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if ((txtMaLoaiHang.Text == "") && (txtTenLoaiHang.Text == "") )
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from LoaiHang WHERE 1=1";
            if (txtMaLoaiHang.Text != "")
                sql += " AND MaLoaiHang LIKE N'%" + txtMaLoaiHang.Text + "%'";
            if (txtTenLoaiHang.Text != "")
                sql += " AND TenLoaiHang LIKE N'%" + txtTenLoaiHang.Text + "%'";
            

            tblData = Extend.Funcions.GetDataToTable(sql);
            if (tblData.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblData.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvData.DataSource = tblData;
            ResetValue();
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            LoadDataGridView();
            frmLoaiHang_Load(this, EventArgs.Empty);
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
            exRange.Range["C2:E2"].Value = "DANH SÁCH LOẠI HÀNG ";

            tblInPutData = tblData;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:F5"].Font.Bold = true;
            exRange.Range["A5:F5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:F5"].ColumnWidth = 18;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã Loại Hàng";
            exRange.Range["C5:C5"].Value = "Tên Loại Hàng";
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
            exSheet.Name = "Danh Sách Loại Hàng";
            exApp.Visible = true;
        }
    }
}
