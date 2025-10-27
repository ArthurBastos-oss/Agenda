using Agenda.Mdl;
using Agenda.Svc;
using Agenda.Data;
using Oracle.ManagedDataAccess.Client;

namespace Agenda.Tst
{
    [TestClass]
    public class SvcContatoTst
    {
        private OracleConnection conn;

        [TestInitialize]
        public void Setup()
        {
            conn = new Conexao().AbrirConexao();

            using (var cmd = new OracleCommand("DELETE FROM Contato WHERE Email LIKE 'teste%@teste.com'", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        { 
            conn.Close();
            conn.Dispose();
        }

        [TestMethod]
        public void TesteListar()
        {                       
            var lista = SvcContato.ListarContato();
            
            Assert.IsTrue(lista.Count > 0);
        }

        [TestMethod]
        public void TesteAddContato()
        {
            var contato = new Contato();
            contato.Nome = "Guilherme";
            contato.Email = "teste%@teste.com";
            contato.Telefone = "1521-3094";

            SvcContato.AddContato(contato);

            Assert.IsTrue(contato.Id > 0, "O contato não foi inserido");
        }

        [TestMethod]
        [ExpectedException(typeof(OracleException))]
        public void TesteAddContatoInvelido()
        {
            var contato = new Contato();
            contato.Nome = null;
            contato.Email = "teste%@teste.com";
            contato.Telefone = "1521-3094";

            SvcContato.AddContato(contato);
        }

        [TestMethod]
        public void TesteEditContato()
        {
            var contato = new Contato();
            contato.Nome = "Teste";
            contato.Email = "teste%@teste.com";
            contato.Telefone = "9999-9999";

            SvcContato.AddContato(contato);

            contato.Nome = "Teste Atualizado";
            contato.Email = "testeup@testeup.com";
            contato.Telefone = "0000-0000";

            SvcContato.EditContato(contato);

            using (var cmd = new OracleCommand(
                "SELECT Nome, Email, Telefone FROM Contato WHERE Id = :id", conn))
            {
                cmd.Parameters.Add(new OracleParameter("id", contato.Id));
                using (var reader = cmd.ExecuteReader())
                {
                    Assert.IsTrue(reader.Read(), "Contato não encontrado");
                    Assert.AreEqual("Teste Atualizado", reader.GetString(0));
                    Assert.AreEqual("testeup@testeup.com", reader.GetString(1));
                    Assert.AreEqual("0000-0000", reader.GetString(2));
                }
            }
        }
    }
}
