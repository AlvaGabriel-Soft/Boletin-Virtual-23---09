function mostrarPopupRegistro(nombreId, apellidoId, dniId, emailId, carreraId, legajoId) {
    try {
        // Obtener valores del formulario
       
        var nombre = document.getElementById(nombreId).value;
        var apellido = document.getElementById(apellidoId).value;
        var dni = document.getElementById(dniId).value;
        var email = document.getElementById(emailId).value;
        var legajo = document.getElementById(legajoId).value;
        // Obtener el texto seleccionado del DropDownList
        var carreraSelect = document.getElementById(carreraId);
        var carrera = carreraSelect.options[carreraSelect.selectedIndex].text;
        // Mostrar valores en el popup
       
        document.getElementById("popupNombre").innerText = nombre;
        document.getElementById("popupApellido").innerText = apellido;
        document.getElementById("popupDni").innerText = dni;
        document.getElementById("popupEmail").innerText = email;
        document.getElementById("popupCarrera").innerText = carrera;
        document.getElementById("popupLegajo").innerText = legajo;
        

        // Mostrar popup principal
        document.getElementById("popup").style.display = "flex";

    } catch (err) {
        alert("Error al abrir popup: " + err);
    }
}

function cerrarPopup() {
    var popup = document.getElementById("popup");
    if (popup) {
        popup.style.display = "none"; // Oculta el popup
    }
}