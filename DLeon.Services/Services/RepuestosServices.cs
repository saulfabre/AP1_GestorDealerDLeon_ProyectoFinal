using System.Linq.Expressions;
using Aplicada1.Core;
using DLeon.Data.Context;
using DLeon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DLeon.Services;

public class RepuestosServices(IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Repuestos, int>
{
    public async Task<Result<Repuestos?>> Buscar(int repuestoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Repuestos
            .FirstOrDefaultAsync(r => r.RepuestoId == repuestoId);
    }

    public async Task<Result<List<Repuestos>>> GetList(Expression<Func<Repuestos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Repuestos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Existe(int repuestoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Repuestos
            .AnyAsync(r => r.RepuestoId == repuestoId);
    }

    public async Task<bool> Insertar(Repuestos repuesto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Repuestos.Add(repuesto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Repuestos repuesto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(repuesto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int repuestoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Repuestos
            .Where(r => r.RepuestoId == repuestoId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result> Guardar(Repuestos repuesto)
    {
        if (!await Existe(repuesto.RepuestoId))
        {
            return Result.Success(await Insertar(repuesto));
        }
        else
        {
            return Result.Success(await Modificar(repuesto));
        }
    }
}
