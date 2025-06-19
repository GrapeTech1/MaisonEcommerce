using System.Data;

namespace MaisonEcommerce.Models
{
    public class Pacote
    {
        public int IdPacote { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Desconto { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
