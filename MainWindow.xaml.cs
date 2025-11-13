using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Controls;
using SD106_Onewhero_Assessment_2.Model;
using SD106_Onewhero_Assessment_2.View;

namespace SD106_Onewhero_Assessment_2
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public User CurrentUser { get; set; }
        private string boundText = string.Empty;
        public string BoundText
        {
            get => boundText;
            set
            {
                boundText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoundText)));
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

        public event PropertyChangedEventHandler? PropertyChanged;

     
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