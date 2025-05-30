using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Prng;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class PlanoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar(Plano plano)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertPlano(@nome, @descricao, @duracao, @preco)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = plano.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                cmd.Parameters.Add("@duracao", MySqlDbType.Int32).Value = plano.DuracaoPlano;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = plano.Preco;

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
                return linhasAfetadas;
            }
        }

        public bool Atualizar(Plano plano)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Plano set Nome=@nome, Descricao=@descricao, DuracaoPlano=@duracao, Preco=@preco where IdPlano=@idPlano", conexao);
                    cmd.Parameters.Add("@IdPlano", MySqlDbType.Int32).Value = plano.IdPlano;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = plano.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                    cmd.Parameters.Add("@duracao", MySqlDbType.Int32).Value = plano.DuracaoPlano;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = plano.Preco;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
