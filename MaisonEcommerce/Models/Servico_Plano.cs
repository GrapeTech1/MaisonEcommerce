namespace MaisonEcommerce.Models
{
    public class Servico_Plano
    {
        public int IdServicoPlano { get; set; }
        public int IdServico { get; set; } // foreign key para a tabela Servico
        public int IdPlano { get; set; } // foreign key para a tabela Plano
    }
}
