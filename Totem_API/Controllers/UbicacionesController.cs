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

        // GET: api/Ubicaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicaciones()
        {
            return await _context.Ubicacion.ToListAsync();
        }

        // GET: api/Ubicaciones/BuscarPorNombre
        [HttpGet("BuscarPorNombre")]
        public async Task<ActionResult<Ubicacion>> GetUbicacionPorNombre([FromQuery] string nombre)
        {
            // Convertimos la primera letra a mayúscula y el resto a minúscula para asegurar coincidencia con la base de datos
            string nombreFormateado = char.ToUpper(nombre[0]) + nombre.Substring(1).ToLower();

            // Log de verificación
            Console.WriteLine($"Nombre recibido del frontend: {nombre}");
            Console.WriteLine($"Nombre formateado: {nombreFormateado}");

            var ubicacion = await _context.Ubicacion.FirstOrDefaultAsync(u => u.Nombre == nombreFormateado);

            if (ubicacion == null)
            {
                return NotFound("Ubicación no encontrada.");
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

        // POST: api/Ubicaciones
        [HttpPost]
        public async Task<ActionResult<Ubicacion>> PostUbicacion(Ubicacion ubicacion)
        {
            _context.Ubicacion.Add(ubicacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUbicacion), new { id = ubicacion.Id }, ubicacion);
        }

        // PUT: api/Ubicaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, Ubicacion ubicacion)
        {
            if (id != ubicacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(ubicacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbicacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
