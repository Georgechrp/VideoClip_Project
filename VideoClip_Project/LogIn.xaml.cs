using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


namespace VideoClip_Project
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public static string username = "";
        public LogIn()
        {
            InitializeComponent();
        }

        //checks if the username and the password is valid
        private bool ValidateUser(string username, string password)
        {
            bool isValidUser = false;

            try
            {
                string connstring = "server=localhost; uid=root; pwd=gr3ty; database=ratemyclipdb";
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();

                    // SQL query to check if a user exists with the provided username and password
                    string sql = "SELECT COUNT(*) FROM user WHERE username = @username AND password = @password";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Add parameters to the SQL query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Execute the query and get the result
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                        // If userCount is greater than 0, it means the user exists
                        isValidUser = userCount > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
                Console.Write(ex.ToString());
            }

            return isValidUser;
        }

        //connect button clicked
        private void BtnConnect(object sender, RoutedEventArgs e)
        {
            if (Username.Text != string.Empty && Password.Password != string.Empty)
            {
                username = Username.Text;
                string password = Password.Password;

                if (ValidateUser(username, password))
                {
                    MessageBox.Show("Welcome " + username + "!", "Succesfull");
                    MenuWindow menu = new MenuWindow();
                    this.Visibility = Visibility.Hidden;
                    menu.Show();
                    
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.");
                }  
            }
            else
            {
                MessageBox.Show("Please enter the correct details");
            }
        }


        //go to signup
        private void LinkTextBlock_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            // Αντικατέστησε τη διεύθυνση URL με την πραγματική σελίδα που θέλεις να ανοίξει ο χρήστης
            //Process.Start(new ProcessStartInfo("https://www.example.com") { UseShellExecute = true });
            SignUp signup = new SignUp();
            this.Visibility = Visibility.Hidden;
            signup.Show();
        }


        // when clicking username box
        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "Όνομα χρήστη(username)") {
                Username.Text = "";
                Username.Foreground = new SolidColorBrush(Colors.Black);
            }

        }
        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if(Username.Text == "")
            {
                Username.Text = "Όνομα χρήστη(username)";
                Username.Foreground = new SolidColorBrush(Colors.Black);
            }
            
        }
        // when clicking password box
        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "")
            {
                //Password.Password = "";
                //Password.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "")
            {
                //Password.Password = "";
                //Password.Foreground = new SolidColorBrush(Colors.Black);
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
