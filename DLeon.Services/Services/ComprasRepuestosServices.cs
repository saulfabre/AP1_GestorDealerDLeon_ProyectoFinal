using System.Linq.Expressions;
using Aplicada1.Core;
using DLeon.Data.Context;
using DLeon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DLeon.Services;

public class ComprasRepuestosServices(IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<ComprasRepuestos, int>
{
    public async Task<Result<ComprasRepuestos?>> Buscar(int compraId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.ComprasRepuestos
            .Include(c => c.Proveedor)
            .Include(c => c.Detalles)
            .ThenInclude(d => d.Repuesto)
            .FirstOrDefaultAsync(c => c.CompraId == compraId);
    }

    public async Task<bool> Existe(int compraId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.ComprasRepuestos
            .AnyAsync(c => c.CompraId == compraId);
    }

    public async Task<Result<List<ComprasRepuestos>>> GetList(Expression<Func<ComprasRepuestos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.ComprasRepuestos
            .Include(c => c.Proveedor)
            .Include(c => c.Detalles)
            .ThenInclude(d => d.Repuesto)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Insertar(ComprasRepuestos compra)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        foreach (var detalle in compra.Detalles)
        {
            var repuesto = await contexto.Repuestos
                .SingleAsync(r => r.RepuestoId == detalle.RepuestoId);

            repuesto.Existencia += detalle.Cantidad;
        }

        compra.Total = compra.Detalles.Sum(d => d.Cantidad * d.Precio);
        contexto.ComprasRepuestos.Add(compra);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(ComprasRepuestos compra)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var compraExistente = await contexto.ComprasRepuestos
            .Include(c => c.Detalles)
            .FirstOrDefaultAsync(c => c.CompraId == compra.CompraId);

        if (compraExistente == null)
        {
            return false;
        }

        foreach (var detalle in compraExistente.Detalles.ToList())
        {
            var repuesto = await contexto.Repuestos
                .SingleAsync(r => r.RepuestoId == detalle.RepuestoId);

            repuesto.Existencia -= detalle.Cantidad;
            contexto.ComprasRepuestosDetalles.Remove(detalle);
        }

        foreach (var detalle in compra.Detalles)
        {
            var repuesto = await contexto.Repuestos
                .SingleAsync(r => r.RepuestoId == detalle.RepuestoId);

            repuesto.Existencia += detalle.Cantidad;

            contexto.ComprasRepuestosDetalles.Add(new ComprasRepuestosDetalles
            {
                CompraId = compra.CompraId,
                RepuestoId = detalle.RepuestoId,
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio
            });
        }

        compraExistente.ProveedorId = compra.ProveedorId;
        compraExistente.Fecha = compra.Fecha;
        compraExistente.NumeroFactura = compra.NumeroFactura;
        compraExistente.Observacion = compra.Observacion;
        compraExistente.Total = compra.Detalles.Sum(d => d.Cantidad * d.Precio);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int compraId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var compra = await contexto.ComprasRepuestos
            .Include(c => c.Detalles)
            .FirstOrDefaultAsync(c => c.CompraId == compraId);

        if (compra == null)
        {
            return Result.Failure(new Error("CompraNoEncontrada", "La compra no existe."));
        }

        await AfectarRepuestos(compra.Detalles.ToArray(), false);
        contexto.ComprasRepuestosDetalles.RemoveRange(compra.Detalles);
        contexto.ComprasRepuestos.Remove(compra);

        var eliminado = await contexto.SaveChangesAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task AfectarRepuestos(ComprasRepuestosDetalles[] detalles, bool op)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        foreach (var detalle in detalles)
        {
            var repuesto = await contexto.Repuestos
                .SingleAsync(r => r.RepuestoId == detalle.RepuestoId);

            if (op)
            {
                repuesto.Existencia += detalle.Cantidad;
            }
            else
            {
                repuesto.Existencia -= detalle.Cantidad;
            }
        }

        await contexto.SaveChangesAsync();
    }

    public async Task<Result> Guardar(ComprasRepuestos compra)
    {
        if (!await Existe(compra.CompraId))
        {
            return Result.Success(await Insertar(compra));
        }
        else
        {
            return Result.Success(await Modificar(compra));
        }
    }
}
