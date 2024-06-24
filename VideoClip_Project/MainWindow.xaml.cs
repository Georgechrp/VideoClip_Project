using Microsoft.Win32;
using MySql.Data.MySqlClient;
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

        public MainWindow()
        {
            InitializeComponent();
            LoadVideoFiles();
            DisplayCurrentVideo();
            LoadAndBindData();
        }


        private void LoadAndBindData()
        {
            List<VideoRating> videoRatings = GetVideoRatings();

            // Ομαδοποίηση και υπολογισμός του μέσου όρου των αξιολογήσεων βάσει του τίτλου της ταινίας
            var averagedRatings = videoRatings
                .GroupBy(vr => vr.VideoTitle)
                .Select(g => new VideoRating
                {
                    VideoTitle = g.Key,
                    Rating = (int)g.Average(vr => vr.Rating), // Υπολογισμός μέσου όρου και μετατροπή σε ακέραιο
                    RatingCount = g.Count() // Αριθμός αξιολογήσεων
                })
                .ToList();

            videoListBox.ItemsSource = averagedRatings;
        }
        public class VideoRating
        {
            public string Username { get; set; } = string.Empty;
            public int Rating { get; set; }
            public string VideoTitle { get; set; } = string.Empty;

            // Νέα πεδία για μέσο όρο και αριθμό αξιολογήσεων
            public double AverageRating { get; set; }
            public int RatingCount { get; set; }

            // Υπολογιζόμενο πεδίο για εμφάνιση πληροφοριών
           // public string DisplayInfo => $"{VideoTitle}, Μέσος Όρος Αξιολογήσεων: {AverageRating:F1}, {RatingCount} Αξιολογήσεις";
        }



        public static List<VideoRating> GetVideoRatings()
        {
            var videoRatings = new List<VideoRating>();

            string connectionString = "server=localhost; uid=root; pwd=gr3ty; database=videoclipdb";
            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();

                string sql = "SELECT username, rating, video_title FROM video_ratings";
                using (var cmd = new MySqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var videoRating = new VideoRating
                            {
                                Username = reader["username"].ToString(),
                                Rating = Convert.ToInt32(reader["rating"]),
                                VideoTitle = reader["video_title"].ToString()
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
    }
}