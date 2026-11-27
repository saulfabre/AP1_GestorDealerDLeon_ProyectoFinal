using System.ComponentModel.DataAnnotations;

namespace DLeon.Data.Models;

public class Proveedores
{
    [Key]
    public int ProveedorId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = "";

    public string RNC { get; set; } = "";

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    public string Telefono { get; set; } = "";

    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    public string Correo { get; set; } = "";

    public string Direccion { get; set; } = "";
    
    public bool Activo { get; set; } = true;
}
