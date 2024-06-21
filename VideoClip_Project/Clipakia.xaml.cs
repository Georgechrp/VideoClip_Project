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

        public Clipakia()
        {

            InitializeComponent();
            LoadVideoClips();
        }

        // Method to save video clips to a JSON file
        private void SaveVideoClips()
        {
            var json = JsonConvert.SerializeObject(videoClips, Formatting.Indented);
            File.WriteAllText(SaveFilePath, json);
        }

        // Method to load video clips from a JSON file
        private void LoadVideoClips()
        {
            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);
                videoClips = JsonConvert.DeserializeObject<ObservableCollection<VideoClip>>(json) ?? new ObservableCollection<VideoClip>();
            }
            else
            {
                videoClips = new ObservableCollection<VideoClip>();
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video Files|*.mp4;*.avi;*.wmv;*.mov"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                mediaElement.Source = new Uri(filePath);

                // Prompt user for the video title
                string videoTitle = PromptForTitle();
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

        private string PromptForTitle()
        {
            InputDialog inputDialog = new InputDialog("Enter video title:");
            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.ResponseText;
            }
            return string.Empty;
        }



        private string connstring = "server=localhost; uid=root; pwd=gr3ty; database=videoclipdb";
        public void AddRating(string username, string rating, string videoTitle)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();

                    // SQL query to insert the rating into the video_ratings table
                    string sql = "INSERT INTO video_ratings (username, rating, video_title) VALUES (@username, @rating, @videoTitle)";

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
            AddRating(LogIn.username, stars, InputDialog.videoTitle);
           
            if (mediaElement.Source != null)
            {
                VideoClip? currentVideo = videoClips.FirstOrDefault(v => v.Path == mediaElement.Source.LocalPath);
                if (currentVideo != null)
                {
                    if (cbRating.SelectedItem is ComboBoxItem selectedRating && selectedRating.Content != null)
                    {
                        string? ratingString = selectedRating.Content.ToString();
                        if (!string.IsNullOrEmpty(ratingString))
                        {
                            if (int.TryParse(ratingString, out int rating))
                            {
                                currentVideo.Rating = rating;
                                SaveVideoClips();
                                MessageBox.Show($"Rated {currentVideo.Title} with {currentVideo.Rating} stars.");
                            }
                            else
                            {
                                MessageBox.Show("Invalid rating selected.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Rating content is null or empty.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a rating.");
                    }
                }
                else
                {
                    MessageBox.Show("No video clip found for the current media element source.");
                }
            }
            else
            {
                MessageBox.Show("Media element source is null.");
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            var results = videoClips.Where(v => v.Title.ToLower().Contains(searchText)).ToList();
            lbResults.Items.Clear();
            foreach (var result in results)
            {
                lbResults.Items.Add($"{result.Title} (Rating: {result.Rating})");
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                string videoTitle = PromptForTitle();
                if (!string.IsNullOrEmpty(videoTitle))
                {
                    VideoClip newVideoClip = new VideoClip
                    {
                        Title = videoTitle,
                        Path = mediaElement.Source.LocalPath,
                        Rating = 0 // Default rating, can be updated later
                    };

                    videoClips.Add(newVideoClip);
                    SaveVideoClips();
                    MessageBox.Show("Video clip saved successfully.");
                }
                else
                {
                    MessageBox.Show("Video title cannot be empty.");
                }
            }
            else
            {
                MessageBox.Show("No video clip to save.");
            }
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
    }
}




