namespace Phan_mem_quan_li_nha_hang_dokki;

public class FormOrderMon : Form
{
    private ComboBox cbTableSelect = null!;
    private TextBox txtCustomerName = null!;
    private NumericUpDown nudAdult = null!;
    private NumericUpDown nudChild = null!;
    private FlowLayoutPanel pnlFoodMenu = null!;
    private DataGridView gridOrderDetails = null!;
    private Label lblBuffetSummary = null!;
    private Label lblGrandTotal = null!;
    private TextBox txtSearchFood = null!;

    private int currentTableNumber = 1;
    private ActiveTableOrder currentOrder;
    private string selectedCategoryFilter = "Tất cả";

    public FormOrderMon(int initialTableNumber = 1)
    {
        currentTableNumber = initialTableNumber;
        currentOrder = DokkiStore.GetOrCreateOrder(currentTableNumber);

        FormUi.StyleForm(this, "DOKKI | Hệ thống Gọi món Trực tuyến & Order POS");
        Size = new Size(1350, 820);
        WindowState = FormWindowState.Maximized;

        // Header thông tin bàn & khách
        var pnlTop = BuildTopBar();

        // Thân giao diện chia 2 cột: Thực đơn (Trái) & Hóa đơn Order (Phải)
        const int orderPanelWidth = 580; // Cố định chiều rộng chuẩn POS, không bao giờ phải kéo tay

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterWidth = 8,
            BackColor = Color.FromArgb(230, 232, 238),
            FixedPanel = FixedPanel.Panel2,
            IsSplitterFixed = true // Khóa cố định thanh ngăn cách, ngăn việc vô tình kéo xô lệch
        };

        void AdjustOrderSplitter()
        {
            if (split.Width < 700) return;
            try
            {
                int desiredDistance = split.Width - orderPanelWidth - split.SplitterWidth;
                if (desiredDistance > 200 && desiredDistance < split.Width - 200)
                {
                    split.SplitterDistance = desiredDistance;
                }
            }
            catch
            {
                // Bỏ qua nếu WinForms chưa hoàn tất đo đạc kích thước
            }
        }

        split.SizeChanged += (_, _) => AdjustOrderSplitter();
        Shown += (_, _) => AdjustOrderSplitter();

