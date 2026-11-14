namespace Phan_mem_quan_li_nha_hang_dokki;

/// <summary>Dialog thêm danh mục món ăn mới.</summary>
public class FormThemDanhMuc : Form
{
    public FormThemDanhMuc()
    {
        FormUi.StyleForm(this, "DOKKI | Thêm danh mục mới");
        MinimumSize = new Size(520, 380);
        MaximumSize = new Size(520, 380);
        StartPosition = FormStartPosition.CenterParent;
        BuildView();
    }

    private void BuildView()
    {
        var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32, 24, 32, 20) };

        var lblTitle = new Label
        {
            Text = "THÊM DANH MỤC MỚI",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = FormUi.DarkRed,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Mã danh mục
        var lblCode = FormUi.Label("Mã danh mục *", 10F, FontStyle.Bold);
        var txtCode = new TextBox { Width = 200, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "VD: DM031" };

        // Tên danh mục
        var lblName = FormUi.Label("Tên danh mục *", 10F, FontStyle.Bold);
        var txtName = new TextBox { Width = 420, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle };

        // Ghi chú
        var lblNote = FormUi.Label("Ghi chú", 10F, FontStyle.Bold);
        var txtNote = new TextBox { Width = 420, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle };

        // Nút lưu / huỷ
        var btnSave   = FormUi.Button("💾  Lưu danh mục", FormUi.Red, 155);
        var btnCancel = FormUi.Button("Huỷ", FormUi.Muted, 90);
        btnCancel.Click += (_, _) => Close();

        btnSave.Click += (_, _) =>
        {
            var code = txtCode.Text.Trim().ToUpper();
            var name = txtName.Text.Trim();
            var note = txtNote.Text.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng điền đầy đủ Mã và Tên danh mục!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DokkiStore.Categories.Any(c => c.Code == code))
            {
                MessageBox.Show($"Mã danh mục '{code}' đã tồn tại!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cat = new CategoryItem(code, name, 0, note, "Đang phục vụ");
            DokkiStore.AddCategory(cat);
            MessageBox.Show($"✅ Đã thêm danh mục '{name}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 4; i++)
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

        layout.Controls.Add(lblCode, 0, 0); layout.Controls.Add(txtCode, 1, 0);
        layout.Controls.Add(lblName, 0, 1); layout.Controls.Add(txtName, 1, 1);
        layout.Controls.Add(lblNote, 0, 2); layout.Controls.Add(txtNote, 1, 2);

        var btnPanel = new FlowLayoutPanel { Height = 46, Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 4, 0, 0) };
        btnPanel.Controls.Add(btnCancel);
        btnPanel.Controls.Add(btnSave);

        pnl.Controls.Add(layout);
        pnl.Controls.Add(lblTitle);
        pnl.Controls.Add(btnPanel);
        Controls.Add(pnl);
    }
}
