using MySql.Data.MySqlClient;
using SD106_Onewhero_Assessment_2.Helpers;
using SD106_Onewhero_Assessment_2.View;
using System.Windows;
using System.Windows.Controls;




namespace SD106_Onewhero_Assessment_2.Model
{

    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_Register_Click(object sender, RoutedEventArgs e) // navigate to register page
        {
            NavigationService?.Navigate(new RegisterPage()); // navigate to register page
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) // button click event to login
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            using var conn = DBHelper.GetConnection(); // get database connection
            conn.Open();
            var cmd = new MySqlCommand("SELECT user_id, password_hash, role, name FROM User WHERE email = @e", conn); // SQL command to select user by email
            cmd.Parameters.AddWithValue("@e", email); // set email parameter
            using var reader = cmd.ExecuteReader(); // execute SQL command

            if (!reader.Read()) // if no user found
            {
                MessageBox.Show("Invalid email or password.", "Login Failed"); // show error message
                return;
            }

            string hash = reader.GetString("password_hash"); // get password hash from database
            if (!BCrypt.Net.BCrypt.Verify(password, hash)) // verify password

            {
                MessageBox.Show("Invalid email or password.", "Login Failed");
                return;
            }

            int userId = reader.GetInt32("user_id"); // get user ID
            string role = reader.GetString("role"); // get user role
            string name = reader.GetString("name"); // get user name
            reader.Close();

            var user = new User { Email = email, UserId = userId, Role = role, Name = name }; // create user object

            var mainWindow = (MainWindow)Application.Current.MainWindow; // get main window
            mainWindow.CurrentUser = user; // set current user

            if (role == "admin") // if user is admin
            {
                mainWindow.MainFrame.Navigate(new AdminDashboardPage()); // navigate to admin dashboard
                mainWindow.HeaderFrame.Navigate(new HeaderPage());
                mainWindow.FooterFrame.Navigate(new FooterPage());
            }
            else // if user is visitor
            {
                mainWindow.MainFrame.Navigate(new HomePage()); // navigate to home page
                mainWindow.HeaderFrame.Navigate(new HeaderPage());
                mainWindow.FooterFrame.Navigate(new FooterPage());
            
            }
        }
    }
}


