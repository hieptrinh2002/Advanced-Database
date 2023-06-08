using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_DatHang_GiaoHang
{
    public partial class NhanVien_ThongKe : Form
    {
        
        public NhanVien_ThongKe()
        {
            InitializeComponent();
            
        }
        void centerAlign_TabTraCuu(DataGridView temp)
        {
            temp.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
         
            temp.Columns["MA_CUA_HANG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            temp.Columns["TEN_CUA_HANG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            temp.Columns["DOANG_THU"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            temp.Columns["SO_DON"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            temp.Columns["TIEN_HOA_HONG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }
        private void NhanVien_ThongKe_Load(object sender, EventArgs e)
        {
            panel_thoigian_tabDoanhThu.Enabled = false;
            //centerAlign_TabTraCuu(dataGridView_DoanhThu);
            Refresh_DataGrigview_Doanhthu();

        }

        public void Refresh_DataGrigview_Doanhthu()
        {
            try
            {
                string MaCuaHang = textBox_NhapMaCH.Text;
                dataGridView_DoanhThu.Refresh();
                string query = "EXEC XemThongKe_curdate @MaCuaHang";
                dataGridView_DoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView_DoanhThu.DataSource = Dataprovider.Instance.ExecuteQuery(query, new object[] { MaCuaHang });
                displaytotalmoney();
               
               
            }
            catch (Exception error)
            {
                MessageBox.Show("lỗi kết nối hiển thị doanh thu : " + error.ToString());
            }
        }

        private void radioButton_thongke_tabDoanhThu_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_thongke_tabDoanhThu.Checked == true)
            {
                panel_thoigian_tabDoanhThu.Enabled = true;
            }
            else
            {
                panel_thoigian_tabDoanhThu.Enabled = false;
            }
        }

        private void radioButton_xemALL_tabDoanhThu_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_xemALL_tabDoanhThu.Checked == true)
            {
                panel_thoigian_tabDoanhThu.Enabled = false;
            }
            else
            {
                dataGridView_DoanhThu.Refresh();
            }
        }

  
        public void displaytotalmoney()
        {

            textBox_tongdonhang.Text = "0";
            textBox_thonghoahong.Text = "0";
            textBox_tongdoanhthu.Text = "0";

            for ( int i = 0; i< dataGridView_DoanhThu.Rows.Count-1; i++)
            {

                textBox_tongdonhang.Text = Convert.ToString(double.Parse(textBox_tongdonhang.Text)
                                        + double.Parse(dataGridView_DoanhThu.Rows[i].Cells[2].Value.ToString()));

                textBox_tongdoanhthu.Text = Convert.ToString(double.Parse(textBox_tongdoanhthu.Text)
                                        + double.Parse(dataGridView_DoanhThu.Rows[i].Cells[3].Value.ToString()));

                textBox_thonghoahong.Text = Convert.ToString(double.Parse(textBox_thonghoahong.Text)
                                        + double.Parse(dataGridView_DoanhThu.Rows[i].Cells[4].Value.ToString()));

            }

        }
       
        private void button_thongke_Click(object sender, EventArgs e)
        {
            string MACH = textBox_NhapMaCH.Text;
            if(MACH == "")
            {
                MessageBox.Show("vui lòng nhập mã cửa hàng trước khi xem thống kê");
                return;
            }    
            else
            {
               if(radioButton_xemALL_tabDoanhThu.Checked == true)// thống kê trong tháng hiện tại
               {
                    string query = "EXEC XemThongKe_curdate @MACH";
                    try
                    {
                        dataGridView_DoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView_DoanhThu.DataSource = Dataprovider.Instance.ExecuteQuery(query, new object[] { MACH });
                        displaytotalmoney();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("lỗi kết nối hiển thị doanh thu 1: " + error.ToString());
                    }
               }

               else
               {
                    DateTime start = (DateTime)dateTimePicker_TU.Value;
                    DateTime end = (DateTime)dateTimePicker_Den.Value;

                    string query = "EXEC XemThongKe @MACH , @start , @end";
                    try
                    {
                        dataGridView_DoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView_DoanhThu.DataSource = Dataprovider.Instance.ExecuteQuery(query, new object[] { MACH , start , end });
                        displaytotalmoney();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("lỗi kết nối hiển thị doanh thu 2: " + error.ToString());
                    }

                }
            }    
        }
    }
}
