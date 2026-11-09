namespace Phan_mem_quan_li_nha_hang_dokki;

public class FormThemDatBan : Form
{
    private readonly TextBox txtCode;
    private readonly TextBox txtGuest;
    private readonly TextBox txtPhone;
    private readonly DateTimePicker dtpTime;
    private readonly NumericUpDown nudPax;
    private readonly ComboBox cbTable;
    private readonly ComboBox cbStatus;

    public FormThemDatBan()
    {
        Text = "DOKKI | Tiếp nhận đặt bàn mới";
        Size = new Size(580, 560);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;
        AutoScaleMode = AutoScaleMode.Dpi;

        var nextCode = $"DB{DokkiStore.Bookings.Count + 1:000}";

        // Header
        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = FormUi.DarkRed, Padding = new Padding(24, 12, 24, 12) };
        var lblTitle = FormUi.Label("TIẾP NHẬN ĐẶT BÀN MỚI", 12.5F, FontStyle.Bold, Color.White);
        var lblSub = FormUi.Label("Lưu trữ thông tin khách hàng và xếp bàn ăn trước giờ phục vụ", 9F, FontStyle.Regular, Color.FromArgb(255, 225, 225));
        lblSub.Location = new Point(24, 38);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSub);
        Controls.Add(pnlHeader);

        // Body TableLayoutPanel
        var tblLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(24, 16, 24, 10),
            BackColor = Color.White
        };
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F)); // Cực kỳ rộng rãi, không bao giờ bị đè nhãn
        tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        for (int i = 0; i < 7; i++)
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

        txtGuest = new TextBox
        {
            PlaceholderText = "Nhập họ tên khách hàng đặt bàn...",
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };

        txtPhone = new TextBox
        {
            PlaceholderText = "Số điện thoại liên hệ (VD: 0912 345 678)...",
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };

        dtpTime = new DateTimePicker
        {
            Format = DateTimePickerFormat.Time,
            ShowUpDown = true,
            Value = DateTime.Now.AddHours(1),
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };

        // Ô nhập số lượng khách với NumericUpDown căn giữa, rõ ràng, không bị che
        var pnlPax = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            Margin = new Padding(0),
            Padding = new Padding(0, 2, 0, 0)
        };
        nudPax = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 50,
            Value = 4,
            Width = 110,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Center
        };
        var lblPaxUnit = FormUi.Label("khách (người lớn / trẻ em)", 10F, FontStyle.Regular, FormUi.Muted);
        lblPaxUnit.Margin = new Padding(8, 4, 0, 0);
        pnlPax.Controls.Add(nudPax);
        pnlPax.Controls.Add(lblPaxUnit);

        // Danh sách bàn ăn: hiển thị rộng rãi, rõ tên bàn, khu vực và sức chứa
        cbTable = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill,
            DropDownWidth = 380
        };
        foreach (var tbl in DokkiStore.Tables)
        {
            cbTable.Items.Add($"{tbl.Name} — {tbl.Area} [{tbl.Status} - {tbl.Capacity} chỗ]");
        }
        if (cbTable.Items.Count > 0) cbTable.SelectedIndex = 0;

        cbStatus = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = FormUi.BodyFont,
            Dock = DockStyle.Fill
        };
        cbStatus.Items.AddRange(new object[] { "Đã xác nhận", "Chờ xác nhận" });
        cbStatus.SelectedIndex = 0;

        void AddRow(int row, string labelText, Control control)
        {
            var lbl = FormUi.Label(labelText, 10F, FontStyle.Bold);
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            tblLayout.Controls.Add(lbl, 0, row);
            tblLayout.Controls.Add(control, 1, row);
        }

        AddRow(0, "Mã đặt bàn:", txtCode);
        AddRow(1, "Tên khách hàng:", txtGuest);
        AddRow(2, "Số điện thoại:", txtPhone);
        AddRow(3, "Giờ hẹn:", dtpTime);
        AddRow(4, "Số lượng khách:", pnlPax);
        AddRow(5, "Chọn bàn xếp chỗ:", cbTable);
        AddRow(6, "Trạng thái đặt:", cbStatus);

        // Buttons Footer
        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(24, 12, 24, 12) };
        var pnlBtnFlow = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, WrapContents = false };

        var btnSave = FormUi.Button("✓ Lưu đặt bàn", FormUi.Red, 140);
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
        if (string.IsNullOrWhiteSpace(txtGuest.Text))
        {
            MessageBox.Show("Vui lòng nhập họ tên khách hàng đặt bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtGuest.Focus();
            return;
        }

        var tableStr = cbTable.SelectedItem?.ToString() ?? "BÀN 01";
        var tableName = tableStr.Contains('—') ? tableStr.Split('—')[0].Trim() : tableStr.Split(' ')[0] + " " + tableStr.Split(' ')[1];

        var booking = new BookingItem(
            txtCode.Text.Trim(),
            dtpTime.Value.ToString("HH:mm"),
            txtGuest.Text.Trim(),
            string.IsNullOrWhiteSpace(txtPhone.Text) ? "09xx xxx xxx" : txtPhone.Text.Trim(),
            $"{nudPax.Value} người",
            tableName,
            cbStatus.SelectedItem?.ToString() ?? "Đã xác nhận"
        );

        DokkiStore.AddBooking(booking);

        // Nếu đặt bàn thành công và trạng thái là 'Đã xác nhận', cập nhật trạng thái bàn thành 'Đã đặt'
        if (booking.Status == "Đã xác nhận")
        {
            var matchedTable = DokkiStore.Tables.FirstOrDefault(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));
            if (matchedTable != null && matchedTable.Status == "Trống")
            {
                DokkiStore.SetTableStatus(matchedTable.Number, "Đã đặt");
            }
        }

        MessageBox.Show($"Đặt bàn cho khách '{booking.Guest}' tại {booking.Table} ({nudPax.Value} khách) thành công!", "DOKKI", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
