using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoClip_Project.Models;
using static VideoClip_Project.MainWindow;

namespace VideoClip_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<VideoClip> videoClips = new ObservableCollection<VideoClip>();
        private string[] videoFiles = { };
        private int currentIndex = 0;
        private const string SaveFilePath = "videoClips.json";
        private string connstring = "server=localhost; uid=root; pwd=gr3ty; database=ratemyclipdb";
        public MainWindow()
        {
            InitializeComponent();
            LoadVideoFiles();
            DisplayCurrentVideo();
            LoadAndBindData();


            RateThisVideo.Visibility = Visibility.Hidden;
            cbRating.Visibility = Visibility.Hidden;
            btnRate.Visibility = Visibility.Hidden;
            if(UserSession.Username == string.Empty)
            {
                LogOut.Visibility = Visibility.Hidden;
            }
            else
            {
                cbRating.Visibility = Visibility.Visible;
            }
            
        }


        private void LoadAndBindData()
        {
            List<VideoRating> videoRatings = GetVideoRatings();


            var aggregatedRatings = videoRatings
                .GroupBy(vr => vr.VideoTitle)
                .Select(g => new VideoRating
                {
                    VideoTitle = g.Key,
                    AverageRating = g.Average(vr => vr.Rating),
                    RatingCount = g.Count()
                })
                .ToList();

            videoListBox.ItemsSource = aggregatedRatings;
        }
        public class VideoRating
        {
            public string Username { get; set; } = string.Empty;
            public int Rating { get; set; }
            public string VideoTitle { get; set; } = string.Empty;

            public double AverageRating { get; set; }
            public int RatingCount { get; set; }

            // Δημιουργούμε μια λίστα για τα αστέρια
            public List<int> Stars => Enumerable.Range(1, (int)Math.Round(AverageRating)).ToList();
        }

        public static List<VideoRating> GetVideoRatings()
        {
            var videoRatings = new List<VideoRating>();

            string connectionString = "server=localhost; uid=root; pwd=gr3ty; database=ratemyclipdb";
            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();

                string sql = "SELECT username, title, rating FROM ratings";
                using (var cmd = new MySqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var videoRating = new VideoRating
                            {
                                Username = reader["username"].ToString(),
                                VideoTitle = reader["title"].ToString(),
                                Rating = Convert.ToInt32(reader["rating"])
                                
                            };

                            videoRatings.Add(videoRating);
                        }
                    }
                }
                con.Close();
            }

            return videoRatings;
        }



        private void LoadVideoFiles()
        {
            string folderPath = @"C:\Users\georg\OneDrive - unipi.gr\Επιφάνεια εργασίας\VideoClip_Project\videos";
            //string folderPath = @"VideoClip_Project\videos";
            videoFiles = Directory.GetFiles(folderPath, "*.mp4");

            if (videoFiles.Length == 0)
            {
                //MessageBox.Show("No video files found in the directory.");
            }
        }
        private void DisplayCurrentVideo()
        {
            if (videoFiles.Length > 0)
            {
                string currentVideoPath = videoFiles[currentIndex];
                mediaElement.Source = new Uri(currentVideoPath);
                string temp = System.IO.Path.GetFileName(currentVideoPath);
                videoNameTextBlock.Text = temp.Substring(0, temp.Length - 4);
            }
        }

        //My buttons
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (videoFiles.Length > 0)
            {
                currentIndex = (currentIndex - 1 + videoFiles.Length) % videoFiles.Length;
                DisplayCurrentVideo();
            }
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (videoFiles.Length > 0)
            {
                currentIndex = (currentIndex + 1) % videoFiles.Length;
                DisplayCurrentVideo();
            }
        }

        //Auto buttons
        private void BtnLogIn(object sender, RoutedEventArgs e)
        {
            LogIn obgLogIn = new LogIn();
            this.Visibility = Visibility.Hidden;

            obgLogIn.Left = SystemParameters.PrimaryScreenWidth - obgLogIn.Width;
            obgLogIn.Top = SystemParameters.PrimaryScreenHeight - obgLogIn.Height;

            obgLogIn.Show();
        }
        private void BtnSignUp(object sender, RoutedEventArgs e)
        {
            SignUp obgSignUp = new SignUp();
            this.Visibility = Visibility.Hidden;

            obgSignUp.Left = SystemParameters.PrimaryScreenWidth - obgSignUp.Width;
            obgSignUp.Top = SystemParameters.PrimaryScreenHeight - obgSignUp.Height;

            obgSignUp.Show();
        }
        public void AddRating(string username, string rating, string videoTitle)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();

                    int ratingInt;
                    if (!int.TryParse(rating, out ratingInt))
                    {
                        throw new ArgumentException("Invalid rating value");
                    }

                    // SQL query to insert the rating into the video_ratings table
                    string sql = "INSERT INTO ratings (username, title, rating) VALUES (@username, @videoTitle, @rating)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Add parameters to the SQL query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@rating", rating);
                        cmd.Parameters.AddWithValue("@videoTitle", videoTitle);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                // Optional: Handle exception (e.g., log the error, rethrow the exception, etc.)
            }
            
        }
        private void BtnRate_Click(object sender, RoutedEventArgs e)
        {
            string stars = cbRating.Text;//posa asteria 
            //name_of_upload_video = videoNameTextBlock.Text;
            AddRating(UserSession.Username, stars, videoNameTextBlock.Text);
            MessageBox.Show($"Rated {videoNameTextBlock.Text} with {stars} stars.");
            
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Username = string.Empty;
            ButtonLogIn.Visibility = Visibility.Visible;
            ButtonSignUp.Visibility = Visibility.Visible;

            RateThisVideo.Visibility = Visibility.Hidden;
            cbRating.Visibility = Visibility.Hidden;
            btnRate.Visibility = Visibility.Hidden;

            MessageBox.Show("Log Out Succesfully");
        }
    }
}