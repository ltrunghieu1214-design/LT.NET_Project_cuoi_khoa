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
            BuildLoginLayout();
        }

        private void BuildLoginLayout()
        {
            SuspendLayout();
            Controls.Clear();
            Text = "DOKKI | Đăng nhập";
            ClientSize = new Size(980, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = FormUi.Background;
            ForeColor = FormUi.Ink;
            Font = FormUi.BodyFont;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 1,
                BackColor = FormUi.Background,
                Padding = new Padding(42),
                Margin = Padding.Empty
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var card = new Panel
            {
                Size = new Size(850, 470),
                Anchor = AnchorStyles.None,
                BackColor = FormUi.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = Padding.Empty
            };

            var cardLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = FormUi.Surface,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            cardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            cardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var brandPanel = new Panel { Dock = DockStyle.Fill, BackColor = FormUi.DarkRed, Padding = new Padding(30, 32, 24, 28) };
            var brandLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            brandLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            var logo = new Label
            {
                Text = "DOKKI",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var branch = new Label
            {
                Text = "CHI NHÁNH DOKKI EPU",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = FormUi.BrightGold,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var welcome = new Label
            {
                Text = "QUẢN LÝ NHÀ HÀNG",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var description = new Label
            {
                Text = "Quản lý thực đơn, bàn ăn và lịch đặt bàn trong một nơi.",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(238, 224, 224),
                TextAlign = ContentAlignment.TopLeft
            };
            brandLayout.Controls.Add(logo, 0, 0);
            brandLayout.Controls.Add(branch, 0, 1);
            brandLayout.Controls.Add(welcome, 0, 3);
            brandLayout.Controls.Add(description, 0, 4);
            brandPanel.Controls.Add(brandLayout);

            var loginPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = FormUi.Surface,
                Padding = new Padding(42, 30, 42, 24)
            };
            var loginLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 9,
                BackColor = FormUi.Surface,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            loginLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblTieuDe.Text = "Chào mừng trở lại";
            lblTieuDe.AutoSize = false;
            lblTieuDe.Dock = DockStyle.Fill;
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = FormUi.Ink;
            lblTieuDe.TextAlign = ContentAlignment.MiddleLeft;

            var subtitle = new Label
            {
                Text = "Đăng nhập để tiếp tục sử dụng hệ thống.",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = FormUi.Muted,
                TextAlign = ContentAlignment.MiddleLeft
            };

            StyleLoginLabel(lblTenDangNhap, "Tên đăng nhập");
            StyleLoginLabel(lblMatKhau, "Mật khẩu");
            txtTenDangNhap.Dock = DockStyle.Fill;
            txtTenDangNhap.Font = new Font("Segoe UI", 11F);
            txtTenDangNhap.BorderStyle = BorderStyle.FixedSingle;
            txtTenDangNhap.PlaceholderText = "Nhập tên đăng nhập";
            txtTenDangNhap.Margin = new Padding(0, 2, 0, 4);
            txtTenDangNhap.TabIndex = 0;

            txtMatKhau.Dock = DockStyle.Fill;
            txtMatKhau.Font = new Font("Segoe UI", 11F);
            txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
            txtMatKhau.PlaceholderText = "Nhập mật khẩu";
            txtMatKhau.Margin = new Padding(0, 2, 0, 4);
            txtMatKhau.TabIndex = 1;

            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.ForeColor = FormUi.Muted;
            chkHienMatKhau.Font = new Font("Segoe UI", 9F);
            chkHienMatKhau.Anchor = AnchorStyles.Left;
            chkHienMatKhau.TabIndex = 2;

            btnDangNhap.Text = "ĐĂNG NHẬP";
            btnDangNhap.Size = new Size(270, 42);
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.BackColor = FormUi.Red;
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatAppearance.MouseOverBackColor = FormUi.DarkRed;
            btnDangNhap.FlatAppearance.MouseDownBackColor = Color.FromArgb(95, 14, 19);
            btnDangNhap.Cursor = Cursors.Hand;
            btnDangNhap.TabIndex = 3;

            btnThoat.Text = "Thoát";
            btnThoat.Size = new Size(100, 42);
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.BackColor = FormUi.Background;
            btnThoat.ForeColor = FormUi.Muted;
            btnThoat.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.FlatAppearance.BorderColor = FormUi.BorderColor;
            btnThoat.FlatAppearance.BorderSize = 1;
            btnThoat.Cursor = Cursors.Hand;
            btnThoat.TabIndex = 4;

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = Padding.Empty,
                Padding = new Padding(0, 4, 0, 0)
            };
            buttons.Controls.Add(btnDangNhap);
            buttons.Controls.Add(btnThoat);

            var footer = new Label
            {
                Text = "DOKKI EPU  •  HỆ THỐNG QUẢN LÝ NHÀ HÀNG",
                Dock = DockStyle.Bottom,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = FormUi.Muted,
                TextAlign = ContentAlignment.BottomLeft
            };

            loginLayout.Controls.Add(lblTieuDe, 0, 0);
            loginLayout.Controls.Add(subtitle, 0, 1);
            loginLayout.Controls.Add(lblTenDangNhap, 0, 2);
            loginLayout.Controls.Add(txtTenDangNhap, 0, 3);
            loginLayout.Controls.Add(lblMatKhau, 0, 4);
            loginLayout.Controls.Add(txtMatKhau, 0, 5);
            loginLayout.Controls.Add(chkHienMatKhau, 0, 6);
            loginLayout.Controls.Add(buttons, 0, 7);
            loginLayout.Controls.Add(footer, 0, 8);
            loginPanel.Controls.Add(loginLayout);

            cardLayout.Controls.Add(brandPanel, 0, 0);
            cardLayout.Controls.Add(loginPanel, 1, 0);
            card.Controls.Add(cardLayout);
            root.Controls.Add(card, 0, 0);
            Controls.Add(root);
            AcceptButton = btnDangNhap;
            CancelButton = btnThoat;
            ResumeLayout(true);
        }

        private static void StyleLoginLabel(Label label, string text)
        {
            label.Text = text;
            label.AutoSize = false;
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            label.ForeColor = FormUi.Ink;
            label.TextAlign = ContentAlignment.MiddleLeft;
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
            if (txtTenDangNhap.Text.Trim() == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            try
            {
                using (SqlConnection conn = Database_QL_He_Thong.GetConnection())
                {
                    conn.Open();

                    const string sql = @"
                        SELECT MaTaiKhoan, TenDangNhap, Quyen, TrangThai
                        FROM dbo.TaiKhoan
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
                            if (!reader.Read())
                            {
                                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                                return;
                            }

                            if (!Convert.ToBoolean(reader["TrangThai"]))
                            {
                                MessageBox.Show("Tài khoản đã bị khóa!");
                                return;
                            }

                            Session.MaTaiKhoan = Convert.ToInt32(reader["MaTaiKhoan"]);
                            Session.TenDangNhap = reader["TenDangNhap"].ToString() ?? string.Empty;
                            Session.Quyen = reader["Quyen"].ToString() ?? string.Empty;
                        }
                    }
                }

                MessageBox.Show(
                    "Đăng nhập thành công!\nXin chào: " + Session.TenDangNhap +
                    "\nQuyền: " + Session.Quyen);

                Hide();
                using var form = new FormMain();
                form.ShowDialog();

                if (form.LogoutRequested)
                {
                    Session.MaTaiKhoan = 0;
                    Session.TenDangNhap = string.Empty;
                    Session.Quyen = string.Empty;
                    txtMatKhau.Clear();
                    Show();
                    Activate();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                Show();
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

