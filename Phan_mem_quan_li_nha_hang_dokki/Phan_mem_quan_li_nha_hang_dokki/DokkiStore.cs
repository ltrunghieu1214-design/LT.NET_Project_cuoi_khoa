namespace Phan_mem_quan_li_nha_hang_dokki;

public record FoodItem(string Code, string Name, string Category, decimal Price, string Unit, string Status);
public record CategoryItem(string Code, string Name, int Count, string Note, string Status);
public record TableItem(int Number, string Name, string Status, int Capacity, string Area);
public record BookingItem(string Code, string Time, string Guest, string Phone, string Pax, string Table, string Status);
public record OrderBill(string Code, string Time, string Table, string Customer, string Amount, string Status);

public class OrderDetailItem
{
    public string FoodCode { get; set; } = "";
    public string FoodName { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
}

public class ActiveTableOrder
{
    public int TableNumber { get; set; }
    public string CustomerName { get; set; } = "Khách vãng lai";
    public int AdultCount { get; set; } = 2; // Buffet người lớn 139.000 đ
    public int ChildCount { get; set; } = 0; // Buffet trẻ em 69.000 đ
    public List<OrderDetailItem> Details { get; } = new();

    public decimal BuffetTotal => (AdultCount * 139000m) + (ChildCount * 69000m);
    public decimal ExtraTotal => Details.Sum(d => d.TotalPrice);
    public decimal GrandTotal => BuffetTotal + ExtraTotal;
}

public static class DokkiStore
{
    public static event Action? DataChanged;

    public static List<FoodItem> Foods { get; } = new();
    public static List<CategoryItem> Categories { get; } = new();
    public static List<TableItem> Tables { get; } = new();
    public static List<BookingItem> Bookings { get; } = new();
    public static List<OrderBill> Bills { get; } = new();
    public static List<string> Activities { get; } = new();
    public static Dictionary<int, ActiveTableOrder> ActiveOrders { get; } = new();

    public static decimal TodayRevenue { get; set; } = 0m;

    // Trạng thái kết nối database
    public static bool IsDbConnected { get; private set; } = false;

    static DokkiStore()
    {
        LoadFromDatabase();
    }

    public static void NotifyChanged()
    {
        DataChanged?.Invoke();
    }

    public static ActiveTableOrder GetOrCreateOrder(int tableNumber)
    {
        if (!ActiveOrders.TryGetValue(tableNumber, out var order))
        {
            order = new ActiveTableOrder { TableNumber = tableNumber };
            ActiveOrders[tableNumber] = order;
        }
        return order;
    }

