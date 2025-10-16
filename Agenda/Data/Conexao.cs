using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Agenda.Data
{
    public class Conexao : IDisposable
    {
        private OracleConnection conexao;

        public OracleConnection AbrirConexao()
        {
            string connectionString =
            "User Id=aluno;Password=senha123;" +
            "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))" +
            "(CONNECT_DATA=(SERVICE_NAME=FREEPDB1)))";

            conexao = new OracleConnection(connectionString);
            conexao.Open();
            return conexao;
        }

        public void Dispose()
        {
            FecharConexao();
            conexao.Dispose();
        }

        public void FecharConexao()
        {
            if (conexao != null && conexao.State == System.Data.ConnectionState.Open)
                conexao.Close();
        }
    }
}
