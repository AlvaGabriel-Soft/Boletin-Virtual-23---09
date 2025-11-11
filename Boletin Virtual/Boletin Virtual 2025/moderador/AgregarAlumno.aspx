<%@ Page Title="" Language="C#" MasterPageFile="~/moderador/MenuModerador.Master" AutoEventWireup="true" CodeBehind="AgregarAlumno.aspx.cs" Inherits="Boletin_Virtual_2025.AgregarAlumno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="formbold-main-wrapper">
        <div class="formbold-form-wrapper">
            
            
            <div class="formbold-form-title">
                <h2 class="">Registro de Alumnos</h2>

                <p>Ingresar datos del Alumno</p>
            </div> 

             <div class="formbold-input-flex">
                <div>
                    <label for="firstname" class="formbold-form-label">Primer nombre</label>
                   


                   <label for="firstname" class="formbold-form-label">Primer nombre</label>


<asp:TextBox ID="primernombre" runat="server" CssClass="formbold-form-input"> </asp:TextBox>

<!-- Validador: campo obligatorio -->
<asp:RequiredFieldValidator ID="rfvPrimerNombre" runat="server" ControlToValidate="primernombre" ErrorMessage="El nombre es obligatorio" ForeColor="Red" Display="Dynamic">
</asp:RequiredFieldValidator>

<!-- Validador: solo letras -->
<asp:RegularExpressionValidator ID="revPrimerNombre" runat="server" ControlToValidate="primernombre" ValidationExpression="^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$" ErrorMessage="Solo se permiten letras en el nombre" ForeColor="Red" Display="Dynamic">
</asp:RegularExpressionValidator>

                </div>
                <div >
                    <label for="lastname" class="formbold-form-label">Apellido</label>
                    <asp:TextBox ID="apellido" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvApellidotext" runat="server" ControlToValidate="apellido" ErrorMessage="El apellido es obligatorio" ForeColor="Red" Display="Dynamic">
</asp:RequiredFieldValidator>

<asp:RegularExpressionValidator ID="revApellidotext" runat="server" ControlToValidate="apellido" ValidationExpression="^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$" ErrorMessage="Solo se permiten letras en el apellido" ForeColor="Red" Display="Dynamic">
</asp:RegularExpressionValidator>

                </div>
            </div>

            <div class="formbold-mb-3">
                <label for="address" class="formbold-form-label">DNI</label>
                <asp:TextBox ID="dni" runat="server" CssClass="formbold-form-input"></asp:TextBox>
            </div>

<asp:RegularExpressionValidator ID="rfvDNItext" runat="server" ControlToValidate="dni" ValidationExpression="^\d+$" ErrorMessage="Solo se permiten numeros" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

<asp:RegularExpressionValidator ID="revDNItext" runat="server" ControlToValidate="dni" ValidationExpression="^\d+$" ErrorMessage="Solo se permiten numeros en el DNI" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

            <div class="formbold-mb-3">
                <label for="address2" class="formbold-form-label">Email</label>
                <asp:TextBox ID="email" runat="server" CssClass="formbold-form-input"></asp:TextBox>

<asp:RequiredFieldValidator ID="rfvEmailtext" runat="server" ControlToValidate="email" ErrorMessage="El correo electrónico es obligatorio" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

<asp:RegularExpressionValidator ID="revEmailtext" runat="server" ControlToValidate="email" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" ErrorMessage="Formato de correo no válido" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>


            </div>
             <div class="formbold-mb-3">
                <label  class="formbold-form-label">Seleccionar la carrera</label>
          <asp:DropDownList ID="carreras" runat="server" CssClass="formbold-form-input" Visible="true"  >
</asp:DropDownList>
            </div>
                 <div class="formbold-mb-3">
                <label  class="formbold-form-label">Legajo</label>
                <asp:TextBox ID="legajo" runat="server" CssClass="formbold-form-input"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="rfvlegajotext" runat="server" ControlToValidate="legajo" ErrorMessage="El correo electrónico es obligatorio" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

<asp:RegularExpressionValidator ID="revlegajotext" runat="server" ControlToValidate="legajo" ValidationExpression="^\d+$" ErrorMessage="Solo se permiten numeros" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>



            <!--<div class="formbold-input-flex">
                <div>
                    <label for="state" class="formbold-form-label">State/Prvince</label>
                    <asp:TextBox ID="state" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
                <div>
                    <label for="country" class="formbold-form-label">Country</label>
                    <asp:TextBox ID="country" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
            </div>

            <div class="formbold-input-flex">
                <div>
                    <label for="post" class="formbold-form-label">Post/Zip code</label>
                    <asp:TextBox ID="post" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
                <div>
                    <label for="area" class="formbold-form-label">Area Code</label>
                    <asp:TextBox ID="area" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
            </div>     -->

              <!-- 🔹 BOTÓN PARA MOSTRAR POPUP -->
            <div class="formbold-checkbox-wrapper">
               <button type="button"
        class="formbold-btn"
        onclick="mostrarPopupRegistro(
            '<%= primernombre.ClientID %>',
            '<%= apellido.ClientID %>',
            '<%= dni.ClientID %>',
            '<%= email.ClientID %>',
            '<%= carreras.ClientID %>',
            '<%= legajo.ClientID %>'
        )">
    Revisar datos
</button>
            </div>
            <asp:Label ID="UsuarioRepetido" runat="server" EnableViewState="false" />
        </div>

        <!-- 🔹 POPUP -->
        <div class="popup" id="popup">
            <div class="popup-content">
                <h3>Revisa tus datos</h3>
                <p><strong>Nombre:</strong> <span id="popupNombre"></span></p>
                <p><strong>Apellido:</strong> <span id="popupApellido"></span></p>
                <p><strong>DNI:</strong> <span id="popupDni"></span></p>
                <p><strong>Email:</strong> <span id="popupEmail"></span></p>
                <p id="popupCarreraContainer"><strong>Carrera:</strong> <span id="popupCarrera"></span></p>
                <p><strong>legajo:</strong> <span id="popupLegajo"></span></p>


                <asp:Label ID="lblConfirmacion" runat="server" 
                           Text="¿Estás seguro de que tus datos son correctos?"></asp:Label>
                <br /><br />
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" OnClick="btnRegister_Click" CssClass="formbold-btn" />
                <button type="button" class="close-btn" onclick="cerrarPopup()">Cancelar</button>
            </div>
        </div>
    </div>
</asp:Content>