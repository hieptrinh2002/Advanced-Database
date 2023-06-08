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
    public partial class DS_Mon : Form
    {
        private string MaCH, MaDon, makh;
        public DS_Mon(string MaCH, string MaDon, string maKH)
        {
            this.MaCH = MaCH;
            this.MaDon = MaDon;
            this.makh = maKH;
            InitializeComponent();
            refresh_dataGridView_chinhanh();
            loadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = GetDSMon().Tables[0];
        }

        private void loadData()
        {
            dataGridView1.Refresh();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = GetDSMon().Tables[0];
        }
        DataSet GetDSMon()
        {
            DataSet data1 = new DataSet();

            string query1 = "exec sp_KH_xem_MonAN  '" + MaCH + "'";

            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(query1, connection);
                adapter1.Fill(data1);
                connection.Close();
            }
            return data1;
        }

        private void refresh_dataGridView_chinhanh()
        {
            dataGridView2.Refresh();
            string query = "select MA_CHI_NHANH, DIA_CHI, SDT, TINH_TRANG from CHINHANH  where MA_CUA_HANG= '" + MaCH + "'";
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.DataSource = Dataprovider.Instance.ExecuteQuery(query);
        }
        private void bt_ql_KH_Click(object sender, EventArgs e)
        {
            string query = "select count(*) from CHITIET_DONHANG where MA_DON= '" + MaDon + "'";
            int flg = (int)Dataprovider.Instance.ExecuteScalar(query);
            string pr2 = " select count(*) from DONDATHANG where MA_DON = '" + MaDon + "'";
            int flg2 = (int)Dataprovider.Instance.ExecuteScalar(pr2);
            if (flg < 1 && flg2 < 1)
            {
                string sql1 = " exec xoa_data_temp_tab @maDon  ";
                int key = Dataprovider.Instance.ExecuteNonQuery(sql1, new object[] { MaDon });
            }
            this.Close();
        }


        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string test = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            float dongia = (float)Convert.ToDouble(dataGridView1.SelectedRows[0].Cells[2].Value);
            if (test == "có bán")
            {
                tb_mamon.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                tb_tenmon.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                tb_giamon.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            }
            else
            {
                MessageBox.Show("Món ăn này không còn hôm nay", "Thông báo");
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {


            string maCN = tb_chinhanh.Text.Trim().ToString();
            if (maCN.Length == 0)
            {
                MessageBox.Show("Chưa chọn chi nhánh", "Thông báo");
                return;
            }
            else
            {
                MessageBox.Show("Đặt hàng thành công", "Thông báo");
                form_xem_mon_dat f = new form_xem_mon_dat(MaDon, makh, maCN);
                f.Show();
            }
        }

        private void DS_Mon_FormClosed(object sender, FormClosedEventArgs e)
        {
            string query = "select count(*) from CHITIET_DONHANG where MA_DON= '" + MaDon + "'";
            int flg = (int)Dataprovider.Instance.ExecuteScalar(query);
            string pr2 = " select count(*) from DONDATHANG where MA_DON = '" + MaDon + "'";
            int flg2 = (int)Dataprovider.Instance.ExecuteScalar(pr2);
            if (flg < 1 && flg2 < 1)
            {
                string sql1 = " exec xoa_data_temp_tab @maDon  ";
                int key = Dataprovider.Instance.ExecuteNonQuery(sql1, new object[] { MaDon });
            }
        }

        private void DS_Mon_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tb_chinhanh.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void bt_themmon_KH_Click(object sender, EventArgs e)
        {
            string mamon = tb_mamon.Text.Trim().ToString();
            if (tb_soluong.Text.Trim().ToString().Length == 0)
            {
                MessageBox.Show("Chưa chọn số lượng", "Thông báo");
                return;
            }
            var soluong = 0;
            soluong = int.Parse(tb_soluong.Text.Trim().ToString());

            if (mamon.Length == 0 | soluong <= 0)
            {
                MessageBox.Show("Thiếu mã món hoặc số lượng không đúng", "Thông báo");
            }
            string sql1 = " exec them_chi_tiet @maDH , @mamon , @soluong ";
            int key = Dataprovider.Instance.ExecuteNonQuery(sql1, new object[] { MaDon, mamon, soluong });
            dataGridView1.Refresh();
            if (key > 0)
            {
                MessageBox.Show("Thêm món thành công", "Thông báo");
            }
            else MessageBox.Show("Thêm món không thành công", "Thông báo");
        }
    }
}
