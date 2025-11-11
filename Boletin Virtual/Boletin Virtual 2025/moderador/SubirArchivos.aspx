<%@ Page Title="" Language="C#" MasterPageFile="~/moderador/MenuModerador.Master" AutoEventWireup="true" CodeBehind="SubirArchivos.aspx.cs" Inherits="Boletin_Virtual_2025.moderador.SubirArchivos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:FileUpload ID="fuCsv" runat="server" />
<asp:Button ID="btnPreview" runat="server" Text="📄 Previsualizar CSV" OnClick="btnPreview_Click" />
<asp:Button ID="btnUpload" runat="server" Text="⬆️ Cargar en Base de Datos" OnClick="btnUpload_Click" />
<br /><br />
<asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
<br />
<asp:GridView ID="gvPreview" runat="server" AutoGenerateColumns="true" BorderWidth="1" GridLines="Both"></asp:GridView>

</asp:Content>
