namespace Phan_mem_quan_li_nha_hang_dokki
{
    partial class FrmTaiKhoan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTieuDe = new Label();
            grpThongTin = new GroupBox();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            cboTrangThai = new ComboBox();
            lblTrangThai = new Label();
            cboQuyen = new ComboBox();
            lblQuyen = new Label();
            txtMatKhau = new TextBox();
            lblMatKhau = new Label();
            txtTenDangNhap = new TextBox();
            lblTenDangNhap = new Label();
            txtMaTaiKhoan = new TextBox();
            lblMaTaiKhoan = new Label();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            groupBox1 = new GroupBox();
            dgvTaiKhoan = new DataGridView();
            grpThongTin.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTaiKhoan).BeginInit();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTieuDe.Location = new Point(297, 9);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(214, 28);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "QUẢN LÝ TÀI KHOẢN";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpThongTin
            // 
            grpThongTin.Controls.Add(btnLamMoi);
            grpThongTin.Controls.Add(btnXoa);
            grpThongTin.Controls.Add(btnSua);
            grpThongTin.Controls.Add(btnThem);
            grpThongTin.Controls.Add(cboTrangThai);
            grpThongTin.Controls.Add(lblTrangThai);
            grpThongTin.Controls.Add(cboQuyen);
            grpThongTin.Controls.Add(lblQuyen);
            grpThongTin.Controls.Add(txtMatKhau);
            grpThongTin.Controls.Add(lblMatKhau);
            grpThongTin.Controls.Add(txtTenDangNhap);
            grpThongTin.Controls.Add(lblTenDangNhap);
            grpThongTin.Controls.Add(txtMaTaiKhoan);
            grpThongTin.Controls.Add(lblMaTaiKhoan);
            grpThongTin.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpThongTin.Location = new Point(5, 32);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(792, 189);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin tài khoản";
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLamMoi.Location = new Point(549, 150);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 13;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXoa.Location = new Point(412, 150);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 12;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSua.Location = new Point(271, 150);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 11;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThem.Location = new Point(130, 150);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 10;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // cboTrangThai
            // 
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.FormattingEnabled = true;
            cboTrangThai.Location = new Point(179, 116);
            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.Size = new Size(151, 28);
            cboTrangThai.TabIndex = 9;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTrangThai.Location = new Point(74, 119);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(78, 20);
            lblTrangThai.TabIndex = 8;
            lblTrangThai.Text = "Trạng thái";
            // 
            // cboQuyen
            // 
            cboQuyen.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuyen.FormattingEnabled = true;
            cboQuyen.Location = new Point(501, 74);
            cboQuyen.Name = "cboQuyen";
            cboQuyen.Size = new Size(151, 28);
            cboQuyen.TabIndex = 7;
            // 
            // lblQuyen
            // 
            lblQuyen.AutoSize = true;
            lblQuyen.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblQuyen.Location = new Point(384, 77);
            lblQuyen.Name = "lblQuyen";
            lblQuyen.Size = new Size(54, 20);
            lblQuyen.TabIndex = 6;
            lblQuyen.Text = "Quyền";
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(179, 74);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(125, 27);
            txtMatKhau.TabIndex = 5;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMatKhau.Location = new Point(74, 77);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(74, 20);
            lblMatKhau.TabIndex = 4;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Location = new Point(501, 32);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(125, 27);
            txtTenDangNhap.TabIndex = 3;
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTenDangNhap.Location = new Point(384, 35);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(111, 20);
            lblTenDangNhap.TabIndex = 2;
            lblTenDangNhap.Text = "Tên đăng nhập";
            // 
            // txtMaTaiKhoan
            // 
            txtMaTaiKhoan.Location = new Point(179, 32);
            txtMaTaiKhoan.Name = "txtMaTaiKhoan";
            txtMaTaiKhoan.ReadOnly = true;
            txtMaTaiKhoan.Size = new Size(125, 27);
            txtMaTaiKhoan.TabIndex = 1;
            // 
            // lblMaTaiKhoan
            // 
            lblMaTaiKhoan.AutoSize = true;
            lblMaTaiKhoan.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMaTaiKhoan.Location = new Point(74, 32);
            lblMaTaiKhoan.Name = "lblMaTaiKhoan";
            lblMaTaiKhoan.Size = new Size(99, 20);
            lblMaTaiKhoan.TabIndex = 0;
            lblMaTaiKhoan.Text = "Mã tài khoản";
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTimKiem.Location = new Point(60, 26);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(139, 20);
            lblTimKiem.TabIndex = 9;
            lblTimKiem.Text = "Tìm kiếm tài khoản";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(205, 23);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(503, 27);
            txtTimKiem.TabIndex = 10;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvTaiKhoan);
            groupBox1.Controls.Add(lblTimKiem);
            groupBox1.Controls.Add(txtTimKiem);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(5, 227);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(792, 248);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Danh sách tài khoản";
            // 
            // dgvTaiKhoan
            // 
            dgvTaiKhoan.AllowUserToResizeRows = false;
            dgvTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTaiKhoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTaiKhoan.Location = new Point(5, 55);
            dgvTaiKhoan.MultiSelect = false;
            dgvTaiKhoan.Name = "dgvTaiKhoan";
            dgvTaiKhoan.ReadOnly = true;
            dgvTaiKhoan.RowHeadersVisible = false;
            dgvTaiKhoan.RowHeadersWidth = 51;
            dgvTaiKhoan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaiKhoan.Size = new Size(781, 188);
            dgvTaiKhoan.TabIndex = 11;
            dgvTaiKhoan.CellContentClick += dgvTaiKhoan_CellContentClick;
            // 
            // FrmTaiKhoan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 477);
            Controls.Add(groupBox1);
            Controls.Add(grpThongTin);
            Controls.Add(lblTieuDe);
            Name = "FrmTaiKhoan";
            Text = "FrmTaiKhoan";
            Load += FrmTaiKhoan_Load;
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTaiKhoan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTieuDe;
        private GroupBox grpThongTin;
        private ComboBox cboTrangThai;
        private Label lblTrangThai;
        private ComboBox cboQuyen;
        private Label lblQuyen;
        private TextBox txtMatKhau;
        private Label lblMatKhau;
        private Label lblTenDangNhap;
        private TextBox txtMaTaiKhoan;
        private Label lblMaTaiKhoan;
        private Button btnLamMoi;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private TextBox txtTenDangNhap;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private GroupBox groupBox1;
        private DataGridView dgvTaiKhoan;
    }
}