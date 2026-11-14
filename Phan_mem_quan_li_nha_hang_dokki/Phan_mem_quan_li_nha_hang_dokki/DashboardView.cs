using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;

namespace Phan_mem_quan_li_nha_hang_dokki;

internal sealed class DashboardView : UserControl
{
    private readonly bool canSeeRevenue;
    private readonly Action openTables;
    private readonly Action openBooking;
    private readonly TableLayoutPanel pageLayout;
    private readonly List<KpiCard> kpiCards = new();
    private readonly BufferedPanel categoryChart = new();
    private readonly BufferedPanel tableChart = new();
    private DataGridView gridBookings = null!;
    private DataGridView gridStaff = null!;
    private Label greetingLabel = null!;
    private Label clockLabel = null!;
    private Label dataStatusLabel = null!;
    private readonly System.Windows.Forms.Timer clockTimer = new() { Interval = 1000 };
    private readonly System.Windows.Forms.Timer refreshTimer = new() { Interval = 30000 };
    private DashboardSummary? summary;
    private List<DashboardStaffItem> staff = new();

    public DashboardView(bool canSeeRevenue, Action openTables, Action openBooking)
    {
        this.canSeeRevenue = canSeeRevenue;
        this.openTables = openTables;
        this.openBooking = openBooking;
        Dock = DockStyle.Fill;
        AutoScroll = true;
        BackColor = FormUi.Background;
        Padding = new Padding(12, 8, 12, 12);

        pageLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 5,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        pageLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 245F));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
        Controls.Add(pageLayout);
        Resize += (_, _) => pageLayout.Width = Math.Max(400, ClientSize.Width - Padding.Horizontal - SystemInformation.VerticalScrollBarWidth);

        var header = BuildHeader(out greetingLabel, out clockLabel);
        var actions = BuildQuickActions();
        var kpis = BuildKpis();
        var charts = BuildCharts();
        var lists = BuildLists(out gridBookings, out gridStaff);
        dataStatusLabel = FormUi.Label("", 9F, FontStyle.Regular, FormUi.Muted);
        dataStatusLabel.Dock = DockStyle.Bottom;
        lists.Controls.Add(dataStatusLabel);

        pageLayout.Controls.Add(header, 0, 0);
        pageLayout.Controls.Add(actions, 0, 1);
        pageLayout.Controls.Add(kpis, 0, 2);
        pageLayout.Controls.Add(charts, 0, 3);
        pageLayout.Controls.Add(lists, 0, 4);

        categoryChart.Paint += (_, e) => DrawCategoryChart(e.Graphics, categoryChart.ClientRectangle);
        tableChart.Paint += (_, e) => DrawTableChart(e.Graphics, tableChart.ClientRectangle);
        DokkiStore.DataChanged += RefreshDashboard;
        Disposed += (_, _) =>
        {
            DokkiStore.DataChanged -= RefreshDashboard;
            clockTimer.Stop();
            refreshTimer.Stop();
            clockTimer.Dispose();
            refreshTimer.Dispose();
        };

        UpdateClock();
        clockTimer.Tick += (_, _) => UpdateClock();
        refreshTimer.Tick += (_, _) => RefreshDashboard();
        clockTimer.Start();
        refreshTimer.Start();
        RefreshDashboard();
    }

    private static Panel BuildHeader(out Label greeting, out Label clock)
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = FormUi.Background };
        greeting = new Label
        {
            Text = $"Xin chào, {Session.TenDangNhap}  -  Quyền: {Session.Quyen}",
            Dock = DockStyle.Left,
            Width = 650,
            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            ForeColor = FormUi.Ink,
            TextAlign = ContentAlignment.MiddleLeft
        };
        clock = new Label
        {
            Dock = DockStyle.Right,
            Width = 300,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = FormUi.Muted,
            TextAlign = ContentAlignment.MiddleRight
        };
        panel.Controls.Add(greeting);
        panel.Controls.Add(clock);
        return panel;
    }

    private Panel BuildQuickActions()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(0, 3, 0, 0),
            BackColor = FormUi.Background
        };
        var btnTable = FormUi.Button("+ Chọn bàn / Tạo đơn", FormUi.Red, 210);
        var btnBooking = FormUi.Button("+ Đặt bàn mới", FormUi.Gold, 145);
        btnBooking.ForeColor = Color.Black;
        var btnExport = FormUi.Button("Xuất báo cáo ngày", FormUi.Blue, 175);
        btnTable.Click += (_, _) => openTables();
        btnBooking.Click += (_, _) => openBooking();
        btnExport.Click += (_, _) => ExportDailyReport();
        panel.Controls.Add(btnTable);
        panel.Controls.Add(btnBooking);
        panel.Controls.Add(btnExport);
        return panel;
    }

    private Control BuildKpis()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        for (var i = 0; i < 4; i++)
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        kpiCards.Add(CreateKpiCard("DOANH THU HÔM NAY", FormUi.Red));
        kpiCards.Add(CreateKpiCard("BÀN ĐANG PHỤC VỤ", FormUi.Green));
        kpiCards.Add(CreateKpiCard("HÓA ĐƠN HÔM NAY", FormUi.Blue));
        kpiCards.Add(CreateKpiCard("MÓN ĐANG BÁN", FormUi.Gold));
        for (var i = 0; i < kpiCards.Count; i++)
            panel.Controls.Add(kpiCards[i].Panel, i, 0);
        return panel;
    }

    private static KpiCard CreateKpiCard(string title, Color accent)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = FormUi.Surface,
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(16, 11, 12, 8)
        };
        var stripe = new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accent };
        var titleLabel = FormUi.Label(title, 8.5F, FontStyle.Bold, FormUi.Muted);
        titleLabel.Location = new Point(14, 10);
        titleLabel.MaximumSize = new Size(240, 24);
        var valueLabel = FormUi.Label("—", 18F, FontStyle.Bold, FormUi.Ink);
        valueLabel.Location = new Point(26, 32);
        valueLabel.AutoSize = false;
        valueLabel.Size = new Size(220, 38);
        valueLabel.TextAlign = ContentAlignment.MiddleLeft;
        var detailLabel = FormUi.Label("", 8.5F, FontStyle.Regular, FormUi.Muted);
        detailLabel.Location = new Point(26, 72);
        detailLabel.MaximumSize = new Size(250, 22);
        card.Controls.Add(titleLabel);
        card.Controls.Add(valueLabel);
        card.Controls.Add(detailLabel);
        card.Controls.Add(stripe);
        return new KpiCard(card, titleLabel, valueLabel, detailLabel);
    }

    private Control BuildCharts()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        panel.Controls.Add(CreateChartSurface("SỐ MÓN THEO DANH MỤC (TOP 8)", categoryChart), 0, 0);
        panel.Controls.Add(CreateChartSurface("TRẠNG THÁI BÀN ĂN", tableChart), 1, 0);
        return panel;
    }

    private static Panel CreateChartSurface(string title, Control chart)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = FormUi.Surface,
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(12, 8, 12, 8)
        };
        var titleLabel = FormUi.Label(title, 9F, FontStyle.Bold, FormUi.Muted);
        titleLabel.Dock = DockStyle.Top;
        titleLabel.Height = 25;
        chart.Dock = DockStyle.Fill;
        panel.Controls.Add(chart);
        panel.Controls.Add(titleLabel);
        titleLabel.BringToFront();
        return panel;
    }

    private static Control BuildLists(out DataGridView bookings, out DataGridView employees)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        bookings = CreateGrid();
        bookings.Columns.Add("Guest", "Khách");
        bookings.Columns.Add("Phone", "SĐT");
        bookings.Columns.Add("Time", "Giờ đến");
        bookings.Columns.Add("Pax", "Số người");
        bookings.Columns.Add("Table", "Bàn");
        bookings.Columns[0].FillWeight = 125;
        bookings.Columns[1].FillWeight = 95;
        bookings.Columns[2].FillWeight = 60;
        bookings.Columns[3].FillWeight = 55;
        bookings.Columns[4].FillWeight = 55;

        employees = CreateGrid();
        employees.Columns.Add("Name", "Họ tên");
        employees.Columns.Add("Position", "Chức vụ");
        employees.Columns.Add("Phone", "Số điện thoại");
        employees.Columns[0].FillWeight = 110;
        employees.Columns[1].FillWeight = 90;
        employees.Columns[2].FillWeight = 100;

        panel.Controls.Add(CreateGridSurface("LỊCH ĐẶT BÀN SẮP TỚI", bookings), 0, 0);
        panel.Controls.Add(CreateGridSurface("NHÂN SỰ / CA TRỰC", employees), 1, 0);
        return panel;
    }

    private static DataGridView CreateGrid()
    {
        var grid = FormUi.Grid();
        grid.Dock = DockStyle.Fill;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ColumnHeadersHeight = 36;
        grid.RowTemplate.Height = 34;
        grid.DefaultCellStyle.Padding = new Padding(4);
        grid.ScrollBars = ScrollBars.Vertical;
        return grid;
    }

    private static Panel CreateGridSurface(string title, DataGridView grid)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = FormUi.Surface,
            Margin = new Padding(0, 0, 10, 0),
            Padding = Padding.Empty
        };
        var titleLabel = FormUi.Label(title, 9F, FontStyle.Bold, FormUi.Muted);
        titleLabel.Location = new Point(10, 7);
        titleLabel.Height = 25;
        grid.Dock = DockStyle.None;
        grid.Location = new Point(8, 40);
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.Size = new Size(Math.Max(0, panel.ClientSize.Width - 16), Math.Max(0, panel.ClientSize.Height - 48));
        panel.Resize += (_, _) =>
        {
            grid.Size = new Size(Math.Max(0, panel.ClientSize.Width - 16), Math.Max(0, panel.ClientSize.Height - 48));
        };
        panel.Controls.Add(grid);
        panel.Controls.Add(titleLabel);
        titleLabel.BringToFront();
        return panel;
    }

    private void RefreshDashboard()
    {
        try
        {
            summary = DatabaseHelper.LoadDashboardSummary();
            staff = Database_QL_He_Thong.LoadDashboardStaff();
            dataStatusLabel.Text = "Dữ liệu cập nhật lúc " + DateTime.Now.ToString("HH:mm:ss");
        }
        catch (Exception ex)
        {
            dataStatusLabel.Text = "Không thể cập nhật dashboard: " + ex.Message;
        }

        UpdateKpis();
        UpdateGrids();
        categoryChart.Invalidate();
        tableChart.Invalidate();
        UpdateClock();
    }

    private void UpdateKpis()
    {
        if (summary is null) return;

        var bookedToday = GetUpcomingBookings().Count;
        var availableTables = Math.Max(0, summary.TotalTables - summary.ServingTables -
            summary.TableStatusCounts.Where(x => x.Status.Contains("đặt", StringComparison.OrdinalIgnoreCase)).Sum(x => x.Count));
        if (canSeeRevenue)
        {
            SetKpi(0, "DOANH THU HÔM NAY", $"{summary.RevenueToday:N0} đ", "Hóa đơn đã thanh toán trong ngày");
            SetKpi(2, "HÓA ĐƠN HÔM NAY", summary.PaidBillsToday.ToString("N0"), "Tổng hóa đơn đã thanh toán");
        }
        else
        {
            SetKpi(0, "BÀN TRỐNG", availableTables.ToString("N0"), "Bàn sẵn sàng phục vụ");
            SetKpi(2, "LỊCH ĐẶT BÀN", bookedToday.ToString("N0"), "Lịch chưa hoàn tất sắp tới");
        }

        var occupancy = summary.TotalTables == 0 ? 0 : (int)Math.Round(summary.ServingTables * 100d / summary.TotalTables);
        SetKpi(1, "BÀN ĐANG PHỤC VỤ", $"{summary.ServingTables} / {summary.TotalTables}", $"Tỷ lệ bàn đang phục vụ {occupancy}%");
        SetKpi(3, "MÓN ĐANG BÁN", summary.ActiveMenuItems.ToString("N0"), "Mặt hàng đang mở bán");
    }

    private void SetKpi(int index, string title, string value, string detail)
    {
        kpiCards[index].Title.Text = title;
        kpiCards[index].Value.Text = value;
        kpiCards[index].Detail.Text = detail;
    }

    private List<BookingItem> GetUpcomingBookings()
    {
        if (summary is null) return new List<BookingItem>();
        var now = DateTime.Now.TimeOfDay;
        return summary.Bookings
            .Where(item => TimeSpan.TryParse(item.Time, out var time) && time >= now)
            .OrderBy(item => TimeSpan.TryParse(item.Time, out var time) ? time : TimeSpan.MaxValue)
            .Take(5)
            .ToList();
    }

    private void UpdateGrids()
    {
        gridBookings.Rows.Clear();
        foreach (var booking in GetUpcomingBookings())
            gridBookings.Rows.Add(booking.Guest, booking.Phone, booking.Time, booking.Pax, booking.Table);
        if (gridBookings.Rows.Count == 0)
            gridBookings.Rows.Add("Không có lịch đặt sắp tới", "", "", "", "");

        gridStaff.Rows.Clear();
        foreach (var employee in staff.Take(8))
            gridStaff.Rows.Add(employee.Name, employee.Position, employee.Phone);
        if (gridStaff.Rows.Count == 0)
            gridStaff.Rows.Add("Chưa có dữ liệu nhân sự", "", "");

        SetRowsHeight(gridBookings);
        SetRowsHeight(gridStaff);
    }

    private static void SetRowsHeight(DataGridView grid)
    {
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (!row.IsNewRow)
                row.Height = 34;
        }
    }

    private void DrawCategoryChart(Graphics graphics, Rectangle bounds)
    {
        graphics.Clear(FormUi.Surface);
        if (summary is null || summary.CategoryCounts.Count == 0)
        {
            DrawCenteredMessage(graphics, bounds, "Chưa có dữ liệu danh mục");
            return;
        }

        var items = summary.CategoryCounts.OrderByDescending(item => item.Count).Take(8).ToList();
        var labelWidth = Math.Min(190, Math.Max(110, bounds.Width / 3));
        var chartWidth = Math.Max(40, bounds.Width - labelWidth - 42);
        var rowHeight = Math.Max(18, (bounds.Height - 46) / items.Count);
        var maxCount = Math.Max(1, items.Max(item => item.Count));
        using var font = new Font("Segoe UI", 8.5F);
        using var boldFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        using var textBrush = new SolidBrush(FormUi.Ink);
        using var backgroundBrush = new SolidBrush(Color.FromArgb(239, 241, 245));
        using var stringFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center };
        var colors = new[] { FormUi.Red, FormUi.Gold, FormUi.Blue, FormUi.Green, Color.MediumPurple, Color.DarkOrange, Color.Teal, Color.SlateBlue };

        for (var i = 0; i < items.Count; i++)
        {
            var y = 42 + i * rowHeight;
            var labelRect = new Rectangle(2, y, labelWidth - 8, rowHeight - 2);
            graphics.DrawString(items[i].Name, font, textBrush, labelRect, stringFormat);
            var barRect = new Rectangle(labelWidth, y + 5, chartWidth, Math.Max(8, rowHeight - 12));
            graphics.FillRectangle(backgroundBrush, barRect);
            var fillWidth = (int)(barRect.Width * items[i].Count / (double)maxCount);
            using var barBrush = new SolidBrush(colors[i % colors.Length]);
            graphics.FillRectangle(barBrush, new Rectangle(barRect.X, barRect.Y, fillWidth, barRect.Height));
            graphics.DrawString(items[i].Count.ToString(), boldFont, textBrush, labelWidth + chartWidth + 6, y + 2);
        }
    }

    private void DrawTableChart(Graphics graphics, Rectangle bounds)
    {
        graphics.Clear(FormUi.Surface);
        var slices = summary?.TableStatusCounts.Where(item => item.Count > 0).ToList() ?? new List<DashboardTableStatusCount>();
        if (slices.Count == 0)
        {
            DrawCenteredMessage(graphics, bounds, "Chưa có dữ liệu bàn");
            return;
        }

        var colors = new List<Color>();
        foreach (var slice in slices)
        {
            colors.Add(slice.Status.Contains("trống", StringComparison.OrdinalIgnoreCase) ? FormUi.Green :
                slice.Status.Contains("phục vụ", StringComparison.OrdinalIgnoreCase) ? FormUi.Red :
                slice.Status.Contains("đặt", StringComparison.OrdinalIgnoreCase) ? FormUi.Gold : FormUi.Blue);
        }

        var diameter = Math.Min(bounds.Height - 12, bounds.Width / 2);
        var pie = new Rectangle(bounds.Left + 8, bounds.Top + (bounds.Height - diameter) / 2, diameter, diameter);
        var total = slices.Sum(item => item.Count);
        var startAngle = -90f;
        for (var i = 0; i < slices.Count; i++)
        {
            var sweep = 360f * slices[i].Count / total;
            using var brush = new SolidBrush(colors[i]);
            graphics.FillPie(brush, pie, startAngle, sweep);
            startAngle += sweep;
        }
        var hole = new Rectangle(pie.X + diameter / 4, pie.Y + diameter / 4, diameter / 2, diameter / 2);
        using (var holeBrush = new SolidBrush(FormUi.Surface))
            graphics.FillEllipse(holeBrush, hole);
        using (var totalFont = new Font("Segoe UI", 13F, FontStyle.Bold))
        using (var centerBrush = new SolidBrush(FormUi.Ink))
        {
            var text = total.ToString();
            var size = graphics.MeasureString(text, totalFont);
            graphics.DrawString(text, totalFont, centerBrush, hole.X + (hole.Width - size.Width) / 2, hole.Y + (hole.Height - size.Height) / 2);
        }

        var legendX = pie.Right + 12;
        var legendY = Math.Max(8, (bounds.Height - slices.Count * 32) / 2);
        using var font = new Font("Segoe UI", 8.5F);
        using var textBrush = new SolidBrush(FormUi.Ink);
        for (var i = 0; i < slices.Count; i++)
        {
            using var brush = new SolidBrush(colors[i]);
            graphics.FillEllipse(brush, legendX, legendY + i * 32 + 4, 10, 10);
            graphics.DrawString($"{slices[i].Status}: {slices[i].Count}", font, textBrush, legendX + 16, legendY + i * 32);
        }
    }

    private static void DrawCenteredMessage(Graphics graphics, Rectangle bounds, string text)
    {
        using var font = new Font("Segoe UI", 9F);
        using var brush = new SolidBrush(FormUi.Muted);
        var size = graphics.MeasureString(text, font);
        graphics.DrawString(text, font, brush, bounds.Left + (bounds.Width - size.Width) / 2, bounds.Top + (bounds.Height - size.Height) / 2);
    }

    private void UpdateClock()
    {
        greetingLabel.Text = $"Xin chào, {Session.TenDangNhap}  -  Quyền: {Session.Quyen}";
        clockLabel.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy  HH:mm:ss");
    }

    private void ExportDailyReport()
    {
        if (summary is null)
        {
            MessageBox.Show("Chưa có dữ liệu để xuất báo cáo.", "Báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "CSV UTF-8 (*.csv)|*.csv",
            FileName = $"BaoCao_DOKKI_{DateTime.Now:yyyyMMdd}.csv",
            Title = "Xuất báo cáo ngày"
        };
        if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;

        var lines = new List<string>
        {
            "Bao cao DOKKI," + DateTime.Now.ToString("dd/MM/yyyy"),
            "Chi so,Gia tri"
        };
        if (canSeeRevenue)
            lines.Add($"Doanh thu hom nay,{summary.RevenueToday.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"Hoa don hom nay,{summary.PaidBillsToday}");
        lines.Add($"Ban dang phuc vu,{summary.ServingTables}/{summary.TotalTables}");
        lines.Add($"Mon dang ban,{summary.ActiveMenuItems}");
        lines.Add(string.Empty);
        lines.Add("Khach dat ban,So dien thoai,Gio den,So nguoi,Ban");
        lines.AddRange(GetUpcomingBookings().Select(item => string.Join(",", Csv(item.Guest), Csv(item.Phone), Csv(item.Time), Csv(item.Pax), Csv(item.Table))));
        File.WriteAllLines(dialog.FileName, lines, new UTF8Encoding(true));
        MessageBox.Show("Đã xuất báo cáo thành công.", "Báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static string Csv(string value) => "\"" + value.Replace("\"", "\"\"") + "\"";

    private sealed record KpiCard(Panel Panel, Label Title, Label Value, Label Detail);

    private sealed class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = FormUi.Surface;
        }
    }
}
