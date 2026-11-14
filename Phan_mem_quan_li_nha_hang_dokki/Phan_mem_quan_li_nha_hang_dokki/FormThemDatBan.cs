namespace Phan_mem_quan_li_nha_hang_dokki;

/// <summary>Dialog đặt bàn mới cho khách.</summary>
public class FormThemDatBan : Form
{
    public FormThemDatBan()
    {
        FormUi.StyleForm(this, "DOKKI | Đặt bàn mới");
        MinimumSize = new Size(520, 460);
        MaximumSize = new Size(520, 460);
        StartPosition = FormStartPosition.CenterParent;
        BuildView();
    }

    private void BuildView()
    {
        var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32, 24, 32, 20) };

        var lblTitle = new Label
        {
            Text = "ĐẶT BÀN MỚI",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = FormUi.DarkRed,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Mã đặt bàn (tự sinh)
        var nextCode = $"DB{DokkiStore.Bookings.Count + 1:000}";
        var lblCode = FormUi.Label("Mã đặt bàn", 10F, FontStyle.Bold);
        var txtCode = new TextBox { Width = 150, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle, Text = nextCode, ReadOnly = true, BackColor = Color.FromArgb(240, 240, 240) };

        // Tên khách
        var lblGuest = FormUi.Label("Tên khách hàng *", 10F, FontStyle.Bold);
        var txtGuest = new TextBox { Width = 420, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle };

        // Số điện thoại
        var lblPhone = FormUi.Label("Số điện thoại *", 10F, FontStyle.Bold);
        var txtPhone = new TextBox { Width = 220, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "VD: 0912 345 678" };

        // Giờ hẹn
        var lblTime = FormUi.Label("Giờ hẹn *", 10F, FontStyle.Bold);
        var txtTime = new TextBox { Width = 120, Font = FormUi.BodyFont, BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "VD: 18:30" };

        // Số người
        var lblPax = FormUi.Label("Số người *", 10F, FontStyle.Bold);
        var cbPax = new ComboBox { Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, Font = FormUi.BodyFont };
        for (int i = 1; i <= 20; i++) cbPax.Items.Add($"{i} người");
        cbPax.SelectedIndex = 1; // Mặc định 2 người

        // Chọn bàn
        var lblTable = FormUi.Label("Chọn bàn *", 10F, FontStyle.Bold);
        var cbTable = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Font = FormUi.BodyFont };
        foreach (var t in DokkiStore.Tables.Where(t => t.Status == "Trống"))
            cbTable.Items.Add($"Bàn {t.Number:00} ({t.Area})");
        if (cbTable.Items.Count > 0) cbTable.SelectedIndex = 0;

        // Nút lưu / huỷ
        var btnSave   = FormUi.Button("💾  Xác nhận đặt bàn", FormUi.Gold, 180);
        btnSave.ForeColor = Color.Black;
        var btnCancel = FormUi.Button("Huỷ", FormUi.Muted, 90);
        btnCancel.Click += (_, _) => Close();

        btnSave.Click += (_, _) =>
        {
            var code  = txtCode.Text.Trim();
            var guest = txtGuest.Text.Trim();
            var phone = txtPhone.Text.Trim();
            var time  = txtTime.Text.Trim();
            var pax   = cbPax.SelectedItem?.ToString() ?? "";
            var tableRaw = cbTable.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(guest) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(tableRaw))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường bắt buộc (*)!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy tên bàn ("Bàn 03 (Khu vực Tầng 1)" → "Bàn 03")
            var tableName = tableRaw.Contains('(') ? tableRaw[..tableRaw.IndexOf('(')].Trim() : tableRaw;

            var booking = new BookingItem(code, time, guest, phone, pax, tableName, "Đã xác nhận");
            DokkiStore.AddBooking(booking);
            MessageBox.Show($"✅ Đặt bàn cho khách '{guest}' lúc {time} thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 7; i++)
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));

        layout.Controls.Add(lblCode,  0, 0); layout.Controls.Add(txtCode,  1, 0);
        layout.Controls.Add(lblGuest, 0, 1); layout.Controls.Add(txtGuest, 1, 1);
        layout.Controls.Add(lblPhone, 0, 2); layout.Controls.Add(txtPhone, 1, 2);
        layout.Controls.Add(lblTime,  0, 3); layout.Controls.Add(txtTime,  1, 3);
        layout.Controls.Add(lblPax,   0, 4); layout.Controls.Add(cbPax,    1, 4);
        layout.Controls.Add(lblTable, 0, 5); layout.Controls.Add(cbTable,  1, 5);

        var btnPanel = new FlowLayoutPanel { Height = 46, Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 4, 0, 0) };
        btnPanel.Controls.Add(btnCancel);
        btnPanel.Controls.Add(btnSave);

        pnl.Controls.Add(layout);
        pnl.Controls.Add(lblTitle);
        pnl.Controls.Add(btnPanel);
        Controls.Add(pnl);
    }
}
