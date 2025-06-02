using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Prng;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class PacoteRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaMySQL = configuration.GetConnectionString("ConexaoMySQL");
        public int Cadastrar(Pacote pacote)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("Call insertPacote(@nome, @descricao, @preco)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = pacote.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = pacote.Descricao;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = pacote.Preco;
                int linhasAfetadas = cmd.ExecuteNonQuery();
                conexao.Close();
                return linhasAfetadas;
            }
        }
        public bool Atualizar(Pacote pacote)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update tb_Pacote set Nome=@nome, Descricao=@descricao, Preco=@preco where IdPacote=@idPacote", conexao);
                    cmd.Parameters.Add("@IdPacote", MySqlDbType.Int32).Value = pacote.IdPacote;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = pacote.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = pacote.Descricao;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = pacote.Preco;
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
    {
    }
}
