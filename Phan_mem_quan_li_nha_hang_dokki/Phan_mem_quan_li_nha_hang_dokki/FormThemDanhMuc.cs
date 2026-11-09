namespace Phan_mem_quan_li_nha_hang_dokki;

public class FormThemDanhMuc : Form
{
    private readonly TextBox txtCode;
    private readonly TextBox txtName;
    private readonly TextBox txtNote;
    private readonly ComboBox cbStatus;

    public FormThemDanhMuc()
    {
        Text = "DOKKI | Thêm danh mục món ăn";
        Size = new Size(540, 430);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;
        AutoScaleMode = AutoScaleMode.Dpi;

        var nextCode = $"DM{DokkiStore.Categories.Count + 1:000}";

        // Header
        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = FormUi.DarkRed, Padding = new Padding(24, 12, 24, 12) };
        var lblTitle = FormUi.Label("THÊM DANH MỤC MỚI", 12.5F, FontStyle.Bold, Color.White);
        var lblSub = FormUi.Label("Phân loại các món ăn trên toàn hệ thống nhà hàng", 9F, FontStyle.Regular, Color.FromArgb(255, 225, 225));
        lblSub.Location = new Point(24, 38);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSub);
        Controls.Add(pnlHeader);

        // Body TableLayoutPanel
        var tblLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(24, 20, 24, 10),
            BackColor = Color.White
        };
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

        txtCode = new TextBox
        {
            Text = nextCode,
            ReadOnly = true,
            BackColor = Color.FromArgb(245, 246, 250),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            Dock = DockStyle.Fill
        };

        txtName = new TextBox
        {
            PlaceholderText = "Nhập tên danh mục (VD: Lẩu Tokbokki, Đồ chiên...)...",
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };

        txtNote = new TextBox
        {
            PlaceholderText = "Ghi chú phân loại...",
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };

        cbStatus = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };
        cbStatus.Items.AddRange(new object[] { "Đang phục vụ", "Tạm ẩn" });
        cbStatus.SelectedIndex = 0;

        void AddRow(int row, string labelText, Control control)
        {
            var lbl = FormUi.Label(labelText, 10F, FontStyle.Bold);
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            tblLayout.Controls.Add(lbl, 0, row);
            tblLayout.Controls.Add(control, 1, row);
        }

        AddRow(0, "Mã danh mục:", txtCode);
        AddRow(1, "Tên danh mục:", txtName);
        AddRow(2, "Ghi chú:", txtNote);
        AddRow(3, "Trạng thái:", cbStatus);

        // Buttons Footer
        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(24, 12, 24, 12) };
        var pnlBtnFlow = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, WrapContents = false };

        var btnSave = FormUi.Button("✓ Lưu danh mục", FormUi.Red, 140);
        btnSave.Click += (_, _) => Save();

        var btnCancel = FormUi.Button("Hủy", FormUi.Muted, 90);
        btnCancel.Click += (_, _) => Close();

        pnlBtnFlow.Controls.Add(btnSave);
        pnlBtnFlow.Controls.Add(btnCancel);
        pnlButtons.Controls.Add(pnlBtnFlow);

        Controls.Add(tblLayout);
        Controls.Add(pnlButtons);
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Vui lòng nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return;
        }

        var cat = new CategoryItem(
            txtCode.Text.Trim(),
            txtName.Text.Trim(),
            0,
            txtNote.Text.Trim(),
            cbStatus.SelectedItem?.ToString() ?? "Đang phục vụ"
        );

        DokkiStore.AddCategory(cat);
        MessageBox.Show($"Đã thêm danh mục '{cat.Name}' thành công!", "DOKKI", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
