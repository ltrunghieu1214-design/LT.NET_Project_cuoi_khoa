namespace Phan_mem_quan_li_nha_hang_dokki;

public sealed class FormMain : Form
{
    private readonly Panel content;
    private readonly bool isAdmin;
    private readonly bool canSeeRevenue;
    public bool LogoutRequested { get; private set; }

    public FormMain()
    {
        FormUi.StyleForm(this, "DOKKI | Hệ thống quản lý nhà hàng");
        WindowState = FormWindowState.Maximized;

        isAdmin = string.Equals(Session.Quyen?.Trim(), "Admin", StringComparison.OrdinalIgnoreCase);
        canSeeRevenue = isAdmin || string.Equals(Session.Quyen?.Trim(), "Quản lý", StringComparison.OrdinalIgnoreCase);
        var navigation = FormUi.Navigation(
            "HOME",
            Navigate,
            isAdmin,
            Session.TenDangNhap ?? string.Empty,
            Session.Quyen ?? string.Empty,
            RequestLogout);
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
        if (target == "HOME")
        {
            ShowManagementHome();
            return;
        }

        Form? form = target switch
        {
            "TABLE" => new FormQuanLyBanAnVaDatBan(),
            "FOOD" => new FormQuanLyMonAn(),
            "CATEGORY" => new FormQuanLyDanhMucMonAn(),
            "STAFF" when isAdmin => new FrmNhanVien(),
            "ACCOUNTS" when isAdmin => new FrmTaiKhoan(),
            _ => null
        };
        if (form is null) return;
        using (form) form.ShowDialog(this);
    }

    private void ShowManagementHome()
    {
        foreach (Control control in content.Controls.Cast<Control>().ToArray())
            control.Dispose();
        content.Controls.Clear();
        var dashboard = new DashboardView(
            canSeeRevenue,
            () => Navigate("TABLE"),
            () =>
            {
                using var form = new FormThemDatBan();
                form.ShowDialog(this);
            });
        content.Controls.Add(dashboard);
    }

    private void RequestLogout()
    {
        LogoutRequested = true;
        Close();
    }
}
