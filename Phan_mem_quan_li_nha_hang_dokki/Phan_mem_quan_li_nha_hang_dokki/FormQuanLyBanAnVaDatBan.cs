namespace Phan_mem_quan_li_nha_hang_dokki;

public class FormQuanLyBanAnVaDatBan : Form
{
    private FlowLayoutPanel pnlTables = null!;
    private DataGridView gridBookings = null!;
    private Label lblEmptyCount = null!;
    private Label lblServingCount = null!;
    private Label lblBookedCount = null!;

    public FormQuanLyBanAnVaDatBan()
    {
        FormUi.StyleForm(this, "DOKKI | Bàn ăn & đặt bàn");
        WindowState = FormWindowState.Maximized;
        BuildView();
        DokkiStore.DataChanged += ReloadAllData;
        FormClosed += (_, _) => DokkiStore.DataChanged -= ReloadAllData;
    }

    private void BuildView()
    {
        var header = FormUi.Header("Bàn ăn & đặt bàn", "Theo dõi sơ đồ bàn trực tiếp và danh sách lịch hẹn khách hàng — Chi nhánh DOKKI EPU");
        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(28, 6, 28, 20) };

        // Toolbar trên cùng: Thống kê trạng thái + Nút thêm bàn / Đặt bàn
        var toolbar = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.Transparent };
        var pnlLegend = new FlowLayoutPanel { Dock = DockStyle.Left, Width = 520, Height = 50, WrapContents = false, Padding = new Padding(0, 10, 0, 0) };

        var lblLegendTitle = FormUi.Label("TRẠNG THÁI:", 10.5F, FontStyle.Bold);
        lblEmptyCount = FormUi.Label("  ● Trống", 10F, FontStyle.Bold, FormUi.Green);
        lblServingCount = FormUi.Label("  ● Đang phục vụ", 10F, FontStyle.Bold, FormUi.Red);
        lblBookedCount = FormUi.Label("  ● Đã đặt trước", 10F, FontStyle.Bold, FormUi.Gold);

        pnlLegend.Controls.Add(lblLegendTitle);
        pnlLegend.Controls.Add(lblEmptyCount);
        pnlLegend.Controls.Add(lblServingCount);
        pnlLegend.Controls.Add(lblBookedCount);

        var pnlActions = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 300, Height = 50, WrapContents = false, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 4, 0, 0) };

        var btnAddBooking = FormUi.Button("+ Đặt bàn mới", FormUi.Gold, 130);
        btnAddBooking.ForeColor = Color.Black;
        btnAddBooking.Click += (_, _) =>
        {
            using var f = new FormThemDatBan();
            f.ShowDialog(this);
        };

        var btnAddTable = FormUi.Button("+ Thêm bàn", FormUi.Blue, 120);
        btnAddTable.Click += (_, _) =>
        {
            using var f = new FormThemBanAn();
            f.ShowDialog(this);
        };

        pnlActions.Controls.Add(btnAddBooking);
        pnlActions.Controls.Add(btnAddTable);

        toolbar.Controls.Add(pnlLegend);
        toolbar.Controls.Add(pnlActions);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterDistance = 620,
            FixedPanel = FixedPanel.Panel2,
            IsSplitterFixed = false
        };

        // Panel 1: Sơ đồ bàn ăn
        var pnlTableContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
        var lblTableTitle = new Label { Text = "SƠ ĐỒ BÀN ĂN CHI NHÁNH (Bấm vào bàn để đổi trạng thái)", Dock = DockStyle.Top, Height = 28, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = FormUi.Muted };
        pnlTables = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Padding = new Padding(6) };
        pnlTableContainer.Controls.Add(pnlTables);
        pnlTableContainer.Controls.Add(lblTableTitle);
        lblTableTitle.SendToBack();
        pnlTables.BringToFront();
        split.Panel1.Controls.Add(pnlTableContainer);

        // Panel 2: Danh sách lịch đặt bàn
        var pnlBookingContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
        var lblBookingTitle = new Label { Text = "DANH SÁCH KHÁCH ĐẶT BÀN TRONG NGÀY", Dock = DockStyle.Top, Height = 28, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = FormUi.Muted };

        gridBookings = FormUi.Grid();
        gridBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridBookings.ScrollBars = ScrollBars.Vertical; // Vừa vặn 100% chiều ngang, không cần kéo tay

        var colBCode = new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Mã", FillWeight = 50, MinimumWidth = 60 };
        var colBTime = new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "Giờ", FillWeight = 45, MinimumWidth = 55 };
        var colBGuest = new DataGridViewTextBoxColumn { Name = "Guest", HeaderText = "Khách hàng", FillWeight = 125, MinimumWidth = 120 };
        var colBPhone = new DataGridViewTextBoxColumn { Name = "Phone", HeaderText = "Điện thoại", FillWeight = 85, MinimumWidth = 90 };
        var colBPax = new DataGridViewTextBoxColumn { Name = "Pax", HeaderText = "Số khách", FillWeight = 55, MinimumWidth = 65 };
        var colBTable = new DataGridViewTextBoxColumn { Name = "Table", HeaderText = "Bàn", FillWeight = 55, MinimumWidth = 65 };
        var colBStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", FillWeight = 75, MinimumWidth = 85 };

        gridBookings.Columns.AddRange(colBCode, colBTime, colBGuest, colBPhone, colBPax, colBTable, colBStatus);

        pnlBookingContainer.Controls.Add(gridBookings);
        pnlBookingContainer.Controls.Add(lblBookingTitle);
        lblBookingTitle.SendToBack();
        gridBookings.BringToFront();
        split.Panel2.Controls.Add(pnlBookingContainer);

        split.SizeChanged += (_, _) =>
        {
            if (split.Width > 600) split.SplitterDistance = (int)(split.Width * 0.52);
        };

        var bodyLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        bodyLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
        bodyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        bodyLayout.Controls.Add(toolbar, 0, 0);
        bodyLayout.Controls.Add(split, 0, 1);
        body.Controls.Add(bodyLayout);

        var pageLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        pageLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pageLayout.Controls.Add(header, 0, 0);
        pageLayout.Controls.Add(body, 0, 1);
        Controls.Add(pageLayout);

        ReloadAllData();
    }

    private void ReloadAllData()
    {
        // 1. Thống kê số lượng
        var empty = DokkiStore.Tables.Count(t => t.Status == "Trống");
        var serving = DokkiStore.Tables.Count(t => t.Status == "Đang phục vụ");
        var booked = DokkiStore.Tables.Count(t => t.Status == "Đã đặt");

        lblEmptyCount.Text = $"  ● Trống ({empty})";
        lblServingCount.Text = $"  ● Đang phục vụ ({serving})";
        lblBookedCount.Text = $"  ● Đã đặt ({booked})";

        // 2. Render lại Sơ đồ bàn
        pnlTables.SuspendLayout();
        pnlTables.Controls.Clear();

        foreach (var tbl in DokkiStore.Tables)
        {
            var statusColor = tbl.Status == "Đang phục vụ" ? FormUi.Red : (tbl.Status == "Đã đặt" ? FormUi.Gold : FormUi.Green);
            var btn = new Button
            {
                Text = $"{tbl.Name}\n{tbl.Status}\n({tbl.Capacity} chỗ)",
                Width = 132,
                Height = 82,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(252, 252, 254),
                ForeColor = statusColor,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Margin = new Padding(6),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = statusColor;
            btn.FlatAppearance.BorderSize = 2;

            var tableNumber = tbl.Number;
            btn.Click += (_, _) => OnTableClicked(tableNumber);
            pnlTables.Controls.Add(btn);
        }

        pnlTables.ResumeLayout();

        // 3. Render lại danh sách đặt bàn
        gridBookings.Rows.Clear();
        foreach (var b in DokkiStore.Bookings)
        {
            gridBookings.Rows.Add(b.Code, b.Time, b.Guest, b.Phone, b.Pax, b.Table, b.Status);
        }
    }

    private void OnTableClicked(int tableNumber)
    {
        var tbl = DokkiStore.Tables.FirstOrDefault(t => t.Number == tableNumber);
        if (tbl is null) return;

        var result = MessageBox.Show(
            $"BÀN {tableNumber:00} ({tbl.Area})\n" +
            $"Trạng thái hiện tại: {tbl.Status}\n\n" +
            "Chọn [Yes] để đổi trạng thái bàn (Trống / Đang phục vụ / Đã đặt).\n" +
            "Chọn [No] để đóng.",
            $"Thao tác với {tbl.Name}",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (result == DialogResult.Yes)
        {
            // Xoay vòng trạng thái
            var nextStatus = tbl.Status switch
            {
                "Trống" => "Đang phục vụ",
                "Đang phục vụ" => "Đã đặt",
                _ => "Trống"
            };
            DokkiStore.SetTableStatus(tableNumber, nextStatus);
        }
    }
}
