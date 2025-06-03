using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Prng;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class AgendamentoRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");
        // Implementar métodos para cadastrar, atualizar, excluir e consultar agendamentos
        // Exemplo de método para cadastrar um agendamento
        public int Cadastrar(Agendamento agendamento)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("Call insertAgendamento(@data, @hora, @clienteId)", conexao);
                cmd.Parameters.Add("@data", MySqlDbType.Date).Value = agendamento.Data;
                cmd.Parameters.Add("@hora", MySqlDbType.Time).Value = agendamento.Hora;
                cmd.Parameters.Add("@clienteId", MySqlDbType.Int32).Value = agendamento.ClienteId;
                int linhasAfetadas = cmd.ExecuteNonQuery();
                conexao.Close();
                return linhasAfetadas;
            }
        }
        // Outros métodos (Atualizar, Excluir, Consultar) podem ser implementados aqui
    } 
    {
        
    }
}
