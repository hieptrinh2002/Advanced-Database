using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QL_DatHang_GiaoHang
{
    public partial class form_DDN_TX : Form
    {
        public string matx_ddn;
        SqlConnection connect;
        public form_DDN_TX(string matx)
        {
            matx_ddn = matx;
            InitializeComponent();
        }

        private void bt_XemCT_Click(object sender, EventArgs e)
        {
            connect = new SqlConnection(@"data Source=DESKTOP-5LDFCQ4\SQLEXPRESS;Initial Catalog=Ql_DATHANG_BANHANG;Integrated Security=True");

            connect.Open();
            string sql6 = "select TONG_TIEN FROM DONDATHANG WHERE TRANG_THAI = 1 AND MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd6 = new SqlCommand(sql6, connect);
            //textBox6.Text = Convert.ToString(cmd6.ExecuteScalar());

            string sql = "select MA_DON FROM DONDATHANG WHERE TRANG_THAI = 1 AND MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd = new SqlCommand(sql, connect);


            Form f = new form_CTDon_TX(Convert.ToString(cmd.ExecuteScalar()), Convert.ToString(cmd6.ExecuteScalar()));

            f.Show();
        }

        private void form_DDN_TX_Load(object sender, EventArgs e)
        {
            connect = new SqlConnection(@"data Source=DESKTOP-5LDFCQ4\SQLEXPRESS;Initial Catalog=Ql_DATHANG_BANHANG;Integrated Security=True");

            connect.Open();
            string sql1 = "select KH.HOTEN FROM KHACHHANG KH, DONDATHANG DH WHERE KH.MA_KHACH_HANG = DH.MA_KHACH_HANG AND DH.TRANG_THAI = 1 AND DH.MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd1 = new SqlCommand(sql1, connect);
            textBox1.Text = Convert.ToString(cmd1.ExecuteScalar());
            textBox1.Enabled = false;

            string sql2 = "select DIA_CHI FROM DONDATHANG WHERE TRANG_THAI = 1 AND MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd2 = new SqlCommand(sql2, connect);
            textBox4.Text = Convert.ToString(cmd2.ExecuteScalar());

            string sql3 = "select KH.SDT FROM KHACHHANG KH, DONDATHANG DH WHERE KH.MA_KHACH_HANG = DH.MA_KHACH_HANG AND DH.TRANG_THAI = 1 AND DH.MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd3 = new SqlCommand(sql3, connect);
            textBox2.Text = Convert.ToString(cmd3.ExecuteScalar());

            string sql4 = "select PHI_VAN_CHUYEN FROM DONDATHANG WHERE TRANG_THAI = 1 AND MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd4 = new SqlCommand(sql4, connect);
            textBox5.Text = Convert.ToString(cmd4.ExecuteScalar());

            string sql5 = "select TINH_TRANG FROM DONDATHANG WHERE TRANG_THAI = 1 AND MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd5 = new SqlCommand(sql5, connect);
            textBox3.Text = Convert.ToString(cmd5.ExecuteScalar());

            string sql6 = "select TONG_TIEN FROM DONDATHANG WHERE TRANG_THAI = 1 AND MA_TAI_XE = '" + matx_ddn + "'";
            SqlCommand cmd6 = new SqlCommand(sql6, connect);
            textBox6.Text = Convert.ToString(cmd6.ExecuteScalar());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string tinhtrang = te
        }
    }
}
