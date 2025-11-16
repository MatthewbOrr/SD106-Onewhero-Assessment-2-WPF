using System.Collections.Generic;
using System.Windows.Controls;
using SD106_Onewhero_Assessment_2.Helpers;
using MySql.Data.MySqlClient;
using System.Windows;

namespace SD106_Onewhero_Assessment_2.View
{
    public partial class AdminDashboardPage : Page
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
            LoadUser();
            LoadBookings();
        }

        private void LoadUser()
        {
            var users = new List<dynamic>();

            using var conn = DBHelper.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT user_id, name, email, phone, role FROM User", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new
                {
                    UserId = reader.GetInt32("user_id"),
                    Name = reader.GetString("name"),
                    Email = reader.GetString("email"),
                    Phone = reader.GetString("phone"),
                    Role = reader.GetString("role")
                });
            }

            dataUsers.ItemsSource = users;
            txtStatus.Text = $"Total Users: {users.Count}";
        }

        private void LoadBookings()
        {
            var bookings = new List<dynamic>();

            using var conn = DBHelper.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT 
                    b.booking_id, 
                    u.name AS user_name, 
                    e.title AS event_title, 
                    b.number_of_tickets, 
                    b.status 
                FROM Booking b
                JOIN User u ON b.user_id = u.user_id
                JOIN Event e ON b.event_id = e.event_id", conn);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bookings.Add(new
                {
                    BookingId = reader.GetInt32("booking_id"),
                    UserName = reader.GetString("user_name"),
                    EventTitle = reader.GetString("event_title"),
                    NumberOfTickets = reader.GetInt32("number_of_tickets"),
                    Status = reader.GetString("status")
                });
            }
            dataBookings.ItemsSource = bookings;
            txtStatus.Text = $"Total Bookings: {bookings.Count}";
        }

        private void UpdateBookingStatus (int bookingId, string newStatus)
        {
            using var conn = DBHelper.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("UPDATE Booking SET status = @status WHERE booking_id = @id", conn);
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@id", bookingId);

            cmd.ExecuteNonQuery();
            txtStatus.Text = $"Booking {bookingId} updated to {newStatus}.";
            LoadBookings();

        }
    }
}