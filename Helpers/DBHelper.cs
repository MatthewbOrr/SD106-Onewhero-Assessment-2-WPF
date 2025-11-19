using MySql.Data.MySqlClient;
using System.Configuration;

namespace SD106_Onewhero_Assessment_2.Helpers
{
    // Database Helper Class
    public static class DBHelper    
    {
        // Connection string from App.config
        public static string ConnStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // Method to get a new MySqlConnection
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnStr);
        }
    }
}
