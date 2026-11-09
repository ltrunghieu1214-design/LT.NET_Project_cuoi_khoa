namespace Phan_mem_quan_li_nha_hang_dokki;

public sealed class FormMain : Form
{
    private readonly Panel content;
    private TableLayoutPanel cards = null!;
    private DataGridView ordersGrid = null!;
    private ListBox noticesList = null!;

    public FormMain()
    {
        FormUi.StyleForm(this, "DOKKI | Hệ thống quản lý nhà hàng");
        WindowState = FormWindowState.Maximized;

        var navigation = FormUi.Navigation("HOME", Navigate);
        content = new Panel { Dock = DockStyle.Fill, BackColor = FormUi.Background };

        // QUY TẮC VÀNG VỀ DOCKING TRONG WINFORMS:
        // Control Dock = Left/Top/Bottom/Right PHẢI ĐƯỢC SendToBack() để chiếm mép trước
        // Control Dock = Fill PHẢI ĐƯỢC BringToFront() để lấp đầy phần diện tích CÒN LẠI
        Controls.Add(content);
        Controls.Add(navigation);
        navigation.Dock = DockStyle.Left;
        content.Dock = DockStyle.Fill;
        navigation.SendToBack();
        content.BringToFront();

        ShowDashboard();

        DokkiStore.DataChanged += RefreshDashboardData;
        FormClosed += (_, _) => DokkiStore.DataChanged -= RefreshDashboardData;
    }

    private void Navigate(string target)
    {
        if (target == "HOME") { ShowDashboard(); return; }
        Form? form = target switch
        {
            "ORDER" => new FormOrderMon(),
            "TABLE" => new FormQuanLyBanAnVaDatBan(),
            "FOOD" => new FormQuanLyMonAn(),
            "CATEGORY" => new FormQuanLyDanhMucMonAn(),
            _ => null
        };
        if (form is null) return;
        using (form) form.ShowDialog(this);
    }

    private void ShowDashboard()
    {
        content.Controls.Clear();

        var header = FormUi.Header("Tổng quan hôm nay", "Thứ Hai, 24 tháng 6, 2024  •  Chi nhánh DOKKI EPU");
        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(28, 12, 28, 20), AutoScroll = true };

        var dashboard = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        dashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        dashboard.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
        dashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // 4 Thẻ thống kê
        cards = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        for (var column = 0; column < 4; column++)
        {
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }
        cards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        PopulateCards();

        // Phần dưới: Đơn hàng gần đây (Trái) & Hoạt động trong ngày (Phải)
        var lower = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = FormUi.Background,
            Margin = new Padding(0, 14, 0, 0),
            Padding = Padding.Empty
        };
        lower.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        lower.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        lower.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        ordersGrid = FormUi.Grid();
        ordersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        ordersGrid.ScrollBars = ScrollBars.Vertical; // Chỉ cuộn dọc, tự động vừa khít chiều ngang không cần kéo tay

        var colCode = new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Mã HĐ", FillWeight = 55, MinimumWidth = 75 };
        var colTime = new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "Giờ", FillWeight = 45, MinimumWidth = 60 };
        var colTable = new DataGridViewTextBoxColumn { Name = "Table", HeaderText = "Bàn", FillWeight = 55, MinimumWidth = 70 };
        var colCust = new DataGridViewTextBoxColumn { Name = "Customer", HeaderText = "Khách hàng", FillWeight = 130, MinimumWidth = 130 };
        var colAmt = new DataGridViewTextBoxColumn { Name = "Amount", HeaderText = "Tổng tiền", FillWeight = 90, MinimumWidth = 100 };
        var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", FillWeight = 85, MinimumWidth = 100 };

        ordersGrid.Columns.AddRange(colCode, colTime, colTable, colCust, colAmt, colStatus);

        foreach (var b in DokkiStore.Bills)
        {
            ordersGrid.Rows.Add(b.Code, b.Time, b.Table, b.Customer, b.Amount, b.Status);
        }

        var orderCard = Wrap("Đơn hàng & Hóa đơn gần đây (25 đơn)", ordersGrid);
        lower.Controls.Add(orderCard, 0, 0);

        noticesList = new ListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = FormUi.BodyFont,
            IntegralHeight = false
        };

        foreach (var act in DokkiStore.Activities)
        {
            noticesList.Items.Add(act);
        }

        lower.Controls.Add(Wrap("Nhật ký hoạt động chi nhánh (20 thông báo)", noticesList), 1, 0);

        dashboard.Controls.Add(cards, 0, 0);
        dashboard.Controls.Add(lower, 0, 1);
        body.Controls.Add(dashboard);

        // Z-order layout chuẩn: Header ở đáy để chiếm mép trên, Body ở đỉnh để lấp đầy phần còn lại
        content.Controls.Add(body);
        content.Controls.Add(header);
        header.Dock = DockStyle.Top;
        body.Dock = DockStyle.Fill;
        header.SendToBack();
        body.BringToFront();
    }

    private void PopulateCards()
    {
        cards.Controls.Clear();
        var servingCount = DokkiStore.Tables.Count(t => t.Status == "Đang phục vụ");
        var totalTables = DokkiStore.Tables.Count;
        var activeBookings = DokkiStore.Bookings.Count(b => b.Status != "Đã hủy");

        cards.Controls.Add(FormUi.Card("Doanh thu hôm nay", $"{DokkiStore.TodayRevenue:N0} đ", FormUi.Gold), 0, 0);
        cards.Controls.Add(FormUi.Card("Đơn hàng hoàn tất", $"{DokkiStore.Bills.Count}", FormUi.Red), 1, 0);
        cards.Controls.Add(FormUi.Card("Bàn đang phục vụ", $"{servingCount} / {totalTables}", FormUi.Green), 2, 0);
        cards.Controls.Add(FormUi.Card("Đặt bàn trong ngày", $"{activeBookings}", FormUi.Blue), 3, 0);
    }

    private void RefreshDashboardData()
    {
        if (InvokeRequired)
        {
            Invoke(RefreshDashboardData);
            return;
        }

        PopulateCards();

        ordersGrid.Rows.Clear();
        foreach (var b in DokkiStore.Bills)
        {
            ordersGrid.Rows.Add(b.Code, b.Time, b.Table, b.Customer, b.Amount, b.Status);
        }

        noticesList.Items.Clear();
        foreach (var act in DokkiStore.Activities)
        {
            noticesList.Items.Add(act);
        }
    }

    private static Panel Wrap(string title, Control child)
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16), Margin = new Padding(0, 0, 12, 0) };
        var label = FormUi.Label(title, 11, FontStyle.Bold);
        label.Dock = DockStyle.Top;
        label.Height = 32;
        panel.Controls.Add(child);
        panel.Controls.Add(label);
        label.SendToBack();
        child.BringToFront();
        return panel;
    }
}
