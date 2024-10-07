using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Totem_API.Data;
using Totem_API.Models;

namespace Totem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivoController : ControllerBase
    {
        private readonly TotemContext _totemContext;

        public ArchivoController(TotemContext totemContext) => _totemContext = totemContext;

        [HttpPost, Route("{totemId}")]
        public async Task<IActionResult> UploadFile([FromRoute] int totemId, [FromForm] IFormFile file)
        {
            if(file != null)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var archivo = new Archivo()
                {
                    NombreArchivo = file.FileName,
                    ContenidoArchivo = fileBytes,
                    TotemId = totemId
                };

                await _totemContext.Archivos.AddAsync(archivo);
                await _totemContext.SaveChangesAsync();

                return CreatedAtAction(nameof(UploadFile), new { status = "Archivo subido exitosamente" });
            }
            return BadRequest();
        }

        [HttpGet, Route("{totemId}")]
        public async Task<IActionResult> GetFilesByTotem([FromRoute] int totemId)
        {
            var listFiles = await _totemContext.Archivos.Select(f => new ArchivoDto
            {
                Id = f.Id,
                NombreArchivo = f.NombreArchivo,
                TotemId = f.TotemId
            }).Where(f => f.TotemId.Equals(totemId)).ToListAsync();

            return Ok(listFiles);
        }

        [HttpGet, Route("FilesContent/{totemId:int}")]
        public async Task<IActionResult> GetFilesWithContent([FromRoute] int totemId)
        {
            var listFiles = await _totemContext.Archivos.Select(f => new Archivo
            {
                TotemId = f.TotemId,
                ContenidoArchivo = f.ContenidoArchivo
            }).Where(f => f.TotemId.Equals(totemId)).ToListAsync();
            return Ok(listFiles);
        }

        [HttpDelete, Route("{fileId}")]
        public async Task<IActionResult> DeleteFile([FromRoute]int fileId)
        {
            var file = await FindById(fileId);
            if(file != null)
            {
                _totemContext.Archivos.Remove(file);
                await _totemContext.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        async Task<Archivo> FindById(int id)
        {
            var findFile = await _totemContext.Archivos.FirstOrDefaultAsync(f => f.Id.Equals(id));
            return findFile!;
        }
    }
}
