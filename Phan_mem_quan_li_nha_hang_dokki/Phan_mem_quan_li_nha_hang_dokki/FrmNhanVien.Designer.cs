namespace Phan_mem_quan_li_nha_hang_dokki
{
    partial class FrmNhanVien
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
            groupBox1 = new GroupBox();
            btnLamMoi = new Button();
            txtDiaChi = new TextBox();
            btnXoa = new Button();
            lblDiaChi = new Label();
            btnSua = new Button();
            cboChucVu = new ComboBox();
            btnThem = new Button();
            lblChucVu = new Label();
            txtSDT = new TextBox();
            lblSDT = new Label();
            cboGioiTinh = new ComboBox();
            lblGioiTinh = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblNgaySinh = new Label();
            txtHoTen = new TextBox();
            lblHoTen = new Label();
            txtMaNV = new TextBox();
            lblMaNV = new Label();
            groupBox2 = new GroupBox();
            dgvNhanVien = new DataGridView();
            txtTimKiem = new TextBox();
            lblTimKiem = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNhanVien).BeginInit();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTieuDe.Location = new Point(407, 9);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(217, 28);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "QUẢN LÝ NHÂN VIÊN";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnLamMoi);
            groupBox1.Controls.Add(txtDiaChi);
            groupBox1.Controls.Add(btnXoa);
            groupBox1.Controls.Add(lblDiaChi);
            groupBox1.Controls.Add(btnSua);
            groupBox1.Controls.Add(cboChucVu);
            groupBox1.Controls.Add(btnThem);
            groupBox1.Controls.Add(lblChucVu);
            groupBox1.Controls.Add(txtSDT);
            groupBox1.Controls.Add(lblSDT);
            groupBox1.Controls.Add(cboGioiTinh);
            groupBox1.Controls.Add(lblGioiTinh);
            groupBox1.Controls.Add(dtpNgaySinh);
            groupBox1.Controls.Add(lblNgaySinh);
            groupBox1.Controls.Add(txtHoTen);
            groupBox1.Controls.Add(lblHoTen);
            groupBox1.Controls.Add(txtMaNV);
            groupBox1.Controls.Add(lblMaNV);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(12, 40);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1009, 191);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin nhân viên";
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(690, 155);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 5;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Location = new Point(169, 122);
            txtDiaChi.Multiline = true;
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(421, 27);
            txtDiaChi.TabIndex = 15;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(522, 155);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 4;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // lblDiaChi
            // 
            lblDiaChi.AutoSize = true;
            lblDiaChi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiaChi.Location = new Point(51, 129);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(60, 20);
            lblDiaChi.TabIndex = 14;
            lblDiaChi.Text = "Địa chỉ:";
            // 
            // btnSua
            // 
            btnSua.Location = new Point(339, 155);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 3;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // cboChucVu
            // 
            cboChucVu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChucVu.FormattingEnabled = true;
            cboChucVu.Items.AddRange(new object[] { "Quản lý", "Thu ngân", "Nhân viên phục vụ", "Đầu bếp", "Phụ bếp" });
            cboChucVu.Location = new Point(781, 80);
            cboChucVu.Name = "cboChucVu";
            cboChucVu.Size = new Size(151, 28);
            cboChucVu.TabIndex = 13;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(169, 155);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 2;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // lblChucVu
            // 
            lblChucVu.AutoSize = true;
            lblChucVu.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblChucVu.Location = new Point(681, 77);
            lblChucVu.Name = "lblChucVu";
            lblChucVu.Size = new Size(68, 20);
            lblChucVu.TabIndex = 12;
            lblChucVu.Text = "Chức vụ:";
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(781, 37);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(166, 27);
            txtSDT.TabIndex = 11;
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSDT.Location = new Point(681, 40);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(103, 20);
            lblSDT.TabIndex = 10;
            lblSDT.Text = "Số điện thoại:";
            // 
            // cboGioiTinh
            // 
            cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioiTinh.FormattingEnabled = true;
            cboGioiTinh.Items.AddRange(new object[] { "Nam ", "Nữ ", "Khác" });
            cboGioiTinh.Location = new Point(439, 73);
            cboGioiTinh.Name = "cboGioiTinh";
            cboGioiTinh.Size = new Size(151, 28);
            cboGioiTinh.TabIndex = 9;
            // 
            // lblGioiTinh
            // 
            lblGioiTinh.AutoSize = true;
            lblGioiTinh.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGioiTinh.Location = new Point(362, 77);
            lblGioiTinh.Name = "lblGioiTinh";
            lblGioiTinh.Size = new Size(71, 20);
            lblGioiTinh.TabIndex = 8;
            lblGioiTinh.Text = "Giới tính:";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dtpNgaySinh.Location = new Point(439, 35);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(214, 27);
            dtpNgaySinh.TabIndex = 7;
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNgaySinh.Location = new Point(351, 35);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(82, 20);
            lblNgaySinh.TabIndex = 6;
            lblNgaySinh.Text = "Ngày sinh:";
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(169, 73);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(125, 27);
            txtHoTen.TabIndex = 5;
            // 
            // lblHoTen
            // 
            lblHoTen.AutoSize = true;
            lblHoTen.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHoTen.Location = new Point(51, 80);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(59, 20);
            lblHoTen.TabIndex = 4;
            lblHoTen.Text = "Họ tên:";
            // 
            // txtMaNV
            // 
            txtMaNV.Location = new Point(169, 32);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.ReadOnly = true;
            txtMaNV.Size = new Size(125, 27);
            txtMaNV.TabIndex = 3;
            // 
            // lblMaNV
            // 
            lblMaNV.AutoSize = true;
            lblMaNV.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMaNV.Location = new Point(51, 35);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new Size(107, 20);
            lblMaNV.TabIndex = 2;
            lblMaNV.Text = "Mã nhân viên:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvNhanVien);
            groupBox2.Controls.Add(txtTimKiem);
            groupBox2.Controls.Add(lblTimKiem);
            groupBox2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(12, 238);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1008, 403);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Danh sách nhân viên";
            // 
            // dgvNhanVien
            // 
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToResizeColumns = false;
            dgvNhanVien.AllowUserToResizeRows = false;
            dgvNhanVien.AllowUserToOrderColumns = false;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.ColumnHeadersHeight = 36;
            dgvNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvNhanVien.Location = new Point(5, 63);
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.Name = "dgvNhanVien";
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.RowHeadersVisible = false;
            dgvNhanVien.RowHeadersWidth = 51;
            dgvNhanVien.RowTemplate.Height = 34;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.Size = new Size(997, 334);
            dgvNhanVien.TabIndex = 17;
            dgvNhanVien.CellContentClick += dgvNhanVien_CellContentClick;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(118, 30);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(768, 27);
            txtTimKiem.TabIndex = 16;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTimKiem.Location = new Point(28, 33);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(71, 20);
            lblTimKiem.TabIndex = 3;
            lblTimKiem.Text = "Tìm kiếm";
            // 
            // FrmNhanVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1032, 653);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(lblTieuDe);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmNhanVien";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmNhanVien";
            Load += FrmNhanVien_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNhanVien).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTieuDe;
        private GroupBox groupBox1;
        private Label lblHoTen;
        private TextBox txtMaNV;
        private Label lblMaNV;
        private ComboBox cboChucVu;
        private Label lblChucVu;
        private TextBox txtSDT;
        private Label lblSDT;
        private ComboBox cboGioiTinh;
        private Label lblGioiTinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblNgaySinh;
        private TextBox txtHoTen;
        private Button btnLamMoi;
        private TextBox txtDiaChi;
        private Button btnXoa;
        private Label lblDiaChi;
        private Button btnSua;
        private Button btnThem;
        private GroupBox groupBox2;
        private Label lblTimKiem;
        private DataGridView dgvNhanVien;
        private TextBox txtTimKiem;
    }
}