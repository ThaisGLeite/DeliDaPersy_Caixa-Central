using Newtonsoft.Json;

namespace Caixa_Central
{
    internal class Pedido
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("preco")]
        public decimal Valor { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        public Pedido(string nome, decimal valor, int quantidade)
        {
            Nome = nome;
            Valor = valor;
            Quantidade = quantidade;
        }
    }
}
