using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MaisonEcommerce.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string? CPF { get; set; }
        public string Nome { get; set; }
        public string? Telefone { get; set; }
        public int Idade { get; set; }
        public string? Sexo { get; set; }
        
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    
    }
}
