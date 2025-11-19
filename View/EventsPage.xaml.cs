using System.Windows.Controls;
using SD106_Onewhero_Assessment_2.View;

namespace SD106_Onewhero_Assessment_2.Model
{
    /// <summary>
    /// Interaction logic for EventsPage.xaml
    /// </summary>
    public partial class EventsPage : Page
    {
        public EventsPage()
        {
            InitializeComponent();
        }

        private void btnBook_Click(object sender, System.Windows.RoutedEventArgs e)
        {
  
            this.NavigationService?.Navigate(new BookingPage());
        }
    }
}
