using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
namespace Phan_mem_quan_li_nha_hang_dokki
{
    public partial class FrmDangNhap : Form
    {
        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHienMatKhau.Checked)
            {
                txtMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtMatKhau.PasswordChar = '*';
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTenDangNhap.Text.Trim() == "" ||
        txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MaTaiKhoan, TenDangNhap, MatKhau, Quyen, TrangThai
                FROM TaiKhoan
                WHERE TenDangNhap = @TenDangNhap
                  AND MatKhau = @MatKhau";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@TenDangNhap",
                            txtTenDangNhap.Text.Trim()
                        );

                        cmd.Parameters.AddWithValue(
                            "@MatKhau",
                            txtMatKhau.Text
                        );

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Kiểm tra tài khoản có bị khóa không
                                if (Convert.ToBoolean(reader["TrangThai"]) == false)
                                {
                                    MessageBox.Show("Tài khoản đã bị khóa!");
                                    return;
                                }

                                // Lưu thông tin người đăng nhập
                                Session.MaTaiKhoan =
                                    Convert.ToInt32(reader["MaTaiKhoan"]);

                                Session.TenDangNhap =
                                    reader["TenDangNhap"].ToString();

                                Session.Quyen =
                                    reader["Quyen"].ToString();

                                MessageBox.Show(
                                    "Đăng nhập thành công!\nXin chào: "
                                    + Session.TenDangNhap
                                    + "\nQuyền: " + Session.Quyen
                                );

                                // Tạm thời đóng kết nối và lưu Session.
                                // Bước tiếp theo sẽ mở Form chính theo quyền.
                            }
                            else
                            {
                                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                 "Bạn có chắc chắn muốn thoát không?",
                 "Xác nhận",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
