using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace TCC
{
    public partial class MainWindow : Window
    {
        private MySqlConnection Conexao;
        private string connectionString = "Server=localhost;Database=Cadastros;Uid=root;Pwd=;";

        public MainWindow()
        {
            InitializeComponent();
            txtEmailLOGIN.Focus();
        }

        public static class SessaoUsuario
        {
            public static int UsuarioId { get; set; }
            public static string Nome { get; set; }
            public static string Email { get; set; }
        }

        public bool ValidarLogin(string email, string senha)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM usuariosCadastrados WHERE email = @Email AND Senha = @Senha";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Senha", senha);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
                return false;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Visible;
            CadastroPanel.Visibility = Visibility.Collapsed;
        }

        private void BtnCadastro_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            CadastroPanel.Visibility = Visibility.Visible;
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmailLOGIN.Text;
            string senha = txtSenhaLOGIN.Password;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT cod, usuario FROM usuariosCadastrados WHERE email = @Email AND Senha = @Senha";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Senha", senha);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SessaoUsuario.UsuarioId = reader.GetInt32("cod");
                            SessaoUsuario.Nome = reader.GetString("usuario");
                            SessaoUsuario.Email = email;

                            MessageBox.Show("Login bem-sucedido!");
                            PrincipalWindow1 novaJanela = new PrincipalWindow1();
                            novaJanela.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Email ou senha inválidos.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
            }
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Password.Trim();
            string confirmaSenha = txtConfirmaSenha.Password.Trim();
            string telefone = txtTelefone.Text.Trim();
            string regiao = (cmbRegiao.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (!Regex.IsMatch(nome, @"^[A-Za-zÀ-ÿ ]{6,}$"))
            {
                MessageBox.Show("Nome inválido! Use apenas letras e mínimo de 6 caracteres.");
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("E-mail inválido!");
                return;
            }

            if (!Regex.IsMatch(senha, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"))
            {
                MessageBox.Show("Senha inválida! Mínimo 6 caracteres, incluindo letras e números.");
                return;
            }

            if (senha != confirmaSenha)
            {
                MessageBox.Show("As senhas não coincidem.");
                return;
            }

            if (!Regex.IsMatch(telefone, @"^\d{8,15}$"))
            {
                MessageBox.Show("Telefone inválido! Use apenas números (mínimo 8 dígitos).");
                return;
            }

            if (string.IsNullOrEmpty(regiao))
            {
                MessageBox.Show("Selecione uma região.");
                return;
            }

            try
            {
                using (Conexao = new MySqlConnection(connectionString))
                {
                    string sql = "INSERT INTO usuariosCadastrados (usuario, email, senha, telefone, regiao) " +
                                 "VALUES (@nome, @email, @senha, @telefone, @regiao)";
                    MySqlCommand comando = new MySqlCommand(sql, Conexao);
                    comando.Parameters.AddWithValue("@nome", nome);
                    comando.Parameters.AddWithValue("@email", email);
                    comando.Parameters.AddWithValue("@senha", senha);
                    comando.Parameters.AddWithValue("@telefone", telefone);
                    comando.Parameters.AddWithValue("@regiao", regiao);

                    Conexao.Open();
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                txtNome.Clear();
                txtEmail.Clear();
                txtSenha.Clear();
                txtConfirmaSenha.Clear();
                txtTelefone.Clear();
                cmbRegiao.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnirpgPrincipal_Click(object sender, RoutedEventArgs e)
        {
            PrincipalWindow1 novaJanela = new PrincipalWindow1();
            novaJanela.Show();
            this.Hide();
        }
    }
}
