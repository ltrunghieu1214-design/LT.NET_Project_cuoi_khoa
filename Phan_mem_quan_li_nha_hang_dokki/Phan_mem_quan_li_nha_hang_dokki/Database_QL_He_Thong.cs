using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phan_mem_quan_li_nha_hang_dokki
{
    public class Database_QL_He_Thong
    {
        public static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=QLHeThong;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static List<DashboardStaffItem> LoadDashboardStaff()
        {
            var staff = new List<DashboardStaffItem>();
            using SqlConnection conn = GetConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand(
                "SELECT TOP (8) HoTen, ChucVu, SoDienThoai FROM dbo.NhanVien ORDER BY HoTen", conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                staff.Add(new DashboardStaffItem(
                    reader.GetString(0),
                    reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    reader.IsDBNull(2) ? string.Empty : reader.GetString(2)));
            }
            return staff;
        }
    }
}

