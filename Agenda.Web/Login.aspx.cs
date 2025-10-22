using Agenda.Mdl;
using Agenda.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Agenda.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtLogar_Click(object sender, EventArgs e)
        {
            string email = TxtEmail.Text;
            string senha = TxtSenha.Text;
            bool logado = SvcUsuario.LogarUsuario(email, senha);


            if (logado)
            {
                HttpCookie login = new HttpCookie("login", TxtEmail.Text);
                Response.Cookies.Add(login);

                Response.Redirect("~/Index.aspx");
            }
            else
            {
                Lmsg.Text = "Usuario não encontrado";
            }


        }            
    }
}