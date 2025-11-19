using SD106_Onewhero_Assessment_2.Model;
using SD106_Onewhero_Assessment_2.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SD106_Onewhero_Assessment_2
{

    public partial class MainWindow : Window, INotifyPropertyChanged // INotifyPropertyChanged interface to notify the UI of property changes
    {

        public User? CurrentUser { get; set; } // Property to hold the currently logged-in user
        private string boundText = string.Empty;
        public string BoundText
        {
            get => boundText;
            set
            {
                boundText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoundText))); // Notify UI of property change
            }
        }
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            LoadLoginView();
            
                }

        public void LoadLoginView()
        {
            HeaderFrame.Navigate(new LoginHeaderPage());
            MainFrame.Navigate(new LoginPage());
            FooterFrame.Navigate(new FooterPage());

        }

        public void LoadMainView()
        {
            HeaderFrame.Navigate(new HeaderPage());
            MainFrame.Navigate(new HomePage());
            FooterFrame.Navigate(new FooterPage());
        }

        public event PropertyChangedEventHandler? PropertyChanged; // Event to notify when a property changes


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}