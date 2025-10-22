using Agenda.Mdl;
using Agenda.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Svc
{
    internal class SvcTarefa
    {
        public static int AddTarefa(DateTime dataInicio, DateTime dataFim, string descricao)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string verificaSql = @"SELECT COUNT(*) FROM Tarefa
                                        WHERE (DataInicio <= :DataFim)
                                        AND (DataFim >= :DataInicio)";

;
                using (OracleCommand cmdVerifica = new OracleCommand(verificaSql, conn))
                {
                    cmdVerifica.Parameters.Add(":DataInicio", dataInicio);
                    cmdVerifica.Parameters.Add(":DataFim", dataFim);

                    int count = Convert.ToInt32(cmdVerifica.ExecuteScalar());

                    if (count > 0)
                    {
                        throw new Exception("Já existe uma tarefa nesse período. Escolha outro intervalo de tempo.");
                    }

                }

                string sql = "INSERT INTO Tarefa(Datainicio, Datafim, Descricao) " +
                            "VALUES(:Datainicio, :DataFim, :Descricao) RETURNING Id INTO :id";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":DataInicio", dataInicio);
                    cmd.Parameters.Add(":DataFim", dataFim);
                    cmd.Parameters.Add(":Descricao", descricao);
                    OracleParameter pid = new OracleParameter(":id", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                    cmd.Parameters.Add(pid);

                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(pid.Value.ToString());
                }
            }
        }

        public static void AddTarefaContato(int idTarefa, int idContato)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "INSERT INTO TarefaContato (IdTarefa, IdContato) " +
                            "VALUES (:idTarefa, :idContato)";
                using (OracleCommand cmd = new OracleCommand (sql, conn))
                {
                    cmd.Parameters.Add(":idTarefa", idTarefa);
                    cmd.Parameters.Add(":idContato", idContato);
                    cmd.ExecuteNonQuery();
                }
            }
        }




    }
}
