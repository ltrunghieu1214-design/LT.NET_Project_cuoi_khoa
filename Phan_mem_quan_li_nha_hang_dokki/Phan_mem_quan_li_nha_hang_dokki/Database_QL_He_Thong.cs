using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phan_mem_quan_li_nha_hang_dokki
{
    public class Database_QL_He_Thong
    {
        public static string connectionString = @"Server=LAPTOP-BTSSHN6I\KTEAM;Database=QLHeThong;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
