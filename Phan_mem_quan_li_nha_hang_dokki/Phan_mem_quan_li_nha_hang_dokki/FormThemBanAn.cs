namespace Phan_mem_quan_li_nha_hang_dokki;

public class FormThemBanAn : Form
{
    private readonly TextBox txtNumber;
    private readonly NumericUpDown nudCapacity;
    private readonly ComboBox cbArea;
    private readonly ComboBox cbInitialStatus;

    public FormThemBanAn()
    {
        Text = "DOKKI | Thêm bàn ăn mới";
        Size = new Size(560, 430);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;
        AutoScaleMode = AutoScaleMode.Dpi;

        var nextNum = DokkiStore.Tables.Count + 1;

        // Header
        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = FormUi.DarkRed, Padding = new Padding(24, 12, 24, 12) };
        var lblTitle = FormUi.Label("THÊM BÀN ĂN MỚI", 12.5F, FontStyle.Bold, Color.White);
        var lblSub = FormUi.Label("Thiết lập mã bàn, số lượng chỗ ngồi và khu vực phục vụ", 9F, FontStyle.Regular, Color.FromArgb(255, 225, 225));
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
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F)); // Rộng rãi 175px, nhãn không bao giờ bị đè
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

        txtNumber = new TextBox
        {
            Text = $"BÀN {nextNum:00}",
            ReadOnly = true,
            BackColor = Color.FromArgb(245, 246, 250),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            Dock = DockStyle.Fill
        };

        // Ô nhập sức chứa với NumericUpDown căn giữa, rõ ràng, có nhãn đơn vị
        var pnlCapacity = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            Margin = new Padding(0),
            Padding = new Padding(0, 2, 0, 0)
        };
        nudCapacity = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 50,
            Value = 4,
            Width = 110,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Center
        };
        var lblUnit = FormUi.Label("chỗ ngồi (người)", 10F, FontStyle.Regular, FormUi.Muted);
        lblUnit.Margin = new Padding(8, 4, 0, 0);
        pnlCapacity.Controls.Add(nudCapacity);
        pnlCapacity.Controls.Add(lblUnit);

        cbArea = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill,
            DropDownWidth = 340
        };
        cbArea.Items.AddRange(new object[] {
            "Khu vực Tầng 1 (Sảnh chính)",
            "Khu vực Tầng 2 (Phòng VIP)",
            "Khu vực Ngoài trời (Sân vườn)"
        });
        cbArea.SelectedIndex = 0;

        cbInitialStatus = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };
        cbInitialStatus.Items.AddRange(new object[] { "Trống", "Đã đặt", "Đang phục vụ" });
        cbInitialStatus.SelectedIndex = 0;

        void AddRow(int row, string labelText, Control control)
        {
            var lbl = FormUi.Label(labelText, 10F, FontStyle.Bold);
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            tblLayout.Controls.Add(lbl, 0, row);
            tblLayout.Controls.Add(control, 1, row);
        }

        AddRow(0, "Tên bàn ăn:", txtNumber);
        AddRow(1, "Sức chứa (người):", pnlCapacity);
        AddRow(2, "Khu vực sắp xếp:", cbArea);
        AddRow(3, "Trạng thái ban đầu:", cbInitialStatus);

        // Buttons Footer
        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(24, 12, 24, 12) };
        var pnlBtnFlow = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, WrapContents = false };

        var btnSave = FormUi.Button("✓ Thêm bàn", FormUi.Red, 130);
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
        var nextNum = DokkiStore.Tables.Count + 1;
        var table = new TableItem(
            nextNum,
            $"BÀN {nextNum:00}",
            cbInitialStatus.SelectedItem?.ToString() ?? "Trống",
            (int)nudCapacity.Value,
            cbArea.SelectedItem?.ToString() ?? "Khu vực Tầng 1 (Sảnh chính)"
        );

        DokkiStore.AddTable(table);
        MessageBox.Show($"Đã thêm {table.Name} ({table.Area}) với sức chứa {table.Capacity} người thành công!", "DOKKI", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
