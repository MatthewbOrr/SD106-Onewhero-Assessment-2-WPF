using System.Collections.Generic;
using System.Windows.Controls;
using SD106_Onewhero_Assessment_2.Helpers;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Linq.Expressions;

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

        private void LoadUser() // Load users from the database
        {
            var users = new List<dynamic>(); // List to hold user data, dynamic type for flexibility

            using var conn = DBHelper.GetConnection(); // Get database connection
            conn.Open();

            var cmd = new MySqlCommand("SELECT user_id, name, email, phone, role FROM User", conn); // SQL command to select user data
            using var reader = cmd.ExecuteReader(); // Execute the command and get a data reader

            while (reader.Read()) // Read each record
            {
                users.Add(new // Add user data to the list
                {
                    user_id = reader.GetInt32("user_id"), // Get user ID
                    name = reader.GetString("name"), // Get user name
                    email = reader.GetString("email"), // Get user email
                    phone = reader.GetString("phone"), // Get user phone
                    role = reader.GetString("role") // Get user role
                });
            }

            dataUsers.ItemsSource = users; // Bind the user data to the data grid
            txtStatus.Text = $"Total Users: {users.Count}"; // Update status text with total user count
        }

        private void LoadBookings() // Load bookings from the database
        {
            var bookings = new List<dynamic>(); // List to hold booking data, dynamic type for flexibility

            using var conn = DBHelper.GetConnection(); // Get database connection
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT 
                    b.booking_id, 
                    u.name AS user_name,
                    e.title AS event_title, 
                    b.number_of_tickets, 
                    b.status 
                FROM Booking b
                JOIN User u ON b.visitor_id = u.user_id
                JOIN Event e ON b.event_id = e.event_id", conn); // SQL command to select booking data with joins

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bookings.Add(new // Add booking data to the list
                {
                    booking_id = reader.GetInt32("booking_id"), // Get booking ID
                    user_name = reader.GetString("user_name"), // Get user name
                    event_title = reader.GetString("event_title"), // Get event title
                    number_of_tickets = reader.GetInt32("number_of_tickets"), // Get number of tickets
                    status = reader.GetString("status") // Get booking status
                });
            }
            dataBookings.ItemsSource = bookings; // Bind the booking data to the data grid
            txtStatus.Text = $"Total Bookings: {bookings.Count}"; // Update status text with total booking count
        }

        private void UpdateBookingStatus(int bookingId, string newStatus) // Update booking status in the database
        {
            try
            {
                using var conn = DBHelper.GetConnection(); // Get database connection
                conn.Open();

                var cmd = new MySqlCommand("UPDATE Booking SET status = @status WHERE booking_id = @id", conn); // SQL command to update booking status
                cmd.Parameters.AddWithValue("@status", newStatus); // Set new status parameter
                cmd.Parameters.AddWithValue("@id", bookingId); // Set booking ID parameter

                cmd.ExecuteNonQuery();
                txtStatus.Text = $"Booking {bookingId} updated to {newStatus}."; // Update status text
                LoadBookings();
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Error updating booking: {ex.Message}";
            }
        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e) // button click event to confirm booking
        {
            var selected = dataBookings.SelectedItem; // get selected booking
            if (selected != null)  // check if a booking is selected
            {
                dynamic booking = selected; // cast selected item to dynamic
                UpdateBookingStatus(booking.booking_id, "Confirmed"); // update booking status to Confirmed

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) // button click event to cancel booking
        {
            var selected = dataBookings.SelectedItem; // get selected booking
            if (selected != null)  // check if a booking is selected
            {
                dynamic booking = selected; // cast selected item to dynamic
                UpdateBookingStatus(booking.booking_id, "Cancelled"); // update booking status to Cancelled
            }
        }
    }
}