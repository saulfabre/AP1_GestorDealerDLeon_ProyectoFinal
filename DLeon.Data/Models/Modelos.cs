using System.ComponentModel.DataAnnotations;

namespace DLeon.Data.Models;

public class Modelos
{
    [Key]
    public int ModeloId { get; set; }

    [Required(ErrorMessage = "La marca es obligatoria.")]
    [StringLength(50)]
    public string Marca { get; set; } = "";

    [Required(ErrorMessage = "El nombre del modelo es obligatorio.")]
    [StringLength(50)]
    public string Nombre { get; set; } = "";

    public bool Activo { get; set; } = true;

    public ICollection<Vehiculos> Vehiculos { get; set; } = new List<Vehiculos>();
}