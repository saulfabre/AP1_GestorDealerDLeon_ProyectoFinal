using System.Linq.Expressions;
using Aplicada1.Core;
using DLeon.Data.Context;
using DLeon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DLeon.Services;

public class ProveedoresServices(IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Proveedores, int>
{
    public async Task<Result<Proveedores?>> Buscar(int proveedorId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Proveedores
            .FirstOrDefaultAsync(p => p.ProveedorId == proveedorId);
    }

    public async Task<Result<List<Proveedores>>> GetList(Expression<Func<Proveedores, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Proveedores
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Existe(int proveedorId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Proveedores
            .AnyAsync(p => p.ProveedorId == proveedorId);
    }

    public async Task<bool> Insertar(Proveedores proveedor)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Proveedores.Add(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Proveedores proveedor)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int proveedorId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Proveedores
            .Where(p => p.ProveedorId == proveedorId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result> Guardar(Proveedores proveedor)
    {
        if (!await Existe(proveedor.ProveedorId))
        {
            return Result.Success(await Insertar(proveedor));
        }
        else
        {
            return Result.Success(await Modificar(proveedor));
        }
    }
}
