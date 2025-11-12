using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Controls;
using SD106_Onewhero_Assessment_2.Model;

namespace SD106_Onewhero_Assessment_2
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            MainFrame.Navigate(new Model.LoginPage());
            HeaderFrame.Navigate(new Model.LoginHeaderPage());

        }
        private string boundText = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string BoundText
        {
            get => boundText;
            set
            {
                boundText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoundText)));
            }
        }
    }
}