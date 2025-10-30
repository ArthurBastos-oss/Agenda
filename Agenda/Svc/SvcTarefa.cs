using Agenda.Data;
using Agenda.Mdl;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Transactions;

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


        public static int AddTarefa(List<Tarefa> pTarefas)
        {
            var erros = new Dictionary<string, List<string>>();

            foreach (var tarefa in pTarefas)
            {
                if (VerificaTarefa(tarefa.DataInicio, tarefa.DataFim))
                {
                    string erro = "Conflito de período";
                    if (!erros.ContainsKey(erro))
                        erros[erro] = new List<string>();

                    erros[erro].Add($"- ({tarefa.DataInicio} a {tarefa.DataFim})");
                }

                if (tarefa.DataFim < tarefa.DataInicio)
                {
                    string erro = "Data de término antes da data de início";
                    if (!erros.ContainsKey(erro))
                        erros[erro] = new List<string>();

                    erros[erro].Add($"- ({tarefa.DataInicio} a {tarefa.DataFim})");
                }
            }
            if (erros.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var kvp in erros)
                {
                    bool varias = kvp.Value.Count > 1; // detecta se tem mais de um erro do mesmo tipo

                    switch (kvp.Key)
                    {
                        case "Conflito de período":
                            sb.AppendLine(varias
                                ? "Já existem tarefas nos seguintes períodos:"
                                : "Já existe uma tarefa no seguinte período:");
                            break;

                        case "Data de término antes da data de início":
                            sb.AppendLine(varias
                                ? "Algumas tarefas possuem a data de término anterior à de início:"
                                : "A tarefa possui a data de término anterior à de início:");
                            break;
                    }

                    foreach (var detalhe in kvp.Value)
                        sb.AppendLine(detalhe);

                    sb.AppendLine(); // linha em branco entre grupos
                }

                throw new Exception(sb.ToString().Trim());
            }

            int tarefasCriadas = 0;

            using (var ts = new TransactionScope())
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {

                foreach (var tarefa in pTarefas)
                {
                    string contatoCsv = CsvListaContatos(tarefa.Contatos);

                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        //cmd.Transaction = ts;
                        cmd.BindByName = true;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "BEGIN :pResult := PKG_TAREFA.CriarTarefaCompleta(:pDataInicio, :pDataFim, :pDescricao, :pRecorrencia, :pContatosCsv); END;";

                        cmd.Parameters.Add("pDataInicio", OracleDbType.Date).Value = tarefa.DataInicio;
                        cmd.Parameters.Add("pDataFim", OracleDbType.Date).Value = tarefa.DataFim;
                        cmd.Parameters.Add("pDescricao", OracleDbType.Varchar2, 4000).Value = tarefa.Descricao ?? (object)DBNull.Value;
                        cmd.Parameters.Add("pRecorrencia", OracleDbType.Int32).Value = (int)tarefa.Recorrencia;
                        cmd.Parameters.Add("pContatosCsv", OracleDbType.Varchar2, 4000).Value = contatoCsv ?? (object)DBNull.Value;


                        var pResult = cmd.Parameters.Add("pResult", OracleDbType.Decimal);
                        pResult.Direction = System.Data.ParameterDirection.ReturnValue;

                        cmd.ExecuteNonQuery();

                        tarefasCriadas = ((Oracle.ManagedDataAccess.Types.OracleDecimal)pResult.Value).ToInt32();
                    }

                }
                ts.Complete();
            }

            return tarefasCriadas;

        }

        public static string CsvListaContatos(List<TarefaContato> listaContatos)
        {
            string contatosCsv = (listaContatos != null && listaContatos.Count > 0)
                ? string.Join(",", listaContatos.Select(c => c.IdContato))
                : null;
            return contatosCsv;
        }

        public static void LimparTarefasDeTeste()
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                using (var cmd = new OracleCommand(
                    @"DELETE FROM TarefaContato 
                    WHERE IdTarefa IN (SELECT Id FROM Tarefa WHERE Descricao LIKE 'Teste%')", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new OracleCommand(
                    @"DELETE FROM Tarefa WHERE Descricao LIKE 'Teste%'", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
