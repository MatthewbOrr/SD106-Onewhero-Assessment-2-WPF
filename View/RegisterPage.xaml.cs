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
        public RegisterPage() // RegisterPage constructor
        {
            InitializeComponent();
        }

        private void Hyperlink_Login_Click(object sender, RoutedEventArgs e) // navigate to login page
        {
            NavigationService?.Navigate(new LoginPage()); // navigate to login page
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e) // button click event to register
        {
            string name = txtName.Text; // get name from textbox
            string email = txtEmail.Text; // get email from textbox
            string phone = txtPhone.Text; // get phone from textbox
            string password = txtPassword.Password; // get password from passwordbox

            /// Validate input fields
            if (string.IsNullOrWhiteSpace(name) || 
                string.IsNullOrWhiteSpace(email) || 
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password))
            /// If any field is empty, show message and return
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }
            


            string hashed = BCrypt.Net.BCrypt.HashPassword(password); // hash the password

            using var conn = DBHelper.GetConnection(); // get database connection
            conn.Open(); // open connection
            var tran = conn.BeginTransaction(); // begin transaction

            try
            {


                var CheckCmd = new MySqlCommand("SELECT COUNT(*) FROM User WHERE email = @e", conn, tran); // check if email already exists
                CheckCmd.Parameters.AddWithValue("@e", email); // set email parameter
                long count = (long)CheckCmd.ExecuteScalar(); // execute SQL command and get count
                if (count > 0) // if email already exists
                {
                    MessageBox.Show("Email already registered. Please use a different email."); // show error message
                    tran.Rollback(); // rollback transaction
                    return;
                }

                var cmdUser = new MySqlCommand("INSERT INTO User (name, email, Phone, password_hash, role) VALUES (@n, @e, @p, @ph, 'visitor')", conn, tran); // SQL command to insert new user
                cmdUser.Parameters.AddWithValue("@n", name); // set name parameter
                cmdUser.Parameters.AddWithValue("@e", email); // set email parameter
                cmdUser.Parameters.AddWithValue("@p", phone); // set phone parameter
                cmdUser.Parameters.AddWithValue("@ph", hashed); // set password hash parameter
                cmdUser.ExecuteNonQuery(); // execute SQL command

                int userId = Convert.ToInt32(cmdUser.LastInsertedId); // get the last inserted user ID
                MessageBox.Show("New User ID: " + userId); // show new user ID

                var cmdVisitor = new MySqlCommand(@"
                INSERT INTO Visitor (visitor_id, registered_date) VALUES (@uid, @reg)", conn, tran); // SQL command to insert visitor record
                cmdVisitor.Parameters.AddWithValue("@uid", userId); // set user ID parameter
                cmdVisitor.Parameters.AddWithValue("@reg", DateTime.Now); // set registered date parameter
                cmdVisitor.ExecuteNonQuery(); // execute SQL command
                MessageBox.Show("Visitor record inserted for user ID: " + userId); // show message

                foreach (CheckBox cb in interestPanel.Children.OfType<CheckBox>()) // loop through interest checkboxes
                {
                    if (cb.IsChecked == true) // if checkbox is checked
                    {
                        int interestId = Convert.ToInt32(cb.Tag); // get interest ID from checkbox tag. ToInt32 is used to convert the tag to integer
                        var cmdInterest = new MySqlCommand(
                            @"INSERT INTO VisitorInterest (visitor_id, interest_id) VALUES (@uid, @iid)", conn, tran); // SQL command to insert visitor interest
                        cmdInterest.Parameters.AddWithValue("@uid", userId); // set user ID parameter
                        cmdInterest.Parameters.AddWithValue("@iid", interestId); // set interest ID parameter
                        cmdInterest.ExecuteNonQuery(); // execute SQL command
                    }
                }

                tran.Commit(); // commit transaction
                MessageBox.Show("Registration Successful!"); // show success message
                NavigationService?.Navigate(new LoginPage()); // navigate to login page

            }
            catch (Exception ex) // catch any exception
            {
                tran.Rollback(); // rollback transaction
                MessageBox.Show("Registration Failed: " + ex.Message); // show error message
            }
        }
    }
}
