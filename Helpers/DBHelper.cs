using MySql.Data.MySqlClient;

namespace SD106_Onewhero_Assessment_2.Helpers
{
    public static class DBHelper    
    {
        public static string ConnStr = "Server=localhost;Database=visitor_management;Uid=root;password=9921004";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnStr);
        }
    }
}
