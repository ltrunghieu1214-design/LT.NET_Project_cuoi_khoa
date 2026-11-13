namespace Phan_mem_quan_li_nha_hang_dokki;

public sealed class FormMain : Form
{
    private readonly Panel content;

    public FormMain()
    {
        FormUi.StyleForm(this, "DOKKI | Hệ thống quản lý nhà hàng");
        WindowState = FormWindowState.Maximized;

        var navigation = FormUi.Navigation("FOOD", Navigate);
        content = new Panel { Dock = DockStyle.Fill, BackColor = FormUi.Background, Margin = Padding.Empty };

        var shell = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = FormUi.Background,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        shell.Controls.Add(navigation, 0, 0);
        shell.Controls.Add(content, 1, 0);
        navigation.Dock = DockStyle.Fill;
        content.Dock = DockStyle.Fill;
        Controls.Add(shell);

        ShowManagementHome();
    }

    private void Navigate(string target)
    {
        Form? form = target switch
        {
            "TABLE" => new FormQuanLyBanAnVaDatBan(),
            "FOOD" => new FormQuanLyMonAn(),
            "CATEGORY" => new FormQuanLyDanhMucMonAn(),
            _ => null
        };
        if (form is null) return;
        using (form) form.ShowDialog(this);
    }

    private void ShowManagementHome()
    {
        content.Controls.Clear();

        var header = FormUi.Header("Quản lý nhà hàng", "Chọn chức năng quản lý ở menu bên trái");
        var body = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = FormUi.Background,
            Padding = new Padding(28, 20, 28, 28),
            Margin = Padding.Empty
        };

        var overview = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            BackColor = FormUi.Background,
            Margin = new Padding(0, 0, 0, 14),
            Padding = Padding.Empty
        };
        for (var column = 0; column < overview.ColumnCount; column++)
        {
            overview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }
        overview.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        overview.Controls.Add(CreateOverviewCard("Món ăn", DokkiStore.Foods.Count.ToString(), FormUi.Red), 0, 0);
        overview.Controls.Add(CreateOverviewCard("Danh mục", DokkiStore.Categories.Count.ToString(), FormUi.Gold), 1, 0);
        overview.Controls.Add(CreateOverviewCard("Bàn ăn", DokkiStore.Tables.Count.ToString(), FormUi.Green), 2, 0);
        overview.Controls.Add(CreateOverviewCard("Lịch đặt bàn", DokkiStore.Bookings.Count.ToString(), FormUi.Blue), 3, 0);

        var message = FormUi.Label(
            "Các chức năng hiện có: quản lý món ăn, danh mục, bàn ăn và đặt bàn.",
            12,
            FontStyle.Regular,
            FormUi.Muted);
        message.Dock = DockStyle.Fill;
        message.TextAlign = ContentAlignment.TopLeft;
        message.Padding = new Padding(0, 18, 0, 0);

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
        bodyLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
        bodyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        bodyLayout.Controls.Add(overview, 0, 0);
        bodyLayout.Controls.Add(message, 0, 1);
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
        content.Controls.Add(pageLayout);
    }

    private static Panel CreateOverviewCard(string title, string value, Color accent)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = FormUi.Surface,
            Margin = new Padding(0, 0, 12, 0),
            Padding = new Padding(18)
        };
        card.Paint += (_, e) =>
        {
            using var brush = new SolidBrush(accent);
            e.Graphics.FillRectangle(brush, 0, 0, 6, card.Height);
        };

        var titleLabel = FormUi.Label(title.ToUpperInvariant(), 9F, FontStyle.Bold, FormUi.Muted);
        titleLabel.Dock = DockStyle.Top;
        titleLabel.Height = 28;

        var valueLabel = FormUi.Label(value, 24F, FontStyle.Bold, FormUi.Ink);
        valueLabel.Dock = DockStyle.Fill;

        card.Controls.Add(valueLabel);
        card.Controls.Add(titleLabel);
        return card;
    }
}
