using System;
using System.Collections.Generic;

namespace Totem_API.Models
{
    public partial class Ubicacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }
    }
}
