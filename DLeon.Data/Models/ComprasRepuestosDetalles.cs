using System.ComponentModel.DataAnnotations;

namespace DLeon.Data.Models;

public class ComprasRepuestosDetalles
{
    [Key]
    public int DetalleId { get; set; }

    public int CompraId { get; set; }

    public int RepuestoId { get; set; }

    public Repuestos? Repuesto { get; set; }

    [Range(1, 1000000, ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public int Cantidad { get; set; }

    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Precio { get; set; }

    public ComprasRepuestos? Compra { get; set; }
}
