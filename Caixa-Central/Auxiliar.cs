using Newtonsoft.Json;
using System.Text;

namespace Caixa_Central
{
    public static class Auxiliar
    {
        public static async Task<string> Login(string n, string s)
        {
            var data = new { nome = n, senha = s };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            string url = "http://srcdymw896.execute-api.us-east-1.amazonaws.com/api-login/logon";
            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                //Tem q retirar a primeira e ultima letras pq ele vem com as aspas
                responseContent = responseContent[1..^1];
                if (responseContent.Equals("Authorized"))
                {
                    return responseContent;
                }
                else
                {
                    MessageBox.Show(responseContent);
                    return "Erro";
                }
            }
            else
            {
                MessageBox.Show($"Erro ao fazer login {response.StatusCode}");
                return "Erro";
            }
        }

    }
}
