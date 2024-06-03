using DoAn_QLBHCellPhoneS.Extend;
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
    public partial class frmSanPham : Form
    {
       
        DataTable tblSanPham;
        string sql;

        public frmSanPham()
        {
            InitializeComponent();
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            LoadDataGridView();
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            txtMaSanPham.Enabled = false;
            txtSoLuong.Enabled = false;
            txtDonGiaNhap.Enabled = false;
            LoadDataGridView();
            Extend.Funcions.FillCombo("SELECT * FROM LoaiHang", cboMaLoaiHang, "MaLoaiHang", "TenLoaiHang");
            Extend.Funcions.FillCombo("SELECT * FROM Kho", cboKho, "MaKho", "TenKho");
            Extend.Funcions.FillCombo("SELECT * FROM NhaCungCap", cboNhaCungCap, "MaNCC", "TenNCC");
            cboMaLoaiHang.SelectedIndex = -1;
            cboNhaCungCap.SelectedIndex = -1;
            cboKho.SelectedIndex = -1;
            ResetValues();
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * from SanPham";
            tblSanPham = Extend.Funcions.GetDataToTable(sql);
            dgvData.DataSource = tblSanPham;
            dgvData.Columns[0].HeaderText = "Mã Sản Phẩm";
            dgvData.Columns[1].HeaderText = "Tên Sản Phẩm";
            dgvData.Columns[2].HeaderText = "Loại Sản Phẩm";
            dgvData.Columns[3].HeaderText = "Số lượng";
            dgvData.Columns[4].HeaderText = "Đơn giá nhập";
            dgvData.Columns[5].HeaderText = "Đơn giá bán";
            dgvData.Columns[6].HeaderText = "Ảnh";
            dgvData.Columns[7].HeaderText = "Thông Tin";
            dgvData.Columns[8].HeaderText = "Kho";
            dgvData.Columns[9].HeaderText = "Nhà Cung Cấp";
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvData.AllowUserToAddRows = false;
            dgvData.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void ResetValues()
        {
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            txtSoLuong.Text = "0";
            txtDonGiaNhap.Text = "0";
            txtDonGiaBan.Text = "0";
            
            txtAnh.Text= "";   
            pAnh.Image = null;
            txtThongTin.Text = "";
        }

       

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            if (cboMaLoaiHang.SelectedValue == null || string.IsNullOrEmpty(cboMaLoaiHang.SelectedValue.ToString()))
            {
                MessageBox.Show("Vui lòng chọn mã loại hàng trước khi thực hiện chức năng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboNhaCungCap.SelectedValue == null || string.IsNullOrEmpty(cboNhaCungCap.SelectedValue.ToString()))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp trước khi thực hiện chức năng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboKho.SelectedValue == null || string.IsNullOrEmpty(cboKho.SelectedValue.ToString()))
            {
                MessageBox.Show("Vui lòng chọn kho hàng trước khi thực hiện chức năng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //Tạo mã mới từ mã cuối
            txtMaSanPham.Text = Extend.Funcions.TaoMaMoi("MaSanPham", "SanPham", cboMaLoaiHang.SelectedValue.ToString()+cboKho.SelectedValue.ToString()+cboNhaCungCap.SelectedValue.ToString());
            // Các thao tác khác sau khi tạo mã sản phẩm mới
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            txtSoLuong.Enabled = false;
            txtDonGiaNhap.Enabled = false;
            
            
            txtTenSanPham.Focus();
            
        }

        internal void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            
            if (txtTenSanPham.Text.Trim().Length == 0 || txtTenSanPham.Text.Trim().Length > 50)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenSanPham.Focus();
                return;
            }
            if (cboKho.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn kho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboKho.Focus();
                return;
            }
            if (cboNhaCungCap.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboNhaCungCap.Focus();
                return;
            }
            if (txtAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn hình ảnh cho sản phẩm(Vui lòng ấn nút CHỌN ẢNH)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnMo.Focus();
                return;
            }
            if (txtThongTin.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập thông tin sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtThongTin.Focus();
                return;
            }
            sql = "INSERT INTO SanPham(MaSanPham,TenSanPham,MaLoaiHang,SoLuong,DonGiaNhap, DonGiaBan,Anh,GhiChu,MaKho,MaNCC) VALUES(N'"
                + txtMaSanPham.Text.Trim() + "',N'" + txtTenSanPham.Text.Trim() +
                "',N'" + cboMaLoaiHang.SelectedValue.ToString() +
                "'," + 0 + "," + 0 +
                "," + 0 + ",'" + txtAnh.Text + "',N'" + txtThongTin.Text.Trim() + "',N'" + cboKho.SelectedValue.ToString()+"',N'" + cboNhaCungCap.SelectedValue.ToString() + "')";

            Extend.Funcions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnBoQua.Enabled = false;
            cboMaLoaiHang.SelectedIndex = -1;

           

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            
            if (txtTenSanPham.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenSanPham.Focus();
                return;
            }
            if (cboMaLoaiHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn loại hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaLoaiHang.Focus();
                return;
            }
            
            sql = "UPDATE SanPham SET TenSanPham=N'" + txtTenSanPham.Text.Trim().ToString() +
                "',MaLoaiHang=N'" + cboMaLoaiHang.SelectedValue +
                "',SoLuong=" + float.Parse(txtSoLuong.Text) +
                ",DonGiaNhap=" + float.Parse(txtDonGiaNhap.Text) +
                ",DonGiaBan=" + float.Parse(txtDonGiaBan.Text) +
                ",Anh='" + txtAnh.Text + 
                "',GhiChu=N'" + txtThongTin.Text +
                "',MaKho=N'" + cboKho.SelectedValue +
                "',MaNCC=N'" + cboNhaCungCap.SelectedValue +
                "' WHERE MaSanPham=N'" + txtMaSanPham.Text + "'";
            Extend.Funcions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnBoQua.Enabled = false;
            txtMaSanPham.Text = "";
            cboMaLoaiHang.SelectedIndex = -1;

            
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE SanPham WHERE MaSanPham=N'" + txtMaSanPham.Text + "'";
                Extend.Funcions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
                txtMaSanPham.Text = "";
            }
            cboMaLoaiHang.SelectedIndex = -1;

           
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            cboMaLoaiHang.SelectedIndex = -1;
            txtMaSanPham.Text = "";
        }

        private void btnMo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn ảnh minh hoạ cho sản phẩm";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                pAnh.Image = Image.FromFile(dlgOpen.FileName);
                txtAnh.Text = dlgOpen.FileName;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;
            if ((txtMaSanPham.Text == "") && (txtTenSanPham.Text == "") && (cboMaLoaiHang.Text == ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from SanPham WHERE 1=1";
            if (txtMaSanPham.Text != "")
                sql += " AND MaSanPham LIKE N'%" + txtMaSanPham.Text + "%'";
            if (txtTenSanPham.Text != "")
                sql += " AND TenSanPham LIKE N'%" + txtTenSanPham.Text + "%'";
            if (cboMaLoaiHang.Text != "")
                sql += " AND MaLoaiHang LIKE N'%" + cboMaLoaiHang.SelectedValue + "%'";
            if (cboKho.Text != "")
                sql += " AND Kho LIKE N'%" + cboKho.SelectedValue + "%'";
            if (cboNhaCungCap.Text != "")
                sql += " AND MaNCC LIKE N'%" + cboNhaCungCap.SelectedValue + "%'";
            tblSanPham = Extend.Funcions.GetDataToTable(sql);
            if (tblSanPham.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblSanPham.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvData.DataSource = tblSanPham;
            ResetValues();
            cboMaLoaiHang.SelectedIndex = -1;
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            int vitri = dgvData.CurrentCell.RowIndex;
            txtMaSanPham.Text = dgvData.Rows[vitri].Cells[0].Value.ToString();
            txtTenSanPham.Text = dgvData.Rows[vitri].Cells[1].Value.ToString();
            cboMaLoaiHang.SelectedValue = dgvData.Rows[vitri].Cells[2].Value.ToString();
            txtSoLuong.Text = dgvData.Rows[vitri].Cells[3].Value.ToString();
            txtDonGiaNhap.Text = dgvData.Rows[vitri].Cells[4].Value.ToString();
            txtDonGiaBan.Text = dgvData.Rows[vitri].Cells[5].Value.ToString();
            txtAnh.Text = dgvData.Rows[vitri].Cells[6].Value.ToString();
            if (txtAnh.Text!="")
            {
                pAnh.Image = Image.FromFile(txtAnh.Text);
            }
            else
            {
                pAnh.Image = null;
            }
            txtThongTin.Text = dgvData.Rows[vitri].Cells[7].Value.ToString();
            cboKho.SelectedValue = dgvData.Rows[vitri].Cells[8].Value.ToString();
            cboNhaCungCap.SelectedValue = dgvData.Rows[vitri].Cells[9].Value.ToString();
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
            exRange.Range["A2:B2"].Value = "Địa Chỉ : " + Extend.frmLogin.diaChiChiNhanh;
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại : " + Extend.frmLogin.SDTChiNhanh; ;
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "DANH SÁCH SẢN PHẨM ";

            tblThongTinNhanVien = tblSanPham;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A5:K5"].Font.Bold = true;
            exRange.Range["A5:K5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C5:K5"].ColumnWidth = 20;
            exRange.Range["A5:A5"].Value = "STT";
            exRange.Range["B5:B5"].Value = "Mã Sản Phẩm";
            exRange.Range["C5:C5"].Value = "Tên Sản Phẩm";
            exRange.Range["D5:D5"].Value = "Loại Hàng";
            exRange.Range["E5:E5"].Value = "Số Lượng";
            exRange.Range["F5:F5"].Value = "Đơn Giá Nhập";
            exRange.Range["G5:G5"].Value = "Đơn Giá Bán";
            exRange.Range["H5:H5"].Value = "Ảnh";
            exRange.Range["I5:I5"].Value = "Ghi Chú";
            exRange.Range["J5:J5"].Value = "Kho";
            exRange.Range["K5:K5"].Value = "Nhà Cung Cấp";
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
            exSheet.Name = "Danh Sách Sản Phẩm";
            exApp.Visible = true;


        }
    }
}
