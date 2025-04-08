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
            CarregarPerfil();
        }

        private void CarregarPerfil()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT usuario, email, telefone, cidade FROM usuariosCadastrados WHERE cod = @cod";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cod", SessaoUsuario.UsuarioId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblNome.Text = reader.GetString("usuario");
                            lblEmail.Text = reader.GetString("email");
                            lblTelefone.Text = reader.GetString("telefone");
                            lblLocal.Text = reader.GetString("cidade");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar o perfil: " + ex.Message);
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            txtNome.Text = lblNome.Text;
            txtEmail.Text = lblEmail.Text;
            txtTelefone.Text = lblTelefone.Text;
            txtLocal.Text = lblLocal.Text;

            ViewPanel.Visibility = Visibility.Collapsed;
            EditPanel.Visibility = Visibility.Visible;
            BtnEditar.Visibility = Visibility.Collapsed;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ViewPanel.Visibility = Visibility.Visible;
            EditPanel.Visibility = Visibility.Collapsed;
            BtnEditar.Visibility = Visibility.Visible;
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE usuariosCadastrados SET usuario = @nome, email = @email, telefone = @telefone, cidade = @cidade WHERE cod = @cod";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@cidade", txtLocal.Text);
                    cmd.Parameters.AddWithValue("@cod", SessaoUsuario.UsuarioId);

                    cmd.ExecuteNonQuery();

                    // Atualiza visualização
                    lblNome.Text = txtNome.Text;
                    lblEmail.Text = txtEmail.Text;
                    lblTelefone.Text = txtTelefone.Text;
                    lblLocal.Text = txtLocal.Text;

                    ViewPanel.Visibility = Visibility.Visible;
                    EditPanel.Visibility = Visibility.Collapsed;
                    BtnEditar.Visibility = Visibility.Visible;

                    MessageBox.Show("Dados atualizados com sucesso!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar alterações: " + ex.Message);
            }
        }
    }
}
