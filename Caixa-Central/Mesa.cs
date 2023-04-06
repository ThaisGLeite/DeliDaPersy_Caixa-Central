using Newtonsoft.Json;

namespace Caixa_Central
{
    internal class Mesa
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("pedidos")]
        public Dictionary<string, Pedido> Pedidos { get; set; }

        public Mesa(int id, string client, Dictionary<string, Pedido> pedidos)
        {
            Id = id;
            Cliente = client;
            Pedidos = pedidos;
        }

    }
}
