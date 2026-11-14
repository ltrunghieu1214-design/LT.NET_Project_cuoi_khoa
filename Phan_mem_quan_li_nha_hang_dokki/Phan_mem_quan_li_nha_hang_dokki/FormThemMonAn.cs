namespace Phan_mem_quan_li_nha_hang_dokki;

/// <summary>Dialog thêm món ăn mới vào hệ thống.</summary>
public class FormThemMonAn : Form
{
    public FormThemMonAn()
    {
        FormUi.StyleForm(this, "DOKKI | Thêm món ăn mới");
        MinimumSize = new Size(520, 440);
        MaximumSize = new Size(520, 440);
        StartPosition = FormStartPosition.CenterParent;
        BuildView();
    }

    private void BuildView()
    {
        var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32, 24, 32, 20) };

        var lblTitle = new Label
        {
            Text = "THÊM MÓN ĂN MỚI",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = FormUi.DarkRed,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Mã món
        var lblCode = FormUi.Label("Mã món ăn *", 10F, FontStyle.Bold);
        var txtCode = new TextBox { Width = 200, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "VD: MA036" };

        // Tên món
        var lblName = FormUi.Label("Tên món ăn *", 10F, FontStyle.Bold);
        var txtName = new TextBox { Width = 420, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle };

        // Danh mục
        var lblCat = FormUi.Label("Danh mục *", 10F, FontStyle.Bold);
        var cbCat = new ComboBox { Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, Font = FormUi.BodyFont };
        foreach (var c in DokkiStore.Categories)
            cbCat.Items.Add(c.Name);
        if (cbCat.Items.Count > 0) cbCat.SelectedIndex = 0;

        // Giá bán
        var lblPrice = FormUi.Label("Giá bán (đ) *", 10F, FontStyle.Bold);
        var txtPrice = new TextBox { Width = 200, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "VD: 59000" };

        // Đơn vị tính
        var lblUnit = FormUi.Label("Đơn vị tính *", 10F, FontStyle.Bold);
        var txtUnit = new TextBox { Width = 200, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "VD: Phần, Đĩa 200g..." };

        // Nút lưu / huỷ
        var btnSave = FormUi.Button("💾  Lưu món ăn", FormUi.Red, 150);
        var btnCancel = FormUi.Button("Huỷ", FormUi.Muted, 90);
        btnCancel.Click += (_, _) => Close();

        btnSave.Click += (_, _) =>
        {
            var code  = txtCode.Text.Trim().ToUpper();
            var name  = txtName.Text.Trim();
            var cat   = cbCat.SelectedItem?.ToString() ?? "";
            var unit  = txtUnit.Text.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(cat) || string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường bắt buộc (*)!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtPrice.Text.Trim(), out var price) || price <= 0)
            {
                MessageBox.Show("Giá bán phải là số dương!", "Giá không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DokkiStore.Foods.Any(f => f.Code == code))
            {
                MessageBox.Show($"Mã món '{code}' đã tồn tại. Vui lòng chọn mã khác!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var food = new FoodItem(code, name, cat, price, unit, "Đang bán");
            DokkiStore.AddFood(food);
            MessageBox.Show($"✅ Đã thêm món '{name}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        };

        // Layout
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(0, 8, 0, 0)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 7; i++)
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));

        layout.Controls.Add(lblCode,  0, 0); layout.Controls.Add(txtCode,  1, 0);
        layout.Controls.Add(lblName,  0, 1); layout.Controls.Add(txtName,  1, 1);
        layout.Controls.Add(lblCat,   0, 2); layout.Controls.Add(cbCat,    1, 2);
        layout.Controls.Add(lblPrice, 0, 3); layout.Controls.Add(txtPrice, 1, 3);
        layout.Controls.Add(lblUnit,  0, 4); layout.Controls.Add(txtUnit,  1, 4);

        var btnPanel = new FlowLayoutPanel { Height = 46, Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 4, 0, 0) };
        btnPanel.Controls.Add(btnCancel);
        btnPanel.Controls.Add(btnSave);

        pnl.Controls.Add(layout);
        pnl.Controls.Add(lblTitle);
        pnl.Controls.Add(btnPanel);
        Controls.Add(pnl);
    }
}
