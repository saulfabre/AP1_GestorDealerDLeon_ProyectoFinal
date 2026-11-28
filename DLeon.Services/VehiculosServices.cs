using System.Linq.Expressions;
using Aplicada1.Core;
using DLeon.Data.Context;
using DLeon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DLeon.Services;

public class VehiculosServices(IDbContextFactory<Contexto> contextFactory)
    : IServiceResult<Vehiculos, int>
{
    public async Task<Result<Vehiculos?>> Buscar(int vehiculoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Vehiculos
            .FirstOrDefaultAsync(v => v.VehiculoId == vehiculoId);
    }

    public async Task<Result<List<Vehiculos>>> GetList(Expression<Func<Vehiculos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Vehiculos
            .Include(v => v.Modelo)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Existe(int vehiculoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Vehiculos.AnyAsync(v => v.VehiculoId == vehiculoId);
    }

    public async Task<bool> ExisteMatricula(string matricula, int vehiculoId = 0)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Vehiculos.AnyAsync(v =>
            v.Matricula == matricula && v.VehiculoId != vehiculoId);
    }

    public async Task<bool> Insertar(Vehiculos vehiculo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Vehiculos.Add(vehiculo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Vehiculos vehiculo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Vehiculos.Update(vehiculo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int vehiculoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Vehiculos
            .Where(v => v.VehiculoId == vehiculoId && v.Estado != "Vendido")
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result> CambiarEstado(int vehiculoId, string estado)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var modificado = await contexto.Vehiculos
            .Where(v => v.VehiculoId == vehiculoId && v.Estado != "Vendido")
            .ExecuteUpdateAsync(s => s.SetProperty(v => v.Estado, estado)) > 0;
        return Result.Success(modificado);
    }

    public async Task<Result> Guardar(Vehiculos vehiculo)
    {
        if (!await Existe(vehiculo.VehiculoId))
        {
            return Result.Success(await Insertar(vehiculo));
        }

        return Result.Success(await Modificar(vehiculo));
    }
}
