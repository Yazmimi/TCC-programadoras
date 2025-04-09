using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TCC
{
    public partial class HomePage : Page
    {
        private int resdesen = 0;

        public HomePage()
        {
            InitializeComponent();
            InicializarMapaAsync();
            AtualizarClima();
            AtualizarMapa(2); // Exemplo inicial
        }

        private async void InicializarMapaAsync()
        {
            string pathHTML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "mapa.html");

            await webViewMapa.EnsureCoreWebView2Async();

            if (File.Exists(pathHTML))
            {
                string uriLocal = $"file:///{pathHTML.Replace("\\", "/")}";
                webViewMapa.CoreWebView2.Navigate(uriLocal);
            }
            else
            {
                System.Windows.MessageBox.Show("Arquivo mapa.html não encontrado.");
            }
        }

        public async void AtualizarMapa(int novoValor)
        {
            resdesen = novoValor;

            if (webViewMapa.CoreWebView2 != null)
            {
                try
                {
                    await webViewMapa.ExecuteScriptAsync($"atualizarIcone({resdesen});");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao atualizar ícone no mapa: " + ex.Message);
                }
            }
        }

        private async void AtualizarClima()
        {
            try
            {
                string apiKey = "548b073f98cffe2684c58d9c00c4714c"; // sua chave
                string url = $"https://api.openweathermap.org/data/2.5/forecast?q=Curitiba,br&appid={apiKey}&units=metric&lang=pt_br";

                using HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(url);
                var json = JsonDocument.Parse(response);

                var previsao = json.RootElement.GetProperty("list")[0];
                double temperatura = previsao.GetProperty("main").GetProperty("temp").GetDouble();
                string descricao = previsao.GetProperty("weather")[0].GetProperty("description").GetString();

                txtTemperatura.Text = $"Temp. prevista: {temperatura:0.#} °C";
                txtDescricao.Text = $"Descrição: {descricao}";

                if (previsao.TryGetProperty("pop", out JsonElement popElement))
                {
                    double chanceChuva = popElement.GetDouble() * 100;
                    txtChuva.Text = $"Chance de chuva: {chanceChuva:0.#}%";
                }
                else
                {
                    txtChuva.Text = "Sem previsão de chuva.";
                }
            }
            catch
            {
                txtTemperatura.Text = "Erro ao obter clima.";
                txtDescricao.Text = "";
                txtChuva.Text = "Erro ao obter previsão.";
            }

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(10)
            };
            timer.Tick += (s, e) => AtualizarClima();
            timer.Start();
        }
    }
}
