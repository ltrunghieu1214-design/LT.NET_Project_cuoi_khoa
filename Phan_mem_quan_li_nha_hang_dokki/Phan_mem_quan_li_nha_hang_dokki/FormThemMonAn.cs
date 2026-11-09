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
        Size = new Size(500, 480);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;

        var nextCode = $"MA{DokkiStore.Foods.Count + 1:000}";

        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = FormUi.DarkRed, Padding = new Padding(20, 16, 20, 16) };
        var lblTitle = FormUi.Label("THÊM MÓN ĂN MỚI", 13, FontStyle.Bold, Color.White);
        pnlHeader.Controls.Add(lblTitle);
        Controls.Add(pnlHeader);

        var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(25, 20, 25, 20) };

        var y = 15;
        void AddField(string labelText, Control control)
        {
            var lbl = FormUi.Label(labelText, 10, FontStyle.Bold);
            lbl.Location = new Point(25, y);
            control.Location = new Point(160, y - 4);
            control.Width = 280;
            pnlBody.Controls.Add(lbl);
            pnlBody.Controls.Add(control);
            y += 46;
        }

        txtCode = new TextBox { Text = nextCode, ReadOnly = true, BackColor = Color.FromArgb(245, 245, 245) };
        txtName = new TextBox { PlaceholderText = "Nhập tên món ăn..." };
        cbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var cat in DokkiStore.Categories)
        {
            cbCategory.Items.Add(cat.Name);
        }
        if (cbCategory.Items.Count > 0) cbCategory.SelectedIndex = 0;

        txtPrice = new TextBox { Text = "59000", PlaceholderText = "Giá bán (VNĐ)..." };
        cbUnit = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown };
        cbUnit.Items.AddRange(new object[] { "Đĩa 200g", "Đĩa 150g", "Phần", "Đĩa 6 con", "3 xiên", "Chai 500ml", "Ly 700ml", "Cốc", "Gói 110g" });
        cbUnit.SelectedIndex = 0;

        cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cbStatus.Items.AddRange(new object[] { "Đang bán", "Tạm hết" });
        cbStatus.SelectedIndex = 0;

        AddField("Mã món:", txtCode);
        AddField("Tên món ăn:", txtName);
        AddField("Danh mục:", cbCategory);
        AddField("Giá bán (VNĐ):", txtPrice);
        AddField("Đơn vị tính:", cbUnit);
        AddField("Trạng thái:", cbStatus);

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(20) };
        var btnSave = FormUi.Button("Lưu món ăn", FormUi.Red, 130);
        btnSave.Location = new Point(200, 14);
        btnSave.Click += (_, _) => Save();

        var btnCancel = FormUi.Button("Hủy", FormUi.Muted, 90);
        btnCancel.Location = new Point(340, 14);
        btnCancel.Click += (_, _) => Close();

        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnCancel);

        Controls.Add(pnlBody);
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
