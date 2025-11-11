<%@ Page Title="" Language="C#" MasterPageFile="~/Alumno/MenuAlumnos.Master" AutoEventWireup="true" CodeBehind="SimularFecha.aspx.cs" Inherits="Boletin_Virtual_2025.Alumno.SimularFecha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .contenedor {
            background-color: #fff;
            width: 400px;
            margin: 30px auto;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 3px 8px rgba(0, 0, 0, 0.2);
            font-family: "Segoe UI", Arial, sans-serif;
        }

        h2 {
            text-align: center;
            color: #333;
        }

        label {
            display: block;
            margin-top: 15px;
            font-weight: bold;
        }

        input[type="text"] {
            width: 100%;
            padding: 8px;
            margin-top: 5px;
            border-radius: 6px;
            border: 1px solid #ccc;
        }

        .boton {
            margin-top: 15px;
            width: 100%;
            padding: 10px;
            background-color: #0078D7;
            color: white;
            font-weight: bold;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }

        .boton:hover {
            background-color: #005fa3;
        }

        .fecha-actual {
            margin-top: 20px;
            text-align: center;
            font-size: 18px;
            color: #333;
        }
    </style>

    <div class="contenedor">
        <h2>Simular Fecha del Sistema</h2>

        <label for="txtFechaSimulada">Nueva Fecha:</label>
        <asp:TextBox ID="txtFechaSimulada" runat="server" placeholder="dd/mm/aaaa"></asp:TextBox>

        <asp:Button ID="btnCambiarFecha" runat="server" Text="Cambiar Fecha" CssClass="boton" OnClick="btnCambiarFecha_Click" />

        <div class="fecha-actual">
            <asp:Label ID="lblFechaActual" runat="server" Text=""></asp:Label>
        </div>
    </div>

</asp:Content>
