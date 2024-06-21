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

namespace VideoClip_Project
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public static string videoTitle = "";
        public string ResponseText
        {
            get; set;
        }
        public InputDialog(string question)
        {
            InitializeComponent();
            ResponseText = string.Empty;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = txtResponse.Text;
            videoTitle = txtResponse.Text;
            this.DialogResult = true;
            this.Close();
        }


        private void txtResponse_GotFocus(object sender, RoutedEventArgs e)
        {
            txtResponse.Text = "";
            txtResponse.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void txtResponse_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtResponse.Text == "")
            {
                txtResponse.Text = "Πληκτρολογήστε το όνομα του video clip σας";
                txtResponse.Foreground = new SolidColorBrush(Colors.Gray);
            }

        }

    }
}
