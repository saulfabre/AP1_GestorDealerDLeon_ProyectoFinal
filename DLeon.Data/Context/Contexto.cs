using Microsoft.EntityFrameworkCore;

namespace DLeon.Data.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<Models.Modelos> Modelos => Set<Models.Modelos>();
    public DbSet<Models.Vehiculos> Vehiculos => Set<Models.Vehiculos>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Modelos>()
            .HasIndex(m => new { m.Marca, m.Nombre })
            .IsUnique();

        modelBuilder.Entity<Models.Vehiculos>()
            .HasIndex(v => v.Matricula)
            .IsUnique();

        modelBuilder.Entity<Models.Vehiculos>()
            .Property(v => v.Precio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Models.Vehiculos>()
            .HasOne(v => v.Modelo)
            .WithMany(m => m.Vehiculos)
            .HasForeignKey(v => v.ModeloId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
