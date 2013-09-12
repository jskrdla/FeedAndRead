<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Button ID="btnGetFeed" runat="server" Text="Get Feed" />
    <asp:Button ID="btnGetImages" runat="server" Text="Get Images" /><br />
    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
</asp:Content>

