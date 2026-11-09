namespace Phan_mem_quan_li_nha_hang_dokki;

public partial class FormQuanLyMonAn : Form
{
    private DataGridView grid = null!;
    private TextBox search = null!;
    private ComboBox cbCategory = null!;

    public FormQuanLyMonAn()
    {
        InitializeComponent();
        FormUi.StyleForm(this, "DOKKI | Quản lý món ăn");
        WindowState = FormWindowState.Maximized;
        BuildView();
        DokkiStore.DataChanged += ReloadGrid;
        FormClosed += (_, _) => DokkiStore.DataChanged -= ReloadGrid;
    }

    private void BuildView()
    {
        var header = FormUi.Header("Thực đơn món ăn", "Quản lý 35 món ăn toàn hệ thống kinh doanh — Chi nhánh DOKKI EPU");
        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(28, 8, 28, 20) };

        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, WrapContents = false };
        search = FormUi.SearchBox();
        search.Width = 320;
        search.PlaceholderText = "  🔍 Tìm tên hoặc mã món ăn...";
        search.TextChanged += (_, _) => ReloadGrid();

        cbCategory = new ComboBox { Width = 260, Height = 36, DropDownStyle = ComboBoxStyle.DropDownList, Font = FormUi.BodyFont, Margin = new Padding(4) };
        cbCategory.Items.Add("Tất cả danh mục");
        foreach (var cat in DokkiStore.Categories)
        {
            cbCategory.Items.Add(cat.Name);
        }
        cbCategory.SelectedIndex = 0;
        cbCategory.SelectedIndexChanged += (_, _) => ReloadGrid();

        var add = FormUi.Button("+ Thêm món mới", FormUi.Red, 145);
        add.Click += (_, _) =>
        {
            using var f = new FormThemMonAn();
            f.ShowDialog(this);
        };

        var btnToggle = FormUi.Button("Đổi trạng thái", FormUi.Blue, 130);
        btnToggle.Click += (_, _) => ToggleSelectedFoodStatus();

        toolbar.Controls.Add(search);
        toolbar.Controls.Add(cbCategory);
        toolbar.Controls.Add(add);
        toolbar.Controls.Add(btnToggle);

        grid = FormUi.Grid();
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.ScrollBars = ScrollBars.Vertical; // Tự động vừa khít 100% chiều ngang, không cần kéo tay

        var colCode = new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Mã món", FillWeight = 50, MinimumWidth = 75 };
        var colName = new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Tên món ăn", FillWeight = 160, MinimumWidth = 180 };
        var colCategory = new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Danh mục", FillWeight = 110, MinimumWidth = 130 };
        var colPrice = new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Giá bán", FillWeight = 70, MinimumWidth = 90 };
        var colUnit = new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "Đơn vị tính", FillWeight = 60, MinimumWidth = 80 };
        var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", FillWeight = 65, MinimumWidth = 85 };

        grid.Columns.AddRange(colCode, colName, colCategory, colPrice, colUnit, colStatus);

        // QUY TẮC DOCKING CHUẨN: Toolbar ở đáy để chiếm mép trên, Grid ở đỉnh để lấp đầy phần còn lại
        body.Controls.Add(grid);
        body.Controls.Add(toolbar);
        toolbar.Dock = DockStyle.Top;
        grid.Dock = DockStyle.Fill;
        toolbar.SendToBack();
        grid.BringToFront();

        Controls.Add(body);
        Controls.Add(header);
        header.Dock = DockStyle.Top;
        body.Dock = DockStyle.Fill;
        header.SendToBack();
        body.BringToFront();

        ReloadGrid();
    }

    private void ReloadGrid()
    {
        grid.Rows.Clear();
        var query = search.Text.Trim().ToLower();
        var selectedCat = cbCategory.SelectedItem?.ToString() ?? "Tất cả danh mục";

        var filtered = DokkiStore.Foods.Where(f =>
            (selectedCat == "Tất cả danh mục" || f.Category == selectedCat) &&
            (string.IsNullOrEmpty(query) || f.Name.ToLower().Contains(query) || f.Code.ToLower().Contains(query))
        );

        foreach (var f in filtered)
        {
            grid.Rows.Add(f.Code, f.Name, f.Category, $"{f.Price:N0} đ", f.Unit, f.Status);
        }
    }

    private void ToggleSelectedFoodStatus()
    {
        if (grid.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn một món ăn trên bảng để đổi trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var code = grid.SelectedRows[0].Cells["Code"].Value?.ToString();
        var idx = DokkiStore.Foods.FindIndex(f => f.Code == code);
        if (idx >= 0)
        {
            var old = DokkiStore.Foods[idx];
            var newStatus = old.Status == "Đang bán" ? "Tạm hết" : "Đang bán";
            DokkiStore.Foods[idx] = old with { Status = newStatus };
            DokkiStore.NotifyChanged();
        }
    }
}
