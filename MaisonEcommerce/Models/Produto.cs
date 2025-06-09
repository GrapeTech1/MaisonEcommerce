using System.Data;

namespace MaisonEcommerce.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public byte[]? Foto { get; set; }
        public string? TipoFoto { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
