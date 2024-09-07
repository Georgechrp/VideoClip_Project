using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace VideoClip_Project
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private ObservableCollection<VideoClip> videoClips = new ObservableCollection<VideoClip>();
        private const string SaveFilePath = "videoClips.json";
        public MenuWindow()
        {
            InitializeComponent();
        }


        private void LoadVideoButton_Click(object sender, RoutedEventArgs e)
        {
            Clipakia clip = new Clipakia();
            this.Visibility = Visibility.Hidden;
            clip.Show();
        }

        private void DiscoverVideosButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            
            mainWindow.ButtonLogIn.Visibility = Visibility.Hidden;
            mainWindow.ButtonSignUp.Visibility = Visibility.Hidden;
            mainWindow.Show();
        }

        private void GotoMainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();

            //RateThisVideo.Visibility = Visibility.Hidden;
            //cbRating.Visibility = Visibility.Hidden;
            // btnRate.Visibility = Visibility.Hidden;
            mainWindow.RateThisVideo.Visibility = Visibility.Visible;
            mainWindow.cbRating.Visibility = Visibility.Visible;
            mainWindow.btnRate.Visibility = Visibility.Visible;

            mainWindow.ButtonLogIn.Visibility = Visibility.Hidden;
            mainWindow.ButtonSignUp.Visibility = Visibility.Hidden;
        }
    }
}
