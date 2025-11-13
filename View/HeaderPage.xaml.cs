using SD106_Onewhero_Assessment_2.View;
using SD106_Onewhero_Assessment_2.Model;    
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Input;


namespace SD106_Onewhero_Assessment_2.View
{
    /// <summary>
    /// Interaction logic for HeaderPage.xaml
    /// </summary>
    public partial class HeaderPage : Page
    {
        private readonly int visitorId;

        public HeaderPage()
        {
            InitializeComponent();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if(parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
            
        }
        private void btnTitle_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new HomePage());
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Attractions_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new AttractionsPage());
        }
        private void Events_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new EventsPage());
        }
        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new AboutUsPage());
        }

        private void Title_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new HomePage());
        }
        private void Booking_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new BookingPage(mainWindow.CurrentUser.UserId));
        }

        private void VisitorDashboard_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new VisitorDashboardPage(mainWindow.CurrentUser.UserId));
        }
    }
}
