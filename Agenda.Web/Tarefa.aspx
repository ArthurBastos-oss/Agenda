<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePrincipal.Master" AutoEventWireup="true" CodeBehind="Tarefa.aspx.cs" Inherits="Agenda.Web.Tarefa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
    <asp:Label ID="Label1" runat="server" Text="Label">Descrição</asp:Label>
    <br />
    <asp:TextBox ID="TxtDesc" runat="server" Width="200px" ></asp:TextBox>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Label">Inicio da Tarefa</asp:Label>
    <br />
    <asp:TextBox ID="DataIniBox" runat="server" TextMode="DateTimeLocal" Width="200px"></asp:TextBox>
    <br />
    <asp:Label ID="Label3" runat="server" Text="Label">Termino da Tarefa</asp:Label>
    <br />
    <asp:TextBox ID="DataEndBox" runat="server" TextMode="DateTimeLocal" Width="200px"></asp:TextBox>
    <br />
    <asp:Label ID="LRecorrencia" runat="server" Text="Label">Tipo de rotina</asp:Label>
    <asp:DropDownList ID="ddlRecorrencia" runat="server" CssClass="form-select">
        <asp:ListItem Text="Nenhuma" Value="0" />
        <asp:ListItem Text="Diária" Value="1" />
        <asp:ListItem Text="Semanal" Value="2" />
        <asp:ListItem Text="Mensal" Value="3" />
        <asp:ListItem Text="Anual" Value="4" />
    </asp:DropDownList>
    <br />
    <asp:Panel ID="LstContatos" runat="server">
        <asp:Label runat="server" Text="Label">Contatos relacionados:</asp:Label>
        <br />
        <asp:DropDownList ID="ddContatos" runat="server" Width="200px" />
        <br />
        <asp:Button ID="remContato" runat="server" Text="Remover Contato" OnClick="BtRemover_Click" />
    </asp:Panel>
    <asp:Button ID="addContato" runat="server" Text="Adicionar Contato" OnClick="BtAdicionar_Click" />
    <br />
    <asp:Label ID="Lmsg" runat="server" Text=""></asp:Label>
    <br />
    <asp:Button ID="BtSalvar" runat="server" Text="Salvar" OnClick="BtSalvar_Click" />
</asp:Content>
