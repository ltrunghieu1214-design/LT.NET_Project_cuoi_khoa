namespace Phan_mem_quan_li_nha_hang_dokki;

public class FormThemBanAn : Form
{
    private readonly TextBox txtNumber;
    private readonly NumericUpDown nudCapacity;
    private readonly ComboBox cbArea;

    public FormThemBanAn()
    {
        Text = "DOKKI | Thêm bàn ăn mới";
        Size = new Size(480, 360);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;

        var nextNum = DokkiStore.Tables.Count + 1;

        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = FormUi.DarkRed, Padding = new Padding(20, 16, 20, 16) };
        var lblTitle = FormUi.Label("THÊM BÀN ĂN MỚI", 13, FontStyle.Bold, Color.White);
        pnlHeader.Controls.Add(lblTitle);
        Controls.Add(pnlHeader);

        var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(25, 20, 25, 20) };

        var y = 15;
        void AddField(string labelText, Control control)
        {
            var lbl = FormUi.Label(labelText, 10, FontStyle.Bold);
            lbl.Location = new Point(25, y);
            control.Location = new Point(160, y - 4);
            control.Width = 260;
            pnlBody.Controls.Add(lbl);
            pnlBody.Controls.Add(control);
            y += 46;
        }

        txtNumber = new TextBox { Text = $"BÀN {nextNum:00}", ReadOnly = true, BackColor = Color.FromArgb(245, 245, 245) };
        nudCapacity = new NumericUpDown { Minimum = 2, Maximum = 20, Value = 4 };
        cbArea = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cbArea.Items.AddRange(new object[] { "Khu vực Tầng 1 (Sảnh chính)", "Khu vực Tầng 2 (VIP)", "Khu vực Ngoài trời" });
        cbArea.SelectedIndex = 0;

        AddField("Tên bàn:", txtNumber);
        AddField("Sức chứa (người):", nudCapacity);
        AddField("Khu vực sắp xếp:", cbArea);

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(20) };
        var btnSave = FormUi.Button("Thêm bàn", FormUi.Red, 120);
        btnSave.Location = new Point(190, 14);
        btnSave.Click += (_, _) => Save();

        var btnCancel = FormUi.Button("Hủy", FormUi.Muted, 90);
        btnCancel.Location = new Point(330, 14);
        btnCancel.Click += (_, _) => Close();

        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnCancel);

        Controls.Add(pnlBody);
        Controls.Add(pnlButtons);
    }

    private void Save()
    {
        var nextNum = DokkiStore.Tables.Count + 1;
        var table = new TableItem(
            nextNum,
            $"BÀN {nextNum:00}",
            "Trống",
            (int)nudCapacity.Value,
            cbArea.SelectedItem?.ToString() ?? "Khu vực Tầng 1"
        );

        DokkiStore.AddTable(table);
        MessageBox.Show($"Đã thêm {table.Name} ({table.Area}) thành công!", "DOKKI", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