    // ================================================================
    //  LOAD DỮ LIỆU TỪ DATABASE
    // ================================================================
    private static void LoadFromDatabase()
    {
        var connError = DatabaseHelper.TestConnection();
        if (connError != null)
        {
            IsDbConnected = false;
            // Nếu không kết nối được → báo lỗi khi app load xong
            Application.Idle += ShowConnectionError;
            return;
        }

        IsDbConnected = true;

        try
        {
            // Tương thích với database cũ chưa có cột NgayThanhToan.
            DatabaseHelper.EnsureHoaDonDateColumn();

            // Load tất cả dữ liệu từ SQL Server
            Categories.AddRange(DatabaseHelper.LoadCategories());
            Foods.AddRange(DatabaseHelper.LoadFoods());
            Tables.AddRange(DatabaseHelper.LoadTables());
            Bookings.AddRange(DatabaseHelper.LoadBookings());
            Bills.AddRange(DatabaseHelper.LoadBills());
            Activities.AddRange(DatabaseHelper.LoadActivities());
            TodayRevenue = DatabaseHelper.GetTodayRevenue();

            // Tạo active orders cho các bàn đang phục vụ
            foreach (var t in Tables.Where(t => t.Status == "Đang phục vụ"))
            {
                var o = new ActiveTableOrder
                {
                    TableNumber = t.Number,
                    CustomerName = $"Khách bàn {t.Number:00}",
                    AdultCount = 3,
                    ChildCount = 1
                };
                o.Details.Add(new OrderDetailItem { FoodCode = "MA001", FoodName = "Ba chỉ bò Mỹ cuộn", UnitPrice = 89000, Quantity = 2 });
                o.Details.Add(new OrderDetailItem { FoodCode = "MA012", FoodName = "Viên phô mai trứng muối tan chảy", UnitPrice = 59000, Quantity = 1 });
                ActiveOrders[t.Number] = o;
            }
        }
        catch (Exception ex)
        {
            IsDbConnected = false;
            Application.Idle += (s, e) =>
            {
                Application.Idle -= (s2, e2) => { };
                MessageBox.Show(
                    $"Lỗi khi tải dữ liệu từ database:\n\n{ex.Message}\n\nVui lòng kiểm tra file DokkiDatabase.sql đã được chạy chưa.",
                    "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }
    }

    private static void ShowConnectionError(object? sender, EventArgs e)
    {
        Application.Idle -= ShowConnectionError;
        MessageBox.Show(
            "Không thể kết nối đến SQL Server!\n\n" +
            "Vui lòng kiểm tra:\n" +
            "  1. SQL Server đang chạy\n" +
            "  2. Database 'DokkiRestaurant' đã được tạo (chạy DokkiDatabase.sql)\n" +
            "  3. Connection string trong DatabaseHelper.cs\n\n" +
            "Ứng dụng sẽ chạy với dữ liệu trống.",
            "Lỗi kết nối Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    // ================================================================
    //  THÊM MÓN ĂN
    // ================================================================
    public static void AddFood(FoodItem food)
    {
        Foods.Insert(0, food);
        if (IsDbConnected)
        {
            DatabaseHelper.InsertFood(food);
            DatabaseHelper.InsertActivity($"[Món ăn] Thêm món mới: {food.Name} ({food.Code})");
        }
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Món ăn] Thêm món mới: {food.Name} ({food.Code})");
        NotifyChanged();
    }

    // ================================================================
    //  THÊM DANH MỤC
    // ================================================================
    public static void AddCategory(CategoryItem cat)
    {
        Categories.Insert(0, cat);
        if (IsDbConnected)
        {
            DatabaseHelper.InsertCategory(cat);
            DatabaseHelper.InsertActivity($"[Danh mục] Thêm danh mục mới: {cat.Name} ({cat.Code})");
        }
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Danh mục] Thêm danh mục mới: {cat.Name} ({cat.Code})");
        NotifyChanged();
    }

    // ================================================================
    //  THÊM BÀN ĂN
    // ================================================================
    public static void AddTable(TableItem table)
    {
        Tables.Add(table);
        if (IsDbConnected)
        {
            DatabaseHelper.InsertTable(table);
            DatabaseHelper.InsertActivity($"[Bàn ăn] Thêm {table.Name} ({table.Area})");
        }
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Bàn ăn] Thêm {table.Name} ({table.Area})");
        NotifyChanged();
    }

    // ================================================================
    //  THÊM ĐẶT BÀN
    // ================================================================
    public static void AddBooking(BookingItem booking)
    {
        Bookings.Insert(0, booking);
        var tableNum = int.TryParse(booking.Table.Replace("Bàn", "").Trim(), out var n) ? n : 0;
        if (tableNum > 0)
        {
            SetTableStatus(tableNum, "Đã đặt");
        }
        if (IsDbConnected)
        {
            DatabaseHelper.InsertBooking(booking);
            DatabaseHelper.InsertActivity($"[Đặt bàn] Tiếp nhận đặt bàn mới từ {booking.Guest} ({booking.Table})");
        }
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Đặt bàn] Tiếp nhận đặt bàn mới từ {booking.Guest} ({booking.Table})");
        NotifyChanged();
    }

    // ================================================================
    //  CẬP NHẬT TRẠNG THÁI BÀN
    // ================================================================
    public static void SetTableStatus(int tableNumber, string newStatus)
    {
        var idx = Tables.FindIndex(t => t.Number == tableNumber);
        if (idx >= 0)
        {
            var old = Tables[idx];
            Tables[idx] = old with { Status = newStatus };
            if (IsDbConnected)
                DatabaseHelper.UpdateTableStatus(tableNumber, newStatus);
            NotifyChanged();
        }
    }

    // ================================================================
    //  THANH TOÁN / CHECKOUT BÀN
    // ================================================================
    public static void CheckoutTable(int tableNumber, string customerName, decimal amount)
    {
        var billNumber = IsDbConnected ? DatabaseHelper.GetNextBillNumber() : 1087 + Bills.Count;
        var billCode = $"HD{billNumber}";
        var time = DateTime.Now.ToString("HH:mm");
        var bill = new OrderBill(billCode, time, $"Bàn {tableNumber:00}", customerName, $"{amount:N0} đ", "Đã thanh toán");

        Bills.Insert(0, bill);
        TodayRevenue += amount;
        ActiveOrders.Remove(tableNumber);
        SetTableStatus(tableNumber, "Trống");

        if (IsDbConnected)
        {
            DatabaseHelper.InsertBill(billCode, time, $"Bàn {tableNumber:00}", customerName, amount, "Đã thanh toán");
            DatabaseHelper.InsertActivity($"[Thanh toán] Bàn {tableNumber:00} hoàn tất thanh toán {amount:N0} đ ({billCode})");
        }
        Activities.Insert(0, $"  {time}   [Thanh toán] Bàn {tableNumber:00} hoàn tất thanh toán {amount:N0} đ ({billCode})");
        NotifyChanged();
    }
}
