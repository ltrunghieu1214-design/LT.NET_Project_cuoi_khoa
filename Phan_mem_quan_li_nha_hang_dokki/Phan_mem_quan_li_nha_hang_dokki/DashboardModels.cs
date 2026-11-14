namespace Phan_mem_quan_li_nha_hang_dokki;

public sealed record DashboardCategoryCount(string Name, int Count);
public sealed record DashboardTableStatusCount(string Status, int Count);
public sealed record DashboardSummary(
    decimal RevenueToday,
    int PaidBillsToday,
    int TotalTables,
    int ServingTables,
    int ActiveMenuItems,
    IReadOnlyList<DashboardCategoryCount> CategoryCounts,
    IReadOnlyList<DashboardTableStatusCount> TableStatusCounts,
    IReadOnlyList<BookingItem> Bookings);
public sealed record DashboardStaffItem(string Name, string Position, string Phone);
