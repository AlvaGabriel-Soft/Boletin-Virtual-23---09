<%@ Page Title="" Language="C#" MasterPageFile="~/moderador/MenuModerador.Master" AutoEventWireup="true" CodeBehind="AsignarProfesorMateria.aspx.cs" Inherits="Boletin_Virtual_2025.moderador.AsignarProfesorMateria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="formbold-main-wrapper">
    <div class="formbold-form-wrapper-profesormateria">
        
        <div class="formbold-form-title">
            <h2>Asignar profesores a materias</h2>
            <p></p>
        </div>

        <!-- Mensajes al usuario -->
        <asp:Label ID="lblMensaje" runat="server" CssClass="formbold-message" />

        <div class="formbold-input-flex">
            <div>
                <label for="DniBusqueda" class="formbold-form-label">Ingrese el Dni del profesor para comenzar la busqueda</label>
                <asp:TextBox ID="DniBusqueda" runat="server" CssClass="formbold-form-input"
                             AutoPostBack="true" OnTextChanged="DniBusqueda_TextChanged" />
            </div>
          
        </div>

        <div class="formbold-mb-3">
          
                <label for="Nombre" class="formbold-form-label">Nombre</label>
                <asp:TextBox ID="Nombre" runat="server" CssClass="formbold-form-input" ReadOnly="true" />
            
            <label for="Apellido" class="formbold-form-label">Apellido</label>
            <asp:TextBox ID="Apellido" runat="server" CssClass="formbold-form-input" ReadOnly="true" />
        </div> 

        <!-- Campo oculto para guardar el id_profesor -->
        <asp:HiddenField ID="hiddenIdProfesor" runat="server" />     
        <hr />

        <asp:HiddenField ID="HiddenField1" runat="server" />     
<br />
<hr />

        <!-- GridView con materias -->
         <div class="formbold-mb-3">
          
                <label for="Nombre" class="formbold-form-label">Seleccionar la materia</label>
               
        </div> 


        <asp:GridView ID="gvMaterias" runat="server" AutoGenerateColumns="False" 
                      DataKeyNames="id_materia" OnRowDataBound="gvMaterias_RowDataBound">
            <Columns>
                <asp:BoundField DataField="nombre_materia" HeaderText="Materia" />
                <asp:BoundField DataField="nombre_carrera" HeaderText="Carrera" />
                <asp:TemplateField HeaderText="Seleccionar">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSeleccionar" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <!-- Botón para guardar selección -->
        <asp:Button ID="btnAsignarMaterias" runat="server" Text="Asignar Materias"
                    OnClick="btnAsignarMaterias_Click" CssClass="formbold-btn" Enabled="false" />

    </div>
</div>

</asp:Content>