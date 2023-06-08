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
using System.Threading;

namespace QL_DatHang_GiaoHang
{
    public partial class Admin : Form
    {
        Thread t;
        public Admin()
        {
            InitializeComponent();
        }

        public void DisplayLG()
        {
            string sql = "SELECT * FROM TAIKHOAN";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = Dataprovider.Instance.ExecuteQuery(sql);
            dataGridView1.RowHeadersVisible = false;

        }

        public void DisplayNV()
        {
            string sql = "SELECT * FROM NHANVIEN";
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.DataSource = Dataprovider.Instance.ExecuteQuery(sql);
            dataGridView3.RowHeadersVisible = false;

        }







        public void open_FormDangNhap(object obj)
        {
            Application.Run(new Login());
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có muốn đăng xuất không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                t = new Thread(open_FormDangNhap);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlselect1 = "select count(*) from TAIKHOAN";
            int id = 0;
            id = (int)Dataprovider.Instance.ExecuteScalar(sqlselect1) + 1;
            string username = textBox9.Text;
            string password = textBox10.Text;
            int loaitk = 4;
            string sqlselect2 = "select count(*) from TAIKHOAN where TEN_DANG_NHAP = @username";
            if ((int)Dataprovider.Instance.ExecuteScalar(sqlselect2, new object[] { username }) == 1)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thông báo!");
            }
            else
            {
                string sql3 = "exec ThemTK @idtk , @username , @pass , @loaitk";
                Dataprovider.Instance.ExecuteQuery(sql3, new object[] { id, username, password, loaitk });
                MessageBox.Show("Thêm thành công!", "Thông báo!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql1 = "select count(*) from NHANVIEN";
            int ma = (int)Dataprovider.Instance.ExecuteScalar(sql1) + 1;
            string manv = "";
            if (ma > 10)
            {
                manv = "KH0" + ma;
            }
            if (ma > 100)
            {
                manv = "KH" + ma;
            }
            if (ma < 10)
            {
                manv = "KH00" + ma;
            }
            string sqlselect1 = "select count(*) from TAIKHOAN";
            int id = 0;
            id = (int)Dataprovider.Instance.ExecuteScalar(sqlselect1) + 1;
            string hoten = textBox11.Text;
            string diachi = textBox12.Text;
            string sdt = textBox13.Text;
            string email = textBox14.Text;
            if (textBox11.Text.Trim() == string.Empty || textBox12.Text.Trim() == string.Empty || textBox13.Text.Trim() == string.Empty || textBox14.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo!");
            }
            else
            {
                string sqlKH = "exec ThemNV @makh , @hoten , @diachi , @sdt , @email ";
                Dataprovider.Instance.ExecuteQuery(sqlKH, new object[] { manv, hoten, diachi, sdt, email });
                groupBox8.Visible = true;
                button6.Visible = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sqlselect1 = "select count(*) from TAIKHOAN";
            int id = 0;
            id = (int)Dataprovider.Instance.ExecuteScalar(sqlselect1) + 1;
            string username = textBox15.Text;
            string password = textBox16.Text;
            int loaitk = 4;
            string sqlselect2 = "select count(*) from TAIKHOAN where TEN_DANG_NHAP = @username";
            if ((int)Dataprovider.Instance.ExecuteScalar(sqlselect2, new object[] { username }) == 1)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thông báo!");
            }
            else
            {
                string sql3 = "exec ThemTK @idtk , @username , @pass , @loaitk";
                Dataprovider.Instance.ExecuteQuery(sql3, new object[] { id, username, password, loaitk });

                string sql4 = "update NHANVIEN set ID_TAI_KHOAN = @id where gmail = @email";
                string email = textBox14.Text;
                Dataprovider.Instance.ExecuteQuery(sql4, new object[] { id, email });
                MessageBox.Show("Đăng kí thành công!", "Thông báo!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string username = textBox17.Text;
            string sqlselect2 = "select count(*) from TAIKHOAN where TEN_DANG_NHAP = @username";
            if ((int)Dataprovider.Instance.ExecuteScalar(sqlselect2, new object[] { username }) != 1)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại!", "Thông báo!");
            }
            else
            {
                string sql = "update TAIKHOAN set TRANG_THAI = 0 where TEN_DANG_NHAP = @username";
                Dataprovider.Instance.ExecuteQuery(sql, new object[] { username });
                MessageBox.Show("Khóa thành công!", "Thông báo!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string username = textBox18.Text;
            string sqlselect2 = "select count(*) from TAIKHOAN where TEN_DANG_NHAP = @username";
            if ((int)Dataprovider.Instance.ExecuteScalar(sqlselect2, new object[] { username }) != 1)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại!", "Thông báo!");
            }
            else
            {
                string sql = "update TAIKHOAN set TRANG_THAI = 1 where TEN_DANG_NHAP = @username";
                Dataprovider.Instance.ExecuteQuery(sql, new object[] { username });
                MessageBox.Show("Mở khóa thành công!", "Thông báo!");
            }
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Xóa_Click(object sender, EventArgs e)
        {
            string username = textBox5.Text;
            string sqlselect2 = "select count(*) from TAIKHOAN where TEN_DANG_NHAP = @username";
            if ((int)Dataprovider.Instance.ExecuteScalar(sqlselect2, new object[] { username }) != 1)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại!", "Thông báo!");
            }
            else
            {
                string sql = "update TAIKHOAN set TRANG_THAI = 2 where TEN_DANG_NHAP = @username";
                Dataprovider.Instance.ExecuteQuery(sql, new object[] { username });
                MessageBox.Show("Xóa thành công!", "Thông báo!");
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string tdn = textBox1.Text;
            string mk = textBox2.Text;
            string sqlselect2 = "select count(*) from TAIKHOAN where TEN_DANG_NHAP = @tdn";
            if ((int)Dataprovider.Instance.ExecuteScalar(sqlselect2, new object[] { tdn }) != 1)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại!", "Thông báo!");
            }
            else
            {
                string sql3 = "exec capnhapTK @tdn , @mk";
                Dataprovider.Instance.ExecuteQuery(sql3, new object[] { tdn, mk });
                MessageBox.Show("Cập nhập thành công!", "Thông báo!");
                DisplayLG();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string manv = textBox7.Text;
            string diachi = textBox8.Text;
            string sdt = textBox19.Text;
            string gmail = textBox20.Text;
            string sql = "exec capnhapNV @manv , @diachi , @sdt , @gmail";
            Dataprovider.Instance.ExecuteQuery(sql, new object[] { manv, diachi, sdt, gmail });
            MessageBox.Show("Cập nhập thành công!", "Thông báo!");
            DisplayNV();
        }
    }
}
