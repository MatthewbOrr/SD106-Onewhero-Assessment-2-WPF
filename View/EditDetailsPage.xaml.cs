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

        public EditDetailsPage(User user) // Constructor accepting User object
        {
            InitializeComponent();
            currentUser = user; // Store the passed User object

            txtName.Text = user.Name; // Pre-fill name
            txtEmail.Text = user.Email; // Pre-fill email
            txtPhone.Text = user.Phone; // Pre-fill phone

        }

        private void btnSave_Click(object sender, RoutedEventArgs e) // Save button click event handler
        {
            currentUser.Name = txtName.Text; // Update name
            currentUser.Email = txtEmail.Text; // Update email
            currentUser.Phone = txtPhone.Text; // Update phone

            try
            {
                using (var conn = DBHelper.GetConnection()) // Get database connection
                {
                    conn.Open();

                    var CheckCmd = new MySqlCommand("SELECT COUNT(*) FROM User WHERE email = @e AND user_id<> @id", conn); // Check for email uniqueness
                    CheckCmd.Parameters.AddWithValue("@e", currentUser.Email); // Email parameter
                    CheckCmd.Parameters.AddWithValue("@id", currentUser.UserId); // User ID parameter to exclude current user
                    long count = (long)CheckCmd.ExecuteScalar(); // Execute the query

                    if (count > 0) // Email already in use
                    {
                        MessageBox.Show("Email already in use by another account. Please use a different email."); 
                        return;
                    }

                    string query; // SQL query string
                    var cmd = new MySqlCommand(); // Create command
                    cmd.Connection = conn; // Set connection

                    if (!string.IsNullOrWhiteSpace(txtPassword.Password)) // If password field is not empty
                    {
                        string hashed = BCrypt.Net.BCrypt.HashPassword(txtPassword.Password); // Hash the new password
                        query = "UPDATE User SET name=@n, email=@e, phone=@p, password_hash=@ph WHERE user_id=@id"; // Update query with password
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ph", hashed); // Password parameter
                    }
                    else
                    {
                        query = "UPDATE User SET name = @n, email = @e, phone = @p WHERE user_id = @id"; // Update query without password
                        cmd.CommandText = query;
                    }
                        
                        cmd.Parameters.AddWithValue("@n", currentUser.Name); // Name parameter
                        cmd.Parameters.AddWithValue("@e", currentUser.Email); // Email parameter
                        cmd.Parameters.AddWithValue("@p", currentUser.Phone); // Phone parameter
                        cmd.Parameters.AddWithValue("@id", currentUser.UserId); // User ID parameter
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Details updated successfully.");
                

            }
            catch (Exception ex) // Handle any errors during update
            {
                MessageBox.Show("Error updating details: " + ex.Message); // Show error message
                return;
            }

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new VisitorDashboardPage(currentUser.UserId)); // Navigate back to dashboard

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) // Cancel button click event handler
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new VisitorDashboardPage(currentUser.UserId)); // Navigate back to dashboard
        }

    }
}