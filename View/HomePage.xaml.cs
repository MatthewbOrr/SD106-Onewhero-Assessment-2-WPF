using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SD106_Onewhero_Assessment_2.Model
{

    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) // Allow window dragging
        {
            if (e.LeftButton == MouseButtonState.Pressed) // If left mouse button is pressed
            {
                Window parentWindow = Window.GetWindow(this);
                parentWindow?.DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e) // Minimize window
        {
            Window parentWindow = Window.GetWindow(this); 
            if (parentWindow != null) // Check if parentWindow is not null
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) // Close application
        {
            Application.Current.Shutdown(); // Shutdown the application
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e) // Handle image size change
        {

        }

    }
}
