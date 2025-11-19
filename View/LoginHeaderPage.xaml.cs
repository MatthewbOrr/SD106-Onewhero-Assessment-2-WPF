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

namespace SD106_Onewhero_Assessment_2.View
{
    
    public partial class LoginHeaderPage : Page
    {
        private void btnMinimize_Click(object sender, RoutedEventArgs e) //button to minimize the window
        {
            Window parentWindow = Window.GetWindow(this); // Get the parent window of the current page
            if (parentWindow != null) // Check if the parent window is not null
            {
                parentWindow.WindowState = WindowState.Minimized; // Minimize the parent window
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e) //button to close the application
        {
            Application.Current.Shutdown(); // Close the application
        }
        public LoginHeaderPage()
        {
            InitializeComponent();
        }
    }
}
