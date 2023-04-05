using Newtonsoft.Json;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace Caixa_Central
{
    public partial class JanelaCentral : Form
    {
        readonly Usuario usuario;
        private HttpClient httpClient;

        public JanelaCentral(string nome)
        {
            InitializeComponent();
            usuario = new(nome);
            this.Text = "Logado como: " + usuario.Nome;
            httpClient = new HttpClient();
        }

        private void JanelaCentral_FormClosed(object sender, FormClosedEventArgs e) => Application.Exit();

        private void TextBoxCadastroAssinantesSobreNome_TextChanged(object sender, EventArgs e)
        {
            if ((textBoxCadastroAssinantesNome.Text.Length > 3) && (textBoxCadastroAssinantesSobreNome.Text.Length > 3))
            {
                groupBoxCadastroAssinantesPlanoEscolhido.Visible = true;
            }
        }

        private void RadioButtonCadastroAssinantesForFun_Click(object sender, EventArgs e)
        {
            if (radioButtonCadastroAssinantesHolics.Checked || radioButtonCadastroAssinantesForFun.Checked || radioButtonCadastroAssinantesFamily.Checked)
            {
                groupBoxCadastroAssinantesTempoPlano.Visible = true;
            }
        }

        private void RadioButtonCadastroAssinantes1mes_Click(object sender, EventArgs e)
        {
            if (radioButtonCadastroAssinantes1mes.Checked || radioButtonCadastroAssinantes6meses.Checked || radioButtonCadastroAssinantes12meses.Checked)
            {
                buttonCadastroAssianteConfirmarDados.Visible = true;
            }
        }

        private async void ButtonCadastroAssianteConfirmarDados_Click(object sender, EventArgs e)
        {
            //Tornar tudo visivel
            groupBoxCadastroAssinantesPagar.Visible = true;
            labelCadastroAssinantesValorTotal.Visible = true;
            labelCadastroAssinantesValorTotalTexto.Visible = true;

            //Vai na api pegar o valor do plano escolhido e espera um pouquinho para continuar
            string valorAssinaturaText = await GetValorPlano();
            decimal valorAssinatura = decimal.Parse(valorAssinaturaText, CultureInfo.InvariantCulture);
            string formattedValue = valorAssinatura.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

            labelCadastroAssinantesValorTotal.Text = formattedValue;
            //TODO: Ir na api para preencher o saldo de persycoins
            // ----
            labelCadastroAssinantesSaldoPersyCoins.Text = "R$ 0,00";
            decimal saldoPersyCoins = 0.0M;
            currencyTextBoxCadastroAssinantePersyCoins.MaxValue = saldoPersyCoins;

            //Seta o valor de todo mundo para no máximo o valor da assinatura
            currencyTextBoxCadastroAssinanteCredito.MaxValue = valorAssinatura;
            currencyTextBoxCadastroAssinanteDebito.MaxValue = valorAssinatura;
            currencyTextBoxCadastroAssinanteDinheiro.MaxValue = valorAssinatura;
            currencyTextBoxCadastroAssinantePicpay.MaxValue = valorAssinatura;
            currencyTextBoxCadastroAssinantePix.MaxValue = valorAssinatura;
        }

        private async Task<string> GetValorPlano()
        {
            //Ir na api para preencher o valor do plano escolhido           
            string responseContent;

            //Preencher a URL com o plano escolhido
            string url = "http://h6sdpd5uhc.execute-api.us-east-1.amazonaws.com/assinatura/planos";
            if (radioButtonCadastroAssinantesForFun.Checked)
            {
                url += "/Fun";
            }
            else if (radioButtonCadastroAssinantesHolics.Checked)
            {
                url += "/Holics";
            }
            else if (radioButtonCadastroAssinantesFamily.Checked)
            {
                url += "/Family";
            }
            if (radioButtonCadastroAssinantes1mes.Checked)
            {
                url += "/1";
            }
            else if (radioButtonCadastroAssinantes6meses.Checked)
            {
                url += "/6";
            }
            else if (radioButtonCadastroAssinantes12meses.Checked)
            {
                url += "/12";
            }
            try
            {
                // Send GET request to API
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // Check if response is successful
                response.EnsureSuccessStatusCode();

                // Read response content
                responseContent = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                responseContent = "error";
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            return responseContent.ToLower();
        }

        private async void ButtonCadastroAssiantePagar_ClickAsync(object sender, EventArgs e)
        {
            decimal troco = decimal.Parse(labelCadastroAssinantesTroco.Text, CultureInfo.InvariantCulture);

            //Chamar a API para gravar o novo assinante
            string plano = "Fun";
            if (radioButtonCadastroAssinantesHolics.Checked)
            {
                plano = "Holics";
            }
            else if (radioButtonCadastroAssinantesFamily.Checked)
            {
                plano = "Family";
            }
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddMonths(1);
            if (radioButtonCadastroAssinantes6meses.Checked)
            {
                futureDate = currentDate.AddMonths(6);
            }
            else if (radioButtonCadastroAssinantes12meses.Checked)
            {
                futureDate = currentDate.AddMonths(12);

            }
            string validade = futureDate.ToString("yyyy-MM-dd");

            var assinante = new
            {
                nome = textBoxCadastroAssinantesNome.Text,
                sobrenome = textBoxCadastroAssinantesSobreNome.Text,
                plano,
                validade,
                datainicio = currentDate.ToString("yyyy-MM-dd"),
            };
            string responseContent = await PostNewAssinante(assinante);
            MessageBox.Show(responseContent);
            //Deu tudo certo sai limpando todos os campos
            LimparAbaAssinantes();
        }

        private async Task<string> PostNewAssinante(object assinante)
        {
            var json = JsonConvert.SerializeObject(assinante);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = "http://h6sdpd5uhc.execute-api.us-east-1.amazonaws.com/assinatura/novaassinatura";
            var response = await httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Erro ao criar um novo assinante " + response.StatusCode;
            }
        }

        private void LimparAbaAssinantes()
        {
            textBoxCadastroAssinantesNome.Text = string.Empty;
            textBoxCadastroAssinantesSobreNome.Text = string.Empty;
            groupBoxCadastroAssinantesPlanoEscolhido.Visible = false;
            groupBoxCadastroAssinantesTempoPlano.Visible = false;
            buttonCadastroAssianteConfirmarDados.Visible = false;
            groupBoxCadastroAssinantesPagar.Visible = false;
            labelCadastroAssinantesValorTotal.Visible = false;
            labelCadastroAssinantesValorTotalTexto.Visible = false;
            groupBoxCadastroAssinantesPagar.Visible = false;
        }

        private void CalcularTotal()
        {
            decimal valorTotal = 0;
            valorTotal += currencyTextBoxCadastroAssinantePix.DecimalValue;
            valorTotal += currencyTextBoxCadastroAssinantePicpay.DecimalValue;
            valorTotal += currencyTextBoxCadastroAssinantePersyCoins.DecimalValue;
            valorTotal += currencyTextBoxCadastroAssinanteDinheiro.DecimalValue;
            valorTotal += currencyTextBoxCadastroAssinanteDebito.DecimalValue;
            valorTotal += currencyTextBoxCadastroAssinanteCredito.DecimalValue;

            labelCadastroAssinantesTotalSendoPago.Text = valorTotal.ToString();
            decimal valorAssinatura = DeLabelParaDecimal(labelCadastroAssinantesValorTotal.Text);
            decimal troco = valorTotal - valorAssinatura;
            labelCadastroAssinantesTroco.Text = troco.ToString();

            if (valorTotal >= valorAssinatura)
            {
                buttonCadastroAssiantePagar.Visible = true;
            }

        }

        private static decimal DeLabelParaDecimal(string valor)
        {
            return decimal.Parse(valor, NumberStyles.Currency);
        }

        private void CurrencyTextBoxCadastroAssinantePersyCoins_TextChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void TabPageClientes_Enter(object sender, EventArgs e)
        {
            //TODO: Atualizar a lista de clientes
            // atualizar o array de mesas ocupadas
        }

        private void ButtonCliente1_Click(object sender, EventArgs e)
        {
            groupBoxClientesNovaMesa.Visible = true;

        }
    }
}
