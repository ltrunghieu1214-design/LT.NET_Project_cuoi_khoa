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
        Text = "DOKKI | Đặt bàn ăn mới";
        Size = new Size(500, 480);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = FormUi.BodyFont;

        var nextCode = $"DB{DokkiStore.Bookings.Count + 1:000}";

        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = FormUi.DarkRed, Padding = new Padding(20, 16, 20, 16) };
        var lblTitle = FormUi.Label("TIẾP NHẬN ĐẶT BÀN MỚI", 13, FontStyle.Bold, Color.White);
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
        txtGuest = new TextBox { PlaceholderText = "Nhập tên khách hàng..." };
        txtPhone = new TextBox { PlaceholderText = "Số điện thoại liên hệ..." };
        dtpTime = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true, Value = DateTime.Now.AddHours(1) };
        nudPax = new NumericUpDown { Minimum = 1, Maximum = 30, Value = 4 };

        cbTable = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        // Ưu tiên các bàn đang Trống
        foreach (var tbl in DokkiStore.Tables)
        {
            cbTable.Items.Add($"{tbl.Name} ({tbl.Status} - {tbl.Capacity} người)");
        }
        if (cbTable.Items.Count > 0) cbTable.SelectedIndex = 0;

        cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cbStatus.Items.AddRange(new object[] { "Đã xác nhận", "Chờ xác nhận" });
        cbStatus.SelectedIndex = 0;

        AddField("Mã đặt bàn:", txtCode);
        AddField("Tên khách hàng:", txtGuest);
        AddField("Số điện thoại:", txtPhone);
        AddField("Giờ hẹn:", dtpTime);
        AddField("Số lượng khách:", nudPax);
        AddField("Chọn bàn xếp chỗ:", cbTable);
        AddField("Trạng thái:", cbStatus);

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(20) };
        var btnSave = FormUi.Button("Lưu đặt bàn", FormUi.Red, 130);
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
        if (string.IsNullOrWhiteSpace(txtGuest.Text))
        {
            MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtGuest.Focus();
            return;
        }

        var tableStr = cbTable.SelectedItem?.ToString() ?? "Bàn 01";
        var tableName = tableStr.Split(' ')[0] + " " + tableStr.Split(' ')[1]; // "Bàn 01"

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
        MessageBox.Show($"Đặt bàn cho khách '{booking.Guest}' tại {booking.Table} thành công!", "DOKKI", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
