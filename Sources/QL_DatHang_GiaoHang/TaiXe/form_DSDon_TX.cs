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
    public partial class form_DSDon_TX : Form
    {
        public string matx_dsd;
        public form_DSDon_TX(string matx)
        {
            matx_dsd = matx;
            InitializeComponent();
        }

        private void bt_LOAD_ds_Click(object sender, EventArgs e)
        {

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = GetDSDon().Tables[0];
        }
        DataSet GetDSDon()
        {
            DataSet data1 = new DataSet();

            string query2 = "select MA_DON, MA_KHACH_HANG, DIA_CHI, TINH_TRANG, TONG_TIEN from DONDATHANG where TINH_TRANG=N'chờ xác nhận' ";

            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query2, connection);
                adapter.Fill(data1);
                connection.Close();
            }
            return data1;
        }

        private void form_DSDon_TX_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string madh = textBox1.Text;
            string sql = "exec NhanDH @matx , @madh";
            Dataprovider.Instance.ExecuteQuery(sql, new object[] { matx_dsd, madh });
            MessageBox.Show("Nhận đơn thành công!", "Thông báo!");
        }
    }
}
