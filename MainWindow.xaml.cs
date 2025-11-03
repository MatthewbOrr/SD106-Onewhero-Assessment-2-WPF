using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SD106_Onewhero_Assessment_2
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        bool running  = false;
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            MainFrame.Navigate(new HomePage());

        }
        private string boundText;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string BoundText
        {
            get { return boundText; }
            set
            {
                boundText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundText"));
                //OnPropertyChanged();
            }
        }

        private void test1_Click(object sender, RoutedEventArgs e)
        {
         
        }
    }
}