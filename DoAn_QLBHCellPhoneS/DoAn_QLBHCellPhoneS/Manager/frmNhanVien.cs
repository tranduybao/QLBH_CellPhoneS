using DoAn_QLBHCellPhoneS.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMExcel = Microsoft.Office.Interop.Excel;


namespace DoAn_QLBHCellPhoneS.Manager
{
    public partial class frmNhanVien : Form
    {
        
        DataTable tblNV;
        string sql;
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            txtMaNhanVien.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            Extend.Funcions.FillCombo("SELECT * FROM ChiNhanh", cboChiNhanh, "MaChiNhanh", "TenChiNhanh");
            Extend.Funcions.FillCombo("SELECT * FROM ChucVu", cboChucVu, "MaChucVu", "TenChucVu");
            
            sql = "SELECT COUNT(*) from NhanVien WHERE MaNhanVien LIKE N'%NV%'";
            SqlCommand command = new SqlCommand(sql, Extend.KetNoi.Con);
            txtSoLuongNhanVien.Text = command.ExecuteScalar().ToString();
            sql = "SELECT COUNT(*) from NhanVien WHERE MaNhanVien LIKE N'%QL%'";
            command = new SqlCommand(sql, Extend.KetNoi.Con);
            txtSoLuongQuanLy.Text = command.ExecuteScalar().ToString();

