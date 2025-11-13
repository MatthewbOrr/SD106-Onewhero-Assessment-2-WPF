using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SD106_Onewhero_Assessment_2.Helpers;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace SD106_Onewhero_Assessment_2.Model
{

    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_Login_Click(object sender, RoutedEventArgs e)
         {
            NavigationService?.Navigate(new LoginPage());
         }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }



            string hashed = BCrypt.Net.BCrypt.HashPassword(password);

            using var conn = DBHelper.GetConnection();
            conn.Open();
            var tran = conn.BeginTransaction();

            try
            {


                var CheckCmd = new MySqlCommand("SELECT COUNT(*) FROM User WHERE email = @e", conn, tran);
                CheckCmd.Parameters.AddWithValue("@e", email);
                long count = (long)CheckCmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Email already registered. Please use a different email.");
                    tran.Rollback();
                    return;
                }

                var cmdUser = new MySqlCommand("INSERT INTO User (name, email, Phone, password_hash, role) VALUES (@n, @e, @p, @ph, 'visitor')", conn, tran);
                cmdUser.Parameters.AddWithValue("@n", name);
                cmdUser.Parameters.AddWithValue("@e", email);
                cmdUser.Parameters.AddWithValue("@p", phone);
                cmdUser.Parameters.AddWithValue("@ph", hashed);
                cmdUser.ExecuteNonQuery();
                
                int userId = Convert.ToInt32(cmdUser.LastInsertedId);
                MessageBox.Show("New User ID: " + userId);

                var cmdVisitor = new MySqlCommand(@"
                INSERT INTO Visitor (visitor_id, registered_date) VALUES (@uid, @reg)", conn, tran);
                cmdVisitor.Parameters.AddWithValue("@uid", userId);
                cmdVisitor.Parameters.AddWithValue("@reg", DateTime.Now);
                cmdVisitor.ExecuteNonQuery();
                MessageBox.Show("Visitor record inserted for user ID: " + userId);

                foreach (CheckBox cb in interestPanel.Children.OfType<CheckBox>())
                {
                    if (cb.IsChecked == true)
                    {
                        int interestId = Convert.ToInt32(cb.Tag);
                        var cmdInterest = new MySqlCommand(
                            @"INSERT INTO VisitorInterest (visitor_id, interest_id) VALUES (@uid, @iid)", conn, tran);
                        cmdInterest.Parameters.AddWithValue("@uid", userId);
                        cmdInterest.Parameters.AddWithValue("@iid", interestId);
                        cmdInterest.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                MessageBox.Show("Registration Successful!");
                NavigationService?.Navigate(new LoginPage());
            
            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show("Registration Failed: " + ex.Message);
            }
        }
    }
}
