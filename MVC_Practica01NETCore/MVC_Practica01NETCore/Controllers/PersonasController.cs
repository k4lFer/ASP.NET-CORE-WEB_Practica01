using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,TipoDocumento,Dni,CarnetExtranjero,Pasaporte,FechaNacimiento")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                if (CalcularEdad(persona.FechaNacimiento) >= 18)
                {
                    if (DocumentosNoIguales(persona.Dni, persona.CarnetExtranjero, persona.Pasaporte))
                    {
                        persona.Id = Guid.NewGuid();
                        _context.Add(persona);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Los documentos no pueden ser iguales.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La persona debe ser mayor de 18 años para registrarse.");
                }
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
                    if (DocumentosNoIguales(persona.Dni, persona.CarnetExtranjero, persona.Pasaporte))
                    {
                        try
                        {
                            _context.Update(persona);
                            await _context.SaveChangesAsync();
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
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Los documentos no pueden ser iguales.");
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
    }
}
