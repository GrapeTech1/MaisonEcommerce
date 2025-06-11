using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;


namespace MaisonEcommerce.Repositorio
{
    
    public class ServicoPlanoRepositorio(IConfiguration configuration) 
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("XonexaoMySQL");

        public int Cadastrar(Servico_Plano servicoPlano)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertServicoPlano(@servico, @plano);", conexao);
                cmd.Parameters.Add("@servico", MySqlDbType.VarChar).Value = servicoPlano.NomeServico;
                cmd.Parameters.Add("@plano", MySqlDbType.VarChar).Value = servicoPlano.NomePlano;
                
                int linhasAfetadas = cmd .ExecuteNonQuery();
                conexao .Close();
                return linhasAfetadas;
            }
        }

        public bool Atualizar(Servico_Plano ServicoPlano)
        {
            try
        }
    }
}
