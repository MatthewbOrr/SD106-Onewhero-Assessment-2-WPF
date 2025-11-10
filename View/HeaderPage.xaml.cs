using System;
using System.Collections.Generic;
using System.Linq;
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
using SD106_Onewhero_Assessment_2.View;

namespace SD106_Onewhero_Assessment_2.Model
{
    /// <summary>
    /// Interaction logic for HeaderPage.xaml
    /// </summary>
    public partial class HeaderPage : Page
    {

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if(parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public HeaderPage()
        {
            InitializeComponent();
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

            mainWindow.MainFrame.Navigate(new BookingPage());
        }

        private void VisitorDashboard_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new VisitorDashboardPage());
        }
    }
}
