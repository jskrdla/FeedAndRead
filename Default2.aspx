<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div><asp:Button ID="btnSave" runat="server" Text="Save" />
    <asp:Button ID="btnDelete" runat="server" Text="Delete" /></div>
    <div> Remaining images: <asp:Label ID="lblImageCount" runat="server" Text=""></asp:Label></div>
    <br />
    <asp:Image ID="Image1" runat="server" /><asp:HiddenField ID="HiddenField1" runat="server" />
</asp:Content>

