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

            SvcUsuario.LimparUsuariosDeTeste(conn);
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

            int count = SvcUsuario.BuscarUsuario(usuario.Email);

            Assert.AreEqual(1, count, "Usuário não foi inserido no banco de dados.");
            
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
        [ExpectedException(typeof(OracleException))]
        public void TesteAddUsuarioInvalido()
        {
            var usuario1 = new Usuario();
            usuario1.Email = null;
            usuario1.Nome = "Teste1";
            usuario1.Senha = "teste123";
            SvcUsuario.AddUsuario(usuario1);
        }

        [TestMethod]
        public void TesteEditUsuario()
        {

            var usuario = new Usuario();
            usuario.Email = "teste@teste.com";
            usuario.Nome = "Teste";
            usuario.Senha = "teste123";
            SvcUsuario.AddUsuario(usuario);

            usuario.Nome = "Teste Atualizado";
            usuario.Senha = "teste12345";
            SvcUsuario.EditUsuario(usuario);

            var usuarioEditado = SvcUsuario.BuscarUsuarioPorEmail(usuario.Email);

            Assert.IsNotNull(usuarioEditado, "Usuario não encontrado");
            Assert.AreEqual("Teste Atualizado", usuarioEditado.Nome);
            Assert.AreEqual("teste12345", usuarioEditado.Senha);

        }

        [TestMethod]
        public void TesteLogarUsuario()
        {
            var usuario = new Usuario();
            usuario.Email = "teste@teste.com";
            usuario.Nome = "Teste";
            usuario.Senha = "teste123";
            SvcUsuario.AddUsuario(usuario);

            bool logado = SvcUsuario.LogarUsuario(usuario.Email, usuario.Senha);

            Assert.IsTrue(logado, "Usuario não existente.");
        }

        [TestMethod]
        public void TesteLogarUsuarioInexistente()
        {
            string email = "testein@teste.com";
            string senha = "teste123";

            bool logado = SvcUsuario.LogarUsuario(email, senha);

            Assert.IsFalse(logado);
        }

        [TestMethod]
        public void TesteDeleteUsuario()
        {
            var usuario = new Usuario();
            usuario.Email = "teste@teste.com";
            usuario.Nome = "Teste";
            usuario.Senha = "teste123";
            SvcUsuario.AddUsuario(usuario);

            bool logado = SvcUsuario.DeleteUsuario(usuario.Email);

            Assert.IsTrue(logado, "Usuario não existente.");
        }
    }
}
