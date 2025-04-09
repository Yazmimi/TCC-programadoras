using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace TCC
{
    public partial class ProfileEditarPage : Page
    {
        private string connectionString = "Server=localhost;Database=Cadastros;Uid=root;Pwd=;";

        public ProfileEditarPage()
        {
            InitializeComponent();
            CarregarDadosUsuario();
        }

        private void CarregarDadosUsuario()
        {
            txtNome.Text = MainWindow.SessaoUsuario.Nome;
            txtEmail.Text = MainWindow.SessaoUsuario.Email;
            txtTelefone.Text = MainWindow.SessaoUsuario.Telefone;
            txtRegiao.Text = MainWindow.SessaoUsuario.Regiao;

            // Carrega a senha diretamente do banco
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT senha FROM usuariosCadastrados WHERE cod = @cod";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cod", MainWindow.SessaoUsuario.Cod);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string senha = txtSenha.Password; // ✅ Correto para PasswordBox


                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro ao carregar senha: " + ex.Message);
                }
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefone = txtTelefone.Text.Trim();
            string regiao = txtRegiao.Text.Trim();
            string senha = txtSenha.Password; // ✅ Correto para PasswordBox


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE usuariosCadastrados 
                             SET usuario = @nome, email = @email, telefone = @telefone, regiao = @regiao, senha = @senha
                             WHERE cod = @cod";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@regiao", regiao);
                    cmd.Parameters.AddWithValue("@senha", senha);
                    cmd.Parameters.AddWithValue("@cod", MainWindow.SessaoUsuario.Cod);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MainWindow.SessaoUsuario.Nome = nome;
                        MainWindow.SessaoUsuario.Email = email;
                        MainWindow.SessaoUsuario.Telefone = telefone;
                        MainWindow.SessaoUsuario.Regiao = regiao;

                        MessageBox.Show("Dados atualizados com sucesso!");

                        // Voltar para a página de perfil
                        NavigationService.Navigate(new ProfilePage());
                    }
                    else
                    {
                        MessageBox.Show("Nenhum dado foi atualizado.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro ao atualizar: " + ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage());
        }
    }
}
