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
    public partial class FrmNhanVien : Form
    {
        private void LoadNhanVien()
        {
            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = "SELECT * FROM NhanVien";

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    dgvNhanVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       "Lỗi tải danh sách nhân viên: " + ex.Message,
                       "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra thông tin bắt buộc
            if (txtHoTen.Text.Trim() == "" ||
                cboGioiTinh.Text == "" ||
                txtSDT.Text.Trim() == "" ||
                cboChucVu.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"INSERT INTO NhanVien
                (HoTen, NgaySinh, GioiTinh,
                 SoDienThoai, ChucVu, DiaChi)
                VALUES
                (@HoTen, @NgaySinh, @GioiTinh,
                 @SoDienThoai, @ChucVu, @DiaChi)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@HoTen", txtHoTen.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@NgaySinh", dtpNgaySinh.Value.Date);

                        cmd.Parameters.AddWithValue(
                            "@GioiTinh", cboGioiTinh.Text);

                        cmd.Parameters.AddWithValue(
                            "@SoDienThoai", txtSDT.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@ChucVu", cboChucVu.Text);

                        cmd.Parameters.AddWithValue(
                            "@DiaChi", txtDiaChi.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm nhân viên thành công!");

                LoadNhanVien();

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm nhân viên: " + ex.Message);
            }
        }
        private void ClearForm()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();

            cboGioiTinh.SelectedIndex = -1;
            cboChucVu.SelectedIndex = -1;

            dtpNgaySinh.Value = DateTime.Today;

            txtHoTen.Focus();
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

            txtMaNV.Text = row.Cells["MaNhanVien"].Value.ToString();
            txtHoTen.Text = row.Cells["HoTen"].Value.ToString();

            if (row.Cells["NgaySinh"].Value != DBNull.Value)
            {
                dtpNgaySinh.Value =
                    Convert.ToDateTime(row.Cells["NgaySinh"].Value);
            }

            cboGioiTinh.Text = row.Cells["GioiTinh"].Value.ToString();
            txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
            cboChucVu.Text = row.Cells["ChucVu"].Value.ToString();
            txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!");
                return;
            }

            if (txtHoTen.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @"UPDATE NhanVien
                           SET HoTen = @HoTen,
                               NgaySinh = @NgaySinh,
                               GioiTinh = @GioiTinh,
                               SoDienThoai = @SoDienThoai,
                               ChucVu = @ChucVu,
                               DiaChi = @DiaChi
                           WHERE MaNhanVien = @MaNhanVien";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@HoTen", txtHoTen.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@NgaySinh", dtpNgaySinh.Value.Date);

                        cmd.Parameters.AddWithValue(
                            "@GioiTinh", cboGioiTinh.Text);

                        cmd.Parameters.AddWithValue(
                            "@SoDienThoai", txtSDT.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@ChucVu", cboChucVu.Text);

                        cmd.Parameters.AddWithValue(
                            "@DiaChi", txtDiaChi.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@MaNhanVien", Convert.ToInt32(txtMaNV.Text));

                        int ketQua = cmd.ExecuteNonQuery();

                        if (ketQua > 0)
                        {
                            MessageBox.Show("Cập nhật nhân viên thành công!");
                            LoadNhanVien();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên cần sửa!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa nhân viên: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn nhân viên chưa
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!");
                return;
            }

            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                    {
                        conn.Open();

                        string sql = "DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue(
                                "@MaNhanVien",
                                Convert.ToInt32(txtMaNV.Text)
                            );

                            int rows = cmd.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                MessageBox.Show("Xóa nhân viên thành công!");
                                LoadNhanVien();
                                ClearForm();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhân viên cần xóa!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa nhân viên: " + ex.Message);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            // Bỏ chọn dòng trong DataGridView
            dgvNhanVien.ClearSelection();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    string sql = @" SELECT * FROM NhanVien WHERE CAST(MaNhanVien AS NVARCHAR(20)) LIKE @TuKhoa OR HoTen LIKE @TuKhoa
                                   OR SoDienThoai LIKE @TuKhoa";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@TuKhoa",
                            "%" + txtTimKiem.Text.Trim() + "%"
                        );

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        dgvNhanVien.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }
    }
}
