using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Practica01NETCore.Models;

public partial class Persona
{
    public Guid Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? TipoDocumento { get; set; }

    public string? Dni { get; set; }

    public string? CarnetExtranjero { get; set; }

    public string? Pasaporte { get; set; }

    public DateTime? FechaNacimiento { get; set; }



    [NotMapped] // Indica que no se mapeará a la base de datos
    public int Edad
    {
        get
        {
            if (FechaNacimiento.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - FechaNacimiento.Value.Year;
                if (FechaNacimiento.Value.Date > today.AddYears(-age))
                    age--;
                return age;
            }
            return 0; // Otra opción si no se proporciona FechaNacimiento
        }
    }
}
