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
                    SELECT b.booking_id, e.title, b.number_of_tickets, b.status
                    FROM Booking b
                    JOIN Event e ON b.event_id = e.event_id
                    WHERE b.visitor_id = @id And b.status = 'pending'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", currentUser.UserId);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        bookings.Add(new BookingItem
                        {
                            BookingId = reader.GetInt32(0),
                            EventTitle = reader.GetString(1),
                            NumberOfTickets = reader.GetInt32(2),
                            Status = reader.GetString(3)
                        });
                    }
                }
                RenderBookings(bookings);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during registration: " + ex.Message);
            }
        }

        private void RenderBookings(List<BookingItem> bookings)
        {
            EventListPanel.Children.Clear();
            
            foreach (var booking in bookings)
            {
                Expander expander = new Expander
                { 
                    Header = booking.EventTitle,
                    Foreground = Brushes.White,
                    Background = Brushes.Transparent,
                    Width = 680,
                    Margin = new Thickness(0, 0, 0, 10),

                };

                Grid grid = new Grid
                {
                    Background = new SolidColorBrush(Color.FromRgb(229, 229, 229))
                };

                TextBlock details = new TextBlock
                {
                    Text = $"Tickets: {booking.NumberOfTickets}\nStatus: {booking.Status}",
                    Foreground = Brushes.Black,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10)

                };
                Button cancelbtn = new Button
                {
                    Content = "Cancel Booking",
                    Width = 80,
                    Height = 24,
                    Background = new SolidColorBrush(Color.FromRgb(21, 106, 172)),
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(10),
                    Cursor = Cursors.Hand,
                    Tag = booking.BookingId
                };
                cancelbtn.Click += CancelBooking_Click;

                grid.Children.Add(details);
                grid.Children.Add(cancelbtn);
                expander.Content = grid;

                EventListPanel.Children.Add(expander);

            }
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            int bookingId;

            if (btn == null || btn.Tag == null || !int.TryParse(btn.Tag.ToString(), out bookingId))
            {
                MessageBox.Show("Invalid booking selection.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure to cancel?", "Confirm Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    using (var conn = DBHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM Booking WHERE booking_id = @bid";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@bid", bookingId);
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new EditDetailsPage(currentUser));
        }
    }
}

