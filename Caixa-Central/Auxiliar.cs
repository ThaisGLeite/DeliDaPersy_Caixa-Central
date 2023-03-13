using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Caixa_Central
{
    public static class Auxiliar
    {
        public static async Task Login(string n, string s)
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
                responseContent = responseContent.Substring(1, responseContent.Length - 2);
                if (responseContent.Equals("Authorized"))
                {
                    
                }
                else
                {
                    MessageBox.Show(responseContent);
                }
            }
            else
            {
                MessageBox.Show($"Erro ao fazer login {response.StatusCode}");
            }
        }

    }
}
