using Microsoft.Data.SqlClient;

namespace Phan_mem_quan_li_nha_hang_dokki;

/// <summary>
/// Quản lý kết nối và truy vấn SQL Server cho hệ thống DOKKI.
/// Thay đổi ConnectionString bên dưới cho phù hợp với môi trường của bạn.
/// </summary>
public static class DatabaseHelper
{
    // ================================================================
    //  🔧 CẤU HÌNH KẾT NỐI - Chỉnh sửa tại đây nếu cần
    // ================================================================
    private const string ConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=DokkiRestaurant;Integrated Security=True;TrustServerCertificate=True;";
    //  Server=.          → SQL Server cài trên máy local (tên mặc định)
    //  Nếu dùng tên instance: Server=.\SQLEXPRESS
    //  Nếu dùng tài khoản SA: User Id=sa;Password=mật_khẩu;

    // ================================================================
    //  KẾT NỐI
    // ================================================================
    public static SqlConnection GetConnection() => new(ConnectionString);

    /// <summary>
    /// Bổ sung cột ngày thanh toán cho database cũ được tạo trước khi schema được cập nhật.
    /// Database mới chạy DokkiDatabase.sql sẽ không cần thay đổi gì.
    /// </summary>
    public static void EnsureHoaDonDateColumn()
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "IF COL_LENGTH('dbo.HoaDon', 'NgayThanhToan') IS NULL " +
            "BEGIN " +
            "ALTER TABLE dbo.HoaDon ADD NgayThanhToan DATETIME NOT NULL " +
            "CONSTRAINT DF_HoaDon_NgayThanhToan DEFAULT GETDATE() WITH VALUES; " +
            "END", conn);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Kiểm tra kết nối đến database. Trả về null nếu thành công, trả về thông báo lỗi nếu thất bại.</summary>
    public static string? TestConnection()
    {
        try
        {
            using var conn = GetConnection();
            conn.Open();
            return null; // thành công
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    // ================================================================
    //  DANH MỤC
    // ================================================================
    public static List<CategoryItem> LoadCategories()
    {
        var list = new List<CategoryItem>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT MaDanhMuc, TenDanhMuc, SoLuongMon, GhiChu, TrangThai FROM dbo.DanhMuc ORDER BY MaDanhMuc", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new CategoryItem(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.IsDBNull(3) ? "" : reader.GetString(3),
                reader.GetString(4)
            ));
        }
        return list;
    }

    public static void InsertCategory(CategoryItem cat)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.DanhMuc (MaDanhMuc, TenDanhMuc, SoLuongMon, GhiChu, TrangThai) " +
            "VALUES (@ma, @ten, @sl, @ghi, @tt)", conn);
        cmd.Parameters.AddWithValue("@ma",  cat.Code);
        cmd.Parameters.AddWithValue("@ten", cat.Name);
        cmd.Parameters.AddWithValue("@sl",  cat.Count);
        cmd.Parameters.AddWithValue("@ghi", cat.Note);
        cmd.Parameters.AddWithValue("@tt",  cat.Status);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateCategoryStatus(string code, string newStatus)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "UPDATE dbo.DanhMuc SET TrangThai = @tt WHERE MaDanhMuc = @ma", conn);
        cmd.Parameters.AddWithValue("@tt", newStatus);
        cmd.Parameters.AddWithValue("@ma", code);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteCategory(string code)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand("DELETE FROM dbo.DanhMuc WHERE MaDanhMuc = @ma", conn);
        cmd.Parameters.AddWithValue("@ma", code);
        cmd.ExecuteNonQuery();
    }

    // ================================================================
    //  MÓN ĂN
    // ================================================================
    public static List<FoodItem> LoadFoods()
    {
        var list = new List<FoodItem>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT MaMon, TenMon, DanhMuc, Gia, DonVi, TrangThai FROM dbo.MonAn ORDER BY MaMon", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new FoodItem(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetDecimal(3),
                reader.GetString(4),
                reader.GetString(5)
            ));
        }
        return list;
    }

    public static void InsertFood(FoodItem food)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.MonAn (MaMon, TenMon, DanhMuc, Gia, DonVi, TrangThai) " +
            "VALUES (@ma, @ten, @dm, @gia, @dv, @tt)", conn);
        cmd.Parameters.AddWithValue("@ma",  food.Code);
        cmd.Parameters.AddWithValue("@ten", food.Name);
        cmd.Parameters.AddWithValue("@dm",  food.Category);
        cmd.Parameters.AddWithValue("@gia", food.Price);
        cmd.Parameters.AddWithValue("@dv",  food.Unit);
        cmd.Parameters.AddWithValue("@tt",  food.Status);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateFoodStatus(string code, string newStatus)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "UPDATE dbo.MonAn SET TrangThai = @tt WHERE MaMon = @ma", conn);
        cmd.Parameters.AddWithValue("@tt", newStatus);
        cmd.Parameters.AddWithValue("@ma", code);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteFood(string code)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand("DELETE FROM dbo.MonAn WHERE MaMon = @ma", conn);
        cmd.Parameters.AddWithValue("@ma", code);
        cmd.ExecuteNonQuery();
    }

    // ================================================================
    //  BÀN ĂN
    // ================================================================
    public static List<TableItem> LoadTables()
    {
        var list = new List<TableItem>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT SoBan, TenBan, TrangThai, SucChua, KhuVuc FROM dbo.BanAn ORDER BY SoBan", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new TableItem(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetString(4)
            ));
        }
        return list;
    }

    public static void InsertTable(TableItem table)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.BanAn (SoBan, TenBan, TrangThai, SucChua, KhuVuc) " +
            "VALUES (@so, @ten, @tt, @sc, @kv)", conn);
        cmd.Parameters.AddWithValue("@so",  table.Number);
        cmd.Parameters.AddWithValue("@ten", table.Name);
        cmd.Parameters.AddWithValue("@tt",  table.Status);
        cmd.Parameters.AddWithValue("@sc",  table.Capacity);
        cmd.Parameters.AddWithValue("@kv",  table.Area);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateTableStatus(int tableNumber, string newStatus)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "UPDATE dbo.BanAn SET TrangThai = @tt WHERE SoBan = @so", conn);
        cmd.Parameters.AddWithValue("@tt", newStatus);
        cmd.Parameters.AddWithValue("@so", tableNumber);
        cmd.ExecuteNonQuery();
    }

    // ================================================================
    //  ĐẶT BÀN
    // ================================================================
    public static List<BookingItem> LoadBookings()
    {
        var list = new List<BookingItem>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT MaDatBan, GioHen, TenKhach, SoDienThoai, SoNguoi, TenBan, TrangThai " +
            "FROM dbo.DatBan ORDER BY MaDatBan", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new BookingItem(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6)
            ));
        }
        return list;
    }

    public static void InsertBooking(BookingItem booking)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.DatBan (MaDatBan, GioHen, TenKhach, SoDienThoai, SoNguoi, TenBan, TrangThai) " +
            "VALUES (@ma, @gio, @ten, @sdt, @sl, @ban, @tt)", conn);
        cmd.Parameters.AddWithValue("@ma",  booking.Code);
        cmd.Parameters.AddWithValue("@gio", booking.Time);
        cmd.Parameters.AddWithValue("@ten", booking.Guest);
        cmd.Parameters.AddWithValue("@sdt", booking.Phone);
        cmd.Parameters.AddWithValue("@sl",  booking.Pax);
        cmd.Parameters.AddWithValue("@ban", booking.Table);
        cmd.Parameters.AddWithValue("@tt",  booking.Status);
        cmd.ExecuteNonQuery();
    }

    // ================================================================
    //  HÓA ĐƠN
    // ================================================================
    public static List<OrderBill> LoadBills()
    {
        var list = new List<OrderBill>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT MaHoaDon, GioThanhToan, TenBan, TenKhach, TongTien, TrangThai " +
            "FROM dbo.HoaDon ORDER BY MaHoaDon DESC", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var amount = reader.GetDecimal(4);
            list.Add(new OrderBill(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                $"{amount:N0} đ",
                reader.GetString(5)
            ));
        }
        return list;
    }

    public static void InsertBill(string maHD, string gio, string tenBan, string tenKhach, decimal tongTien, string trangThai)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.HoaDon (MaHoaDon, GioThanhToan, NgayThanhToan, TenBan, TenKhach, TongTien, TrangThai) " +
            "VALUES (@ma, @gio, @ngay, @ban, @ten, @tien, @tt)", conn);
        cmd.Parameters.AddWithValue("@ma",   maHD);
        cmd.Parameters.AddWithValue("@gio",  gio);
        cmd.Parameters.Add("@ngay", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
        cmd.Parameters.AddWithValue("@ban",  tenBan);
        cmd.Parameters.AddWithValue("@ten",  tenKhach);
        cmd.Parameters.AddWithValue("@tien", tongTien);
        cmd.Parameters.AddWithValue("@tt",   trangThai);
        cmd.ExecuteNonQuery();
    }

    public static int GetNextBillNumber()
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT ISNULL(MAX(CAST(SUBSTRING(MaHoaDon,3,10) AS INT)), 1086) + 1 FROM dbo.HoaDon", conn);
        return (int)cmd.ExecuteScalar()!;
    }

    // ================================================================
    //  NHẬT KÝ HOẠT ĐỘNG
    // ================================================================
    public static List<string> LoadActivities()
    {
        var list = new List<string>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT TOP 50 FORMAT(ThoiGian,'HH:mm') + '   ' + NoiDung " +
            "FROM dbo.NhatKyHoatDong ORDER BY ThoiGian DESC", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add("  " + reader.GetString(0));
        return list;
    }

    public static void InsertActivity(string noiDung)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.NhatKyHoatDong (NoiDung, ThoiGian) VALUES (@nd, @tg)", conn);
        cmd.Parameters.AddWithValue("@nd", noiDung);
        cmd.Parameters.AddWithValue("@tg", DateTime.Now);
        cmd.ExecuteNonQuery();
    }

    // ================================================================
    //  DOANH THU HÔM NAY
    // ================================================================
    public static decimal GetTodayRevenue()
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT ISNULL(SUM(TongTien), 0) FROM dbo.HoaDon " +
            "WHERE NgayThanhToan >= CAST(GETDATE() AS DATE) " +
            "AND NgayThanhToan < DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) " +
            "AND TrangThai = N'Đã thanh toán'", conn);
        var result = cmd.ExecuteScalar();
        return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
    }
}
