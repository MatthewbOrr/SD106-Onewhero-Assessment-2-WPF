using MySql.Data.MySqlClient;
using SD106_Onewhero_Assessment_2.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SD106_Onewhero_Assessment_2.Model;

namespace SD106_Onewhero_Assessment_2.View
{

    public partial class BookingPage : Page
    {

        public int visitorId; // Logged-in visitor ID

        public BookingPage()
        {
            InitializeComponent();
        }
        public BookingPage(int loggedInVisitorId) // Constructor accepting visitorId

        {
            InitializeComponent();
            visitorId = loggedInVisitorId; // Store visitorId
        }

        private void btnBook_Click(object sender, RoutedEventArgs e) // Method to handle booking button click
        {
            var (eventId, tickets) = GetSelectedEventIdAndTickets(); // Get selected event ID and number of tickets

            if (eventId == null || tickets <= 0) // Validate input
            {
                MessageBox.Show("Please select an event and specify a valid number of tickets.");
                return;
            }
            
            try
            { 
                using (var conn = DBHelper.GetConnection()) // Get database connection
                {
                    conn.Open();
                    string query = "INSERT INTO Booking (event_id, visitor_id, number_of_tickets, status) VALUES (@event_id, @visitor_id, @tickets, 'pending')"; // SQL query to insert booking
                    MySqlCommand cmd = new MySqlCommand(query, conn); // Create command
                    cmd.Parameters.AddWithValue("@event_id", eventId); // Add event_id parameter
                    cmd.Parameters.AddWithValue("@visitor_id", visitorId); // Add visitor_id parameter
                    cmd.Parameters.AddWithValue("@tickets", tickets); // Add number_of_tickets parameter
                    cmd.ExecuteNonQuery(); // Execute command
                }
                MessageBox.Show("Booking successful! You will receive a confirmation via email."); // Show success message
                NavigationService.Navigate(new VisitorDashboardPage(visitorId)); // Navigate to dashboard

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during booking: " + ex.Message);
            }
        }

        private (string? eventId, int tickets) GetSelectedEventIdAndTickets() // Method to get selected event ID and number of tickets
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
        private bool IsChecked(int checkboxRow) // Method to check if a checkbox is checked based on its row
        {
            foreach (UIElement element in ((Grid)MainScroll.Content).Children) // Iterate through grid children
            {
                if (Grid.GetRow(element) == checkboxRow && Grid.GetColumn(element) == 1 && element is CheckBox cb) // Check for checkbox in specified row and column
                {
                    return cb.IsChecked == true; // Return whether the checkbox is checked
                }
            }
            return false;
        }
        }

       
}
