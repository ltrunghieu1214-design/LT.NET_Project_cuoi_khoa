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

    public static decimal TodayRevenue { get; set; } = 28650000m;

    static DokkiStore()
    {
        SeedData();
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

    private static void SeedData()
    {
        // 30 Danh mục thực đơn DOKKI chuẩn
        Categories.AddRange(new[]
        {
            new CategoryItem("DM001", "Bánh gạo & Tokbokki", 12, "Bánh gạo phô mai, bánh gạo khoai môn, truyền thống", "Đang phục vụ"),
            new CategoryItem("DM002", "Thịt bò & Heo nhúng lẩu", 18, "Ba chỉ bò Mỹ, bắp bò Úc, ba chỉ heo cuộn sốt", "Đang phục vụ"),
            new CategoryItem("DM003", "Hải sản nhúng lẩu", 14, "Tôm sú tươi, mực hoa, bạch tuộc baby", "Đang phục vụ"),
            new CategoryItem("DM004", "Topping & Viên nhúng lẩu", 22, "Chả cá Busan, viên trứng muối, xúc xích phô mai", "Đang phục vụ"),
            new CategoryItem("DM005", "Gà rán sốt Hàn Quốc", 8, "Gà sốt Yangnyeom, gà sốt mật ong, gà cay", "Đang phục vụ"),
            new CategoryItem("DM006", "Món chiên & Khai vị giòn", 16, "Gimari rong biển, mandu chiên, khoai tây lắc", "Đang phục vụ"),
            new CategoryItem("DM007", "Kimbap & Cơm trộn quầy line", 10, "Kimbap chiên xù, cơm trộn rong biển cá ngừ", "Đang phục vụ"),
            new CategoryItem("DM008", "Rau sạch & Nấm tươi", 15, "Cải thảo, nấm kim châm, nấm đùi gà", "Đang phục vụ"),
            new CategoryItem("DM009", "Mì gói & Miến khoai lang", 9, "Sari Ramyeon, miến dẹt Ottogi", "Đang phục vụ"),
            new CategoryItem("DM010", "Nước sốt lẩu Dokki", 7, "Sốt Dokki, Ddukmo, Daegu, Gungjung, Rose", "Đang phục vụ"),
            new CategoryItem("DM011", "Đồ uống & Trà đóng chai", 14, "Nước trà ngô Dongsuh, nước gạo rang Morning Rice", "Đang phục vụ"),
            new CategoryItem("DM012", "Kem tươi & Tráng miệng", 6, "Kem vani sốt socola, kem matcha, chè đậu đỏ", "Đang phục vụ"),
            new CategoryItem("DM013", "Buffet Người lớn 139K", 1, "Vé buffet tiêu chuẩn 1 người lớn ăn không giới hạn", "Đang phục vụ"),
            new CategoryItem("DM014", "Buffet Trẻ em 69K", 1, "Trẻ em cao từ 1m đến 1m3", "Đang phục vụ"),
            new CategoryItem("DM015", "Vòng phô mai bắp kéo sợi", 4, "Vòng phô mai Mozzarella hoa hồng viền nồi", "Đang phục vụ"),
            new CategoryItem("DM016", "Panchan & Kimchi chuẩn vị", 8, "Kimchi cải thảo cay, củ cải muối vàng", "Đang phục vụ"),
            new CategoryItem("DM017", "Nước ngọt Dispenser tươi", 5, "Coca-Cola, Sprite, Fanta cam, Soda chanh", "Đang phục vụ"),
            new CategoryItem("DM018", "Gia vị & Nước chấm quầy bar", 11, "Dầu mè thơm, tỏi phi giòn, ớt băm, mè rang", "Đang phục vụ"),
            new CategoryItem("DM019", "Mì lạnh Hàn Quốc Naengmyeon", 4, "Mì lạnh nước dùng thanh mát, mì trộn cay", "Đang phục vụ"),
            new CategoryItem("DM020", "Set đóng gói mang về Takeaway", 5, "Combo tokbokki 2-4 người tự nấu tại nhà", "Đang phục vụ"),
            new CategoryItem("DM021", "Thịt nướng đá sốt Galbi", 8, "Ba chỉ nướng đá, dẻ sườn ướp sốt", "Đang phục vụ"),
            new CategoryItem("DM022", "Bánh cá đậu đỏ Bungeoppang", 4, "Bánh cá nhân kem custard, đậu đỏ mềm", "Đang phục vụ"),
            new CategoryItem("DM023", "Hải sản cao cấp Gọi thêm", 6, "Bào ngư mini, sò điệp Nhật, tôm càng xanh", "Đang phục vụ"),
            new CategoryItem("DM024", "Bánh xèo Hàn Quốc Pajeon", 5, "Bánh xèo hải sản hành lá, bánh xèo kimchi", "Đang phục vụ"),
            new CategoryItem("DM025", "Canh rong biển & Súp chả cá", 4, "Canh rong biển thịt bò Miyeokguk nóng hổi", "Đang phục vụ"),
            new CategoryItem("DM026", "Salad & Trái cây tươi", 7, "Salad mè rang, dưa hấu, ổi tráng miệng", "Đang phục vụ"),
            new CategoryItem("DM027", "Trà sữa & Đá xay Hàn Quốc", 6, "Trà sữa trân châu đường đen, đá xay matcha", "Đang phục vụ"),
            new CategoryItem("DM028", "Thực đơn Trẻ em Dokki Kids", 5, "Cơm nắm phô mai mini, xúc xích bạch tuộc", "Đang phục vụ"),
            new CategoryItem("DM029", "Menu Đặc biệt theo mùa Summer", 6, "Bingsu xoài tuyết, bingsu dưa lưới", "Tạm ẩn"),
            new CategoryItem("DM030", "Gia vị đóng chai Dokki Mart", 8, "Chai sốt lẩu 350ml, bánh gạo tươi đóng túi", "Đang phục vụ")
        });

        // 35 Món ăn mẫu bao phủ các danh mục chính
        Foods.AddRange(new[]
        {
            new FoodItem("MA001", "Ba chỉ bò Mỹ cuộn", "Thịt bò & Heo nhúng lẩu", 89000m, "Đĩa 200g", "Đang bán"),
            new FoodItem("MA002", "Bắp bò Úc thượng hạng", "Thịt bò & Heo nhúng lẩu", 99000m, "Đĩa 180g", "Đang bán"),
            new FoodItem("MA003", "Ba chỉ heo rút sườn sốt BBQ", "Thịt bò & Heo nhúng lẩu", 69000m, "Đĩa 200g", "Đang bán"),
            new FoodItem("MA004", "Tôm sú tươi lột vỏ", "Hải sản nhúng lẩu", 109000m, "Đĩa 6 con", "Đang bán"),
            new FoodItem("MA005", "Mực nang cắt hoa tươi", "Hải sản nhúng lẩu", 95000m, "Đĩa 200g", "Đang bán"),
            new FoodItem("MA006", "Bạch tuộc baby sốt cay", "Hải sản nhúng lẩu", 99000m, "Đĩa 200g", "Đang bán"),
            new FoodItem("MA007", "Tokbokki phô mai bắp", "Bánh gạo & Tokbokki", 59000m, "Phần", "Đang bán"),
            new FoodItem("MA008", "Tokbokki khoai lang tím", "Bánh gạo & Tokbokki", 49000m, "Phần", "Đang bán"),
            new FoodItem("MA009", "Tokbokki hoa hồng sốt Rose", "Bánh gạo & Tokbokki", 55000m, "Phần", "Đang bán"),
            new FoodItem("MA010", "Tokbokki sợi dài truyền thống", "Bánh gạo & Tokbokki", 39000m, "Phần", "Đang bán"),
            new FoodItem("MA011", "Chả cá xiên que Busan Eomuk", "Topping & Viên nhúng lẩu", 45000m, "3 xiên", "Đang bán"),
            new FoodItem("MA012", "Viên phô mai trứng muối tan chảy", "Topping & Viên nhúng lẩu", 59000m, "Đĩa 6 viên", "Đang bán"),
            new FoodItem("MA013", "Viên tôm hùm phô mai Mozzarella", "Topping & Viên nhúng lẩu", 62000m, "Đĩa 6 viên", "Đang bán"),
            new FoodItem("MA014", "Xúc xích xông khói Dokki", "Topping & Viên nhúng lẩu", 49000m, "Đĩa 5 cây", "Đang bán"),
            new FoodItem("MA015", "Thanh cua Surimi tuyết", "Topping & Viên nhúng lẩu", 42000m, "Đĩa 6 thanh", "Đang bán"),
            new FoodItem("MA016", "Gà sốt cay ngọt Yangnyeom", "Gà rán sốt Hàn Quốc", 79000m, "Phần 4 miếng", "Đang bán"),
            new FoodItem("MA017", "Gà sốt xì dầu tỏi mật ong", "Gà rán sốt Hàn Quốc", 79000m, "Phần 4 miếng", "Đang bán"),
            new FoodItem("MA018", "Rong biển cuộn miến Gimari", "Món chiên & Khai vị giòn", 39000m, "Đĩa 5 cuộn", "Đang bán"),
            new FoodItem("MA019", "Mandu chiên giòn nhân thịt", "Món chiên & Khai vị giòn", 45000m, "Đĩa 5 cái", "Đang bán"),
            new FoodItem("MA020", "Khoai tây chiên bơ tỏi lắc phô mai", "Món chiên & Khai vị giòn", 35000m, "Phần", "Đang bán"),
            new FoodItem("MA021", "Kimbap chiên xù xốt cay Mayo", "Kimbap & Cơm trộn quầy line", 49000m, "Đĩa 8 miếng", "Đang bán"),
            new FoodItem("MA022", "Nấm kim châm tươi Hàn Quốc", "Rau sạch & Nấm tươi", 25000m, "Đĩa 150g", "Đang bán"),
            new FoodItem("MA023", "Nấm đùi gà baby thái lát", "Rau sạch & Nấm tươi", 29000m, "Đĩa 150g", "Đang bán"),
            new FoodItem("MA024", "Cải thảo thảo mộc Đà Lạt", "Rau sạch & Nấm tươi", 19000m, "Đĩa 200g", "Đang bán"),
            new FoodItem("MA025", "Mì khô không vị Sari Ramyeon", "Mì gói & Miến khoai lang", 18000m, "Gói 110g", "Đang bán"),
            new FoodItem("MA026", "Miến dẹt tươi khoai lang Ottogi", "Mì gói & Miến khoai lang", 32000m, "Đĩa 150g", "Đang bán"),
            new FoodItem("MA027", "Nước trà ngô rang Dongsuh", "Đồ uống & Trà đóng chai", 29000m, "Chai 500ml", "Đang bán"),
            new FoodItem("MA028", "Nước gạo rang Morning Rice", "Đồ uống & Trà đóng chai", 32000m, "Chai 500ml", "Đang bán"),
            new FoodItem("MA029", "Trà đào sả hạt chia tươi", "Đồ uống & Trà đóng chai", 35000m, "Ly 700ml", "Tạm hết"),
            new FoodItem("MA030", "Kem tươi vani Dokki rưới sốt socola", "Kem tươi & Tráng miệng", 25000m, "Cốc", "Đang bán"),
            new FoodItem("MA031", "Vòng phô mai ngô ngọt kéo sợi", "Vòng phô mai bắp kéo sợi", 69000m, "Vòng", "Đang bán"),
            new FoodItem("MA032", "Kimchi cải thảo chuẩn vị Hàn", "Panchan & Kimchi chuẩn vị", 20000m, "Đĩa 100g", "Đang bán"),
            new FoodItem("MA033", "Củ cải vàng muối chua ngọt", "Panchan & Kimchi chuẩn vị", 15000m, "Đĩa 100g", "Đang bán"),
            new FoodItem("MA034", "Bánh cá kem custard nóng", "Bánh cá đậu đỏ Bungeoppang", 35000m, "2 con", "Đang bán"),
            new FoodItem("MA035", "Mì lạnh Naengmyeon giải nhiệt", "Mì lạnh Hàn Quốc Naengmyeon", 59000m, "Bát to", "Đang bán")
        });

        // 28 Bàn ăn
        for (var i = 1; i <= 28; i++)
        {
            var status = (i % 3 == 0) ? "Đang phục vụ" : (i == 5 || i == 12 || i == 18 || i == 23 || i == 27 ? "Đã đặt" : "Trống");
            Tables.Add(new TableItem(i, $"BÀN {i:00}", status, 4, i <= 14 ? "Khu vực Tầng 1" : "Khu vực Tầng 2"));
            if (status == "Đang phục vụ")
            {
                var o = new ActiveTableOrder { TableNumber = i, CustomerName = $"Khách bàn {i:00}", AdultCount = 3, ChildCount = 1 };
                o.Details.Add(new OrderDetailItem { FoodCode = "MA001", FoodName = "Ba chỉ bò Mỹ cuộn", UnitPrice = 89000, Quantity = 2 });
                o.Details.Add(new OrderDetailItem { FoodCode = "MA012", FoodName = "Viên phô mai trứng muối tan chảy", UnitPrice = 59000, Quantity = 1 });
                ActiveOrders[i] = o;
            }
        }

        // 25 Đặt bàn mẫu
        Bookings.AddRange(new[]
        {
            new BookingItem("DB001", "11:30", "Nguyễn Minh Tuấn", "0912 345 678", "4 người", "Bàn 02", "Đã nhận bàn"),
            new BookingItem("DB002", "11:45", "Trần Thị Mai Anh", "0988 765 432", "2 người", "Bàn 07", "Đã nhận bàn"),
            new BookingItem("DB003", "12:00", "Lê Hoàng Long", "0903 112 233", "6 người", "Bàn 15", "Đã nhận bàn"),
            new BookingItem("DB004", "12:15", "Phạm Hải Đăng", "0977 445 566", "4 người", "Bàn 04", "Đã nhận bàn"),
            new BookingItem("DB005", "12:30", "Vũ Thùy Dương", "0934 998 877", "8 người", "Bàn 08", "Đã nhận bàn"),
            new BookingItem("DB006", "13:00", "Hoàng Kim Ngân", "0915 223 344", "2 người", "Bàn 11", "Đã nhận bàn"),
            new BookingItem("DB007", "13:15", "Đặng Quốc Huy", "0966 332 211", "3 người", "Bàn 19", "Đã nhận bàn"),
            new BookingItem("DB008", "17:30", "Bùi Thanh Hương", "0909 554 433", "4 người", "Bàn 21", "Đang phục vụ"),
            new BookingItem("DB009", "18:00", "Ngô Thành Đạt", "0982 123 789", "5 người", "Bàn 25", "Đang phục vụ"),
            new BookingItem("DB010", "18:30", "Nguyễn Thu Trang", "0945 678 123", "4 người", "Bàn 12", "Đã xác nhận"),
            new BookingItem("DB011", "18:45", "Trịnh Thảo Ly", "0918 234 567", "6 người", "Bàn 05", "Đã xác nhận"),
            new BookingItem("DB012", "19:00", "Vũ Đức Anh", "0976 543 210", "4 người", "Bàn 18", "Đã xác nhận"),
            new BookingItem("DB013", "19:15", "Lâm Mỹ Duyên", "0932 876 543", "2 người", "Bàn 23", "Đã xác nhận"),
            new BookingItem("DB014", "19:30", "Đỗ Khánh Linh", "0904 321 987", "8 người", "Bàn 27", "Đã xác nhận"),
            new BookingItem("DB015", "19:30", "Cao Văn Hùng", "0989 654 321", "4 người", "Bàn 01", "Chờ xác nhận"),
            new BookingItem("DB016", "19:45", "Phan Gia Khiêm", "0913 789 456", "3 người", "Bàn 03", "Chờ xác nhận"),
            new BookingItem("DB017", "20:00", "Dương Bảo Ngọc", "0967 890 123", "6 người", "Bàn 06", "Chờ xác nhận"),
            new BookingItem("DB018", "20:00", "Tạ Quang Dũng", "0938 456 789", "4 người", "Bàn 09", "Chờ xác nhận"),
            new BookingItem("DB019", "20:15", "Mai Phương Thảo", "0971 234 890", "2 người", "Bàn 10", "Chờ xác nhận"),
            new BookingItem("DB020", "20:30", "Hà Trọng Nhân", "0908 901 234", "5 người", "Bàn 13", "Chờ xác nhận"),
            new BookingItem("DB021", "20:30", "Nguyễn Thị Yến", "0943 567 890", "4 người", "Bàn 14", "Chờ xác nhận"),
            new BookingItem("DB022", "20:45", "Lê Tuấn Kiệt", "0985 678 901", "2 người", "Bàn 16", "Chờ xác nhận"),
            new BookingItem("DB023", "21:00", "Trần Diệu Hoa", "0917 890 123", "4 người", "Bàn 17", "Chờ xác nhận"),
            new BookingItem("DB024", "21:00", "Đoàn Minh Trí", "0935 012 345", "6 người", "Bàn 20", "Chờ xác nhận"),
            new BookingItem("DB025", "21:15", "Quách Ánh Nguyệt", "0979 123 456", "3 người", "Bàn 22", "Chờ xác nhận")
        });

        // 25 Hóa đơn hoàn tất
        Bills.AddRange(new[]
        {
            new OrderBill("HD1086", "13:45", "Bàn 08", "Nguyễn Minh Anh", "680.000 đ", "Đã thanh toán"),
            new OrderBill("HD1085", "13:30", "Bàn 15", "Trần Hoàng Nam", "1.240.000 đ", "Đã thanh toán"),
            new OrderBill("HD1084", "13:22", "Bàn 03", "Lê Ngọc Hà", "420.000 đ", "Đã thanh toán"),
            new OrderBill("HD1083", "13:10", "Bàn 21", "Phạm Gia Huy", "920.000 đ", "Đã thanh toán"),
            new OrderBill("HD1082", "12:55", "Bàn 07", "Vũ Mai Phương", "780.000 đ", "Đã thanh toán"),
            new OrderBill("HD1081", "12:48", "Bàn 12", "Hoàng Tuấn Kiệt", "1.560.000 đ", "Đã thanh toán"),
            new OrderBill("HD1080", "12:40", "Bàn 04", "Đặng Thùy Dung", "550.000 đ", "Đã thanh toán"),
            new OrderBill("HD1079", "12:35", "Bàn 18", "Bùi Quốc Khánh", "890.000 đ", "Đã thanh toán"),
            new OrderBill("HD1078", "12:28", "Bàn 02", "Trương Mỹ Linh", "630.000 đ", "Đã thanh toán"),
            new OrderBill("HD1077", "12:15", "Bàn 09", "Đỗ Hải Đăng", "1.120.000 đ", "Đã thanh toán"),
            new OrderBill("HD1076", "12:05", "Bàn 14", "Nguyễn Kim Oanh", "470.000 đ", "Đã thanh toán"),
            new OrderBill("HD1075", "11:58", "Bàn 22", "Phan Nhật Minh", "740.000 đ", "Đã thanh toán"),
            new OrderBill("HD1074", "11:50", "Bàn 06", "Vương Thục Quyên", "980.000 đ", "Đã thanh toán"),
            new OrderBill("HD1073", "11:42", "Bàn 11", "Dương Trung Hiếu", "590.000 đ", "Đã thanh toán"),
            new OrderBill("HD1072", "11:35", "Bàn 17", "Lý Khánh Ngọc", "1.340.000 đ", "Đã thanh toán"),
            new OrderBill("HD1071", "11:25", "Bàn 01", "Hồ Thành Luân", "820.000 đ", "Đã thanh toán"),
            new OrderBill("HD1070", "11:18", "Bàn 19", "Tạ Bích Vân", "660.000 đ", "Đã thanh toán"),
            new OrderBill("HD1069", "11:10", "Bàn 25", "Mai Văn Đức", "1.450.000 đ", "Đã thanh toán"),
            new OrderBill("HD1068", "10:55", "Bàn 10", "Cao Thu Thủy", "510.000 đ", "Đã thanh toán"),
            new OrderBill("HD1067", "10:45", "Bàn 05", "Đoàn Tiến Dũng", "730.000 đ", "Đã thanh toán"),
            new OrderBill("HD1066", "10:30", "Bàn 16", "Nguyễn Diệu Nhi", "880.000 đ", "Đã thanh toán"),
            new OrderBill("HD1065", "10:15", "Bàn 13", "Lâm Bá Phúc", "610.000 đ", "Đã thanh toán"),
            new OrderBill("HD1064", "10:00", "Bàn 20", "Võ Thị Hoài Thu", "1.180.000 đ", "Đã thanh toán"),
            new OrderBill("HD1063", "09:45", "Bàn 24", "Trần Đình Khang", "450.000 đ", "Đã thanh toán"),
            new OrderBill("HD1062", "09:30", "Bàn 23", "Bùi Ngọc Ánh", "790.000 đ", "Đã thanh toán")
        });

        // 20 Hoạt động gần đây
        Activities.AddRange(new[]
        {
            "  14:05   [Bàn 08] Yêu cầu gọi thêm ba chỉ bò Mỹ (x2)",
            "  13:50   [Bàn 15] Thanh toán hóa đơn HD1085: 1.240.000 đ",
            "  13:35   [Kho] Cảnh báo: Sắp hết Ba chỉ bò Mỹ (còn 4kg)",
            "  13:20   [Bàn 04] Khách đặt trước DB004 đã nhận bàn",
            "  13:02   [Bàn 12] Đổi bàn sang Bàn 18 theo yêu cầu khách",
            "  12:45   [Bàn 21] Gọi thanh toán qua chuyển khoản QR",
            "  12:30   [Quầy bar] Bổ sung thêm 20 xiên chả cá Busan",
            "  12:15   [Đặt bàn] Tiếp nhận đặt bàn mới từ Đỗ Khánh Linh (DB014)",
            "  12:00   [Bếp] Đã chuẩn bị xong 10 suất Tokbokki sốt Rose",
            "  11:45   [Bàn 07] Yêu cầu tiếp thêm nước dùng sốt Dokki cay",
            "  11:30   [Bàn 02] Khách đoàn 4 người check-in thành công",
            "  11:15   [Kho] Nhập kho thành công lô hàng Tôm sú tươi (15kg)",
            "  11:00   [Kho] Cảnh báo: Trà đào sả hạt chia tạm thời hết hàng",
            "  10:45   [Đặt bàn] Xác nhận lịch hẹn DB012 cho khách Vũ Đức Anh",
            "  10:30   [Thu ngân] Bắt đầu ca trưa - Kiểm tra két tiền đầu ca",
            "  10:15   [Quầy line] Kiểm tra vệ sinh an toàn thực phẩm hoàn tất",
            "  10:00   [Bếp] Hoàn thành nấu sốt lẩu Ddukmo & Gungjung",
            "  09:45   [Quản lý] Họp giao ban đầu ngày ca sáng chi nhánh",
            "  09:30   [Đặt bàn] Nhận đặt trước 5 lượt cho khung giờ tối",
            "  09:00   [Hệ thống] Mở ca hoạt động ngày 24/06/2024"
        });
    }

    public static void AddFood(FoodItem food)
    {
        Foods.Insert(0, food);
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Món ăn] Thêm món mới: {food.Name} ({food.Code})");
        NotifyChanged();
    }

    public static void AddCategory(CategoryItem cat)
    {
        Categories.Insert(0, cat);
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Danh mục] Thêm danh mục mới: {cat.Name} ({cat.Code})");
        NotifyChanged();
    }

    public static void AddTable(TableItem table)
    {
        Tables.Add(table);
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Bàn ăn] Thêm {table.Name} ({table.Area})");
        NotifyChanged();
    }

    public static void AddBooking(BookingItem booking)
    {
        Bookings.Insert(0, booking);
        var tableNum = int.TryParse(booking.Table.Replace("Bàn", "").Trim(), out var n) ? n : 0;
        if (tableNum > 0)
        {
            SetTableStatus(tableNum, "Đã đặt");
        }
        Activities.Insert(0, $"  {DateTime.Now:HH:mm}   [Đặt bàn] Tiếp nhận đặt bàn mới từ {booking.Guest} ({booking.Table})");
        NotifyChanged();
    }

    public static void SetTableStatus(int tableNumber, string newStatus)
    {
        var idx = Tables.FindIndex(t => t.Number == tableNumber);
        if (idx >= 0)
        {
            var old = Tables[idx];
            Tables[idx] = old with { Status = newStatus };
            NotifyChanged();
        }
    }

    public static void CheckoutTable(int tableNumber, string customerName, decimal amount)
    {
        var billCode = $"HD{1087 + Bills.Count}";
        var time = DateTime.Now.ToString("HH:mm");
        var bill = new OrderBill(billCode, time, $"Bàn {tableNumber:00}", customerName, $"{amount:N0} đ", "Đã thanh toán");
        Bills.Insert(0, bill);
        TodayRevenue += amount;
        ActiveOrders.Remove(tableNumber);
        SetTableStatus(tableNumber, "Trống");
        Activities.Insert(0, $"  {time}   [Thanh toán] {bill.Table} hoàn tất thanh toán {bill.Amount} ({bill.Code})");
        NotifyChanged();
    }
}
