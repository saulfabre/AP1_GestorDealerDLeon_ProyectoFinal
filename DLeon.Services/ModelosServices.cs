using System.Linq.Expressions;
using Aplicada1.Core;
using DLeon.Data.Context;
using DLeon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DLeon.Services;

public class ModelosServices(IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Modelos, int>
{
    public async Task<Result<Modelos?>> Buscar(int modeloId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Modelos
            .FirstOrDefaultAsync(m => m.ModeloId == modeloId);
    }

    public async Task<Result<List<Modelos>>> GetList(Expression<Func<Modelos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Modelos
            .Include(m => m.Vehiculos)
            .Where(criterio)
            .OrderBy(m => m.Marca)
            .ThenBy(m => m.Nombre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Existe(int modeloId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Modelos
            .AnyAsync(m => m.ModeloId == modeloId);
    }

    public async Task<bool> ExisteNombre(string marca, string nombre, int modeloId = 0)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Modelos.AnyAsync(m =>
            m.Marca == marca && m.Nombre == nombre && m.ModeloId != modeloId);
    }

    public async Task<bool> TieneRelaciones(int modeloId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Vehiculos
            .AnyAsync(v => v.ModeloId == modeloId);
    }

    public async Task<bool> Insertar(Modelos modelo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Modelos.Add(modelo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Modelos modelo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(modelo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int modeloId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Modelos
            .Where(m => m.ModeloId == modeloId && !m.Vehiculos.Any())
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result> CambiarActivo(int modeloId, bool activo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var modificado = await contexto.Modelos
            .Where(m => m.ModeloId == modeloId)
            .ExecuteUpdateAsync(s => s.SetProperty(m => m.Activo, activo)) > 0;
        return Result.Success(modificado);
    }

    public async Task<Result> Guardar(Modelos modelo)
    {
        if (!await Existe(modelo.ModeloId))
        {
            return Result.Success(await Insertar(modelo));
        }
        else
        {
            return Result.Success(await Modificar(modelo));
        }
    }
}