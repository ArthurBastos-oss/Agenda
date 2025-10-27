using Agenda.Data;
using Agenda.Mdl;
using Agenda.Svc;
using Oracle.ManagedDataAccess.Client;
using static System.Net.Mime.MediaTypeNames;

namespace Agenda.Tst
{
    [TestClass]
    public class SvcUsuarioTst
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
            var lista = SvcUsuario.ListarUsuario();

            Assert.IsTrue(lista.Count >  0);
        }

        [TestMethod]
        public void TesteAddUsuario()
        {
            var usuario = new Usuario();
            usuario.Email = "teste@teste.com";
            usuario.Nome = "Teste";
            usuario.Senha = "teste123";

            SvcUsuario.AddUsuario(usuario);

            using (var cmd = new OracleCommand("SELECT COUNT(*) FROM Usuario WHERE Email = :Email", conn))
            {
                cmd.Parameters.Add(new OracleParameter("Email", usuario.Email));
                var count = Convert.ToInt32(cmd.ExecuteScalar());

                Assert.AreEqual(1, count, "Usuário não foi inserido no banco de dados.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OracleException))]
        public void TesteAddUsuarioDuplicado()
        {
            var usuario1 = new Usuario();
            usuario1.Email = "teste@teste.com";
            usuario1.Nome = "Teste1";
            usuario1.Senha = "teste123";
            SvcUsuario.AddUsuario(usuario1);

            var usuario2 = new Usuario();
            usuario2.Email = "teste@teste.com";
            usuario2.Nome = "Duplicado";
            usuario2.Senha = "teste123";
            SvcUsuario.AddUsuario(usuario2);

        }

        [TestMethod]
        public void TesteEditUsuario()
        {
            using (var cmd = new OracleCommand("DELETE FROM Usuario WHERE Email = 'teste10@teste.com'", conn))
            {
                cmd.ExecuteNonQuery();
            }

            var usuario = new Usuario();
            usuario.Email = "teste10@teste.com";
            usuario.Nome = "Teste10";
            usuario.Senha = "teste123";
            SvcUsuario.AddUsuario(usuario);

            usuario.Nome = "Teste Atualizado";
            usuario.Senha = "teste12345";
            SvcUsuario.EditUsuario(usuario);

            using (var cmd = new OracleCommand(
                "SELECT Nome, Senha FROM Usuario WHERE Email = :Email", conn))
            {
                cmd.Parameters.Add(new OracleParameter("Email", usuario.Email));
                using (var reader = cmd.ExecuteReader())
                {
                    Assert.IsTrue(reader.Read(), "Usuario não encontrado");

                    string nome = reader.GetString(0);
                    string senha = reader.GetString(1);

                    Assert.AreEqual("Teste Atualizado", nome);
                    Assert.AreEqual("teste12345", senha);
                }
            }

        }
    }
}
