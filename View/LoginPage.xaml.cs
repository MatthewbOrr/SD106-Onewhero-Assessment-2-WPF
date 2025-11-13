using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SD106_Onewhero_Assessment_2.Helpers;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using SD106_Onewhero_Assessment_2.Model;
using System.Security.Cryptography.X509Certificates;
using SD106_Onewhero_Assessment_2.View;



namespace SD106_Onewhero_Assessment_2.Model
{

    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterPage());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            using var conn = DBHelper.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT user_id, password_hash, role, name FROM User WHERE email = @e", conn);
            cmd.Parameters.AddWithValue("@e", email);
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                MessageBox.Show("Invalid email or password.", "Login Failed");
                return;
            }

            string hash = reader.GetString("password_hash");
            if (!BCrypt.Net.BCrypt.Verify(password, hash))
            {
                MessageBox.Show("Invalid email or password.", "Login Failed");
                return;
            }

            int userId = reader.GetInt32("user_id");
            string role = reader.GetString("role");
            string name = reader.GetString("name");
            reader.Close();

            var user = new User { Email = email, UserId = userId, Role = role, Name = name };

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.CurrentUser = user;

            if (role == "admin")
            {
                mainWindow.MainFrame.Navigate(new HomePage());
                mainWindow.HeaderFrame.Navigate(new HeaderPage());
                mainWindow.FooterFrame.Navigate(new FooterPage());
            }
            else
            {
                mainWindow.MainFrame.Navigate(new HomePage());
                mainWindow.HeaderFrame.Navigate(new HeaderPage());
                mainWindow.FooterFrame.Navigate(new LoginHeaderPage());
            
            }
        }
    }
}


