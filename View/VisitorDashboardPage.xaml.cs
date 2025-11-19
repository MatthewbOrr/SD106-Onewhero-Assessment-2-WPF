using System;
using MySql.Data.MySqlClient;
using SD106_Onewhero_Assessment_2.Helpers;
using SD106_Onewhero_Assessment_2.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;


namespace SD106_Onewhero_Assessment_2.View
{

    public partial class VisitorDashboardPage : Page
    {
        private readonly User currentUser; // Logged-in user details
        private readonly int visitorId; // unique identifer for the visitor

        
        public VisitorDashboardPage(int visitorId) // Constructor accepting visitorId
        {
            InitializeComponent();
            this.visitorId = visitorId; // stored visitorId
            this.currentUser = LoadUser(this.visitorId); // Load user details from database
            txtInterest.Text = currentUser.Interests; // Display user interests
            txtWelcome.Text = $"Welcome, {currentUser.Name}"; // Personalized welcome message
            LoadProfile(); // Load user profile details
            LoadBookings(); // Load upcoming bookings
        }

        private User LoadUser(int visitorId) // Method to load user details from database
        {
            User user = new User(); // Create a new User object

            try
            {
                using (var conn = DBHelper.GetConnection()) // Get database connection
                {
                    conn.Open(); // Open connection
                    string query = @" 
                    SELECT u.user_id, u.name, u.email, u.phone, v.registered_date 
                    FROM User u 
                    JOIN Visitor v ON u.user_id = v.visitor_id
                    WHERE visitor_id = @id"; // SQL query to fetch user details
                    MySqlCommand cmd = new MySqlCommand(query, conn); // Create command
                    cmd.Parameters.AddWithValue("@id", visitorId); // Add parameter
                    var reader = cmd.ExecuteReader(); // Execute command

                    if (reader.Read()) // If user found
                    {
                        /// load user details into User object
                        user.UserId = reader.GetInt32("user_id"); 
                        user.Name = reader.GetString("name"); 
                        user.Email = reader.GetString("email");
                        user.Phone = reader.GetString("phone");
                        user.CreatedAt = reader.GetDateTime("registered_date");
                        /// end loading user details
                    }

                    else
                    {
                        MessageBox.Show("User not found.");
                        return user;
                    }
                    reader.Close();
                    
                    string queryInterest = @"
                    SELECT i.Interest_name
                    FROM VisitorInterest vi
                    JOIN Interest i ON vi.interest_id = i.interest_id
                    WHERE vi.visitor_id = @id"; // SQL query to fetch user interests

                    MySqlCommand cmdInterest = new MySqlCommand(queryInterest, conn); // Create command for interests
                    cmdInterest.Parameters.AddWithValue("@id", visitorId); // Add parameter
                    var readerInterest = cmdInterest.ExecuteReader(); // Execute command

                    List<string> interests = new List<string>(); // List to hold interests
                    while (readerInterest.Read()) // Read each interest
                    {
                        interests.Add(readerInterest.GetString("Interest_name")); // Add interest to list
                    }
                    user.Interests = string.Join(", ", interests); // Join interests into a single string
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user: " + ex.Message); // Show error message
            }
            return user; 
        }
        
        private void LoadProfile() // Method to load user profile details into UI
        {
            txtName.Text = currentUser.Name;
            txtEmail.Text = currentUser.Email;
            txtPhone.Text = currentUser.Phone;
            txtRegistered.Text = currentUser.CreatedAt.ToString("yyyy-MM-dd");
        }
        private void LoadBookings() // Method to load upcoming bookings into UI
        {
            List<BookingItem> bookings = new List<BookingItem>(); // List to hold bookings

            try
            {

                using (var conn = DBHelper.GetConnection()) // Get database connection
                {
                    conn.Open();
                    string query = @"
                    SELECT b.booking_id, e.title AS event_title, e.description, e.date AS event_date, e.location, b.number_of_tickets, b.status
                    FROM Booking b
                    JOIN Event e ON b.event_id = e.event_id
                    WHERE b.visitor_id = @id And e.date >= CURDATE()
                    ORDER BY e.date ASC"; // SQL query to fetch upcoming bookings
                    MySqlCommand cmd = new MySqlCommand(query, conn); // Create command
                    cmd.Parameters.AddWithValue("@id", currentUser.UserId); // Add parameter
                    var reader = cmd.ExecuteReader(); // Execute command

                    while (reader.Read()) // Read each booking
                    {
                        bookings.Add(new BookingItem // Add booking to list
                        {
                            /// Load booking details into BookingItem object
                            BookingId = reader.GetInt32("booking_id"),
                            EventTitle = reader.GetString("event_title"),
                            Description = reader.GetString("description"),
                            EventDate = reader.GetDateTime("event_date").ToString("yyyy-MM-dd HH:mm"),
                            Location = reader.GetString("location"),
                            NumberOfTickets = reader.GetInt32("number_of_tickets"),
                            Status = reader.GetString("status")
                        }); /// end loading booking details
                    }
                }
                dataUpcomingEvents.ItemsSource = bookings; // Bind bookings to UI element
                txtStatus.Text = $"Total Bookings: {bookings.Count}"; // Display total bookings
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message); // Show error message
            }
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e) // Method to cancel a booking
        {
            var selected = dataUpcomingEvents.SelectedItem; // Get selected booking
            if (selected != null)
            { // If a booking is selected
                dynamic booking = selected;// Cast selected item to dynamic (dynamic typing for simplicity)

                MessageBoxResult result = MessageBox.Show("Are you sure to cancel this booking?", "Confirm Cancellation", MessageBoxButton.YesNo, MessageBoxImage.Question); // Confirm cancellation
                if (result == MessageBoxResult.Yes)
                { // If user confirms
                    try
                    {
                        using (var conn = DBHelper.GetConnection())
                        {
                            conn.Open();
                            string query = "UPDATE Booking SET status = 'Cancelled' WHERE booking_id = @bid"; // SQL query to cancel booking
                            MySqlCommand cmd = new MySqlCommand(query, conn); // Create command
                            cmd.Parameters.AddWithValue("@bid", booking.BookingId); // Add parameter
                            cmd.ExecuteNonQuery(); // Execute command
                        }
                        LoadBookings(); // Reload bookings to reflect changes
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error cancelling booking: " + ex.Message);
                    }
                }
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e) // Method to navigate to edit details page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow; // Get main window
            mainWindow.MainFrame.Navigate(new EditDetailsPage(currentUser)); // Navigate to edit details page
        }
    }

    public class BookingItem // Class to represent a booking item
    {
        public int BookingId { get; set; }
        public string? EventTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string EventDate { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public int NumberOfTickets { get; set; }
        public string? Status { get; set; } = string.Empty;
    }
}

