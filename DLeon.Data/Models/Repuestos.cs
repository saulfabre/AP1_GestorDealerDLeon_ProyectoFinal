using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLeon.Data.Models;

public class Repuestos
{
    [Key]
    public int RepuestoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = "";

    public string Descripcion { get; set; } = "";

    public string Marca { get; set; } = "";

    public string Categoria { get; set; } = "";

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "El precio de venta debe ser mayor que cero.")]
    public decimal PrecioVenta { get; set; }

    public int Existencia { get; set; } = 0;

    public string? Imagen { get; set; }

    public bool Activo { get; set; } = true;
}
