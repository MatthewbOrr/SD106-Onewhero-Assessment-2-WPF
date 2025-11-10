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
            string password = txtPassword.Password;
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);

            using var conn = DBHelper.GetConnection();
            conn.Open();
            var tran = conn.BeginTransaction();

            try
            {
                var cmdUser = new MySqlCommand("INSERT INTO User (name, email, password_hash, role) VALUES (@n, @e, @ph, 'visitor')", conn, tran);
                cmdUser.Parameters.AddWithValue("@n", name);
                cmdUser.Parameters.AddWithValue("@e", email);
                cmdUser.Parameters.AddWithValue("@ph", hashed);
                cmdUser.ExecuteNonQuery();

                long userId = cmdUser.LastInsertedId;

                var cmdVisitor = new MySqlCommand("INSERT INTO Visitor (user_id) VALUES (@uid)", conn, tran);
                cmdVisitor.Parameters.AddWithValue("@uid", userId);
                cmdVisitor.ExecuteNonQuery();

                foreach (CheckBox cb in interestPanel.Children.OfType<CheckBox>())
                {
                    if (cb.IsChecked == true)
                    {
                        int interestId = Convert.ToInt32(cb.Tag);
                        var cmdInterest = new MySqlCommand("INSERT INTO VisitorInterest (user_id, interest) VALUES (@uid, @int)", conn, tran);
                        cmdInterest.Parameters.AddWithValue("@uid", userId);
                        cmdInterest.Parameters.AddWithValue("@int", cb.Content.ToString());
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
