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
    public partial class form_thongtinKH : Form
    {
        private string makh;
        public form_thongtinKH(string maKH)
        {
            InitializeComponent();
            makh = maKH;
            loadInfor();
            button1.Enabled = false;
        }

        private void loadInfor()
        {
            string query = "exec xem_thong_tin_khach_hang '" + makh + "'";
            DataTable data = Dataprovider.Instance.ExecuteQuery(query);
            tb_name.Text = data.Rows[0][0].ToString();
            tb_address.Text = data.Rows[0][1].ToString();
            tb_number.Text = data.Rows[0][2].ToString();
            tb_email.Text = data.Rows[0][3].ToString();
            setcolorDisable();
        }
        private void setcolorDisable()
        {
            tb_name.ForeColor = Color.Gray;
            tb_address.ForeColor = Color.Gray;
            tb_email.ForeColor = Color.Gray;
            tb_number.ForeColor = Color.Gray;
        }

        private void setcolorEnable()
        {
            tb_name.ForeColor = Color.Black;
            tb_address.ForeColor = Color.Black;

            tb_number.ForeColor = Color.Black;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tb_name.ReadOnly = false;
            tb_address.ReadOnly = false;
            tb_number.ReadOnly = false;
            setcolorEnable();
            button1.Enabled = true;
        }

        string hoten;
        string diachi;
        string sdt;
        private void button1_Click(object sender, EventArgs e)
        {
            hoten = tb_name.Text.Trim().ToString();
            diachi = tb_address.Text.Trim().ToString();
            sdt = tb_number.Text.Trim().ToString();
            if (hoten.Length == 0 | diachi.Length == 0 | sdt.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ dữ liệu !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string sql1 = " exec cap_nhat_tai_khach_hang @makh , @hoten , @diachi , @sdt";
            int key = Dataprovider.Instance.ExecuteNonQuery(sql1, new object[] { makh, hoten, diachi, sdt });
            if (key > 0)
            {
                MessageBox.Show("Cập nhật thành công.", "Thông báo");
                button1.Enabled = false;
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công", "Thông báo");
            }
        }

        private void form_thongtinKH_Load(object sender, EventArgs e)
        {

        }
    }
}