        // Panel trái: Thực đơn món ăn
        var pnlLeftContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = FormUi.Background,
            Padding = new Padding(16, 8, 8, 16)
        };
        var pnlMenuHeader = BuildMenuHeader();
        pnlFoodMenu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.White,
            Padding = new Padding(14),
            Margin = Padding.Empty
        };

        pnlLeftContainer.Controls.Add(pnlFoodMenu);
        pnlLeftContainer.Controls.Add(pnlMenuHeader);
        pnlMenuHeader.Dock = DockStyle.Top;
        pnlFoodMenu.Dock = DockStyle.Fill;
        pnlMenuHeader.SendToBack();
        pnlFoodMenu.BringToFront();

        split.Panel1.Controls.Add(pnlLeftContainer);

        // Panel phải: Hóa đơn order cố định vừa khít toàn bộ trang
        var pnlRightContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(12),
            BorderStyle = BorderStyle.FixedSingle
        };
        var pnlOrderHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 46,
            BackColor = FormUi.DarkRed,
            Padding = new Padding(14, 11, 14, 11)
        };
        var lblOrderTitle = FormUi.Label("CHI TIẾT ORDER BÀN", 11.5F, FontStyle.Bold, Color.White);
        pnlOrderHeader.Controls.Add(lblOrderTitle);

        gridOrderDetails = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeColumns = false, // Cố định 100%, không cho kéo lệch cột
            AllowUserToResizeRows = false,
            AllowUserToOrderColumns = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            RowHeadersVisible = false,
            EnableHeadersVisualStyles = false,
            ScrollBars = ScrollBars.Vertical, // Vừa khít ngang, chỉ cuộn dọc
            Font = FormUi.BodyFont
        };
        gridOrderDetails.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(240, 242, 245),
            ForeColor = FormUi.Ink,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Padding = new Padding(4)
        };
        gridOrderDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridOrderDetails.ColumnHeadersHeight = 36;
        gridOrderDetails.RowTemplate.Height = 36;

        gridOrderDetails.DefaultCellStyle = new DataGridViewCellStyle
        {
            Padding = new Padding(4),
            SelectionBackColor = Color.FromArgb(255, 235, 214),
            SelectionForeColor = FormUi.Ink
        };

        // Cấu hình các cột cố định chuẩn trang:
        var colCode = new DataGridViewTextBoxColumn
        {
            Name = "Code",
            HeaderText = "Mã",
            Visible = false
        };

        // Tên món tự động chiếm trọn phần chiều rộng còn lại của bảng
        var colName = new DataGridViewTextBoxColumn
        {
            Name = "Name",
            HeaderText = "Tên món",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            MinimumWidth = 140
        };

        // Đơn giá: Cố định 85px, căn phải
        var colPrice = new DataGridViewTextBoxColumn
        {
            Name = "Price",
            HeaderText = "Đơn giá",
            Width = 85,
            Resizable = DataGridViewTriState.False
        };
        colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colPrice.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

        // Số lượng: Cố định 42px, căn giữa
        var colQty = new DataGridViewTextBoxColumn
        {
            Name = "Qty",
            HeaderText = "SL",
            Width = 42,
            Resizable = DataGridViewTriState.False
        };
        colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colQty.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        // Thành tiền: Cố định 98px, căn phải
        var colTotal = new DataGridViewTextBoxColumn
        {
            Name = "Total",
            HeaderText = "Thành tiền",
            Width = 98,
            Resizable = DataGridViewTriState.False
        };
        colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colTotal.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

        // Nút thao tác tăng giảm trên DataGridView
        var colDec = new DataGridViewButtonColumn
        {
            Name = "-",
            HeaderText = "-",
            Text = "-",
            UseColumnTextForButtonValue = true,
            Width = 32,
            Resizable = DataGridViewTriState.False
        };
        var colInc = new DataGridViewButtonColumn
        {
            Name = "+",
            HeaderText = "+",
            Text = "+",
            UseColumnTextForButtonValue = true,
            Width = 32,
            Resizable = DataGridViewTriState.False
        };
        var colDel = new DataGridViewButtonColumn
        {
            Name = "Xóa",
            HeaderText = "Xóa",
            Text = "✕",
            UseColumnTextForButtonValue = true,
            Width = 40,
            Resizable = DataGridViewTriState.False
        };

        gridOrderDetails.Columns.AddRange(colCode, colName, colPrice, colQty, colTotal, colDec, colInc, colDel);

        gridOrderDetails.CellContentClick += GridOrderDetails_CellContentClick;

        // Panel tổng tiền và nút hành động
        var pnlOrderSummary = BuildOrderSummary();

        pnlRightContainer.Controls.Add(gridOrderDetails);
        pnlRightContainer.Controls.Add(pnlOrderSummary);
        pnlRightContainer.Controls.Add(pnlOrderHeader);
        pnlOrderHeader.Dock = DockStyle.Top;
        pnlOrderSummary.Dock = DockStyle.Bottom;
        gridOrderDetails.Dock = DockStyle.Fill;
        pnlOrderHeader.SendToBack();
        pnlOrderSummary.SendToBack();
        gridOrderDetails.BringToFront();

        split.Panel2.Controls.Add(pnlRightContainer);

        // QUY TẮC DOCKING CHUẨN: Top ở đáy để chiếm mép trên, Split ở đỉnh để lấp đầy phần còn lại
        Controls.Add(split);
        Controls.Add(pnlTop);
        pnlTop.Dock = DockStyle.Top;
        split.Dock = DockStyle.Fill;
        pnlTop.SendToBack();
        split.BringToFront();

        // Tải dữ liệu ban đầu
        LoadFoodCards();
        SyncOrderUI();
    }

    private Panel BuildTopBar()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 66, BackColor = FormUi.DarkRed, Padding = new Padding(18, 12, 18, 12) };
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = false };

        var lblBrand = FormUi.Label("DOKKI POS ORDER", 13.5F, FontStyle.Bold, Color.White);
        lblBrand.Margin = new Padding(0, 6, 20, 0);

        var lblTbl = FormUi.Label("Bàn:", 10F, FontStyle.Bold, Color.FromArgb(255, 230, 200));
        lblTbl.Margin = new Padding(0, 8, 4, 0);

        cbTableSelect = new ComboBox { Width = 175, Height = 34, DropDownStyle = ComboBoxStyle.DropDownList, Font = FormUi.BodyFont, DropDownWidth = 260 };
        foreach (var tbl in DokkiStore.Tables)
        {
            cbTableSelect.Items.Add($"{tbl.Name} ({tbl.Status})");
        }
        cbTableSelect.SelectedIndex = Math.Clamp(currentTableNumber - 1, 0, cbTableSelect.Items.Count - 1);
        cbTableSelect.SelectedIndexChanged += (_, _) =>
        {
            currentTableNumber = cbTableSelect.SelectedIndex + 1;
            currentOrder = DokkiStore.GetOrCreateOrder(currentTableNumber);
            SyncOrderUI();
        };

        var lblCust = FormUi.Label("Khách:", 10F, FontStyle.Bold, Color.FromArgb(255, 230, 200));
        lblCust.Margin = new Padding(12, 8, 4, 0);

        txtCustomerName = new TextBox { Width = 145, Height = 32, Font = FormUi.BodyFont, Text = currentOrder.CustomerName };
        txtCustomerName.TextChanged += (_, _) => currentOrder.CustomerName = txtCustomerName.Text;

        var lblAdult = FormUi.Label("NL lớn (139K):", 10F, FontStyle.Bold, Color.FromArgb(255, 230, 200));
        lblAdult.Margin = new Padding(12, 8, 4, 0);

        nudAdult = new NumericUpDown { Width = 70, Height = 32, Minimum = 1, Maximum = 50, Value = currentOrder.AdultCount, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), TextAlign = HorizontalAlignment.Center };
        nudAdult.ValueChanged += (_, _) =>
        {
            currentOrder.AdultCount = (int)nudAdult.Value;
            UpdateTotals();
        };

        var lblChild = FormUi.Label("Trẻ em (69K):", 10F, FontStyle.Bold, Color.FromArgb(255, 230, 200));
        lblChild.Margin = new Padding(12, 8, 4, 0);

        nudChild = new NumericUpDown { Width = 70, Height = 32, Minimum = 0, Maximum = 50, Value = currentOrder.ChildCount, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), TextAlign = HorizontalAlignment.Center };
        nudChild.ValueChanged += (_, _) =>
        {
            currentOrder.ChildCount = (int)nudChild.Value;
            UpdateTotals();
        };

        layout.Controls.Add(lblBrand);
        layout.Controls.Add(lblTbl);
        layout.Controls.Add(cbTableSelect);
        layout.Controls.Add(lblCust);
        layout.Controls.Add(txtCustomerName);
        layout.Controls.Add(lblAdult);
        layout.Controls.Add(nudAdult);
        layout.Controls.Add(lblChild);
        layout.Controls.Add(nudChild);

        top.Controls.Add(layout);
        return top;
    }

    // Header thực đơn: Bố trí Bánh gạo Tokbokki ngay cạnh Tất cả, chữ to rõ ràng, không bị trôi
    private Panel BuildMenuHeader()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 145, // Chiều cao 145px rộng rãi
            BackColor = Color.White,
            Padding = new Padding(14, 10, 14, 8),
            Margin = new Padding(0, 0, 0, 10)
        };

        var topRow = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, WrapContents = false };
        txtSearchFood = FormUi.SearchBox();
        txtSearchFood.Width = 380;
        txtSearchFood.PlaceholderText = "  🔍 Tìm món ăn (tokbokki, ba chỉ bò Mỹ, gà cay, mì...)...";
        txtSearchFood.TextChanged += (_, _) => LoadFoodCards();
        topRow.Controls.Add(txtSearchFood);

        var catRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 88, // 88px chứa 2 hàng nút thoáng mắt
            WrapContents = true,
            AutoScroll = false,
            Padding = new Padding(0, 4, 0, 0)
        };

        // Danh sách danh mục sắp xếp hợp lý: Bánh gạo Tokbokki ngay vị trí số 2
        var categoryList = new (string DisplayName, string FilterTarget)[]
        {
            ("Tất cả", "Tất cả"),
            ("Bánh gạo Tokbokki", "Bánh gạo"),
            ("Thịt nhúng lẩu", "Thịt"),
            ("Hải sản nhúng", "Hải sản"),
            ("Topping viên thả", "Topping"),
            ("Gà rán Hàn Quốc", "Gà rán"),
            ("Món chiên giòn", "Món chiên"),
            ("Rau sạch & Nấm", "Rau"),
            ("Mì & Miến Hàn", "Mì"),
            ("Đồ uống", "Đồ uống"),
            ("Tráng miệng", "Tráng miệng")
        };

        foreach (var item in categoryList)
        {
            var btn = new Button
            {
                Text = item.DisplayName,
                Tag = item.FilterTarget,
                Height = 36,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), // Chữ to rõ ràng
                BackColor = item.FilterTarget == selectedCategoryFilter ? FormUi.Red : Color.FromArgb(238, 240, 244),
                ForeColor = item.FilterTarget == selectedCategoryFilter ? Color.White : FormUi.Ink,
                Cursor = Cursors.Hand,
                Padding = new Padding(12, 4, 12, 4),
                Margin = new Padding(0, 0, 8, 8)
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Click += (_, _) =>
            {
                selectedCategoryFilter = (string)btn.Tag!;
                foreach (Control c in catRow.Controls)
                {
                    if (c is Button b)
                    {
                        var isSel = (string)b.Tag! == selectedCategoryFilter;
                        b.BackColor = isSel ? FormUi.Red : Color.FromArgb(238, 240, 244);
                        b.ForeColor = isSel ? Color.White : FormUi.Ink;
                    }
                }
                LoadFoodCards();
            };
            catRow.Controls.Add(btn);
        }

        panel.Controls.Add(topRow);
        panel.Controls.Add(catRow);
        return panel;
    }

    private void LoadFoodCards()
    {
        pnlFoodMenu.SuspendLayout();
        pnlFoodMenu.Controls.Clear();

        var query = txtSearchFood.Text.Trim().ToLower();
        var filtered = DokkiStore.Foods.Where(f =>
            (selectedCategoryFilter == "Tất cả" || f.Category.Contains(selectedCategoryFilter, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(query) || f.Name.ToLower().Contains(query) || f.Code.ToLower().Contains(query))
        );

        foreach (var food in filtered)
        {
            var card = BuildFoodCard(food);
            pnlFoodMenu.Controls.Add(card);
        }

        pnlFoodMenu.ResumeLayout();
    }

    private Panel BuildFoodCard(FoodItem food)
    {
        var card = new Panel
        {
            Width = 236,
            Height = 130,
            BackColor = Color.FromArgb(250, 251, 254),
            Margin = new Padding(6),
            Padding = new Padding(12),
            BorderStyle = BorderStyle.FixedSingle,
            Cursor = Cursors.Hand
        };

        var lblName = new Label
        {
            Text = food.Name,
            Dock = DockStyle.Top,
            Height = 42,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = FormUi.Ink
        };

        var lblUnit = new Label
        {
            Text = $"{food.Category} • {food.Unit}",
            Dock = DockStyle.Top,
            Height = 22,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
            ForeColor = FormUi.Muted
        };

        var bottom = new Panel { Dock = DockStyle.Bottom, Height = 34 };
        var lblPrice = new Label
        {
            Text = $"{food.Price:N0} đ",
            Dock = DockStyle.Left,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = FormUi.Red,
            TextAlign = ContentAlignment.MiddleLeft
        };

        var btnAdd = new Button
        {
            Text = "+ Thêm",
            Dock = DockStyle.Right,
            Width = 80,
            Height = 32,
            FlatStyle = FlatStyle.Flat,
            BackColor = FormUi.Green,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.Click += (_, _) => AddFoodToOrder(food);

        bottom.Controls.Add(lblPrice);
        bottom.Controls.Add(btnAdd);

        card.Controls.Add(bottom);
        card.Controls.Add(lblUnit);
        card.Controls.Add(lblName);

        card.Click += (_, _) => AddFoodToOrder(food);
        lblName.Click += (_, _) => AddFoodToOrder(food);
        lblPrice.Click += (_, _) => AddFoodToOrder(food);

        return card;
    }

    private void AddFoodToOrder(FoodItem food)
    {
        var existing = currentOrder.Details.FirstOrDefault(d => d.FoodCode == food.Code);
        if (existing is not null)
        {
            existing.Quantity++;
        }
        else
        {
            currentOrder.Details.Add(new OrderDetailItem
            {
                FoodCode = food.Code,
                FoodName = food.Name,
                UnitPrice = food.Price,
                Quantity = 1
            });
        }
        RefreshGrid();
        UpdateTotals();
    }

    private void GridOrderDetails_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= currentOrder.Details.Count) return;

        var detail = currentOrder.Details[e.RowIndex];
        var colName = gridOrderDetails.Columns[e.ColumnIndex].HeaderText;

        if (colName == "-")
        {
            detail.Quantity--;
            if (detail.Quantity <= 0)
            {
                currentOrder.Details.RemoveAt(e.RowIndex);
            }
        }
        else if (colName == "+")
        {
            detail.Quantity++;
        }
        else if (colName == "Xóa")
        {
            currentOrder.Details.RemoveAt(e.RowIndex);
        }

        RefreshGrid();
        UpdateTotals();
    }

    private void RefreshGrid()
    {
        gridOrderDetails.Rows.Clear();
        foreach (var item in currentOrder.Details)
        {
            gridOrderDetails.Rows.Add(
                item.FoodCode,
                item.FoodName,
                $"{item.UnitPrice:N0} đ",
                item.Quantity,
                $"{item.TotalPrice:N0} đ"
            );
        }
    }

    private Panel BuildOrderSummary()
    {
        var panel = new Panel { Dock = DockStyle.Bottom, Height = 175, BackColor = Color.FromArgb(248, 249, 252), Padding = new Padding(14, 10, 14, 10), BorderStyle = BorderStyle.FixedSingle };

        lblBuffetSummary = new Label
        {
            Text = "Buffet: 2 NL (278.000 đ) + 0 TE (0 đ)",
            Dock = DockStyle.Top,
            Height = 22,
            Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            ForeColor = FormUi.Ink
        };

        var pnlTotal = new Panel { Dock = DockStyle.Top, Height = 38 };
        var lblTotalTitle = new Label { Text = "TỔNG CỘNG:", Dock = DockStyle.Left, Font = new Font("Segoe UI", 11.5F, FontStyle.Bold), ForeColor = FormUi.Ink, TextAlign = ContentAlignment.MiddleLeft };
        lblGrandTotal = new Label { Text = "278.000 đ", Dock = DockStyle.Right, Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = FormUi.Red, TextAlign = ContentAlignment.MiddleRight };
        pnlTotal.Controls.Add(lblTotalTitle);
        pnlTotal.Controls.Add(lblGrandTotal);

        // Nút hành động bố trí dạng TableLayoutPanel 1 dòng 2 cột (mỗi bên 50%) để vừa khít mọi kích cỡ màn hình
        var pnlActions = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 48,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        pnlActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        pnlActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        pnlActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var btnSendKitchen = new Button
        {
            Text = "✓ Gửi Bếp / Lưu",
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Flat,
            BackColor = FormUi.Blue,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 4, 0)
        };
        btnSendKitchen.FlatAppearance.BorderSize = 0;
        btnSendKitchen.Click += (_, _) =>
        {
            DokkiStore.SetTableStatus(currentTableNumber, "Đang phục vụ");
            MessageBox.Show($"Đã lưu & gửi order Bàn {currentTableNumber:00} xuống bộ phận bếp thành công!", "DOKKI POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        var btnCheckout = new Button
        {
            Text = "★ Thanh toán & Trả",
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Flat,
            BackColor = FormUi.Red,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(4, 0, 0, 0)
        };
        btnCheckout.FlatAppearance.BorderSize = 0;
        btnCheckout.Click += (_, _) => CheckoutCurrentTable();

        pnlActions.Controls.Add(btnSendKitchen, 0, 0);
        pnlActions.Controls.Add(btnCheckout, 1, 0);

        panel.Controls.Add(pnlTotal);
        panel.Controls.Add(lblBuffetSummary);
        panel.Controls.Add(pnlActions);
        return panel;
    }

    private void SyncOrderUI()
    {
        txtCustomerName.Text = currentOrder.CustomerName;
        nudAdult.Value = Math.Clamp(currentOrder.AdultCount, nudAdult.Minimum, nudAdult.Maximum);
        nudChild.Value = Math.Clamp(currentOrder.ChildCount, nudChild.Minimum, nudChild.Maximum);
        RefreshGrid();
        UpdateTotals();
    }

    private void UpdateTotals()
    {
        lblBuffetSummary.Text = $"Buffet: {currentOrder.AdultCount} NL ({currentOrder.AdultCount * 139000m:N0} đ) + {currentOrder.ChildCount} TE ({currentOrder.ChildCount * 69000m:N0} đ)  •  Món thêm: {currentOrder.ExtraTotal:N0} đ";
        lblGrandTotal.Text = $"{currentOrder.GrandTotal:N0} đ";
    }

    private void CheckoutCurrentTable()
    {
        var total = currentOrder.GrandTotal;
        if (total <= 0)
        {
            MessageBox.Show("Bàn hiện chưa có món nào trong order!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"XÁC NHẬN THANH TOÁN BÀN {currentTableNumber:00}\n" +
            $"Khách hàng: {currentOrder.CustomerName}\n" +
            $"Tổng tiền cần thanh toán: {total:N0} VNĐ\n\n" +
            "Bạn có muốn in hóa đơn và hoàn tất trả bàn?",
            "Xác nhận thanh toán DOKKI",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (confirm == DialogResult.Yes)
        {
            DokkiStore.CheckoutTable(currentTableNumber, currentOrder.CustomerName, total);
            MessageBox.Show(
                $"Đã thanh toán thành công {total:N0} VNĐ!\nBàn {currentTableNumber:00} đã được chuyển về trạng thái 'Trống'.",
                "Thanh toán hoàn tất",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            currentOrder = DokkiStore.GetOrCreateOrder(currentTableNumber);
            SyncOrderUI();
        }
    }
}
