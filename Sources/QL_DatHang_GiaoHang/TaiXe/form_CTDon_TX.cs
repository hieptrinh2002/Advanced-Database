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
    public partial class form_CTDon_TX : Form
    {
        public string maDH;
        public string cost;
        public form_CTDon_TX(string madon, string tongtien)
        {
            maDH = madon;
            cost = tongtien;
            InitializeComponent();
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void form_CTDon_TX_Load(object sender, EventArgs e)
        {
            textBox1.Text = maDH;
            textBox2.Text = cost;
            string sql = "select MA.TEN_MON, CTDH.SO_LUONG, CTDH.TONG_TIEN FROM MON_AN MA, CHITIET_DONHANG CTDH WHERE MA.MA_MON_AN = CTDH.MA_MON_AN AND CTDH.MA_DON ='" + maDH + "'";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = Dataprovider.Instance.ExecuteQuery(sql);
            dataGridView1.RowHeadersVisible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

