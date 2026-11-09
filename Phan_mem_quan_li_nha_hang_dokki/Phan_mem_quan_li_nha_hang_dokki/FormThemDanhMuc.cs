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
        Size = new Size(500, 390);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;

        var nextCode = $"DM{DokkiStore.Categories.Count + 1:000}";

        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = FormUi.DarkRed, Padding = new Padding(20, 16, 20, 16) };
        var lblTitle = FormUi.Label("THÊM DANH MỤC MỚI", 13, FontStyle.Bold, Color.White);
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
        txtName = new TextBox { PlaceholderText = "Nhập tên danh mục..." };
        txtNote = new TextBox { PlaceholderText = "Ghi chú phân loại..." };
        cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cbStatus.Items.AddRange(new object[] { "Đang phục vụ", "Tạm ẩn" });
        cbStatus.SelectedIndex = 0;

        AddField("Mã danh mục:", txtCode);
        AddField("Tên danh mục:", txtName);
        AddField("Ghi chú:", txtNote);
        AddField("Trạng thái:", cbStatus);

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(20) };
        var btnSave = FormUi.Button("Lưu danh mục", FormUi.Red, 140);
        btnSave.Location = new Point(190, 14);
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
