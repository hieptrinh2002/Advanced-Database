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
    public partial class form_xem_mon_dat : Form
    {
        string madh;
        string makh;
        string macn;
        public form_xem_mon_dat(string maDH, string maKH, string maCN)
        {
            this.madh = maDH;
            this.makh = maKH;
            this.macn = maCN;
            InitializeComponent();
            loadData();
            loadMoney();
        }
        private void loadData()
        {
            dataGridView1.Refresh();
            string query = "exec xem_temp_table '" + madh + "'";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = Dataprovider.Instance.ExecuteQuery(query);


        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadMoney()
        {
            int pvc = 0;
            string sqlselect2 = "select count(*) from ##chitietdon where MaDon= '" + madh + "'";
            pvc = (int)Dataprovider.Instance.ExecuteScalar(sqlselect2);
            tb_phivc.Text = pvc.ToString();

            string sqlselect1 = "select sum(TongTien) from ##chitietdon where MaDon='" + madh + "'";

            string query = "select sum(TongTien) from ##chitietdon where MaDon='" + madh + "'";
            DataTable data = Dataprovider.Instance.ExecuteQuery(query);
            tb_tongtien.Text = data.Rows[0][0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float tien = float.Parse(tb_tongtien.Text.ToString());
            float phivc = float.Parse(tb_phivc.Text.ToString()) * 1000;
            string diachi = tb_diachi.Text.Trim().ToString();
            string sql1 = " exec them_don_dat_hang @maDon , @tien , @diachi , @phivanchuyen , @makh , @macn ";
            int key = Dataprovider.Instance.ExecuteNonQuery(sql1, new object[] { madh, tien, diachi, phivc, makh, macn });
            if (key > 0)
            {
                string query1 = "exec them_chi_tiet_don_bang_goc @madh ";
                Dataprovider.Instance.ExecuteNonQuery(query1, new object[] { madh });
                MessageBox.Show("Đặt hàng thành công", "Thông báo");
                this.Close();

            }
            else
            {
                MessageBox.Show("Đặt hàng thất bại", "Thông báo");
            }
        }

        private void form_xem_mon_dat_Load(object sender, EventArgs e)
        {

        }
    }
}
