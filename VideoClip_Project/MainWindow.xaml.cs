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

namespace VideoClip_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogIn(object sender, RoutedEventArgs e)
        {
            LogIn obgLogIn = new LogIn();
            this.Visibility = Visibility.Hidden;
            obgLogIn.Show();
        }

        private void BtnSignUp(object sender, RoutedEventArgs e)
        {
            SignUp obgSignUp = new SignUp();
            this.Visibility = Visibility.Hidden;
            obgSignUp.Show();
        }
    }
}