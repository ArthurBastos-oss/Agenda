using Agenda.Data;
using Agenda.Mdl;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

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


        public static int AddTarefa(DateTime dataInicio, DateTime dataFim, string descricao, Recorrencia recorrencia, List<int> contatos = null)
        {
            if (VerificaTarefa(dataInicio, dataFim))
                throw new Exception("Já existe uma tarefa nesse período. Escolha outro intervalo de tempo.");

            if (dataFim < dataInicio)
                throw new Exception("Data de término deve vir após a data de início.");

            string contatosCsv = (contatos != null && contatos.Count > 0)
                ? string.Join(",", contatos)
                : null;

            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "BEGIN :pResult := PKG_TAREFA.CriarTarefaCompleta(:pDataInicio, :pDataFim, :pDescricao, :pRecorrencia, :pContatosCsv); END;";

                    var pDataInicio = cmd.Parameters.Add("pDataInicio", OracleDbType.Date);
                    pDataInicio.Value = dataInicio;

                    var pDataFim = cmd.Parameters.Add("pDataFim", OracleDbType.Date);
                    pDataFim.Value = dataFim;

                    var pDescricao = cmd.Parameters.Add("pDescricao", OracleDbType.Varchar2);
                    pDescricao.Size = 4000;
                    pDescricao.Value = descricao ?? (object)DBNull.Value;

                    var pRecorrencia = cmd.Parameters.Add("pRecorrencia", OracleDbType.Int32);
                    pRecorrencia.Value = (int)recorrencia;

                    var pContatosCsv = cmd.Parameters.Add("pContatosCsv", OracleDbType.Varchar2);
                    pContatosCsv.Size = 4000;
                    pContatosCsv.Value = contatosCsv ?? (object)DBNull.Value;

                    var pResult = cmd.Parameters.Add("pResult", OracleDbType.Decimal);
                    pResult.Direction = System.Data.ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    var od = (Oracle.ManagedDataAccess.Types.OracleDecimal)pResult.Value;
                    return od.IsNull ? 0 : od.ToInt32();
                }
            }
        }



    }
}
