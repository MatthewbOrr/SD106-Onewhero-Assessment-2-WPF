using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using MySql.Data.MySqlClient;
using SD106_Onewhero_Assessment_2.Helpers;
using SD106_Onewhero_Assessment_2.Model;

namespace SD106_Onewhero_Assessment_2.View
{

    public partial class VisitorDashboardPage : Page
    {
        private User currentUser;

        public VisitorDashboardPage(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadProfile();
            LoadBookings();
        }
        
        private void LoadProfile()
        {
            txtName.Text = currentUser.Name;
            txtEmail.Text = currentUser.Email;
            txtPhone.Text = currentUser.Phone;
            txtRegistered.Text = currentUser.RegisteredDate.Tostring("yyyy-MM-dd");
        }
        private void LoadBookings()
        {
            List<BookingPage> bookings = new List<BookingPage>();

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
                    bookings.Add(new BookingPage
                    {
                        bookingId = reader.GetInt32(0),
                        eventTitle = reader.GetString(1),
                        numberOfTickets = reader.GetInt32(2),
                        Status = reader.GetString(3)
                    });
                }
            }
            RenderBookings(bookings);
        }
        Catch (Exception ex)
        { MessageBox.Show("Error during registration: " + ex.Message); 
        }
        };

        private void RenderBookings(List<BookingPage> bookings)
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
                    Backgrouund = new SolidColorBrush(Color.FromRgb(229, 229, 229))
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
                    HorizontalAlignment = HorizontalAlignment.Right
                    Margin = new Thickness(10),
                    Cursor = Cursors.Hand,
                    Tag = booking.BookingId
                };
                cancelBtn.Click += CancelBtn_Click;

                grid.Children.Add(details);
                grid.Children.Add(cancelBtn);
                expander.Content = grid;

                EventListPanel.Children.Add(expander);

            }
        }

        Private void CancelBooking_Click(object sender, RoutedEventArgs e)
    {
        Button btn = sender as Button;
        int bookingId = (int)btn.Tag;

        MessageBoxResult result = MessageBox.Show("Are you sure to cancel?", "Confirm Cancel"), MessageBoxButton.YesNo, MessageBoxImage.Question);
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        MessageBox.Show("Preparing the service");

        }
}

