namespace MaisonEcommerce.Models
{
    public class Agendamento
    {
        public int IdAgendamento { get; set; }
        public int IdCliente_Agen { get; set; } // foreign key para a tabela Cliente
        public int IdServico_Agen { get; set; } // foreign key para a tabela Servico
        public string CPF { get; set; }
        public string NomeCliente { get; set; }
        public string NomeServico { get; set; }
        public DateTime DataHora { get; set; } // data e hora do agendamento
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
