using DLeon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DLeon.Data.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options)
        : base(options)
    {

    }

    public DbSet<Vehiculos> Vehiculos { get; set; }
    public DbSet<Proveedores> Proveedores { get; set; }
    public DbSet<Repuestos> Repuestos { get; set; }
    public DbSet<ComprasRepuestos> ComprasRepuestos { get; set; }
    public DbSet<ComprasRepuestosDetalles> ComprasRepuestosDetalles { get; set; }

}
