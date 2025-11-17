using MySql.Data.MySqlClient;
using SD106_Onewhero_Assessment_2.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SD106_Onewhero_Assessment_2.View
{

    public partial class BookingPage : Page
    {
        public int visitorId;

        public BookingPage(int loggedInVisitorId)
        {
            InitializeComponent();
            visitorId = loggedInVisitorId;
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            var (eventId, tickets) = GetSelectedEventIdAndTickets();

            if (eventId == null || tickets <= 0)
            {
                MessageBox.Show("Please select an event and specify a valid number of tickets.");
                return;
            }
            
            try
            { 
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Booking (event_id, visitor_id, number_of_tickets, status) VALUES (@event_id, @visitor_id, @tickets, 'pending')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@event_id", eventId);
                    cmd.Parameters.AddWithValue("@visitor_id", visitorId);
                    cmd.Parameters.AddWithValue("@tickets", tickets);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Booking successful! You will receive a confirmation via email.");
                NavigationService.Navigate(new VisitorDashboardPage(visitorId));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during booking: " + ex.Message);
            }
        }

        private (string? eventId, int tickets) GetSelectedEventIdAndTickets()
        {
            if (IsChecked(checkboxRow: 2)) return ("1", int.Parse(txtTickets2.Text)); //Māori Flax Weaving Workshop
            if (IsChecked(checkboxRow: 3)) return ("2", int.Parse(txtTickets3.Text)); //Kiwi Twilight Encounter
            if (IsChecked(checkboxRow: 4)) return ("3", int.Parse(txtTickets4.Text)); //Heritage Music and Food Festival
            if (IsChecked(checkboxRow: 5)) return ("4", int.Parse(txtTickets5.Text)); //Guided Nature Trail Walk
            if (IsChecked(checkboxRow: 6)) return ("5", int.Parse(txtTickets6.Text)); //Museum Discovery Day
            if (IsChecked(checkboxRow: 7)) return ("6", int.Parse(txtTickets7.Text)); //Sunrise Yoga by the Lake
            if (IsChecked(checkboxRow: 8)) return ("7", int.Parse(txtTickets8.Text)); //Holiday Craft and Storytelling Fair

            return (null, 0);
        }
        private bool IsChecked(int checkboxRow)
        {
            foreach (UIElement element in ((Grid)MainScroll.Content).Children)
            {
                if (Grid.GetRow(element) == checkboxRow && Grid.GetColumn(element) == 1 && element is CheckBox cb)
                {
                    return cb.IsChecked == true;
                }
            }
            return false;
        }
        }

       
}
