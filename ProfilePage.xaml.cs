using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using static TCC.MainWindow;

namespace TCC
{
    public partial class ProfilePage : Page
    {
        

        public ProfilePage()
        {
            InitializeComponent();
           
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            ProfileEditarPage profileEditarPage = new ProfileEditarPage();
            NavigationService.Navigate(profileEditarPage);
        }
    }
}
