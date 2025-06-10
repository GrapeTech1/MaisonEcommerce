namespace MaisonEcommerce.Models
{
    public class Servico_Pacote
    {
        public int IdServicoPacote { get; set; }
        public int IdServico { get; set; } 
        public int IdPacote { get; set; } 
        public string NomeServico { get; set; }
        public string NomePacote { get; set; }
    }
}
