using Agenda.Data;
using Agenda.Mdl;
using Agenda.Svc;

namespace Agenda.Tst
{
    [TestClass]
    public class SvcTarefaTst
    {
        [TestInitialize]
        public void Setup()
        {
            using var conn = new Conexao().AbrirConexao();

            SvcTarefa.LimparTarefasDeTeste(conn);

        }

        [TestMethod]
        public void TesteVerificaTarefa_TarefaExistente()
        {
            DateTime inicio = new DateTime(2025, 10, 27, 10, 0, 0);
            DateTime fim = new DateTime(2025, 10, 27, 12, 0, 0);

            bool verificar = SvcTarefa.VerificaTarefa(inicio, fim);
            Assert.IsTrue(verificar, "O método retornou falso mesmo sem tarefas existentes.");
        }

        [TestMethod]
        public void TesteVerificaTarefa_TarefaNaoExistente()
        {
            DateTime inicio = new DateTime(2027, 04, 03, 10, 0, 0);
            DateTime fim = new DateTime(2027, 04, 03, 12, 0, 0);

            bool verificar = SvcTarefa.VerificaTarefa(inicio, fim);
            Assert.IsFalse(verificar, "O método retornou verdairo mesmo sem tarefas existentes.");
        }

        [TestMethod]
        public void TesteAddTarefa()
        {
            DateTime inicio = new DateTime(2027, 04, 03, 10, 0, 0);
            DateTime fim = new DateTime(2027, 04, 03, 12, 0, 0);
            string descrição = "Teste";

            int idTarefa = SvcTarefa.AddTarefa(inicio, fim, descrição, Recorrencia.Nenhuma);

            Assert.IsTrue(idTarefa > 0, "O id não foi retornado");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TesteAddTarefaExistente()
        {
            DateTime inicio = new DateTime(2027, 04, 03, 10, 0, 0);
            DateTime fim = new DateTime(2027, 04, 03, 12, 0, 0);
            string descrição = "Teste";

            SvcTarefa.AddTarefa(inicio, fim, descrição, Recorrencia.Nenhuma);

            SvcTarefa.AddTarefa(inicio, fim, "Outra Tarefa", Recorrencia.Nenhuma);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TesteAddTarefa_DataInvetida()
        {
            DateTime inicio = new DateTime(2027, 04, 03, 12, 0, 0);
            DateTime fim = new DateTime(2027, 04, 03, 10, 0, 0);
            string descrição = "Teste";

            SvcTarefa.AddTarefa(inicio, fim, descrição, Recorrencia.Nenhuma);
        }

        [TestMethod]
        public void TesteAddTarefaSemContatos()
        {
            DateTime inicio = new DateTime(2027, 04, 03, 10, 0, 0);
            DateTime fim = new DateTime(2027, 04, 03, 12, 0, 0);
            string descrição = "Teste";

            int idTarefa = SvcTarefa.AddTarefa(inicio, fim, descrição, Recorrencia.Nenhuma, null);

            Assert.IsTrue(idTarefa > 0, "O id não foi retornado");
        }

        [TestMethod]
        public void TesteAddTarefaComContato()
        {
            var contato1 = new Contato();
            contato1.Nome = "Teste1";
            contato1.Email = "teste1%@teste.com";
            contato1.Telefone = "1111-1111";
            SvcContato.AddContato(contato1);

            var contato2 = new Contato();
            contato2.Nome = "Teste2";
            contato2.Email = "teste2%@teste.com";
            contato2.Telefone = "2222-2222";
            SvcContato.AddContato(contato2);

            DateTime inicio = new DateTime(2027, 04, 03, 10, 0, 0);
            DateTime fim = new DateTime(2027, 04, 03, 12, 0, 0);
            string descrição = "Teste";
            List<int> contatos = new List<int> { contato1.Id, contato2.Id };

            int id = SvcTarefa.AddTarefa(inicio, fim, descrição, Recorrencia.Nenhuma, contatos);

            Assert.IsTrue(id > 0, "O id não foi retornado");
        }

    }
}
