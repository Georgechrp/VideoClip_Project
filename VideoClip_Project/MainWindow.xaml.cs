using Microsoft.Win32;
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

        }

      

        private void LoadVideoFiles()
        {
            string folderPath = @"C:\Users\georg\OneDrive - unipi.gr\Επιφάνεια εργασίας\VideoClip_Project\videos";
            videoFiles = Directory.GetFiles(folderPath, "*.mp4");

            if (videoFiles.Length == 0)
            {
                MessageBox.Show("No video files found in the directory.");
            }
        }
        private void DisplayCurrentVideo()
        {
            if (videoFiles.Length > 0)
            {
                string currentVideoPath = videoFiles[currentIndex];
                mediaElement.Source = new Uri(currentVideoPath);
                videoNameTextBlock.Text = System.IO.Path.GetFileName(currentVideoPath);
            }
        }
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


        private string PromptForTitle()
        {
            InputDialog inputDialog = new InputDialog("Enter video title:");
            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.ResponseText;
            }
            return string.Empty;
        }

/*
        private void LoadVideos()
        {
            string filePath = "C:\\Users\\georg\\OneDrive - unipi.gr\\Επιφάνεια εργασίας\\VideoClip_Project\\videos";   //openFileDialog.FileName;
                                                                                                                        //mediaElement.Source = new Uri(filePath);

            // Φιλτράρετε τα αρχεία για να πάρετε μόνο βίντεο (εδώ υποθέτουμε .mp4 αρχεία)
            string[] videoFiles = Directory.GetFiles(filePath, "*.mp4");

            mediaElement.Source = new Uri(filePath);

            // Prompt user for the video title
            string videoTitle = filePath;
            if (!string.IsNullOrEmpty(videoTitle))
            {
                // Add video to the list
                VideoClip video = new VideoClip
                {
                    Title = videoTitle,
                    Path = filePath,
                    Rating = 0
                };
                videoClips.Add(video);
            }
            else
            {
                MessageBox.Show("Video title cannot be empty.");
            }
            
        }*/

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