            cboChucVu.SelectedIndex = -1;
            cboChiNhanh.SelectedIndex = -1;
            LoadDataGridView();
            
        }
        public void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * FROM NhanVien";
            tblNV = Extend.Funcions.GetDataToTable(sql); //lấy dữ liệu
            dgvData.DataSource = tblNV;
            dgvData.Columns[0].HeaderText = "Mã nhân viên";
            dgvData.Columns[1].HeaderText = "Tên nhân viên";
            dgvData.Columns[2].HeaderText = "Chức Vụ";
            dgvData.Columns[3].HeaderText = "Chi Nhánh";
            dgvData.Columns[4].HeaderText = "Ngày Sinh";
            dgvData.Columns[5].HeaderText = "Giới tính";
            dgvData.Columns[6].HeaderText = "Địa chỉ";
            dgvData.Columns[7].HeaderText = "Điện thoại";
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dgvData.AllowUserToAddRows = false;
            dgvData.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            txtMaNhanVien.Text = dgvData.CurrentRow.Cells["MaNhanVien"].Value.ToString();
            txtTenNhanVien.Text = dgvData.CurrentRow.Cells["TenNhanVien"].Value.ToString();
            cboChucVu.SelectedValue= dgvData.CurrentRow.Cells["MaChucVu"].Value.ToString();
            cboChiNhanh.SelectedValue= dgvData.CurrentRow.Cells["MaChiNhanh"].Value.ToString();
            dtpNgaySinh.Text = dgvData.CurrentRow.Cells["NgaySinh"].Value.ToString();
            if (dgvData.CurrentRow.Cells["GioiTinh"].Value.ToString() == "Nam") ckbGioiTinh.Checked = true;
            else ckbGioiTinh.Checked = false;
            txtDiaChi.Text = dgvData.CurrentRow.Cells["DiaChi"].Value.ToString();
            mtbDienThoai.Text = dgvData.CurrentRow.Cells["DienThoai"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }
        private void ResetValues()
        {
            
            txtTenNhanVien.Text = "";
            ckbGioiTinh.Checked = false;
            txtDiaChi.Text = "";
            dtpNgaySinh.Text = "";
            mtbDienThoai.Text = "";
           
            cboChiNhanh.SelectedIndex = -1;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboChucVu.SelectedValue == null || string.IsNullOrEmpty(cboChucVu.SelectedValue.ToString()))
            {
                MessageBox.Show("Vui lòng chọn chức vụ trước khi thực hiện chức năng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txtMaNhanVien.Text = Extend.Funcions.TaoMaMoi("MaNhanVien","NhanVien",cboChucVu.SelectedValue.ToString());
            ResetValues();
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql, gt;
            if (txtMaNhanVien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                return;
            }
            if (txtTenNhanVien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhanVien.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }
            if (mtbDienThoai.Text == "(   )     -")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienThoai.Focus();
                return;
            }
            if (dtpNgaySinh.Text == "  /  /")
            {
                MessageBox.Show("Bạn phải nhập ngày sinh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgaySinh.Focus();
                return;
            }

            if (ckbGioiTinh.Checked == true)
                gt = "Nam";
            else
                gt = "Nữ";
            sql = "SELECT MaNhanVien FROM NhanVien WHERE MaNhanVien=N'" + txtMaNhanVien.Text.Trim() + "'";
            if (Extend.Funcions.CheckKey(sql))
            {
                MessageBox.Show("Mã nhân viên này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                txtMaNhanVien.Text = "";
                return;
            }
            sql = "INSERT INTO NhanVien(MaNhanVien,TenNhanVien,MaChucVu,MaChiNhanh,NgaySinh ,GioiTinh, DiaChi, DienThoai) VALUES (N'" + txtMaNhanVien.Text.Trim() + "',N'" + txtTenNhanVien.Text.Trim() + "',N'" +cboChucVu.SelectedValue + "',N'" +cboChiNhanh.SelectedValue + "','" + dtpNgaySinh.Value + "',N'" + gt + "',N'" + txtDiaChi.Text.Trim() + "','" + mtbDienThoai.Text +  "')";
            Extend.Funcions.RunSQL(sql);
            LoadDataGridView();
            txtMaNhanVien.Text = "";

            ResetValues();
            frmNhanVien_Load(this,EventArgs.Empty);
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql, gt;
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNhanVien.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenNhanVien.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhanVien.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }
            if (mtbDienThoai.Text == "(   )     -")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDienThoai.Focus();
                return;
            }
            if (dtpNgaySinh.Text == "  /  /")
            {
                MessageBox.Show("Bạn phải nhập ngày sinh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgaySinh.Focus();
                return;
            }

            if (ckbGioiTinh.Checked == true)
                gt = "Nam";
            else
                gt = "Nữ";
            sql = "UPDATE NhanVien SET TenNhanVien=N'" + txtTenNhanVien.Text.Trim().ToString() +
                    "',MaChucVu=N'" +cboChucVu.SelectedValue +
                    "',MaChiNhanh=N'" + cboChiNhanh.SelectedValue +
                    "',NgaySinh='" + dtpNgaySinh.Value +
                    "',GioiTinh=N'" + gt +
                    "',DiaChi=N'" + txtDiaChi.Text.Trim().ToString() +
                    "',DienThoai='" + mtbDienThoai.Text.ToString() + 
                    "' WHERE MaNhanVien=N'" + txtMaNhanVien.Text + "'";
            Extend.Funcions.RunSQL(sql);

            LoadDataGridView();
            ResetValues();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNhanVien.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE Account WHERE MaNhanVien=N'" + txtMaNhanVien.Text + "'";
                Extend.Funcions.RunSQL(sql);
                sql = "DELETE NhanVien WHERE MaNhanVien=N'" + txtMaNhanVien.Text + "'";
                Extend.Funcions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();

                btnXoa.Enabled = false;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            frmNhanVien_Load(this, EventArgs.Empty);
            txtMaNhanVien.Text = ""; 
        }

        private void cboChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;
            string soDienThoaiNumbers = new string(mtbDienThoai.Text.Where(char.IsDigit).ToArray());
            if ((txtTenNhanVien.Text == "") && (cboChucVu.Text == "") && (cboChiNhanh.Text == "")&& (cboChucVu.Text=="")&&(txtDiaChi.Text!="") && (soDienThoaiNumbers != ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from NhanVien WHERE 1=1";
            if (txtTenNhanVien.Text != "")
                sql += " AND TenNhanVien LIKE N'%" + txtTenNhanVien.Text + "%'";
            if (cboChucVu.Text != "")
                sql += " AND MaChucVu LIKE N'%" + cboChucVu.SelectedValue + "%'";
            if (cboChiNhanh.Text != "")
                sql += " AND MaChiNhanh LIKE N'%" + cboChiNhanh.SelectedValue + "%'";
            if (txtDiaChi.Text != "")
                sql += " AND DiaChi LIKE N'%" + txtDiaChi.Text + "%'";
            if (soDienThoaiNumbers != "")
                sql += " AND REPLACE(REPLACE(REPLACE(DienThoai, '(', ''), ')', ''), '-', '') LIKE N'%" + soDienThoaiNumbers + "%'";
            if (ckbGioiTinh.Checked)
            {
                sql += " AND GioiTinh LIKE N'%Nam%'";
            }
            else
            {
                sql += " AND GioiTinh LIKE N'%Nữ%'";
            }
            tblNV = Extend.Funcions.GetDataToTable(sql);
            if (tblNV.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblNV.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvData.DataSource = tblNV;
            ResetValues();
            
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            LoadDataGridView();
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXuatDanhSach_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
       
            int hang = 0, cot = 0;
            DataTable tblThongTinNhanVien;
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
            exRange.Range["A2:B2"].Value = "Địa Chỉ : "+Extend.frmLogin.diaChiChiNhanh;
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại : "+ Extend.frmLogin.SDTChiNhanh; ;
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "DANH SÁCH NHÂN VIÊN ";

            tblThongTinNhanVien = tblNV;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:I5"].Font.Bold = true;
            exRange.Range["A5:I5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:I5"].ColumnWidth = 18;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã Nhân Viên";
            exRange.Range["C5:C5"].Value = "Tên Nhân Viên";
            exRange.Range["D5:D5"].Value = "Chức Vụ";
            exRange.Range["E5:E5"].Value = "Chi Nhánh";
            exRange.Range["F5:F5"].Value = "Ngày Sinh";
            exRange.Range["G5:G5"].Value = "Giới Tính";
            exRange.Range["H5:H5"].Value = "Địa Chỉ";
            exRange.Range["I5:I5"].Value = "Điện Thoại";
            for (hang = 0; hang < tblThongTinNhanVien.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 6
                exSheet.Cells[1][hang + 6] = hang + 1;
                for (cot = 0; cot < tblThongTinNhanVien.Columns.Count; cot++)
                //Điền thông tin hàng từ cột thứ 2, dòng 6
                {
                    exSheet.Cells[cot + 2][hang + 6] = tblThongTinNhanVien.Rows[hang][cot].ToString();
                    if (cot == 3) exSheet.Cells[cot + 2][hang + 6] = tblThongTinNhanVien.Rows[hang][cot].ToString();
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
            exSheet.Name = "Danh Sách Nhân Viên";
            exApp.Visible = true;
            
        }
    }
}
