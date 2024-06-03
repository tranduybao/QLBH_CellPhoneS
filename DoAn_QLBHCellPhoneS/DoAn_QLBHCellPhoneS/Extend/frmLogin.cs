using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DoAn_QLBHCellPhoneS.Extend
{
    public partial class frmLogin : Form
    {
        public static string tenNhanVienDangNhap;
        public static string tenChucVu;
        public static string diaChiChiNhanh;
        public static string SDTChiNhanh;

        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Extend.KetNoi.Disconnect();
            Application.Exit();
        }
        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thực sự muốn thoát chương trình ?", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }

        }
        private bool KiemTraDangNhap(string userName, string passWord, string chucVu)
        {
                Extend.KetNoi.Connect();
                string sql = "SELECT COUNT(*) FROM Account AS acc " +
                                "INNER JOIN NhanVien AS nv ON acc.MaNhanVien = nv.MaNhanVien " +
                                "WHERE acc.userName = @UserName AND acc.PassWord = @PassWord AND nv.MaChucVu = @MaChucVu";
                SqlCommand command = new SqlCommand(sql,Extend.KetNoi.Con);
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@PassWord", passWord);
                command.Parameters.AddWithValue("@MaChucVu", chucVu);

                int count = (int)command.ExecuteScalar();

                return count > 0;
            
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {

            string userName = tbTenDangNhap.Text;
            string passWord = tbMatKhau.Text;
            string chucVu = cboChucVu.SelectedValue.ToString();

            // Kiểm tra người dùng đã nhập tên đăng nhập và mật khẩu hay chưa
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiểm tra tài khoản và mật khẩu trong cơ sở dữ liệu
            bool isValid = KiemTraDangNhap(userName, passWord, chucVu);

            if (isValid)
            {
                string sql = "SELECT b.TenNhanVien, c.TenChucVu, d.DiaChi, d.DienThoai FROM Account as a, NhanVien as b, ChucVu as c, ChiNhanh as d WHERE b.MaChiNhanh=d.MaChiNhanh AND b.MaChucVu=c.MaChucVu AND a.MaNhanVien=b.MaNhanVien AND UserName = '" + userName + "' AND PassWord ='" + passWord + "'";
                SqlCommand command = new SqlCommand(sql, Extend.KetNoi.Con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                tenNhanVienDangNhap = reader["TenNhanVien"].ToString();
                tenChucVu= reader["TenChucVu"].ToString();
                diaChiChiNhanh= reader["DiaChi"].ToString();
                SDTChiNhanh = reader["DienThoai"].ToString();
                reader.Close();
                if (chucVu == "NVBH")
                {
                    Staff.frmBanHang f = new Staff.frmBanHang();
                    this.Hide();
                    f.ShowDialog();
                    this.Show();
                    Extend.KetNoi.Disconnect();
                }
                else if (chucVu == "QL")
                {
                    Manager.frmAdmin f = new Manager.frmAdmin();
                    this.Hide();
                    f.ShowDialog();
                    this.Show();
                    Extend.KetNoi.Disconnect();
                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            Extend.KetNoi.Connect();
            MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Extend.Funcions.FillCombo("SELECT * FROM ChucVu", cboChucVu, "MaChucVu", "TenChucVu");
            cboChucVu.SelectedIndex = 1;
        }

        private void ckbHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbHienMatKhau.Checked)
            {
                tbMatKhau.UseSystemPasswordChar = false;
            }
            else
            {
                tbMatKhau.UseSystemPasswordChar = true;
            }
        }
    }
}
