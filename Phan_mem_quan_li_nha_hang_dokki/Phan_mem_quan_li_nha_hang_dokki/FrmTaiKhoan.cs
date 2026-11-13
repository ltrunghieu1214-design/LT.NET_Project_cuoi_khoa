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
    public partial class FrmTaiKhoan : Form
    {
        public FrmTaiKhoan()
        {
            InitializeComponent();
        }

        private void FrmTaiKhoan_Load(object sender, EventArgs e)
        {
            // Thiết lập danh sách quyền
            cboQuyen.Items.Clear();
            cboQuyen.Items.Add("Admin");
            cboQuyen.Items.Add("Quản lý");
            cboQuyen.Items.Add("Nhân viên");

            // Thiết lập trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Hoạt động");
            cboTrangThai.Items.Add("Đã khóa");

            cboQuyen.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = 0;

            // Hiển thị danh sách tài khoản
            LoadTaiKhoan();
        }
        private void LoadTaiKhoan()
        {
            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    MaTaiKhoan,
                    TenDangNhap,
                    Quyen,
                    CASE
                        WHEN TrangThai = 1 THEN N'Hoạt động'
                        ELSE N'Đã khóa'
                    END AS TrangThai
                FROM TaiKhoan
                ORDER BY MaTaiKhoan ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvTaiKhoan.DataSource = dt;
                    }
                }

                // Đặt tên hiển thị cho các cột
                dgvTaiKhoan.Columns["MaTaiKhoan"].HeaderText = "Mã tài khoản";
                dgvTaiKhoan.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                dgvTaiKhoan.Columns["Quyen"].HeaderText = "Quyền";
                dgvTaiKhoan.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách tài khoản: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra thông tin nhập
            if (txtTenDangNhap.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                txtTenDangNhap.Focus();
                return;
            }

            if (txtMatKhau.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                txtMatKhau.Focus();
                return;
            }

            if (cboQuyen.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn quyền!");
                return;
            }

            if (cboTrangThai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                INSERT INTO TaiKhoan
                (TenDangNhap, MatKhau, Quyen, TrangThai)
                VALUES
                (@TenDangNhap, @MatKhau, @Quyen, @TrangThai)";

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

                        cmd.Parameters.AddWithValue(
                            "@Quyen",
                            cboQuyen.Text
                        );

                        int trangThai = cboTrangThai.Text == "Hoạt động" ? 1 : 0;

                        cmd.Parameters.AddWithValue(
                            "@TrangThai",
                            trangThai
                        );

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm tài khoản thành công!");

                LoadTaiKhoan();
                ClearFormTaiKhoan();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                }
                else
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm tài khoản: " + ex.Message);
            }
        }
        private void ClearFormTaiKhoan()
        {
            txtMaTaiKhoan.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();

            cboQuyen.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = 0;

            dgvTaiKhoan.ClearSelection();

            txtTenDangNhap.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn tài khoản chưa
            if (txtMaTaiKhoan.Text == "")
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa!");
                return;
            }

            // Kiểm tra tên đăng nhập
            if (txtTenDangNhap.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                txtTenDangNhap.Focus();
                return;
            }

            if (cboQuyen.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn quyền!");
                return;
            }

            if (cboTrangThai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn cập nhật tài khoản này không?",
                "Xác nhận sửa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                UPDATE TaiKhoan
                SET TenDangNhap = @TenDangNhap,
                    MatKhau = CASE
                        WHEN @MatKhau = '' THEN MatKhau
                        ELSE @MatKhau
                    END,
                    Quyen = @Quyen,
                    TrangThai = @TrangThai
                WHERE MaTaiKhoan = @MaTaiKhoan";

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

                        cmd.Parameters.AddWithValue(
                            "@Quyen",
                            cboQuyen.Text
                        );

                        int trangThai =
                            cboTrangThai.Text == "Hoạt động" ? 1 : 0;

                        cmd.Parameters.AddWithValue(
                            "@TrangThai",
                            trangThai
                        );

                        cmd.Parameters.AddWithValue(
                            "@MaTaiKhoan",
                            Convert.ToInt32(txtMaTaiKhoan.Text)
                        );

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật tài khoản thành công!");

                            LoadTaiKhoan();
                            ClearFormTaiKhoan();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy tài khoản cần sửa!");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                }
                else
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa tài khoản: " + ex.Message);
            }
        }

        private void dgvTaiKhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];

            txtMaTaiKhoan.Text =
                row.Cells["MaTaiKhoan"].Value.ToString();

            txtTenDangNhap.Text =
                row.Cells["TenDangNhap"].Value.ToString();

            // Để trống mật khẩu nếu không muốn đổi
            txtMatKhau.Clear();

            cboQuyen.Text =
                row.Cells["Quyen"].Value.ToString();

            cboTrangThai.Text =
                row.Cells["TrangThai"].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn tài khoản chưa
            if (txtMaTaiKhoan.Text == "")
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!");
                return;
            }

            // Không cho xóa tài khoản admin chính
            if (txtTenDangNhap.Text.Trim().ToLower() == "admin")
            {
                MessageBox.Show("Không thể xóa tài khoản admin!");
                return;
            }

            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tài khoản này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                DELETE FROM TaiKhoan
                WHERE MaTaiKhoan = @MaTaiKhoan";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@MaTaiKhoan",
                            Convert.ToInt32(txtMaTaiKhoan.Text)
                        );

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Xóa tài khoản thành công!");

                            LoadTaiKhoan();
                            ClearFormTaiKhoan();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy tài khoản cần xóa!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa tài khoản: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFormTaiKhoan();
            // Hiển thị lại toàn bộ tài khoản
            LoadTaiKhoan();
            // Xóa nội dung tìm kiếm
            txtTimKiem.Clear();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT
                    MaTaiKhoan,
                    TenDangNhap,
                    Quyen,
                    CASE
                        WHEN TrangThai = 1 THEN N'Hoạt động'
                        ELSE N'Đã khóa'
                    END AS TrangThai
                FROM TaiKhoan
                WHERE CAST(MaTaiKhoan AS NVARCHAR(20)) LIKE @TuKhoa
                   OR TenDangNhap LIKE @TuKhoa
                   OR Quyen LIKE @TuKhoa
                   OR CASE
                        WHEN TrangThai = 1 THEN N'Hoạt động'
                        ELSE N'Đã khóa'
                      END LIKE @TuKhoa
                ORDER BY MaTaiKhoan ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@TuKhoa",
                            "%" + txtTimKiem.Text.Trim() + "%"
                        );

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvTaiKhoan.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm tài khoản: " + ex.Message);
            }
        }
    }
}
