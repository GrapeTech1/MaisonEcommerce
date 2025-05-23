namespace MaisonEcommerce.Models
{
    public class Servico_Pacote
    {
        public int IdServicoPacote { get; set; }
        public int IdServico { get; set; } // foreign key para a tabela Servico
        public int IdPacote { get; set; } // foreign key para a tabela Pacote
    }
}
