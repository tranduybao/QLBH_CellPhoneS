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

    public partial class frmQLNhapXuat : Form
    {
        DataTable tblNhapHang; //Bảng chi tiết hoá đơn bán
        DataTable tblXuatHang;
        DataTable tblCTNhapHang;
        DataTable tblCTXuatHang;
        string sql;
        public frmQLNhapXuat()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnTimNhap_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTime = dtpThoiGian.Value;

            // Lấy các thành phần ngày, tháng và năm
            int day = selectedDateTime.Day;
            int month = selectedDateTime.Month;
            int year = selectedDateTime.Year;
            sql = "SELECT a.MaPhieuNhap, b.TenNhanVien, a.NgayNhap, c.TenNCC, a.TongTien, d.TenKho " +
                    "FROM PhieuNhap AS a, NhanVien AS b, NhaCungCap as c, Kho as d " +
                    "WHERE a.MaNhanVien=b.MaNhanVien AND a.MaNCC=c.MaNCC AND a.MaKho=d.MaKho ";
            if (cboNam.SelectedIndex!=-1)
            {
                sql+= " AND year(NgayNhap)=" + cboNam.Text;
                if (cboThang.SelectedIndex!=-1)
                {
                    sql += " AND month(NgayNhap)=" + cboThang.Text;
                    tblNhapHang = Extend.Funcions.GetDataToTable(sql);
                    dgvNhapKho.DataSource = tblNhapHang;
                    
                }
            }
            if (cboNam.SelectedIndex != -1)
            {
                if (ckbQuy1.Checked)
                {
                    sql += " AND month(NgayNhap) BETWEEN 1 AND 3";
                    
                }
                if (ckbQuy2.Checked)
                {
                    sql += " AND month(NgayNhap) BETWEEN 4 AND 6";
                    
                }
                if (ckbQuy3.Checked)
                {
                    sql += " AND month(NgayNhap) BETWEEN 7 AND 9";
                    
                }
                if (ckbQuy4.Checked)
                {
                    sql += " AND month(NgayNhap) BETWEEN 10 AND 12";
                   
                }
            }
            if (tblNhapHang.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblNhapHang.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            
            ckbQuy1.Enabled = true;
            ckbQuy2.Enabled = true;
            ckbQuy3.Enabled = true;
            ckbQuy4.Enabled = true;

            ThongSo();

        }

        private void LoadNhapHang()
        {
            
            sql = "SELECT a.MaPhieuNhap, b.TenNhanVien, a.NgayNhap, c.TenNCC, a.TongTien, d.TenKho FROM PhieuNhap AS a, NhanVien AS b, NhaCungCap as c, Kho as d WHERE a.MaNhanVien=b.MaNhanVien AND a.MaNCC=c.MaNCC AND a.MaKho=d.MaKho";
            tblNhapHang = Extend.Funcions.GetDataToTable(sql);
            dgvNhapKho.DataSource = tblNhapHang;
            ThongSo();

        }
        private void LoadCTNhapHang()
        {
            sql = "SELECT a.STT,b.TenSanPham, a.SoLuong, a.DonGiaNhap, a.GiamGia, a.ThanhTien FROM CTPhieuNhap AS a, SanPham AS b WHERE a.MaSanPham=b.MaSanPham AND a.MaPhieuNhap='"+ dgvNhapKho.CurrentRow.Cells["MaPhieuNhap"].Value.ToString() + "'";
            tblCTNhapHang = Extend.Funcions.GetDataToTable(sql);
            dgvCTNhapKho.DataSource = tblCTNhapHang;
        }

        private void LoadXuatHang()
        {

            sql = "SELECT a.MaPhieuXuat, b.TenNhanVien, a.NgayXuat, c.TenKhach, a.TongTien, d.TenKho FROM PhieuXuat AS a, NhanVien AS b, KhachHang as c, Kho as d WHERE a.MaNhanVien=b.MaNhanVien AND a.MaKhachHang=c.MaKhachHang AND a.MaKho=d.MaKho";
            tblXuatHang = Extend.Funcions.GetDataToTable(sql);
            dgvXuatKho.DataSource = tblXuatHang;
            ThongSo();

        }

        private void LoadCTXuatHang()
        {

            sql = "SELECT a.STT,b.TenSanPham, a.SoLuong, a.DonGiaBan, a.GiamGia, a.ThanhTien FROM CTPhieuXuat AS a, SanPham AS b WHERE a.MaSanPham=b.MaSanPham AND a.MaPhieuXuat='" + dgvXuatKho.CurrentRow.Cells["MaPhieuXuat"].Value.ToString() + "'";
            tblCTXuatHang = Extend.Funcions.GetDataToTable(sql);
            dgvCTXuatKho.DataSource = tblCTXuatHang;
        }
        private void frmQLNhapXuat_Load(object sender, EventArgs e)
        {
            for (int i = 1990; i <= 2100; i++)
            {
                cboNam.Items.Add(i.ToString());
            }
            LoadNhapHang();
            LoadXuatHang();

            
           


        }
        private void ThongSo()
        {
            decimal chiPhi = TinhTongGiaTriCot(dgvNhapKho, "TongTien");
            decimal doanhThu = TinhTongGiaTriCot(dgvXuatKho, "TongTienBan");

            

            txtDoanhThu.Text = doanhThu.ToString()+ " VNĐ";
            txtChiPhi.Text = chiPhi.ToString()+ " VNĐ";
            decimal VAT = doanhThu * 5 / 100;
            txtThueVAT.Text = VAT.ToString() + " VNĐ";
            txtThueKhac.Text = ((doanhThu - VAT) * 3 / 100).ToString() + " VNĐ";
            txtLoiNhuan.Text = (doanhThu - chiPhi).ToString()+" VNĐ";
        }
        private decimal TinhTongGiaTriCot(DataGridView dataGridView, string columnName)
        {
            decimal tong = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                object cellValue = row.Cells[columnName].Value;
                if (cellValue != null)
                {
                    string valueString = cellValue.ToString();

                    // Loại bỏ ký tự "VNĐ" và khoảng trắng
                    valueString = valueString.Replace("VNĐ", "").Trim();

                    if (decimal.TryParse(valueString, out decimal giaTri))
                    {
                        tong += giaTri;
                    }
                }
            }
            return tong;
        }

        private void dgvNhapKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvNhapKho.CurrentCell == null || dgvNhapKho.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            LoadCTNhapHang();
        }

        private void btnHienDanhSachNhap_Click(object sender, EventArgs e)
        {
            
            LoadNhapHang();

        }

        private void btnHienDanhSachXuat_Click(object sender, EventArgs e)
        {
            LoadXuatHang();
        }

        private void dgvXuatKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvXuatKho.CurrentCell == null || dgvXuatKho.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            LoadCTXuatHang();
        }

        private void btnXoaNhapKho_Click(object sender, EventArgs e)
        {
            
            if (dgvNhapKho.SelectedCells.Count > 0)
            {
                sql = "DELETE CTPhieuNhap WHERE MaPhieuNhap=N'" + dgvNhapKho.CurrentRow.Cells["MaPhieuNhap"].Value.ToString() + "'";
                Extend.Funcions.RunSQL(sql);
                sql = "DELETE PhieuNhap WHERE MaPhieuNhap=N'" + dgvNhapKho.CurrentRow.Cells["MaPhieuNhap"].Value.ToString() + "'";
                Extend.Funcions.RunSQL(sql);
                LoadNhapHang();
                return;
            }
            
            // Thông báo khi không có dòng nào được chọn
            MessageBox.Show("Không có dòng nào được chọn!");



        }
        private void btnXoaCTPhieuNhap_Click(object sender, EventArgs e)
        {
            if (dgvCTNhapKho.SelectedCells.Count > 0)
            {
                sql = "DELETE CTPhieuNhap WHERE STT=N'" + dgvCTNhapKho.CurrentRow.Cells["STT"].Value.ToString() + "'";
                Extend.Funcions.RunSQL(sql);
                LoadCTNhapHang();
                return;
            }
            // Thông báo khi không có dòng nào được chọn
            MessageBox.Show("Không có dòng nào được chọn!");
        }

        private void btnSuaNhapKho_Click(object sender, EventArgs e)
        {
            if (dgvNhapKho.SelectedCells.Count > 0)
            {
                sql = "DELETE CTPhieuNhap WHERE MaPhieuNhap=N'" + dgvNhapKho.CurrentRow.Cells["MaPhieuNhap"].Value.ToString() + "'";
                Extend.Funcions.RunSQL(sql);
                sql = "DELETE PhieuNhap WHERE MaPhieuNhap=N'" + dgvNhapKho.CurrentRow.Cells["MaPhieuNhap"].Value.ToString() + "'";
                Extend.Funcions.RunSQL(sql);
                LoadNhapHang();
                return;
            }

            // Thông báo khi không có dòng nào được chọn
            MessageBox.Show("Không có dòng nào được chọn!");
        }

        private void cboNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < 13; i++)
            {
                cboThang.Items.Add(i.ToString());
            }
            ckbQuy1.Enabled = true;
            ckbQuy2.Enabled = true;
            ckbQuy3.Enabled = true;
            ckbQuy4.Enabled = true;
            cboThang.Enabled = true;
        }



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDateTime = dtpThoiGian.Value;

            // Lấy các thành phần ngày, tháng và năm
            int day = selectedDateTime.Day;
            int month = selectedDateTime.Month;
            int year = selectedDateTime.Year;
            sql = "SELECT a.MaPhieuNhap, b.TenNhanVien, a.NgayNhap, c.TenNCC, a.TongTien, d.TenKho " +
                "FROM PhieuNhap AS a, NhanVien AS b, NhaCungCap as c, Kho as d " +
                "WHERE a.MaNhanVien=b.MaNhanVien AND a.MaNCC=c.MaNCC AND a.MaKho=d.MaKho " +
                " AND day(NgayNhap)=" + day + "AND month(NgayNhap)=" + month + "AND year(NgayNhap)=" + year;
            if (txtNhanVien.Text != "")
                sql += " AND b.TenNhanVien LIKE N'%" + txtNhanVien.Text + "%'";
            if (txtKho.Text != "")
                sql += " AND d.TenKho LIKE N'%" + txtKho.Text + "%'";
            dgvNhapKho.DataSource = Extend.Funcions.GetDataToTable(sql);
            sql = "SELECT a.MaPhieuXuat, b.TenNhanVien, a.NgayXuat, c.TenKhach, a.TongTien, d.TenKho " +
                "FROM PhieuXuat AS a, NhanVien AS b, KhachHang as c, Kho as d " +
                "WHERE a.MaNhanVien=b.MaNhanVien AND a.MaKhachHang=c.MaKhachHang AND a.MaKho=d.MaKho " +
                "AND day(NgayXuat)=" + day + "AND month(NgayXuat)=" + month + "AND year(NgayXuat)=" + year;
            if (txtNhanVien.Text != "")
                sql += " AND b.TenNhanVien LIKE N'%" + txtNhanVien.Text + "%'";
            if (txtKho.Text != "")
                sql += " AND d.TenKho LIKE N'%" + txtKho.Text + "%'";
            dgvXuatKho.DataSource = Extend.Funcions.GetDataToTable(sql);

            ThongSo();
        }

        private void btnTimXuat_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTime = dtpThoiGian.Value;

            // Lấy các thành phần ngày, tháng và năm
            int day = selectedDateTime.Day;
            int month = selectedDateTime.Month;
            int year = selectedDateTime.Year;
            sql = "SELECT a.MaPhieuXuat, b.TenNhanVien, a.NgayXuat, c.TenKhach, a.TongTien, d.TenKho " +
                "FROM PhieuXuat AS a, NhanVien AS b, KhachHang as c, Kho as d " +
                "WHERE a.MaNhanVien=b.MaNhanVien AND a.MaKhachHang=c.MaKhachHang AND a.MaKho=d.MaKho ";
            if (cboNam.SelectedIndex != -1)
            {
                sql += " AND year(NgayXuat)=" + cboNam.Text;
                if (cboThang.SelectedIndex != -1)
                {
                    sql += " AND month(NgayXuat)=" + cboThang.Text;
                    tblXuatHang = Extend.Funcions.GetDataToTable(sql);
                    dgvXuatKho.DataSource = tblXuatHang;

                }
            }
            if (cboNam.SelectedIndex != -1)
            {
                if (ckbQuy1.Checked)
                {
                    sql += " AND month(NgayXuat) BETWEEN 1 AND 3";

                }
                if (ckbQuy2.Checked)
                {
                    sql += " AND month(NgayXuat) BETWEEN 4 AND 6";

                }
                if (ckbQuy3.Checked)
                {
                    sql += " AND month(NgayXuat) BETWEEN 7 AND 9";

                }
                if (ckbQuy4.Checked)
                {
                    sql += " AND month(NgayXuat) BETWEEN 10 AND 12";

                }
            }
            if (tblXuatHang.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblXuatHang.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


            ckbQuy1.Enabled = true;
            ckbQuy2.Enabled = true;
            ckbQuy3.Enabled = true;
            ckbQuy4.Enabled = true;
            ThongSo();
        }

        private void cboThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            ckbQuy1.Enabled = false;
            ckbQuy2.Enabled = false;
            ckbQuy3.Enabled = false;
            ckbQuy4.Enabled = false;
        }

        private void ckbQuy1_CheckedChanged(object sender, EventArgs e)
        {
            cboThang.SelectedIndex = -1;
            ckbQuy1 .Enabled = true;
            ckbQuy2.Enabled = false;
            ckbQuy3.Enabled = false;
            ckbQuy4.Enabled = false;
        }

        private void ckbQuy2_CheckedChanged(object sender, EventArgs e)
        {
            cboThang.SelectedIndex = -1;
            ckbQuy1.Enabled = false;
            ckbQuy2.Enabled = true;
            ckbQuy3.Enabled = false;
            ckbQuy4.Enabled = false;
        }

        private void ckbQuy3_CheckedChanged(object sender, EventArgs e)
        {
            cboThang.SelectedIndex = -1;
            ckbQuy1.Enabled = false;
            ckbQuy2.Enabled = false;
            ckbQuy3.Enabled = true;
            ckbQuy4.Enabled = false;
        }

        private void ckbQuy4_CheckedChanged(object sender, EventArgs e)
        {
            cboThang.SelectedIndex = -1;
            ckbQuy1.Enabled = false;
            ckbQuy2.Enabled = false;
            ckbQuy3.Enabled = false;
            ckbQuy4.Enabled = true;
        }

        private void dgvCTNhapKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvCTNhapKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCTNhapKho.CurrentCell == null || dgvCTNhapKho.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
        }

        private void dgvCTXuatKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCTXuatKho.CurrentCell == null || dgvCTXuatKho.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
        }
    }
}
