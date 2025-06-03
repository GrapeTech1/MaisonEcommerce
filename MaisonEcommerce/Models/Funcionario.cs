using System.Data;

namespace MaisonEcommerce.Models
{
    public class Funcionario
    {
        public int IdFuncionario { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; } // tirar dps
        public string Sexo { get; set; }
        public string Cargo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
