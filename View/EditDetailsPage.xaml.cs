using System.Windows.Controls;
using System.Windows;
using SD106_Onewhero_Assessment_2.Model;

namespace SD106_Onewhero_Assessment_2.View
{
    public partial class EditDetailsPage : Page
    {

        private readonly User currentUser;

        public EditDetailsPage(User user)
        {
            InitializeComponent();
            currentUser = user;

            txtName.Text = user.Name;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            currentUser.Name = txtName.Text;
            currentUser.Email = txtEmail.Text;
            currentUser.Phone = txtPhone.Text;

            MessageBox.Show("Details updated successfully!");

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new VisitorDashboardPage(currentUser.UserId));

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new VisitorDashboardPage(currentUser.UserId));
        }

    }
}