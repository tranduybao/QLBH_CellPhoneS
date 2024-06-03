using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DoAn_QLBHCellPhoneS.Extend
{
    internal class Funcions
    {
        
        //Lấy dữ liệu vào bảng
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, KetNoi.Con); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table); //Đổ kết quả từ câu lệnh sql vào table
            return table;
        }

        //Hàm kiểm tra khoá trùng
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, KetNoi.Con);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }

        //Hàm thực hiện câu lệnh SQL
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = KetNoi.Con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }



        public static void FillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, KetNoi.Con);
            DataTable table = new DataTable();
            dap.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma; //Trường giá trị
            cbo.DisplayMember = ten; //Trường hiển thị
        }

        public static string GetFieldValues(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, KetNoi.Con);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            return ma;
        }
        //Hàm tạo khóa có dạng: TientoNgaythangnam_giophutgiay
        public static string CreateKey(string tiento)
        {
            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            //Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            //Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }
        //Chuyển đổi từ PM sang dạng 24h
        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }

        //Đổi từ số sang chữ
        public static string NumberToWords(decimal number, CultureInfo culture)
        {
            int num = (int)number;

            if (num == 0)
                return "Không";

            if (num < 0)
                return "Âm " + NumberToWords(Math.Abs(num), culture);

            string words = "";

            if ((num / 1000000000) > 0)
            {
                words += NumberToWords(num / 1000000000, culture) + " Tỷ ";
                num %= 1000000000;
            }

            if ((num / 1000000) > 0)
            {
                words += NumberToWords(num / 1000000, culture) + " Triệu ";
                num %= 1000000;
            }

            if ((num / 1000) > 0)
            {
                words += NumberToWords(num / 1000, culture) + " Ngàn ";
                num %= 1000;
            }

            if ((num / 100) > 0)
            {
                words += NumberToWords(num / 100, culture) + " Trăm ";
                num %= 100;
            }

            if (num > 0)
            {
                if (words != "")
                    words += "Lẻ ";

                var unitsMap = new[]
                {
            "Không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín", "Mười",
            "Mười Một", "Mười Hai", "Mười Ba", "Mười Bốn", "Mười Lăm", "Mười Sáu", "Mười Bảy", "Mười Tám", "Mười Chín"
        };

                var tensMap = new[]
                {
            "Không", "Mười", "Hai Mươi", "Ba Mươi", "Bốn Mươi", "Năm Mươi", "Sáu Mươi", "Bảy Mươi", "Tám Mươi", "Chín Mươi"
        };

                if (num < 20)
                    words += unitsMap[num];
                else
                {
                    words += tensMap[num / 10];
                    if ((num % 10) > 0)
                        words += " " + unitsMap[num % 10];
                }
            }

            return words;
        }

        //Tạo mã mới từ mã cuối
        public static string TaoMaMoi(string ID, string TableName, string selectedCategory)
        {

            string sql = $"SELECT {ID} FROM {TableName} WHERE {ID} LIKE '{selectedCategory}%' ORDER BY {ID} DESC";
            SqlCommand command = new SqlCommand(sql, Extend.KetNoi.Con);
            SqlDataReader reader = command.ExecuteReader();

            List<string> productCodes = new List<string>();

            while (reader.Read())
            {
                string maSanPham = reader[ID].ToString();
                productCodes.Add(maSanPham);
            }

            reader.Close();

            // Sắp xếp danh sách productCodes dựa trên phần số thứ tự
            productCodes.Sort((a, b) => {
                int numberA = int.Parse(a.Substring(selectedCategory.Length));
                int numberB = int.Parse(b.Substring(selectedCategory.Length));
                return numberB.CompareTo(numberA); // Thay đổi hướng so sánh tại đây
            });

            string lastMaSP = productCodes.Count > 0 ? productCodes[0] : string.Empty;
            string newMaSP = Extend.Funcions.GenerateNextCode(lastMaSP, selectedCategory);
            return newMaSP;

        }

        //Tạo mã kế tiếp từ mã cuối
        public static string GenerateNextCode(string lastCode, string selectedCategory)
        {
            if (string.IsNullOrEmpty(lastCode))
            {
                return selectedCategory + "1";
            }

            // Tìm phần ký tự và phần số từ mã cuối cùng
            string categoryPart = lastCode.Substring(0, selectedCategory.Length);
            string numberPart = lastCode.Substring(selectedCategory.Length);

            if (int.TryParse(numberPart, out int lastNumber))
            {
                int newNumber = lastNumber + 1;
                string newCode = categoryPart + newNumber.ToString();
                return newCode;
            }

            return string.Empty;
        }

        public static void LuuHDBanVaoCSDL(string query,string maHD,string maNhanVien,string doiTuong, string tongTien, string maKho)
        {
            DateTime ngayBan = DateTime.Now; 
                try
                {
                    string insertQuery = query;
                    SqlCommand insertCommand = new SqlCommand(insertQuery, KetNoi.Con);
                    insertCommand.Parameters.AddWithValue("@1", maHD);
                    insertCommand.Parameters.AddWithValue("@2", maNhanVien);
                    insertCommand.Parameters.AddWithValue("@3", ngayBan);
                    insertCommand.Parameters.AddWithValue("@4", doiTuong);
                    insertCommand.Parameters.AddWithValue("@5", tongTien);
                    insertCommand.Parameters.AddWithValue("@6", maKho);
                insertCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi khi thêm thông tin vào bảng HDBan
                    MessageBox.Show("Lỗi thêm thông tin vào bảng HDBan: " + ex.Message);
                }
            

            MessageBox.Show("Lưu thông tin hóa đơn thành công.");
        }
        public static void LuuChiTietHDVaoCSDL(string query,string STT ,string maPhieu, string maSanPham, string soLuong, string giaSanPham, string giamGia, string thanhTien)
        {
                        try
                        {
                            string insertquery = query;
                            SqlCommand command = new SqlCommand(insertquery,Extend.KetNoi.Con);
                            command.Parameters.AddWithValue("@1", STT);
                            command.Parameters.AddWithValue("@2", maPhieu);
                            command.Parameters.AddWithValue("@3", maSanPham);
                            command.Parameters.AddWithValue("@4", soLuong);
                            command.Parameters.AddWithValue("@5", giaSanPham);
                            command.Parameters.AddWithValue("@6", giamGia);
                            command.Parameters.AddWithValue("@7", thanhTien);

                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            // Xử lý nếu có lỗi khi lưu dữ liệu
                            MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message);
                        }
        }
        public static void TinhTongTien(DataGridView dgv, out string Tong, out string Chu)
        {
            double tongTien = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    double thanhTien = 0;
                    double.TryParse(row.Cells["ThanhTien"].Value.ToString(), out thanhTien);
                    tongTien += thanhTien;
                }
            }
            Tong = tongTien.ToString("N2") + " VNĐ"; // Hiển thị tổng tiền với định dạng số thập phân (N2)

            CultureInfo culture = CultureInfo.CreateSpecificCulture("vi-VN");
            string tienChu = Extend.Funcions.NumberToWords((decimal)tongTien, culture);
            Chu = tienChu + "Đồng";
            

            
        }
    }
}
    

