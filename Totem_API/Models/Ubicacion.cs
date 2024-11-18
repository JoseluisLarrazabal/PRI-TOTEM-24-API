using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Totem_API.Models;

public partial class Ubicacion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string? Direccion { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public bool? Activo { get; set; }

    [Column("id_totem")] // Clave foránea
    public int IdTotem { get; set; }

    [JsonIgnore] // Ignorar durante la serialización/deserialización JSON
    [NotMapped] // Ignorar en validaciones y base de datos
    public virtual Totem? Totem { get; set; } // Propiedad de navegación opcional
}
