using System.Data;

namespace MaisonEcommerce.Models
{
    public class Servico
    {
        public int IdServico { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
