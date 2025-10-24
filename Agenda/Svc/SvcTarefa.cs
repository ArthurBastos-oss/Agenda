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
        public static bool VerificaTarefa(DateTime dataInicio, DateTime dataFim)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                using (OracleCommand cmd = new OracleCommand("PKG_Tarefa.VerificaTarefa", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Parâmetro de retorno deve vir primeiro
                    OracleParameter retorno = new OracleParameter("RETURN_VALOR", OracleDbType.Int32);
                    retorno.Direction = System.Data.ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.Parameters.Add("pDataInicio", OracleDbType.Date).Value = dataInicio;
                    cmd.Parameters.Add("pDataFim", OracleDbType.Date).Value = dataFim;

                    cmd.ExecuteNonQuery();

                    int count = ((Oracle.ManagedDataAccess.Types.OracleDecimal)retorno.Value).ToInt32();

                    return count > 0;
                }

            }
        }


        public static int AddTarefa(DateTime dataInicio, DateTime dataFim, string descricao, Recorrencia recorrencia)
        {
            if (VerificaTarefa(dataInicio, dataFim))
                throw new Exception("Já existe uma tarefa nesse período. Escolha outro intervalo de tempo.");

            if (dataFim < dataInicio)
                throw new Exception("Data de término deve vir após a data de início.");

            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = @"INSERT INTO Tarefa (DataInicio, DataFim, Descricao, Recorrencia)
                       VALUES (:DataInicio, :DataFim, :Descricao, :Recorrencia)
                       RETURNING Id INTO :id";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":DataInicio", dataInicio);
                    cmd.Parameters.Add(":DataFim", dataFim);
                    cmd.Parameters.Add(":Descricao", descricao);
                    cmd.Parameters.Add(":Recorrencia", (int)recorrencia);

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
                
                using (OracleCommand cmd = new OracleCommand(sql, conn)) 
                { 
                    cmd.Parameters.Add(":idTarefa", idTarefa); 
                    cmd.Parameters.Add(":idContato", idContato);
                    cmd.ExecuteNonQuery(); 
                } 
            } 
        }
    }
}
