using System.ComponentModel.DataAnnotations;

namespace DLeon.Data.Models;

public class ComprasRepuestos
{
    [Key]
    public int CompraId { get; set; }

    public int ProveedorId { get; set; }

    public Proveedores? Proveedor { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "El número de factura es obligatorio.")]
    public string NumeroFactura { get; set; } = "";

    public string Observacion { get; set; } = "";

    public decimal Total { get; set; }

    public List<ComprasRepuestosDetalles> Detalles { get; set; } = new();
}
