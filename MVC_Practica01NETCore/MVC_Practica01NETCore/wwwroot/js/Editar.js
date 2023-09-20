$(document).ready(function () {
    // Expresiones regulares para validación
    var dniRegex = /^\d{8}$/;
    var otrosDocumentosRegex = /^\d{12}$/;
    var nombreRegex = /^[A-Z][a-zA-Z]*( [A-Z][a-zA-Z]*)*$/;
    var apellidoRegex = /^[A-Z][a-zA-Z]* [A-Z][a-zA-Z]*$/;

    // Obtener los campos de entrada
    var campos = {
        Dni: {
            regex: dniRegex,
            errorSpan: document.getElementById('DniError'),
            errorMessage: 'El DNI debe contener exactamente 8 dígitos.'
        },
        Nombre: {
            regex: nombreRegex,
            errorSpan: document.getElementById('NombreError'),
            errorMessage: 'Por favor, ingrese un nombre válido.'
        },
        Apellido: {
            regex: apellidoRegex,
            errorSpan: document.getElementById('ApellidoError'),
            errorMessage: 'Por favor, ingrese un apellido válido.'
        },
        Pasaporte: {
            regex: otrosDocumentosRegex,
            errorSpan: document.getElementById('PasaporteError'),
            errorMessage: 'El número de pasaporte debe contener exactamente 12 dígitos.'
        },
        CarnetExtranjero: {
            regex: otrosDocumentosRegex,
            errorSpan: document.getElementById('CarnetExtranjeroError'),
            errorMessage: 'El número de carné de extranjero debe contener exactamente 12 dígitos.'
        }
    };

    // Obtener el ComboBox y el botón de envío utilizando la variable tipoDocumento
    var tipoDocumento = document.getElementById("TipoDocumento");
    var submitButton = document.querySelector('input[type="submit"]');

    // Función para validar un campo y mostrar mensajes de error
    function validarCampo(input, campo) {
        var valor = input.value.trim();
        var regex = campo.regex;
        var errorSpan = campo.errorSpan;

        if (!valor.match(regex)) {
            input.classList.add('is-invalid');
            errorSpan.textContent = campo.errorMessage;
            return false;
        } else {
            input.classList.remove('is-invalid');
            errorSpan.textContent = '';
            return true;
        }
    }

    // Agregar eventos input a los campos de entrada para validar en tiempo real
    Object.keys(campos).forEach(function (campoId) {
        var input = document.getElementById(campoId);
        input.addEventListener('input', function () {
            validarCampo(input, campos[campoId]);
            var isValid = true;
            Object.keys(campos).forEach(function (otroCampoId) {
                if (input.id !== otroCampoId) {
                    if (campos[otroCampoId].errorSpan.textContent !== '') {
                        isValid = false;
                        return;
                    }
                }
            });
            submitButton.disabled = !isValid;
        });
    });

    // Manejar el cambio en el ComboBox
    tipoDocumento.addEventListener('change', function () {
        var selectedTipoDocumento = tipoDocumento.value;

        // Ocultar todos los campos relacionados con tipos de documentos
        document.querySelectorAll('.DNI, .CarnetExtranjero, .Pasaporte').forEach(function (campo) {
            campo.style.display = 'none';
            campo.disabled = true;
        });

        // Mostrar solo el campo correspondiente al TipoDocumento seleccionado
        document.querySelectorAll('.' + selectedTipoDocumento).forEach(function (campo) {
            campo.style.display = 'block';
            campo.disabled = false;
        });

        // Limpiar los mensajes de error
        Object.keys(campos).forEach(function (campoId) {
            if (campoId !== selectedTipoDocumento) {
                campos[campoId].errorSpan.textContent = '';
            }
        });

        // Validar los campos al cambiar el tipo de documento
        campos[selectedTipoDocumento].errorSpan.textContent = '';
        submitButton.disabled = false;
    });

    // Validar campos cuando se carga la página
    Object.keys(campos).forEach(function (campoId) {
        document.getElementById(campoId).dispatchEvent(new Event('input'));
    });

    // Validar campos al cargar la página
    tipoDocumento.dispatchEvent(new Event('change'));
});
