using CrystalDecisions.ReportAppServer.ReportDefModel;
using DoAn_QLBHCellPhoneS.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_QLBHCellPhoneS.Manager
{
    public partial class frmYeuCauNhapHang : Form
    {
        string sql;
        public frmYeuCauNhapHang()
        {
            InitializeComponent();
        }

        private void dtpThoiGian_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDateTime = dtpThoiGian.Value;

            // Lấy các thành phần ngày, tháng và năm
            int day = selectedDateTime.Day;
            int month = selectedDateTime.Month;
            int year = selectedDateTime.Year;
            sql = "SELECT  b.TenNhanVien, a.ThoiGian, c.TenSanPham " +
                " FROM YeuCauNhapHang AS a, NhanVien AS b, SanPham as c " +
                " WHERE a.MaNhanVien=b.MaNhanVien AND a.MaSanPham=c.MaSanPham " +
                " AND day(ThoiGian)= " + day + " AND month(ThoiGian)= " + month + " AND year(ThoiGian)= " + year;
            if (cboNhanVien.Text != "")
                sql += " AND b.TenNhanVien LIKE N'%" + cboNhanVien.SelectedValue + "%'";
            if (cboSanPham.Text != "")
                sql += " AND c.TenSanPham LIKE N'%" + cboSanPham.SelectedValue + "%'";
            dgvData.DataSource = Extend.Funcions.GetDataToTable(sql);
            

            
        }

        private void frmYeuCauNhapHang_Load(object sender, EventArgs e)
        {
            Extend.Funcions.FillCombo("SELECT * FROM NhanVien", cboNhanVien, "MaNhanVien", "TennhanVien");
            Extend.Funcions.FillCombo("SELECT * FROM SanPham", cboSanPham, "MaSanPham", "TenSanPham");
            btnXoaYeuCau.Enabled = false;
            btnXoaToanBo.Enabled = false;
            cboNhanVien.Text = "";
            cboSanPham.Text = "";
            sql = "SELECT  b.TenNhanVien, a.ThoiGian, c.TenSanPham " +
                " FROM YeuCauNhapHang as a, NhanVien as b, SanPham as c " +
                " WHERE a.MaNhanVien=b.MaNhanVien AND a.MaSanPham=c.MaSanPham ";
            dgvData.DataSource = Extend.Funcions.GetDataToTable(sql);
        }

        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {

        }

        private void btnHienDanhSach_Click(object sender, EventArgs e)
        {
            frmYeuCauNhapHang_Load(this,EventArgs.Empty);



        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            cboNhanVien.Text = dgvData.CurrentRow.Cells["MaNhanVien"].Value.ToString();
            cboSanPham.Text = dgvData.CurrentRow.Cells["MaSanPham"].Value.ToString();
            
            btnXoaYeuCau.Enabled = true;
            btnXoaToanBo.Enabled = true;
        }

        private void btnXoaYeuCau_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataGridViewRow selectedRow = dgvData.CurrentRow;
                if (selectedRow != null)
                {
                    string thoiGianValue = selectedRow.Cells["ThoiGian"].Value.ToString();
                    sql = "DELETE YeuCauNhapHang WHERE ThoiGian=N'" + thoiGianValue + "'";
                    Extend.Funcions.RunSQL(sql);
                    frmYeuCauNhapHang_Load(this, EventArgs.Empty);
                    btnXoaYeuCau.Enabled = false;
                    btnXoaToanBo.Enabled = false;
                }
            }
        }

        private void btnXoaToanBo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DateTime selectedDateTime = dtpThoiGian.Value;

                // Lấy các thành phần ngày, tháng và năm
                int day = selectedDateTime.Day;
                int month = selectedDateTime.Month;
                int year = selectedDateTime.Year;
                sql = "DELETE YeuCauNhapHang WHERE day(ThoiGian)=" + day + "AND month(ThoiGian)=" + month + "AND year(ThoiGian)=" + year; 
                Extend.Funcions.RunSQL(sql);
                frmYeuCauNhapHang_Load(this, EventArgs.Empty);
                btnXoaYeuCau.Enabled = false;
                btnXoaToanBo.Enabled = false;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
