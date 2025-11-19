using SD106_Onewhero_Assessment_2.Model;
using System.Windows;
using System.Windows.Controls;


namespace SD106_Onewhero_Assessment_2.View
{
  
    public partial class HeaderPage : Page
    {

        public HeaderPage() 
        {
            InitializeComponent();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e) // button to minimize window  
        {
            Window parentWindow = Window.GetWindow(this); 
            if(parentWindow != null) // Check if window exists
            {
                parentWindow.WindowState = WindowState.Minimized; 
            }
            
        }
        private void btnTitle_Click(object sender, RoutedEventArgs e) // button to navigate to home page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new HomePage());
        }
        private void btnClose_Click(object sender, RoutedEventArgs e) // button to close application
        {
            Application.Current.Shutdown();
        }

        private void Attractions_Click(object sender, RoutedEventArgs e) // button to navigate to attractions page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new AttractionsPage());
        }
        private void Events_Click(object sender, RoutedEventArgs e) // button to navigate to events page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new EventsPage());
        }
        private void AboutUs_Click(object sender, RoutedEventArgs e) // button to navigate to about us page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new AboutUsPage());
        }

        private void Title_Click(object sender, RoutedEventArgs e) // button to navigate to home page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new HomePage());
        }
        private void Booking_Click(object sender, RoutedEventArgs e) // button to navigate to booking page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new BookingPage(mainWindow.CurrentUser.UserId));
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e) // button to navigate to dashboard page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            if (mainWindow.CurrentUser.Role == "admin") // Check if user is admin
            {
                mainWindow.MainFrame.Navigate(new AdminDashboardPage()); // Navigate to admin dashboard if current user is admin
                return;
            }
            else
            {
                mainWindow.MainFrame.Navigate(new VisitorDashboardPage(mainWindow.CurrentUser.UserId)); // Navigate to visitor dashboard if current user is visitor
            }
        }
    }
}
