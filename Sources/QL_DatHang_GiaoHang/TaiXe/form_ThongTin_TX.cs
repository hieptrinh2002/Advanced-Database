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
    public partial class form_ThongTin_TX : Form
    {
        public string matx_TT;
        public form_ThongTin_TX(string matx)
        {
            matx_TT = matx;
            InitializeComponent();
        }

        private void form_ThongTin_TX_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sdt = textBox1.Text;
            string email = textBox3.Text;
            string bienso = textBox4.Text;
            string kvhd = textBox5.Text;
            string sql = "exec capnhapTX @matx , @sdt , @email , @bienso , @kvhd";
            Dataprovider.Instance.ExecuteQuery(sql, new object[] { matx_TT, sdt, email, bienso, kvhd });
            MessageBox.Show("Cập nhập thành công!", "Thông báo!");
        }
    }
}
