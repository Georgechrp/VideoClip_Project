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
                                Rating = Convert.ToInt32(reader["rating"]),
                                VideoTitle = reader["title"].ToString()
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





      
    }
}