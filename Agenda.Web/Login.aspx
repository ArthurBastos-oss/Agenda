<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Agenda.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="E-Mail"></asp:Label>
            <br />
            <asp:TextBox ID="TxtEmail" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Senha"></asp:Label>
            <br />
            <asp:TextBox ID="TxtSenha" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="BtLogar" runat="server" Text="Logar" OnClick="BtLogar_Click" />
            <br />
            <asp:Label ID="Lmsg" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
