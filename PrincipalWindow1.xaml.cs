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

namespace TCC
{
    /// <summary>
    /// Lógica interna para PrincipalWindow1.xaml
    /// </summary>
    public partial class PrincipalWindow1 : Window
    {
        public PrincipalWindow1()
        {
            InitializeComponent();
            MainFrame.Navigate(new HomePage()); // Página inicial
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NotificationsPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage());
        }

    }
}
