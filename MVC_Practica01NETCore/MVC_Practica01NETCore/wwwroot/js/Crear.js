
$(document).ready(function () {


    var dniRegex = /^\d{8}$/;
    var otrosDocumentosRegex = /^\d{12}$/;
    var nombreRegex = /^[A-Z][a-zA-Z]*( [A-Z][a-zA-Z]*)*$/;
    //var nombreRegex = /^[A-Z][a-zA-Z]*( [A-Z][a-zA-Z]*){0,1}$/;
    var apellidoRegex = /^[A-Z][a-zA-Z]* [A-Z][a-zA-Z]*$/;

  
    $('.CarnetExtranjero, .Pasaporte').hide().prop('disabled', true);

 
    var campos = {
        Dni: {
            regex: dniRegex,
            errorSpan: $('#DniError'),
            errorMessage: 'El DNI debe contener exactamente 8 dígitos.'
        },
        Nombre: {
            regex: nombreRegex,
            errorSpan: $('#NombreError'),
            errorMessage: 'Por favor, ingrese un nombre válido.'
        },
        Apellido: {
            regex: apellidoRegex,
            errorSpan: $('#ApellidoError'),
            errorMessage: 'Por favor, ingrese un apellido válido.'
        },
        Pasaporte: {
            regex: otrosDocumentosRegex,
            errorSpan: $('#PasaporteError'),
            errorMessage: 'El número de pasaporte debe contener exactamente 12 dígitos.'
        },
        CarnetExtranjero: {
            regex: otrosDocumentosRegex,
            errorSpan: $('#CarnetExtranjeroError'),
            errorMessage: 'El número de carné de extranjero debe contener exactamente 12 dígitos.'
        }
    };

    // Obtener el botón de envío
    //var submitButton = $('input[type="submit"]');

    // Función para validar un campo y mostrar mensajes de error
    function validarCampo(input, campo) {
        var valor = input.val().trim();
        var regex = campo.regex;
        var errorSpan = campo.errorSpan;

        if (!valor.match(regex)) {
            input.addClass('is-invalid');
            errorSpan.text(campo.errorMessage);
            return false;
        } else {
            input.removeClass('is-invalid');
            errorSpan.text('');
            return true;
        }
    }

    // Agregar eventos input a los campos de entrada
    $.each(campos, function (campoId, campo) {
        $('#' + campoId).on('input', function () {
            var input = $(this);
            validarCampo(input, campo);
            var isValid = true;
            $.each(campos, function (_, otroCampo) {
                if (input.attr('id') !== campoId) {
                    if (otroCampo.errorSpan.text() !== '') {
                        isValid = false;
                        return false;
                    }
                }
            });
            //submitButton.prop('disabled', !isValid);
        });
    });

    // Maneja el cambio en el combo box
    $('#TipoDocumento').change(function () {
        // Oculta y desactiva todos los campos relacionados con tipos de documentos
        $('.DNI, .CarnetExtranjero, .Pasaporte').hide().prop('disabled', true);

        // Muestra y activa el campo correspondiente al tipo de documento seleccionado
        var selectedOption = $(this).val();
        $('.' + selectedOption).show().prop('disabled', false);

        // Limpiar los mensajes de error
        $.each(campos, function (_, campo) {
            campo.errorSpan.text('');
        });

        // Valida los campos al cambiar el tipo de documento
        campos[$(this).val()].errorSpan.text('');
        //submitButton.prop('disabled', false);
    });

    // Validar el botón de envío inicialmente
    $.each(campos, function (_, campo) {
        $('#' + campoId).trigger('input');
    });
});