using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;


namespace MaisonEcommerce.Repositorio
{

    public class ServicoPlanoRepositorio(IConfiguration configuration)
    {
        //private readonly string _conexaoMySQL = configuration.GetConnectionString("XonexaoMySQL");

        //public int Cadastrar(Servico_Plano servicoPlano)
        //{
        //    using (var conexao = new MySqlConnection(_conexaoMySQL))
        //    {
        //        conexao.Open();

        //        MySqlCommand cmd = new MySqlCommand("Call insertServicoPlano(@servico, @plano);", conexao);
        //        cmd.Parameters.Add("@servico", MySqlDbType.VarChar).Value = servicoPlano.NomeServico;
        //        cmd.Parameters.Add("@plano", MySqlDbType.VarChar).Value = servicoPlano.NomePlano;

        //        int linhasAfetadas = cmd.ExecuteNonQuery();
        //        conexao.Close();
        //        return linhasAfetadas;
        //    }
        //}

        //public bool Atualizar(Servico_Plano ServicoPlano)
        //{
        //    try
        //    {
        //        using (var conexao = new MySqlConnection(_conexaoMySQL))
        //        {
        //            conexao.Open();

        //            MySqlCommand cmd = new MySqlCommand("Update tb_Servico_Plano set IdServico=@servico, IdPlano@plano where IdServicoPlano=@idServico", conexao);
        //            cmd.Parameters.Add("@idServicoPlano", MySqlDbType.Int32).Value = ServicoPlano.IdServicoPlano;
        //            cmd.Parameters.Add("@servico", MySqlDbType.Int32).Value = ServicoPlano.NomeServico;
        //            cmd.Parameters.Add("@plano", MySqlDbType.Int32).Value = ServicoPlano.NomePlano;

        //            int linhasAfetadas = cmd.ExecuteNonQuery();
        //            return linhasAfetadas > 0;
        //        }
        //    }

        //    catch (MySqlException ex)
        //    {
        //        Console.WriteLine($"Erro ao alterar serviço-plano: {ex.Message}");
        //        return false;
        //    }
        //}

        //public IEnumerable<Servico_Plano> TodosServicoPlano()
        //{
        //    List<Servico_Plano> servico_Plamos = new List<Servico_Plano>();

        //    using (var conexao = new MySqlConnection(_conexaoMySQL))
        //    {
        //        conexao.Open();
        //        MySqlCommand cmd = new MySqlCommand("select tb_Servico_Plano.IdServicoPlano, tb_Servico.Nome as Nome_Servico, tb_Plano.Nome as Nome_Plano \r\n" +
        //            "fromt tb_Plano, tb_Servico, tb_Servico_Plano\r\n" +
        //            "where tb_Servico_Plano");
        //    }
        //}
    }

   }
