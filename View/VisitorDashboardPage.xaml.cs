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
        private readonly User currentUser;
        private readonly int visitorId;

        public VisitorDashboardPage(int visitorId)
        {
            InitializeComponent();
            this.visitorId = visitorId;
            this.currentUser = LoadUser(this.visitorId);
            txtInterest.Text = currentUser.Interests;
            txtWelcome.Text = $"Welcome, {currentUser.Name}";
            LoadProfile();
            LoadBookings();
        }

        private User LoadUser(int visitorId)
        {
            User user = new User();

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT u.user_id, u.name, u.email, u.phone, v.registered_date 
                    FROM User u 
                    JOIN Visitor v ON u.user_id = v.visitor_id
                    WHERE visitor_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", visitorId);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user.UserId = reader.GetInt32("user_id");
                        user.Name = reader.GetString("name");
                        user.Email = reader.GetString("email");
                        user.Phone = reader.GetString("phone");
                        user.CreatedAt = reader.GetDateTime("registered_date");
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
                    WHERE vi.visitor_id = @id";

                    MySqlCommand cmdInterest = new MySqlCommand(queryInterest, conn);
                    cmdInterest.Parameters.AddWithValue("@id", visitorId);
                    var readerInterest = cmdInterest.ExecuteReader();

                    List<string> interests = new List<string>();
                    while (readerInterest.Read())
                    {
                        interests.Add(readerInterest.GetString("Interest_name"));
                    }
                    user.Interests = string.Join(", ", interests);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user: " + ex.Message);
            }
            return user;
        }
        private void LoadProfile()
        {
            txtName.Text = currentUser.Name;
            txtEmail.Text = currentUser.Email;
            txtPhone.Text = currentUser.Phone;
            txtRegistered.Text = currentUser.CreatedAt.ToString("yyyy-MM-dd");
        }
        private void LoadBookings() 
        {
            List<BookingItem> bookings = new List<BookingItem>();

            try
            {

                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT b.booking_id, e.title AS event_title, e.description, e.date AS event_date, e.location, b.number_of_tickets, b.status
                    FROM Booking b
                    JOIN Event e ON b.event_id = e.event_id
                    WHERE b.visitor_id = @id And e.date >= CURDATE()
                    ORDER BY e.date ASC";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", currentUser.UserId);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        bookings.Add(new BookingItem
                        {
                            BookingId = reader.GetInt32("booking_id"),
                            EventTitle = reader.GetString("event_title"),
                            Description = reader.GetString("description"),
                            EventDate = reader.GetDateTime("event_date").ToString("yyyy-MM-dd HH:mm"),
                            Location = reader.GetString("location"),
                            NumberOfTickets = reader.GetInt32("number_of_tickets"),
                            Status = reader.GetString("status")
                        });
                    }
                }
                dataUpcomingEvents.ItemsSource = bookings;
                txtStatus.Text = $"Total Bookings: {bookings.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message);
            }
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataUpcomingEvents.SelectedItem;
            if (selected != null) {
                dynamic booking = selected;

                MessageBoxResult result = MessageBox.Show("Are you sure to cancel this booking?", "Confirm Cancellation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) {
                    try
                    {
                        using (var conn = DBHelper.GetConnection())
                        {
                            conn.Open();
                            string query = "UPDATE Booking SET status = 'Cancelled' WHERE booking_id = @bid";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@bid", booking.BookingId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadBookings();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error cancelling booking: " + ex.Message);
                    }
                }
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new EditDetailsPage(currentUser));
        }
    }

    public class BookingItem
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

