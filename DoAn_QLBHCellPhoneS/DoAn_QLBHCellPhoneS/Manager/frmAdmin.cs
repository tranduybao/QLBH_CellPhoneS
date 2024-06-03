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
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
        }
        private Form currentFormChild;
        private void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close(); // Đóng form con hiện tại nếu có
            }

            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelFather.Controls.Add(childForm);
            panelFather.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void frmAdmin_Load(object sender, EventArgs e)
        {
            
        }
        

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNhanVien f = new frmNhanVien();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmAccount());
        }

        private void khoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmKho());
        }

        private void chiNhánhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmChiNhanh());
        }

        private void chứcVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmChucVu());
        }

        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSanPham f = new frmSanPham();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void loạiHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmLoaiHang());
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmNhaCungCap());
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKhachHang f = new frmKhachHang();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void nhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNhapHang f = new frmNhapHang();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLýNhậpXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmQLNhapXuat f = new frmQLNhapXuat();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void thôngTinPhầnMềmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Staff.frmThongTinLienHe f = new Staff.frmThongTinLienHe();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void yêuCầuNhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmYeuCauNhapHang());
        }

        
    }
}
