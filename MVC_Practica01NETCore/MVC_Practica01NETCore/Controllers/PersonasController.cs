using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Practica01NETCore.Models;

namespace MVC_Practica01NETCore.Controllers
{
    public class PersonasController : Controller
    {
        private readonly Practica1Ds2Context _context;

        public PersonasController(Practica1Ds2Context context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var personas = await _context.Personas.ToListAsync();
            return View(personas);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // POST: Personas/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,TipoDocumento,Dni,CarnetExtranjero,Pasaporte,FechaNacimiento")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                if (CalcularEdad(persona.FechaNacimiento) >= 18)
                {
                    // Validar el nombre
                    if (!ValidarNombre(persona.Nombre))
                    {
                        ModelState.AddModelError("Nombre", "Formato de nombre incorrecto.");
                        return View(persona);
                    }

                    // Validar el apellido
                    if (!ValidarApellido(persona.Apellido))
                    {
                        ModelState.AddModelError("Apellido", "Formato de apellido incorrecto.");
                        return View(persona);
                    }

                    // Validar el documento
                    if (!ValidarDocumento(persona.TipoDocumento, persona.Dni))
                    {
                        ModelState.AddModelError("Dni", "Formato de documento incorrecto.");
                        return View(persona);
                    }

                    // Verificar si el documento ya existe en la base de datos
                    bool documentoExistente = await VerificarDocumentoExistente(persona);

                    if (documentoExistente)
                    {
                        switch (persona.TipoDocumento)
                        {
                            case "DNI":
                                ModelState.AddModelError("Dni", "El DNI ya existe en la base de datos.");
                                break;
                            case "Pasaporte":
                                ModelState.AddModelError("Pasaporte", "El Pasaporte ya existe en la base de datos.");
                                break;
                            case "CarnetExtranjero":
                                ModelState.AddModelError("CarnetExtranjero", "El Carnet Extranjero ya existe en la base de datos.");
                                break;
                        }
                        return View(persona);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La persona debe ser mayor de 18 años para registrarse.");
                }

                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(persona);
        }



        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Apellido,TipoDocumento,Dni,CarnetExtranjero,Pasaporte,FechaNacimiento")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (CalcularEdad(persona.FechaNacimiento) >= 18)
                {
                    // Validar el nombre
                    if (!ValidarNombre(persona.Nombre))
                    {
                        ModelState.AddModelError("Nombre", "Formato de nombre incorrecto.");
                        return View(persona);
                    }

                    // Validar el apellido
                    if (!ValidarApellido(persona.Apellido))
                    {
                        ModelState.AddModelError("Apellido", "Formato de apellido incorrecto.");
                        return View(persona);
                    }

                    // Verificar si el número de documento ha cambiado y si es válido
                    var personaOriginal = await _context.Personas.FirstOrDefaultAsync(p => p.Id == id);
                    if (personaOriginal == null)
                    {
                        return NotFound();
                    }

                    if (persona.TipoDocumento == "DNI" && persona.Dni != personaOriginal.Dni)
                    {
                        if (!ValidarDocumento(persona.TipoDocumento, persona.Dni))
                        {
                            ModelState.AddModelError("Dni", "Formato de documento incorrecto.");
                            return View(persona);
                        }

                        bool documentoExistente = await VerificarDocumentoExistente(persona);
                        if (documentoExistente)
                        {
                            ModelState.AddModelError("Dni", "El nuevo DNI ya existe en la base de datos.");
                            return View(persona);
                        }
                    }
                    else if (persona.TipoDocumento == "Pasaporte" && persona.Pasaporte != personaOriginal.Pasaporte)
                    {
                        if (!ValidarDocumento(persona.TipoDocumento, persona.Pasaporte))
                        {
                            ModelState.AddModelError("Pasaporte", "Formato de documento incorrecto.");
                            return View(persona);
                        }

                        bool documentoExistente = await VerificarDocumentoExistente(persona);
                        if (documentoExistente)
                        {
                            ModelState.AddModelError("Pasaporte", "El nuevo Pasaporte ya existe en la base de datos.");
                            return View(persona);
                        }
                    }
                    else if (persona.TipoDocumento == "CarnetExtranjero" && persona.CarnetExtranjero != personaOriginal.CarnetExtranjero)
                    {
                        if (!ValidarDocumento(persona.TipoDocumento, persona.CarnetExtranjero))
                        {
                            ModelState.AddModelError("CarnetExtranjero", "Formato de documento incorrecto.");
                            return View(persona);
                        }

                        bool documentoExistente = await VerificarDocumentoExistente(persona);
                        if (documentoExistente)
                        {
                            ModelState.AddModelError("CarnetExtranjero", "El nuevo Carnet Extranjero ya existe en la base de datos.");
                            return View(persona);
                        }
                    }

                    // Actualizar todos los campos de la entidad original con los valores del modelo persona
                    _context.Entry(personaOriginal).CurrentValues.SetValues(persona);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PersonaExists(persona.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La persona debe ser mayor de 18 años para editar sus datos.");
                }
            }

            return View(persona);
        }



        // GET: Personas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var persona = await _context.Personas.FindAsync(id);
            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(Guid id)
        {
            return _context.Personas.Any(e => e.Id == id);
        }

        private int CalcularEdad(DateTime? fechaNacimiento)
        {
            if (fechaNacimiento.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - fechaNacimiento.Value.Year;
                if (fechaNacimiento.Value.Date > today.AddYears(-age))
                    age--;
                return age;
            }
            return 0; // Otra opción si no se proporciona FechaNacimiento
        }

        private bool DocumentosNoIguales(string dni, string carnetExtranjero, string pasaporte)
        {
            return dni != carnetExtranjero && dni != pasaporte && carnetExtranjero != pasaporte;
        }
        private async Task<bool> VerificarDocumentoExistente(Persona persona)
        {
            // Verificar si ya existe un registro con el mismo documento en la base de datos
            return await _context.Personas.AnyAsync(p =>
                (persona.TipoDocumento == "DNI" && p.Dni == persona.Dni) ||
                (persona.TipoDocumento == "Pasaporte" && p.Pasaporte == persona.Pasaporte) ||
                (persona.TipoDocumento == "CarnetExtranjero" && p.CarnetExtranjero == persona.CarnetExtranjero)
            );
        }

        // Función para validar el nombre
        private bool ValidarNombre(string nombre)
        {
            var nombreRegex = new Regex(@"^[A-Z][a-zA-Z]*( [A-Z][a-zA-Z]*){0,1}$");
            return !string.IsNullOrEmpty(nombre) && nombreRegex.IsMatch(nombre);
        }

        // Función para validar el apellido
        private bool ValidarApellido(string apellido)
        {
            var apellidoRegex = new Regex(@"^[A-Z][a-zA-Z]* [A-Z][a-zA-Z]*$");
            return !string.IsNullOrEmpty(apellido) && apellidoRegex.IsMatch(apellido);
        }

        // Función para validar el documento
        private bool ValidarDocumento(string tipoDocumento, string documento)
        {
            if (string.IsNullOrEmpty(documento))
            {
                return false;
            }

            var dniRegex = new Regex(@"^\d{8}$");
            var otrosDocumentosRegex = new Regex(@"^\d{12}$");

            if (tipoDocumento == "DNI")
            {
                return dniRegex.IsMatch(documento);
            }
            else if (tipoDocumento == "Pasaporte" || tipoDocumento == "CarnetExtranjero")
            {
                return otrosDocumentosRegex.IsMatch(documento);
            }
            return false;
        }
    }
}
