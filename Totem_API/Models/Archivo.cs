using System.ComponentModel.DataAnnotations.Schema;

namespace Totem_API.Models
{
    [Table("Archivo")]
    public class Archivo
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] ContenidoArchivo { get; set; }
        public int TotemId { get; set; }
    }
}
