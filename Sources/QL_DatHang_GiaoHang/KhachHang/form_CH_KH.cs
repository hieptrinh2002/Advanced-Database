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
    public partial class form_CH_KH : Form
    {
        string macuahang;
        public string makh;
        public form_CH_KH(string maKH)
        {
            makh = maKH;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            dataGridView1.Refresh();
            string query = "exec sp_KH_XemDSDoiTac";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = Dataprovider.Instance.ExecuteQuery(query);
        }
        private void bt_xemDs_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }

        private void bt_ch_view_Click(object sender, EventArgs e)
        {

            macuahang = txb_maCH.Text.Trim().ToString();
            if (macuahang.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập mã cửa hàng !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                form_xemDSmon dsmon = new form_xemDSmon(macuahang);

                dsmon.Show();

            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txb_maCH.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txbox_tendoitac.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            tx_box_diachi.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString() + ", " + dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void bt_dathang_Click(object sender, EventArgs e)
        {
            macuahang = txb_maCH.Text.Trim().ToString();
            if (macuahang.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập mã cửa hàng !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                string madon;
                madon = "";
                string sqlnew = "select count(*) from DONDATHANG";
                int ma = (int)Dataprovider.Instance.ExecuteScalar(sqlnew) + 2;
                madon = "DH" + ma;

                DS_Mon dsmon = new DS_Mon(macuahang, madon, makh);

                dsmon.Show();

                // DS_Mon dsmon = new DS_Mon(macuahang, madon);


            }
        }

        private void form_CH_KH_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void form_CH_KH_Load(object sender, EventArgs e)
        {

        }
    }
}
