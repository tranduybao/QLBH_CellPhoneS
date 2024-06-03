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

namespace DoAn_QLBHCellPhoneS.Staff
{
    public partial class frmDoiMatKhau : Form
    {
        public frmDoiMatKhau()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool IsCorrectPass(string UserName, string PassWord)
        {
            string query = "SELECT COUNT(*) FROM Account WHERE UserName = @UserName AND PassWord = @PassWord";
            using (SqlCommand command = new SqlCommand(query, Extend.KetNoi.Con))
            {
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@PassWord", PassWord);
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (IsCorrectPass(txtUserName.Text,txtPassCu.Text))
            {
                string sql = "UPDATE Account SET PassWord='"+txtPassMoi.Text+"' WHERE UserName ='"+txtUserName.Text+"'";
                Extend.Funcions.RunSQL(sql);
                MessageBox.Show("Cập nhật thành công!");
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu, vui lòng kiểm tra lại hoặc liên hệ Quản Lý để đổi mật khẩu");
            }
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {
           
        }
    }
}
