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
    public partial class NhanVien_ThongTinChiTiet : Form
    {
        public NhanVien_ThongTinChiTiet(string username, string password)
        {
            InitializeComponent();
            label_tendangnhap.Text = username;
            label_matkhau.Text = password;
            using (SqlConnection connection = new SqlConnection(Connection.conectionstring))
            {
                connection.Open();
                string query = "select nv.TEN_NHAN_VIEN "+
                                    "from NHANVIEN nv, TAIKHOAN tk "+
                                    "where nv.ID_TAI_KHOAN = tk.ID_TAI_KHOAN "+
                                    "and tk.TEN_DANG_NHAP = '" + username + "' and tk.MAT_KHAU = '" + password + "'";

                SqlCommand command = new SqlCommand(query, connection);
                label_hoten.Text = Convert.ToString(command.ExecuteScalar());

            }
            using (SqlConnection connection = new SqlConnection(Connection.conectionstring))
            {
                connection.Open();
                string query = "select nv.GMAIL " +
                                    "from NHANVIEN nv, TAIKHOAN tk " +
                                    "where nv.ID_TAI_KHOAN = tk.ID_TAI_KHOAN " +
                                    "and tk.TEN_DANG_NHAP = '" + username + "' and tk.MAT_KHAU = '" + password + "'";

                SqlCommand command = new SqlCommand(query, connection);
                label_gmail.Text = Convert.ToString(command.ExecuteScalar());
            }
            using (SqlConnection connection = new SqlConnection(Connection.conectionstring))
            {
                connection.Open();
                string query = "select nv.DIA_CHI " +
                                    "from NHANVIEN nv, TAIKHOAN tk " +
                                    "where nv.ID_TAI_KHOAN = tk.ID_TAI_KHOAN " +
                                    "and tk.TEN_DANG_NHAP = '" + username + "' and tk.MAT_KHAU = '" + password + "'";

                SqlCommand command = new SqlCommand(query, connection);
                label_diachi.Text = Convert.ToString(command.ExecuteScalar());
            }
            using (SqlConnection connection = new SqlConnection(Connection.conectionstring))
            {
                connection.Open();
                string query = "select nv.SDT " +
                                    "from NHANVIEN nv, TAIKHOAN tk " +
                                    "where nv.ID_TAI_KHOAN = tk.ID_TAI_KHOAN " +
                                    "and tk.TEN_DANG_NHAP = '" + username + "' and tk.MAT_KHAU = '" + password + "'";

                SqlCommand command = new SqlCommand(query, connection);
                label_sdt.Text = Convert.ToString(command.ExecuteScalar());
            }
        }
        public int GetIDTaIKhoan_byUserName(string UserName)
        {

            return 0;
        }

        private void NhanVien_ThongTinChiTiet_Load(object sender, EventArgs e)
        {

        }

        private void label_sdt_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
