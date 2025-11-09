using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SD106_Onewhero_Assessment_2.Helpers;
using MySql.Data.MySqlClient;

namespace SD106_Onewhero_Assessment_2.Model
{

    public partial class AdminDashboardPage : Page
    {
        public User currentUser;

        public AdminDashboardPage(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadEventList();
        }
        private void LoadEventList()
        {
            using var conn = DBHelper.GetConnection();
        conn.Open();
            
            var cmd = new MySqlCommand("SELECT event_id, title, date, location, capacity FROM Event WHERE admin_id : @aid", conn);
        cmd.Parameters.AddWithValue("@aid", currentUser.UserId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            string title = reader.GetString("title");
        string date = reader.GetDateTime("date").ToString("yyyy-MM-dd HH:mm");
        string location = reader.GetString("location");
        int capacity = reader.GetInt32("capacity");
        var item = new ListBoxItem
        {
            Content = $"{title} - {date} @ {location} (Capacity: {capacity}",
            Foreground = System.Windows.Media.Brushes.DarkBlue,
        };
        lstEvents.Items.Add(item);
        }

        }
    }
}
