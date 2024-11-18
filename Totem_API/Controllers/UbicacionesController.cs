using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Totem_API.Data;
using Totem_API.Models;

namespace Totem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UbicacionesController : ControllerBase
    {
        private readonly TotemContext _context;

        public UbicacionesController(TotemContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicaciones([FromQuery] int? totemId)
        {
            try
            {
                if (totemId.HasValue)
                {
                    var ubicacionesFiltradas = await _context.Ubicacion
                        .Where(u => u.IdTotem == totemId)
                        .ToListAsync();

                    if (!ubicacionesFiltradas.Any())
                    {
                        return NotFound($"No se encontraron ubicaciones para el tótem con ID {totemId}.");
                    }

                    return Ok(ubicacionesFiltradas);
                }

                // Si no se proporciona `totemId`, devuelve todas las ubicaciones
                return Ok(await _context.Ubicacion.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetUbicaciones: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }

        /// GET: api/Ubicaciones/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubicacion>> GetUbicacion(int id)
        {
            var ubicacion = await _context.Ubicacion.FindAsync(id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                ubicacion.Id,
                ubicacion.Nombre,
                ubicacion.Latitud,
                ubicacion.Longitud,
                ubicacion.Direccion
            });
        }

        [HttpPost]
        public async Task<ActionResult<Ubicacion>> PostUbicacion(Ubicacion ubicacion)
        {
            Console.WriteLine("Datos recibidos:");
            Console.WriteLine($"Nombre: {ubicacion.Nombre}");
            Console.WriteLine($"Latitud: {ubicacion.Latitud}");
            Console.WriteLine($"Longitud: {ubicacion.Longitud}");
            Console.WriteLine($"Dirección: {ubicacion.Direccion}");
            Console.WriteLine($"IdTotem: {ubicacion.IdTotem}");

            // Validar si el IdTotem existe en la base de datos
            if (!await _context.Totems.AnyAsync(t => t.IdTotem == ubicacion.IdTotem))
            {
                Console.WriteLine("Error: IdTotem no existe.");
                return BadRequest("El ID del tótem no es válido.");
            }

            try
            {
                // Guarda la ubicación en la base de datos
                _context.Ubicacion.Add(ubicacion);
                await _context.SaveChangesAsync();

                Console.WriteLine("Ubicación guardada exitosamente.");
                return CreatedAtAction(nameof(GetUbicacion), new { id = ubicacion.Id }, ubicacion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la ubicación: {ex.Message}");
                return StatusCode(500, "Error interno en el servidor.");
            }
        }

        // PUT: api/Ubicaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, [FromBody] Ubicacion ubicacion)
        {
            if (ubicacion == null || id <= 0)
            {
                return BadRequest("ID o datos inválidos.");
            }

            var ubicacionExistente = await _context.Ubicacion.FindAsync(id);
            if (ubicacionExistente == null)
            {
                return NotFound($"No se encontró una ubicación con ID {id}.");
            }

            // Actualiza los campos manualmente
            ubicacionExistente.Nombre = ubicacion.Nombre;
            ubicacionExistente.Latitud = ubicacion.Latitud;
            ubicacionExistente.Longitud = ubicacion.Longitud;
            ubicacionExistente.Direccion = ubicacion.Direccion;
            ubicacionExistente.IdTotem = ubicacion.IdTotem;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }



        // GET: api/Ubicaciones/BuscarPorNombre
        [HttpGet("BuscarPorNombre")]
        public async Task<ActionResult<Ubicacion>> GetUbicacionPorNombre([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre no puede estar vacío.");
            }

            var ubicacion = await _context.Ubicacion.FirstOrDefaultAsync(u => u.Nombre == nombre);

            if (ubicacion == null)
            {
                return NotFound($"No se encontró una ubicación con el nombre '{nombre}'.");
            }

            return Ok(ubicacion);
        }



        // DELETE: api/Ubicaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUbicacion(int id)
        {
            var ubicacion = await _context.Ubicacion.FindAsync(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            _context.Ubicacion.Remove(ubicacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool UbicacionExists(int id)
        {
            return _context.Ubicacion.Any(e => e.Id == id);
        }

    }
}
