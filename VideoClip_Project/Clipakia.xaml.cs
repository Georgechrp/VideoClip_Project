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


        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
           
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

        private void DeleteVideoFromFolder(string videoTitle)
        {
            try
            {
                // Φάκελος προορισμού όπου βρίσκεται το βίντεο
                string destinationFolder = "C:\\Users\\georg\\GITHUB PROJECTS\\VideoClip_Project\\videos";

                // Παίρνεις το όνομα και την επέκταση του αρχείου (π.χ., my_video.mp4)
                string fileName = System.IO.Path.GetFileName("C:\\Users\\georg\\Downloads\\" + videoTitle + ".mp4");

                // Ο πλήρης προορισμός του αρχείου
                string filePath = System.IO.Path.Combine(destinationFolder, fileName);

                // Έλεγχος αν το αρχείο υπάρχει πριν τη διαγραφή
                if (File.Exists(filePath))
                {
                    // Διαγραφή του αρχείου
                    File.Delete(filePath);
                    MessageBox.Show("Το βίντεο διαγράφηκε επιτυχώς από το φάκελο.");
                }
                else
                {
                    MessageBox.Show("Το αρχείο δεν βρέθηκε.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Αποτυχία διαγραφής του βίντεο: " + ex.Message);
            }
        }
        private void DeleteClipFromDatabase(string title)
        {
            try
            {
                string connstring = "server=localhost; uid=root; pwd=gr3ty; database=ratemyclipdb";
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();
                    // SQL query για διαγραφή του βίντεο από τον πίνακα
                    string sql = "DELETE FROM video_clips WHERE title = @title";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Προσθήκη παραμέτρου για την αποφυγή SQL injection
                        cmd.Parameters.AddWithValue("@title", title);

                        // Εκτέλεση της διαγραφής
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Έλεγχος αν έγινε η διαγραφή
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Το βίντεο " + title + " διαγράφηκε επιτυχώς από τη βάση.");
                        }
                        else
                        {
                            MessageBox.Show("Αποτυχία διαγραφής από τη βάση.");
                        }
                    }
                    con.Close();
                }
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.ToString());
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Πρώτα διαγράφεις το βίντεο από τον φάκελο
            DeleteVideoFromFolder(videoTitle);

            // Στη συνέχεια διαγράφεις την αντίστοιχη εγγραφή από τη βάση δεδομένων
            DeleteClipFromDatabase(videoTitle);
           
        }

        private void GotoMainButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            this.Visibility = Visibility.Hidden;
            menuWindow.Show();
        }
    }
}




