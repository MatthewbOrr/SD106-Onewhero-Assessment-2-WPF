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
using System.Windows.Shapes;

namespace SD106_Onewhero_Assessment_2.View
{

    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterPage());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow  = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new HomePage());

            mainWindow.FooterFrame.Navigate(new FooterPage());

            mainWindow.HeaderFrame.Navigate(new HeaderPage());
        }
    }
}
