<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePrincipal.Master" AutoEventWireup="true" CodeBehind="weUsuario.aspx.cs" Inherits="Agenda.Web.weUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3>Lista de Usuários</h3>

    <asp:ListView ID="ListViewUsuario" runat="server"
        DataKeyNames="Email"
        InsertItemPosition="FirstItem"
        OnItemInserting="ListViewUsuario_ItemInserting"
        OnItemEditing="ListViewUsuario_ItemEditing" 
        OnItemUpdating="ListViewUsuario_ItemUpdating"
        OnItemCanceling="ListViewUsuario_ItemCanceling"
        OnItemDeleting="ListViewUsuario_ItemDeleting">

        <AlternatingItemTemplate>
            <tr style="background-color:#FFF8DC;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Deletar" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Editar" />
                </td>
                <td><asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("NOME") %>' /></td>
                <td><asp:Label ID="emailLabel" runat="server" Text='<%# Eval("EMAIL") %>' /></td>
                <td><asp:Label ID="senhaLabel" runat="server" Text='<%# Eval("SENHA") %>' /></td>
            </tr>
        </AlternatingItemTemplate>

        <EditItemTemplate>
            <tr style="background-color:#008A8C;color: #FFFFFF;">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Atualizar" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancelar" />
                </td>
                <td><asp:TextBox ID="nomeTextBox" runat="server" Text='<%# Eval("NOME") %>' /></td>
                <td><asp:TextBox ID="emailTextBox" runat="server" Text='<%# Eval("EMAIL") %>' /></td>
                <td><asp:TextBox ID="senhaTextBox" runat="server" Text='<%# Eval("SENHA") %>' /></td>
            </tr>
        </EditItemTemplate>

        <InsertItemTemplate>
            <tr>
                <td>
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Inserir" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Limpar" />
                </td>
                <td><asp:TextBox ID="emailTextBox" runat="server" /></td>
                <td><asp:TextBox ID="nomeTextBox" runat="server" /></td>
                <td><asp:TextBox ID="senhaTextBox" runat="server" /></td>
            </tr>
        </InsertItemTemplate>

        <ItemTemplate>
            <tr style="background-color:#DCDCDC;color: #000000;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Deletar" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Editar" />
                </td>
                <td><asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("NOME") %>' /></td>
                <td><asp:Label ID="emailLabel" runat="server" Text='<%# Eval("EMAIL") %>' /></td>
                <td><asp:Label ID="senhaLabel" runat="server" Text='<%# Eval("SENHA") %>' /></td>
            </tr>
        </ItemTemplate>

        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="1" 
                               style="background-color: #FFFFFF;border-collapse: collapse;">
                            <tr runat="server" style="background-color:#DCDCDC;color: #000000;">
                                <th></th>
                                <th>Email</th>
                                <th>Nome</th>
                                <th>Senha</th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server"></tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="text-align: center;background-color: #CCCCCC;">
                        <asp:DataPager ID="DataPager1" runat="server">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Button" 
                                    ShowFirstPageButton="true" ShowLastPageButton="true" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>

    </asp:ListView>

</asp:Content>
