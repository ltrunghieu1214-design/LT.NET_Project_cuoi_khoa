namespace Phan_mem_quan_li_nha_hang_dokki;

public class FormThemMonAn : Form
{
    private readonly TextBox txtCode;
    private readonly TextBox txtName;
    private readonly ComboBox cbCategory;
    private readonly TextBox txtPrice;
    private readonly ComboBox cbUnit;
    private readonly ComboBox cbStatus;

    public FormThemMonAn()
    {
        Text = "DOKKI | Thêm món ăn mới";
        Size = new Size(560, 520);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;
        AutoScaleMode = AutoScaleMode.Dpi;

        var nextCode = $"MA{DokkiStore.Foods.Count + 1:000}";

        // Header
        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = FormUi.DarkRed, Padding = new Padding(24, 12, 24, 12) };
        var lblTitle = FormUi.Label("THÊM MÓN ĂN MỚI", 12.5F, FontStyle.Bold, Color.White);
        var lblSub = FormUi.Label("Cập nhật món ăn vào thực đơn chi nhánh DOKKI", 9F, FontStyle.Regular, Color.FromArgb(255, 225, 225));
        lblSub.Location = new Point(24, 38);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSub);
        Controls.Add(pnlHeader);

        // Body TableLayoutPanel
        var tblLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 6,
            Padding = new Padding(24, 18, 24, 10),
            BackColor = Color.White
        };
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        for (int i = 0; i < 6; i++)
        {
            tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        }

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
            PlaceholderText = "Nhập tên món ăn...",
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };

        cbCategory = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill,
            DropDownWidth = 280
        };
        foreach (var cat in DokkiStore.Categories)
        {
            cbCategory.Items.Add(cat.Name);
        }
        if (cbCategory.Items.Count > 0) cbCategory.SelectedIndex = 0;

        txtPrice = new TextBox
        {
            Text = "59000",
            PlaceholderText = "Giá bán (VNĐ)...",
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };

        cbUnit = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDown,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };
        cbUnit.Items.AddRange(new object[] { "Đĩa 200g", "Đĩa 150g", "Phần", "Đĩa 6 con", "3 xiên", "Chai 500ml", "Ly 700ml", "Cốc", "Gói 110g" });
        cbUnit.SelectedIndex = 0;

        cbStatus = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };
        cbStatus.Items.AddRange(new object[] { "Đang bán", "Tạm hết" });
        cbStatus.SelectedIndex = 0;

        void AddRow(int row, string labelText, Control control)
        {
            var lbl = FormUi.Label(labelText, 10F, FontStyle.Bold);
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            tblLayout.Controls.Add(lbl, 0, row);
            tblLayout.Controls.Add(control, 1, row);
        }

        AddRow(0, "Mã món:", txtCode);
        AddRow(1, "Tên món ăn:", txtName);
        AddRow(2, "Danh mục:", cbCategory);
        AddRow(3, "Giá bán (VNĐ):", txtPrice);
        AddRow(4, "Đơn vị tính:", cbUnit);
        AddRow(5, "Trạng thái:", cbStatus);

        // Buttons Footer
        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(24, 12, 24, 12) };
        var pnlBtnFlow = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, WrapContents = false };

        var btnSave = FormUi.Button("✓ Lưu món ăn", FormUi.Red, 130);
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
            MessageBox.Show("Vui lòng nhập tên món ăn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return;
        }

        if (!decimal.TryParse(txtPrice.Text.Replace(".", "").Replace(",", "").Trim(), out var price) || price < 0)
        {
            MessageBox.Show("Giá bán không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPrice.Focus();
            return;
        }

        var item = new FoodItem(
            txtCode.Text.Trim(),
            txtName.Text.Trim(),
            cbCategory.SelectedItem?.ToString() ?? "Khác",
            price,
            cbUnit.Text.Trim(),
            cbStatus.SelectedItem?.ToString() ?? "Đang bán"
        );

        DokkiStore.AddFood(item);
        MessageBox.Show($"Đã thêm món '{item.Name}' thành công!", "DOKKI", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
