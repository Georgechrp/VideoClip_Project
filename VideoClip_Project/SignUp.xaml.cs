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
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace VideoClip_Project
{
    public partial class SignUp : Window
    {
        

        public SignUp()
        {
            InitializeComponent();
        }
        //insert data in MySQL table users for signup
        private void insertdata(string fullname, string username, string email, string phone, string password)
        {
            try
            {
                string connstring = "server=localhost; uid=root; pwd=gr3ty; database=ratemyclipdb";
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();
                    // SQL query to insert data into the 'user' table
                    string sql = "INSERT INTO user (fullname, username, email, phone, password) VALUES (@fullname, @username, @email, @phone, @password)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Add parameters to the SQL query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@fullname", fullname);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Display a message box to show the result
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data inserted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Data insertion failed.");
                        }
                    }
                    con.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
    
        private void BtnSignUp(object sender, RoutedEventArgs e)
        {
            if (FullName.Text != string.Empty && Username.Text != string.Empty && Email.Text != string.Empty && Phone.Text != string.Empty && Password.Password != string.Empty)
            {
                string fullname = FullName.Text;
                string username = Username.Text;
                string email = Email.Text;
                string phone = Phone.Text;
                string password = Password.Password;

                insertdata(fullname, username, email, phone, password);

                MenuWindow menu = new MenuWindow();
                this.Visibility = Visibility.Hidden;
                menu.Show();
                
            }
            else
            {
                MessageBox.Show("Please enter the correct details");
            }
        }

        //go to login
        private void LinkTextBlock_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            LogIn login = new LogIn();
            this.Visibility = Visibility.Hidden;
            login.Show();
        }

        //Hide the gray text when box got focus or Unhide if lost focus
        private void FullName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FullName.Text == "Fullname")
            {
                FullName.Text = "";
                FullName.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void FullName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FullName.Text =="")
            {
                FullName.Text = "Fullname";
                FullName.Foreground = new SolidColorBrush(Colors.Gray);
            }
            
        }
        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "Username")
            {
                Username.Text = "";
                Username.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "")
            {
                Username.Text = "Username";
                Username.Foreground = new SolidColorBrush(Colors.Gray);
            }
           
        }
        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "Email")
            {
                Email.Text = "";
                Email.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "")
            {
                Email.Text = "Email";
                Email.Foreground = new SolidColorBrush(Colors.Gray);
            }
            
        }
        private void Phone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Phone.Text == "Phone")
            {
                Phone.Text = "";
                Phone.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void Phone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Phone.Text == "") {
                Phone.Text = "Phone";
                Phone.Foreground = new SolidColorBrush(Colors.Gray);
            }
            
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            //Password.Password = "";
            //Password.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "")
            {
                //Password.Password = "••••••••";
               // Password.Foreground = new SolidColorBrush(Colors.Black);
            }
            
        }

        private void GotoMainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
        }
    }
}
