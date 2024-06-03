using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_QLBHCellPhoneS.Extend
{
    internal class KetNoi
    {
        public static SqlConnection Con;  //Khai báo đối tượng kết nối        
        //Kết Nối
        public static void Connect()
        {
            Con = new SqlConnection();   //Khởi tạo đối tượng
            Con.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CellPhoneS_QLBH;Integrated Security=True";
            if (Con.State != ConnectionState.Open)
            {
                Con.Open();                  //Mở kết nối  
            }
            else
                MessageBox.Show("Không thể kết nối với dữ liệu", "Thông báo!");
        }
        //Ngắt Kết Nối
        public static void Disconnect()
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();   	//Đóng kết nối
                Con.Dispose(); 	//Giải phóng tài nguyên
            }
        }
    }
        
}
