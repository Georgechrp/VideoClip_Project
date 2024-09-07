using Microsoft.Win32;
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
using VideoClip_Project.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using MySql.Data.MySqlClient;

namespace VideoClip_Project
{
    /// <summary>
    /// Interaction logic for Clipakia.xaml
    /// </summary>
    public partial class Clipakia : Window
    {
        private ObservableCollection<VideoClip> videoClips = new ObservableCollection<VideoClip>();
        private const string SaveFilePath = "videoClips.json";
        static string stars = "2";
        static string name_of_upload_video = "";
        string videoTitle;
        string filePath;
        public Clipakia()
        {
            InitializeComponent();
            LoadVideoButton_Click();
            //name_of_upload_video = InputDialog.videoTitle;
            videoNameTextBlock.Text = UserSession.Username;
            
        }

        private void LoadVideoButton_Click()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video Files|*.mp4;*.avi;*.wmv;*.mov"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                mediaElement.Source = new Uri(filePath);

                // Prompt user for the video title
                videoTitle = System.IO.Path.GetFileNameWithoutExtension(filePath);
                //videoTitle = filePath.Substring(0, filePath.Length - 4);    //PromptForTitle();
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
                    SaveVideoClips();
                }
                else
                {
                    MessageBox.Show("Video title cannot be empty.");
                }
            }
        }

        // Method to save video clips to a JSON file
        private void SaveVideoClips()
        {
            var json = JsonConvert.SerializeObject(videoClips, Formatting.Indented);
            File.WriteAllText(SaveFilePath, json);
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

        private string connstring = "server=localhost; uid=root; pwd=gr3ty; database=ratemyclipdb";
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

            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            var results = videoClips.Where(v => v.Title.ToLower().Contains(searchText)).ToList();
            lbResults.Items.Clear();
            foreach (var result in results)
            {
                lbResults.Items.Add($"{System.IO.Path.GetFileName(result.Title)} (Rating: {result.Rating})");
            }
        }

        public void AddVideoClip(string videoTitle, string rating)
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
                    string sql = "INSERT INTO video_clips (title, averageRating) VALUES (@videoTitle, @rating)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Add parameters to the SQL query to prevent SQL injection
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
        private void insertClipInDatabase(string title, int averageRating)
        {
            try
            {
                string connstring = "server=localhost; uid=root; pwd=gr3ty; database=ratemyclipdb";
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();
                    // SQL query to insert data into the 'user' table
                    string sql = "INSERT INTO video_clips (title, averageRating) VALUES (@title, @averageRating)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Add parameters to the SQL query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@averageRating", averageRating);


                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Display a message box to show the result
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Video" + videoTitle + "inserted Successfully");
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
                //MessageBox.Show(ex.ToString());
                Console.Write(ex.ToString());
            }
        }

        private void SaveVideoToFolder(string filePath)
        {
            try
            {
                // Προορισμός: Φάκελος 'Videos' μέσα στο project
                string destinationFolder = "C:\\Users\\georg\\GITHUB PROJECTS\\VideoClip_Project\\videos";

                // Δημιουργείς το φάκελο αν δεν υπάρχει
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                // Παίρνεις το όνομα και την επέκταση του αρχείου (π.χ., my_video.mp4)
                string fileName = System.IO.Path.GetFileName("C:\\Users\\georg\\Downloads\\" + videoTitle + ".mp4");

                // Ο πλήρης προορισμός, δηλαδή που θα αντιγραφεί το αρχείο
                string destinationPath = System.IO.Path.Combine(destinationFolder, fileName);

                // Αντιγράφεις το αρχείο από το source (filePath) στον προορισμό (destinationPath)
                File.Copy(filePath, destinationPath, true);  // true = αντικαθιστά το αρχείο αν υπάρχει ήδη

                MessageBox.Show("Το βίντεο αποθηκεύτηκε επιτυχώς στον φάκελο: " + destinationFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Αποτυχία αποθήκευσης του βίντεο: " + ex.Message);
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //AddVideoClip(name_of_upload_video, "0");
            SaveVideoToFolder(filePath);
            insertClipInDatabase(videoTitle, 0);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                VideoClip? currentVideo = videoClips.FirstOrDefault(v => v.Path == mediaElement.Source.LocalPath);
                if (currentVideo != null)
                {
                    videoClips.Remove(currentVideo);
                    SaveVideoClips();
                    MessageBox.Show($"Video clip '{currentVideo.Title}' deleted successfully.");
                    mediaElement.Source = null; // Clear the mediaElement source if necessary
                }
                else
                {
                    MessageBox.Show("No video clip found to delete.");
                }
            }
            else
            {
                MessageBox.Show("No video clip to delete.");
            }
        }

        private void GotoMainButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            this.Visibility = Visibility.Hidden;
            menuWindow.Show();
        }
    }
}




