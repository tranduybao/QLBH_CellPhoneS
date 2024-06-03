using DoAn_QLBHCellPhoneS.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_QLBHCellPhoneS.Staff
{
    public partial class frmBanHang : Form
    {
        string sql;
        public frmBanHang()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinỨngDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongTinLienHe f = new frmThongTinLienHe();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDoiMatKhau f = new frmDoiMatKhau();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }


        


        private void txtTimKiemMaSP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    
                    // Truy vấn dữ liệu sản phẩm từ SQL Server
                    string query = "SELECT * FROM SanPham WHERE MaSanPham = @MaSanPham";
                    SqlCommand command1 = new SqlCommand(query, Extend.KetNoi.Con);
                    command1.Parameters.AddWithValue("@MaSanPham", txtTimKiemMaSP.Text);
                   
                    SqlDataReader reader = command1.ExecuteReader();

                    // Kiểm tra nếu có kết quả
                    if (reader.Read())
                    {
                        // Hiển thị thông tin sản phẩm lên các TextBox
                        txtTenSanPham.Text = reader["TenSanPham"].ToString();
                        txtLoaiSanPham.Text = reader["MaLoaiHang"].ToString();
                        txtGiaBan.Text = reader["DonGiaBan"].ToString();
                        txtSoLuongTon.Text = reader["SoLuong"].ToString();
                        txtThongTin.Text = reader["GhiChu"].ToString();
                        if (reader["Anh"].ToString() != "")
                        {
                            pAnh.Image = Image.FromFile(reader["Anh"].ToString());
                        }
                        else
                        {
                            pAnh.Image = null;
                        }
                        txtKhoHang.Text= reader["MaKho"].ToString();



                    }
                    else
                    {
                        // Nếu không tìm thấy sản phẩm, xóa nội dung các TextBox

                        MessageBox.Show("Không tìm thấy sản phẩm với Mã Sản Phẩm này", "Thông báo!", MessageBoxButtons.OKCancel);
                        
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi khi truy vấn dữ liệu
                    MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message);
                }



            }
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            Extend.Funcions.FillCombo("SELECT * FROM NhanVien", cboNhanVien, "MaNhanVien", "TenNhanVien");

        }

        private void btnThemHang_Click(object sender, EventArgs e)
        {
            string maSanPham = txtTimKiemMaSP.Text.Trim();
            string tenSanPham = txtTenSanPham.Text.Trim();
            string maLoaiHang = txtLoaiSanPham.Text.Trim();
            double soLuong = double.Parse(nSoLuongBan.Value.ToString());
            double giaSanPham = double.Parse(txtGiaBan.Text.Trim());
            double giamGia = double.Parse(nGiamGia.Value.ToString());
            double thanhTien = soLuong*giaSanPham-giamGia;

            bool daTonTai = false;
            int rowIndex = -1;

            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                DataGridViewRow row = dgvData.Rows[i];

                if (row.Cells["MaSanPham"].Value == null)
                {
                    daTonTai = false;
                    rowIndex = i;
                    break;
                }
                else if (row.Cells["MaSanPham"].Value.ToString() == maSanPham)
                {
                        daTonTai = true;
                        rowIndex = i;
                        break;
                }
            }
            if (daTonTai)
            {
                // Nếu sản phẩm đã tồn tại, kiểm tra số lượng
                double tongSoLuong = double.Parse(dgvData.Rows[rowIndex].Cells["SoLuong"].Value.ToString()) + soLuong;
                double tongGiamGia = double.Parse(dgvData.Rows[rowIndex].Cells["GiamGia"].Value.ToString()) + giamGia;

                if (decimal.Parse(dgvData.Rows[rowIndex].Cells["SoLuong"].Value.ToString()) + nSoLuongBan.Value > decimal.Parse(txtSoLuongTon.Text))
                {
                    MessageBox.Show("Không đủ hàng trong kho.", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Cộng dồn số lượng và cập nhật tổng tiền, thành tiền
                dgvData.Rows[rowIndex].Cells["SoLuong"].Value = tongSoLuong;
                dgvData.Rows[rowIndex].Cells["GiamGia"].Value = tongGiamGia;
                dgvData.Rows[rowIndex].Cells["ThanhTien"].Value = thanhTien;
            }
            else
            {
                if (nSoLuongBan.Value > decimal.Parse(txtSoLuongTon.Text))
                {
                    MessageBox.Show("Không đủ hàng trong kho.", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Nếu sản phẩm chưa tồn tại, thêm mới vào dgvData
                dgvData.Rows.Add(maSanPham, tenSanPham, maLoaiHang, soLuong, giaSanPham, giamGia, thanhTien);

            }
            // Nếu sản phẩm chưa tồn tại, thêm mới vào dgvData
            


            string tongTien, tienChu;
            Extend.Funcions.TinhTongTien(dgvData, out tongTien, out tienChu);
            txtTongTien.Text = tongTien;
            lblTienChu.Text = tienChu;
        }

        private void btnXoaThem_Click(object sender, EventArgs e)
        {
            // Lấy chỉ số hàng đang được chọn trong DataGridView
            int rowIndex = dgvData.SelectedCells[0].RowIndex;

            // Xóa hàng tương ứng khỏi DataTable
            dgvData.Rows.RemoveAt(rowIndex);

            // Ẩn nút Xóa sau khi xóa hàng
            btnXoaThem.Enabled = false;

            string tongTien, tienChu;
            Extend.Funcions.TinhTongTien(dgvData, out tongTien, out tienChu);
            txtTongTien.Text = tongTien;
            lblTienChu.Text = tienChu;
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.CurrentCell == null || dgvData.CurrentCell.Value == null)
            {
                MessageBox.Show("Ô không có dữ liệu hoặc danh sách rỗng !");
                return;
            }
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvData.Rows[e.RowIndex];

                // Hiển thị thông tin từ dòng chọn lên các TextBox tương ứng
                txtTimKiemMaSP.Text = row.Cells["MaSanPham"].Value.ToString();
                txtTimKiemMaSP_KeyPress(this, new KeyPressEventArgs((char)Keys.Enter));
                // Hiển thị nút Xóa
                btnXoaThem.Enabled = true;
            }
            else
            {
                // Nếu không có hàng nào được chọn, ẩn nút Xóa
                btnXoaThem.Enabled = false;
            }



        }
        private bool IsPhoneNumberExist(string phoneNumber)
        {
                string query = "SELECT COUNT(*) FROM KhachHang WHERE DienThoai = @PhoneNumber";
                using (SqlCommand command = new SqlCommand(query, Extend.KetNoi.Con))
                {
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
        }
        private void btnKiemTraKhach_Click(object sender, EventArgs e)
        {
            

            // Kiểm tra số điện thoại có tồn tại trong CSDL và lấy thông tin khách hàng từ CSDL
            if (!string.IsNullOrEmpty(mtbDienThoai.Text))
            {


                // Kiểm tra nếu có kết quả
                if (IsPhoneNumberExist(mtbDienThoai.Text))
                {
                    string query = "SELECT * FROM KhachHang WHERE DienThoai = @PhoneNumber";
                    using (SqlCommand command = new SqlCommand(query, Extend.KetNoi.Con))
                    {
                        command.Parameters.AddWithValue("@PhoneNumber", mtbDienThoai.Text);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtMaKhach.Text = reader["MaKhachHang"].ToString();
                                txtTenKhach.Text = reader["TenKhach"].ToString();
                                txtDiaChi.Text = reader["DiaChi"].ToString();
                                cboHangThanhVien.Text= reader["HangThanhVien"].ToString();
                                // Các trường thông tin khác tương tự
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Số điện thoại không tồn tại trong CSDL.");
                    txtMaKhach.Text = "";
                    txtTenKhach.Text = "";
                    txtDiaChi.Text = "";
                    mtbDienThoai.Text = "";
                }
            }
            else
            {
                // Xử lý trường hợp người dùng chưa nhập số điện thoại
                MessageBox.Show("Vui lòng nhập số điện thoại.");
            }
        }

        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            txtMaKhach.Text = Extend.Funcions.TaoMaMoi("MaKhachHang", "KhachHang", "KH");
            txtDiaChi.Enabled= true;
            txtTenKhach.Enabled = true;
            btnLuu.Enabled = true;
            cboHangThanhVien.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtTenKhach.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenKhach.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiaChi.Focus();
                return;
            }

            //Chèn thêm
            sql = "INSERT INTO KhachHang VALUES (N'" + txtMaKhach.Text.Trim() +
                "',N'" + txtTenKhach.Text.Trim() + "',N'" + txtDiaChi.Text.Trim() + "','" + mtbDienThoai.Text + "',N'" + cboHangThanhVien.Text + "')";
            Extend.Funcions.RunSQL(sql);
            MessageBox.Show("Lưu thành công!");

            btnThemKhach.Enabled = false;
            txtDiaChi.Enabled = false;
            txtTenKhach.Enabled = false;
            btnLuu.Enabled = false;
            cboHangThanhVien.Enabled = false;

        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {


            string query;
            txtMaHDBan.Text = Extend.Funcions.CreateKey("HDB");

            if (cboNhanVien.SelectedIndex == -1 || txtMaKhach.Text == "" )
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin ");
                return;
            }
            query = "INSERT INTO PhieuXuat (MaPhieuXuat, MaNhanVien,NgayXuat, MaKhachHang, TongTien,MaKho) VALUES (@1, @2, @3, @4, @5, @6)";
            Extend.Funcions.LuuHDBanVaoCSDL(query, txtMaHDBan.Text, cboNhanVien.SelectedValue.ToString(), txtMaKhach.Text, txtTongTien.Text, txtKhoHang.Text);
            query = "INSERT INTO CTPhieuXuat (STT,MaPhieuXuat, MaSanPham, SoLuong, DonGiaBan, GiamGia, ThanhTien) VALUES (@1, @2, @3, @4, @5, @6, @7)";
            int i = 0;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (!row.IsNewRow)
                {
                    string STT = txtMaHDBan.Text + "STT" + i.ToString();
                    string maPhieuNhap = txtMaHDBan.Text;
                    string maSanPham = row.Cells["MaSanPham"].Value.ToString();
                    string soLuong = row.Cells["SoLuong"].Value.ToString();
                    string giaSanPham = row.Cells["DonGiaBan"].Value.ToString();
                    string giamGia = row.Cells["GiamGia"].Value.ToString();
                    string thanhTien = row.Cells["ThanhTien"].Value.ToString();

                    Extend.Funcions.LuuChiTietHDVaoCSDL(query, STT, maPhieuNhap, maSanPham, soLuong, giaSanPham, giamGia, thanhTien);
                    string soLuongConLai = (double.Parse(txtSoLuongTon.Text) - double.Parse(row.Cells["SoLuong"].Value.ToString())).ToString();
                    query = "UPDATE SanPham SET SoLuong ='" + soLuongConLai + "' WHERE MaSanPham =N'" + row.Cells["MaSanPham"].Value.ToString()+"'";
                    Extend.Funcions.RunSQL(query);
                    i++;
                }
            }
            Extend.frmBaoCao reportForm = new Extend.frmBaoCao(txtMaHDBan.Text);
            reportForm.Show();
            


        }

        private void thôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gửiYêuCầuNhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cboNhanVien.Text == "") //Nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn phải chọn nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboNhanVien.Focus();
                return;
            }
            sql = "INSERT INTO YeuCauNhapHang VALUES(N'" +
                cboNhanVien.SelectedValue + "',N'" +DateTime.Now+ "',N'" + txtTimKiemMaSP.Text + "')";
            Extend.Funcions.RunSQL(sql); //Thực hiện câu lệnh sql
            MessageBox.Show("Đã gửi yêu cầu nhập hàng đến quản lý vào lúc : "+DateTime.Now, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }
    }
}
