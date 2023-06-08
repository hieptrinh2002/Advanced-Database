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
    public partial class form_CN_TX : Form
    {
        string tendangnhap;
        string matkhau;
        string matkhaumoi;
        string confirmmatkhau;
        public form_CN_TX()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tendangnhap = txtBox_tendangnhap.Text.Trim().ToString();
            matkhau = txtBox_matkhau.Text.Trim().ToString();
            matkhaumoi = tb_pwnew.Text.Trim().ToString();
            confirmmatkhau = tb_confirm.Text.Trim().ToString();

            if (tendangnhap.Length == 0 | matkhau.Length == 0 | matkhaumoi.Length == 0 | confirmmatkhau.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ dữ liệu !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (matkhaumoi != confirmmatkhau)
            {
                MessageBox.Show("Mật khẩu xác nhận không chính xác !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string sqlselect2 = "select count(*) from TAIKHOAN where TEN_DANG_NHAP = @tdn";
            if ((int)Dataprovider.Instance.ExecuteScalar(sqlselect2, new object[] { tendangnhap }) != 1)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại!", "Thông báo!");
            }
            else
            {
                string sql3 = "exec capnhapTK @tdn , @mk";
                Dataprovider.Instance.ExecuteQuery(sql3, new object[] { tendangnhap, matkhaumoi });
                MessageBox.Show("Cập nhập thành công!", "Thông báo!");

            }

            //MessageBox.Show("Sửa thông tin thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void form_CN_TX_Load(object sender, EventArgs e)
        {

        }
    }
}
