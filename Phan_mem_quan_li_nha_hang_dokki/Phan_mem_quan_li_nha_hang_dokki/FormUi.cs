using System.Drawing.Drawing2D;

namespace Phan_mem_quan_li_nha_hang_dokki;

internal static class FormUi
{
    public static readonly Color Red = Color.FromArgb(202, 32, 38);
    public static readonly Color DarkRed = Color.FromArgb(126, 20, 25);
    public static readonly Color Gold = Color.FromArgb(244, 181, 35);
    public static readonly Color BrightGold = Color.FromArgb(255, 215, 0);
    public static readonly Color Green = Color.FromArgb(46, 155, 92);
    public static readonly Color Blue = Color.FromArgb(51, 122, 183);
    public static readonly Color Ink = Color.FromArgb(38, 38, 42);
    public static readonly Color Muted = Color.FromArgb(105, 105, 112);
    public static readonly Color Surface = Color.White;
    public static readonly Color Background = Color.FromArgb(245, 246, 250);
    public static readonly Color BorderColor = Color.FromArgb(228, 230, 235);
    public static readonly Font TitleFont = new("Segoe UI", 19, FontStyle.Bold);
    public static readonly Font SubtitleFont = new("Segoe UI", 10.5F, FontStyle.Regular);
    public static readonly Font BodyFont = new("Segoe UI", 10F);
    public static readonly Font BoldFont = new("Segoe UI", 10F, FontStyle.Bold);

    public static void StyleForm(Form form, string title)
    {
        form.Text = title;
        form.BackColor = Background;
        form.ForeColor = Ink;
        form.Font = BodyFont;
        form.MinimumSize = new Size(1100, 700);
        form.StartPosition = FormStartPosition.CenterScreen;
        form.AutoScaleMode = AutoScaleMode.Dpi;
    }

    public static Label Label(string text, float size = 10F, FontStyle style = FontStyle.Regular, Color? color = null)
    {
        return new Label { Text = text, AutoSize = true, Font = new Font("Segoe UI", size, style), ForeColor = color ?? Ink };
    }

    public static Button Button(string text, Color color, int width = 120)
    {
        var button = new Button
        {
            Text = text,
            Width = width,
            Height = 38,
            FlatStyle = FlatStyle.Flat,
            BackColor = color,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(4),
            UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    public static TextBox SearchBox()
    {
        return new TextBox
        {
            Height = 36,
            Width = 300,
            Font = new Font("Segoe UI", 10.5F),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(4)
        };
    }

    public static DataGridView Grid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Surface,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            EnableHeadersVisualStyles = false,
            Font = BodyFont,
            GridColor = Color.FromArgb(235, 235, 238)
        };
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = DarkRed,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Padding = new Padding(8),
            SelectionBackColor = DarkRed
        };
        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Ink,
            SelectionBackColor = Color.FromArgb(255, 235, 214),
            SelectionForeColor = Ink,
            Padding = new Padding(8)
        };
        grid.RowTemplate.Height = 42;
        return grid;
    }

    // Card thống kê không bao giờ đè chữ: dùng TableLayoutPanel chia 2 dòng riêng biệt
    public static Panel Card(string caption, string value, Color accent)
    {
        var card = new Panel
        {
            BackColor = Surface,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 14, 0),
            Padding = new Padding(18, 12, 14, 12)
        };
        card.Paint += (_, e) =>
        {
            using var brush = new SolidBrush(accent);
            e.Graphics.FillRectangle(brush, 0, 0, 6, card.Height);
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var captionLabel = new Label
        {
            Text = caption.ToUpperInvariant(),
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = Padding.Empty
        };
        var valueLabel = new Label
        {
            Text = value,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 19F, FontStyle.Bold),
            ForeColor = Ink,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = Padding.Empty
        };

        layout.Controls.Add(captionLabel, 0, 0);
        layout.Controls.Add(valueLabel, 0, 1);
        card.Controls.Add(layout);
        return card;
    }

    // Header chuẩn không bao giờ đè chữ: dùng TableLayoutPanel chia 2 dòng riêng biệt
    public static Panel Header(string title, string subtitle)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 88,
            BackColor = Background,
            Padding = new Padding(28, 14, 28, 10)
        };
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var titleLabel = new Label
        {
            Text = title,
            Dock = DockStyle.Fill,
            Font = TitleFont,
            ForeColor = Ink,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = Padding.Empty
        };
        var subtitleLabel = new Label
        {
            Text = subtitle,
            Dock = DockStyle.Fill,
            Font = SubtitleFont,
            ForeColor = Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = Padding.Empty
        };

        layout.Controls.Add(titleLabel, 0, 0);
        layout.Controls.Add(subtitleLabel, 0, 1);
        panel.Controls.Add(layout);
        return panel;
    }

    public static Panel Navigation(string active, Action<string> navigate)
    {
        var nav = new Panel
        {
            Dock = DockStyle.Left,
            Width = 260, // Tăng độ rộng để thoải mái không gian
            BackColor = DarkRed,
            Padding = new Padding(14, 16, 14, 16)
        };

        // Khu vực Logo & Thương hiệu riêng biệt, chữ to rõ ràng không thể che khuất
        var pnlBrand = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = new Padding(6, 6, 6, 6)
        };

        var brandLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        brandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var brand = new Label
        {
            Text = "DOKKI",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 28F, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = Padding.Empty
        };

        var branch = new Label
        {
            Text = "CHI NHÁNH DOKKI EPU",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
            ForeColor = BrightGold, // Màu vàng hoàng kim nổi bật
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = Padding.Empty
        };

        brandLayout.Controls.Add(brand, 0, 0);
        brandLayout.Controls.Add(branch, 0, 1);
        pnlBrand.Controls.Add(brandLayout);

        // Danh sách các nút menu
        var pnlMenu = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 16, 0, 0)
        };

        var items = new[]
        {
            ("Tổng quan", "HOME"),
            ("Gọi món / POS", "ORDER"),
            ("Bàn & đặt bàn", "TABLE"),
            ("Món ăn", "FOOD"),
            ("Danh mục", "CATEGORY")
        };

        var y = 10;
        foreach (var item in items)
        {
            var isCurrent = item.Item2 == active;
            var button = new Button
            {
                Text = "  " + item.Item1,
                Tag = item.Item2,
                Location = new Point(0, y),
                Width = 232,
                Height = 46,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = isCurrent ? Red : DarkRed,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += (_, _) => navigate((string)button.Tag!);
            pnlMenu.Controls.Add(button);
            y += 52;
        }

        var user = Label("●  Quản trị viên / Thu ngân\n    Chi nhánh DOKKI EPU", 9.5F, FontStyle.Regular, Color.White);
        user.Location = new Point(14, nav.Height - 65);
        user.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

        nav.Controls.Add(pnlMenu);
        nav.Controls.Add(pnlBrand);
        nav.Controls.Add(user);

        pnlBrand.SendToBack();
        pnlMenu.BringToFront();

        nav.Resize += (_, _) => user.Location = new Point(14, nav.Height - 65);

        return nav;
    }
}
