namespace Phan_mem_quan_li_nha_hang_dokki;

/// <summary>Dialog thêm bàn ăn mới vào sơ đồ nhà hàng.</summary>
public class FormThemBanAn : Form
{
    public FormThemBanAn()
    {
        FormUi.StyleForm(this, "DOKKI | Thêm bàn ăn mới");
        MinimumSize = new Size(480, 360);
        MaximumSize = new Size(480, 360);
        StartPosition = FormStartPosition.CenterParent;
        BuildView();
    }

    private void BuildView()
    {
        var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32, 24, 32, 20) };

        var lblTitle = new Label
        {
            Text = "THÊM BÀN ĂN MỚI",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = FormUi.DarkRed,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Số bàn (tự sinh từ bàn lớn nhất + 1)
        var nextNumber = (DokkiStore.Tables.Count > 0 ? DokkiStore.Tables.Max(t => t.Number) : 0) + 1;
        var lblNumber = FormUi.Label("Số bàn", 10F, FontStyle.Bold);
        var txtNumber = new TextBox
        {
            Width = 120, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle,
            Text = nextNumber.ToString(), ReadOnly = true,
            BackColor = Color.FromArgb(240, 240, 240)
        };

        // Sức chứa
        var lblCap = FormUi.Label("Sức chứa (người) *", 10F, FontStyle.Bold);
        var cbCap = new ComboBox { Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, Font = FormUi.BodyFont };
        foreach (var n in new[] { 2, 4, 6, 8, 10, 12 }) cbCap.Items.Add(n);
        cbCap.SelectedIndex = 1; // mặc định 4 người

        // Khu vực
        var lblArea = FormUi.Label("Khu vực *", 10F, FontStyle.Bold);
        var cbArea = new ComboBox { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList, Font = FormUi.BodyFont };
        cbArea.Items.AddRange(new object[] { "Khu vực Tầng 1", "Khu vực Tầng 2", "Khu vực Ngoài trời", "Khu vực VIP" });
        cbArea.SelectedIndex = 0;

        // Nút lưu / huỷ
        var btnSave   = FormUi.Button("💾  Thêm bàn", FormUi.Blue, 140);
        var btnCancel = FormUi.Button("Huỷ", FormUi.Muted, 90);
        btnCancel.Click += (_, _) => Close();

        btnSave.Click += (_, _) =>
        {
            if (!int.TryParse(txtNumber.Text, out var number))
            {
                MessageBox.Show("Số bàn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var cap  = (int)(cbCap.SelectedItem ?? 4);
            var area = cbArea.SelectedItem?.ToString() ?? "Khu vực Tầng 1";

            var table = new TableItem(number, $"BÀN {number:00}", "Trống", cap, area);
            DokkiStore.AddTable(table);
            MessageBox.Show($"✅ Đã thêm BÀN {number:00} ({area}) thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        };

        // Layout
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(0, 8, 0, 0)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 4; i++)
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

        layout.Controls.Add(lblNumber, 0, 0); layout.Controls.Add(txtNumber, 1, 0);
        layout.Controls.Add(lblCap,    0, 1); layout.Controls.Add(cbCap,     1, 1);
        layout.Controls.Add(lblArea,   0, 2); layout.Controls.Add(cbArea,    1, 2);

        var btnPanel = new FlowLayoutPanel { Height = 46, Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 4, 0, 0) };
        btnPanel.Controls.Add(btnCancel);
        btnPanel.Controls.Add(btnSave);

        pnl.Controls.Add(layout);
        pnl.Controls.Add(lblTitle);
        pnl.Controls.Add(btnPanel);
        Controls.Add(pnl);
    }
}
