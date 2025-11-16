using System.Windows.Controls;
using System.Windows;
using SD106_Onewhero_Assessment_2.Model;
using SD106_Onewhero_Assessment_2.Helpers;
using MySql.Data.MySqlClient;
using BCrypt.Net;

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

                    var CheckCmd = new MySqlCommand("SELECT COUNT(*) FROM User WHERE email = @e AND user_id<> @id", conn);
                    CheckCmd.Parameters.AddWithValue("@e", currentUser.Email);
                    CheckCmd.Parameters.AddWithValue("@id", currentUser.UserId);
                    long count = (long)CheckCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Email already in use by another account. Please use a different email.");
                        return;
                    }

                    string query;
                    var cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (!string.IsNullOrWhiteSpace(txtPassword.Password))
                    {
                        string hashed = BCrypt.Net.BCrypt.HashPassword(txtPassword.Password);
                        query = "UPDATE User SET name=@n, email=@e, phone=@p, password_hash=@ph WHERE user_id=@id";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ph", hashed);
                    }
                    else
                    {
                        query = "UPDATE User SET name = @n, email = @e, phone = @p WHERE user_id = @id";
                        cmd.CommandText = query;
                    }
                        
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