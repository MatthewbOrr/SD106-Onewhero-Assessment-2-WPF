using System.Windows;
using System.Windows.Controls;

namespace SD106_Onewhero_Assessment_2.Model
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterPage : Page
    {
        public FooterPage()
        {
            InitializeComponent();
        }
        private void AboutUs_Click(object sender, RoutedEventArgs e) // button to navigate to About Us page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new AboutUsPage());
        }

        private void PrivacyP_Click(object sender, RoutedEventArgs e) // button to navigate to Privacy Policy page
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new PrivacyPolicyPage());
        }

    }
}
