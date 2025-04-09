using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using static TCC.MainWindow;

namespace TCC
{
    public partial class ProfilePage : Page
    {
        private string connectionString = "Server=localhost;Database=Cadastros;Uid=root;Pwd=;";

        public ProfilePage()
        {
            InitializeComponent();
            CarregarDadosPerfil();
        }

        private void CarregarDadosPerfil()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT usuario, telefone, regiao, email FROM usuariosCadastrados WHERE cod = @UsuarioId";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UsuarioId", SessaoUsuario.Cod);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtNome.Text = reader["usuario"].ToString();
                            txtTelefone.Text = reader["telefone"].ToString();
                            txtRegiao.Text = reader["regiao"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do perfil: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            ProfileEditarPage profileEditarPage = new ProfileEditarPage();
            NavigationService.Navigate(profileEditarPage);
        }
    }
}
