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
    public partial class form_CapNhat_KH : Form
    {
        string tendangnhap;
        string matkhau;
        string matkhaumoi;
        string confirmmatkhau;
        private string maKH;
        public form_CapNhat_KH(string makh)
        {
            InitializeComponent();
            maKH = makh;
        }

        private void bt_CapNhat_Click(object sender, EventArgs e)
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
            string sql = " exec kiem_tra_tentk_khach_hang @makh , @taikhoan";
            int key1 = Dataprovider.Instance.ExecuteNonQuery(sql, new object[] { maKH, tendangnhap });
            if (key1 < 0)
            {
                MessageBox.Show("Sai tên đăng nhập", "Thông báo");
                return;
            }
            else
            {

                if (matkhaumoi != confirmmatkhau)
                {
                    MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận không giống nhau, vui lòng kiểm tra lại !", "Thông báo");
                    return;
                }
                else
                {
                    string sql1 = " exec cap_nhat_mat_khau @tenTK , @matkhau , @matkhaumoi ";
                    int key = Dataprovider.Instance.ExecuteNonQuery(sql1, new object[] { tendangnhap, matkhau, matkhaumoi });
                    if (key > 0)
                    {
                        MessageBox.Show("Sửa thông tin thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sửa thông tin không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }

        private void form_CapNhat_KH_Load(object sender, EventArgs e)
        {

        }
    }
}
