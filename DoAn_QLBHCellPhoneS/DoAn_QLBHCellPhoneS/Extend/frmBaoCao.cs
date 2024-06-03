using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DoAn_QLBHCellPhoneS.Extend
{
    public partial class frmBaoCao : Form
    {
        
        string maHD;
        public frmBaoCao(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }
        private void frmBaoCao_Load(object sender, EventArgs e)
        {
            
            SqlCommand command = new SqlCommand("FilterCTSanPhamByMaPhieuXuat", Extend.KetNoi.Con);
            command.CommandType = CommandType.StoredProcedure;

            // Thêm tham số vào stored procedure
            command.Parameters.AddWithValue("@MaPhieuXuat", maHD); 

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            // Đổ dữ liệu vào DataTable
            adapter.Fill(dataTable);

           
            

            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source1 = new ReportDataSource("DataSet1", dataTable );
            reportViewer1.LocalReport.ReportPath = "HoaDon.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source1);
            this.reportViewer1.RefreshReport();
        }
    }
}
