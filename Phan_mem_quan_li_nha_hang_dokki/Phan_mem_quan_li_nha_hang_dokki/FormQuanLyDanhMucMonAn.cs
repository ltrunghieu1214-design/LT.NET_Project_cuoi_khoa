namespace Phan_mem_quan_li_nha_hang_dokki;

public partial class FormQuanLyDanhMucMonAn : Form
{
    private DataGridView gridCategories = null!;
    private DataGridView gridFoodsInCategory = null!;
    private TextBox search = null!;
    private Label lblSelectedCategoryTitle = null!;
    private string currentSelectedCategoryName = "";

    public FormQuanLyDanhMucMonAn()
    {
        InitializeComponent();
        FormUi.StyleForm(this, "DOKKI | Quản lý danh mục");
        WindowState = FormWindowState.Maximized;
        BuildView();
        DokkiStore.DataChanged += ReloadAllData;
        FormClosed += (_, _) => DokkiStore.DataChanged -= ReloadAllData;
    }

    private void BuildView()
    {
        var header = FormUi.Header("Danh mục món ăn", "Quản lý 30 nhóm thực đơn và xem chi tiết toàn bộ các món trong từng danh mục — Chi nhánh DOKKI EPU");
        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 8, 24, 20) };

        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, WrapContents = false };
        search = FormUi.SearchBox();
        search.Width = 340;
        search.PlaceholderText = "  🔍 Tìm tên danh mục hoặc mã...";
        search.TextChanged += (_, _) => ReloadCategories();

        var add = FormUi.Button("+ Thêm danh mục", FormUi.Red, 155);
        add.Click += (_, _) =>
        {
            using var f = new FormThemDanhMuc();
            f.ShowDialog(this);
        };

        var btnToggle = FormUi.Button("Đổi trạng thái", FormUi.Blue, 130);
        btnToggle.Click += (_, _) => ToggleSelectedCategoryStatus();

        var btnAddFoodToCat = FormUi.Button("+ Thêm món vào DM", FormUi.Green, 175);
        btnAddFoodToCat.Click += (_, _) =>
        {
            using var f = new FormThemMonAn();
            f.ShowDialog(this);
        };

        toolbar.Controls.Add(search);
        toolbar.Controls.Add(add);
        toolbar.Controls.Add(btnToggle);
        toolbar.Controls.Add(btnAddFoodToCat);

        // Bố cục Master - Detail 2 cột: Chia tỷ lệ tự động, không bao giờ phải kéo tay
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterWidth = 14,
            BackColor = Color.FromArgb(235, 237, 242),
            FixedPanel = FixedPanel.None,
            IsSplitterFixed = false
        };

        // Panel 1: Danh sách 30 Danh mục thực đơn
        var pnlCatContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(12),
            BorderStyle = BorderStyle.FixedSingle
        };
        var lblCatHeader = new Label
        {
            Text = "DANH SÁCH DANH MỤC THỰC ĐƠN (30 DANH MỤC) — Bấm vào để xem món",
            Dock = DockStyle.Top,
            Height = 30,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = FormUi.DarkRed
        };

        gridCategories = FormUi.Grid();
        gridCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động vừa khít 100% cột
        gridCategories.ScrollBars = ScrollBars.Vertical; // Không cần cuộn ngang hay kéo tay

        var colCode = new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Mã DM", FillWeight = 48, MinimumWidth = 65 };
        var colName = new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Tên danh mục", FillWeight = 140, MinimumWidth = 140 };
        var colCount = new DataGridViewTextBoxColumn { Name = "Count", HeaderText = "Số lượng", FillWeight = 55, MinimumWidth = 65 };
        var colNote = new DataGridViewTextBoxColumn { Name = "Note", HeaderText = "Ghi chú phân loại", FillWeight = 150, MinimumWidth = 140 };
        var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", FillWeight = 65, MinimumWidth = 75 };

        gridCategories.Columns.AddRange(colCode, colName, colCount, colNote, colStatus);
        gridCategories.SelectionChanged += (_, _) => OnCategorySelectionChanged();

        pnlCatContainer.Controls.Add(gridCategories);
        pnlCatContainer.Controls.Add(lblCatHeader);
        lblCatHeader.SendToBack();
        gridCategories.BringToFront();

        split.Panel1.Controls.Add(pnlCatContainer);

        // Panel 2: Toàn bộ món ăn thuộc danh mục đang chọn
        var pnlFoodContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(12),
            BorderStyle = BorderStyle.FixedSingle
        };
        lblSelectedCategoryTitle = new Label
        {
            Text = "CÁC MÓN ĂN THUỘC DANH MỤC",
            Dock = DockStyle.Top,
            Height = 30,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = FormUi.DarkRed
        };

        gridFoodsInCategory = FormUi.Grid();
        gridFoodsInCategory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động vừa khít 100% cột
        gridFoodsInCategory.ScrollBars = ScrollBars.Vertical; // Không cần cuộn ngang hay kéo tay

        var colFCode = new DataGridViewTextBoxColumn { Name = "FoodCode", HeaderText = "Mã món", FillWeight = 50, MinimumWidth = 65 };
        var colFName = new DataGridViewTextBoxColumn { Name = "FoodName", HeaderText = "Tên món ăn", FillWeight = 160, MinimumWidth = 150 };
        var colFPrice = new DataGridViewTextBoxColumn { Name = "FoodPrice", HeaderText = "Giá bán", FillWeight = 75, MinimumWidth = 80 };
        var colFUnit = new DataGridViewTextBoxColumn { Name = "FoodUnit", HeaderText = "ĐVT", FillWeight = 58, MinimumWidth = 65 };
        var colFStatus = new DataGridViewTextBoxColumn { Name = "FoodStatus", HeaderText = "Trạng thái", FillWeight = 65, MinimumWidth = 75 };

        gridFoodsInCategory.Columns.AddRange(colFCode, colFName, colFPrice, colFUnit, colFStatus);

        pnlFoodContainer.Controls.Add(gridFoodsInCategory);
        pnlFoodContainer.Controls.Add(lblSelectedCategoryTitle);
        lblSelectedCategoryTitle.SendToBack();
        gridFoodsInCategory.BringToFront();

        split.Panel2.Controls.Add(pnlFoodContainer);

        // Tự động căn chỉnh tỷ lệ 55% cho Danh mục và 45% cho Món ăn theo kích thước màn hình
        split.SizeChanged += (_, _) =>
        {
            if (split.Width > 600)
            {
                split.SplitterDistance = (int)(split.Width * 0.55);
            }
        };

        // QUY TẮC DOCKING CHUẨN: Toolbar ở đáy để chiếm mép trên, Split ở đỉnh để lấp đầy phần còn lại
        body.Controls.Add(split);
        body.Controls.Add(toolbar);
        toolbar.Dock = DockStyle.Top;
        split.Dock = DockStyle.Fill;
        toolbar.SendToBack();
        split.BringToFront();

        Controls.Add(body);
        Controls.Add(header);
        header.Dock = DockStyle.Top;
        body.Dock = DockStyle.Fill;
        header.SendToBack();
        body.BringToFront();

        ReloadAllData();
    }

    private void ReloadAllData()
    {
        ReloadCategories();
        ReloadFoodsOfSelectedCategory();
    }

    private void ReloadCategories()
    {
        gridCategories.Rows.Clear();
        var query = search.Text.Trim().ToLower();

        var filtered = DokkiStore.Categories.Where(c =>
            string.IsNullOrEmpty(query) || c.Name.ToLower().Contains(query) || c.Code.ToLower().Contains(query)
        );

        foreach (var c in filtered)
        {
            var count = DokkiStore.Foods.Count(f =>
                f.Category == c.Name ||
                c.Name.Contains(f.Category, StringComparison.OrdinalIgnoreCase) ||
                f.Category.Contains(c.Name, StringComparison.OrdinalIgnoreCase)
            );
            var displayCount = count > 0 ? $"{count} món" : $"{c.Count} món";
            gridCategories.Rows.Add(c.Code, c.Name, displayCount, c.Note, c.Status);
        }

        if (gridCategories.Rows.Count > 0 && string.IsNullOrEmpty(currentSelectedCategoryName))
        {
            gridCategories.Rows[0].Selected = true;
            currentSelectedCategoryName = gridCategories.Rows[0].Cells["Name"].Value?.ToString() ?? "";
            ReloadFoodsOfSelectedCategory();
        }
    }

    private void OnCategorySelectionChanged()
    {
        if (gridCategories.SelectedRows.Count == 0) return;
        currentSelectedCategoryName = gridCategories.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "";
        ReloadFoodsOfSelectedCategory();
    }

    private void ReloadFoodsOfSelectedCategory()
    {
        gridFoodsInCategory.Rows.Clear();
        if (string.IsNullOrEmpty(currentSelectedCategoryName))
        {
            lblSelectedCategoryTitle.Text = "HÃY CHỌN MỘT DANH MỤC ĐỂ XEM MÓN";
            return;
        }

        var matchingFoods = DokkiStore.Foods.Where(f =>
            f.Category == currentSelectedCategoryName ||
            currentSelectedCategoryName.Contains(f.Category, StringComparison.OrdinalIgnoreCase) ||
            f.Category.Contains(currentSelectedCategoryName, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        lblSelectedCategoryTitle.Text = $"DANH MỤC: {currentSelectedCategoryName.ToUpper()} ({matchingFoods.Count} MÓN)";

        foreach (var food in matchingFoods)
        {
            gridFoodsInCategory.Rows.Add(food.Code, food.Name, $"{food.Price:N0} đ", food.Unit, food.Status);
        }
    }

    private void ToggleSelectedCategoryStatus()
    {
        if (gridCategories.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn một danh mục trên bảng để đổi trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var code = gridCategories.SelectedRows[0].Cells["Code"].Value?.ToString();
        var idx = DokkiStore.Categories.FindIndex(c => c.Code == code);
        if (idx >= 0)
        {
            var old = DokkiStore.Categories[idx];
            var newStatus = old.Status == "Đang phục vụ" ? "Tạm ẩn" : "Đang phục vụ";
            DokkiStore.Categories[idx] = old with { Status = newStatus };
            DokkiStore.NotifyChanged();
        }
    }
}
