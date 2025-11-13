using System.Windows.Controls;
using System.Windows;
using SD106_Onewhero_Assessment_2.Model;
using SD106_Onewhero_Assessment_2.Helpers;
using MySql.Data.MySqlClient;

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

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE User SET name=@n, email=@e, phone=@p WHERE user_id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@n", currentUser.Name);
                    cmd.Parameters.AddWithValue("@e", currentUser.Email);
                    cmd.Parameters.AddWithValue("@p", currentUser.Phone);
                    cmd.Parameters.AddWithValue("@id", currentUser.UserId);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Details updated successfully.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating details: " + ex.Message);
                return;
            }

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