using Newtonsoft.Json;

namespace Caixa_Central
{
    internal class Mesa
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("pedidos")]
        public Dictionary<string, Pedido>? Pedidos { get; set; }

        public bool Ocupada { get; set; }

        public Mesa(string id, string client)
        {
            Id = id;
            Cliente = client;
        }
    }
}
