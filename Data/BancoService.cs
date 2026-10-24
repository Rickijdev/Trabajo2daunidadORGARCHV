//BancoService la lógica del banco

using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using BancoApp.Models;   // <- antes: BancoApp.Data.Models

namespace BancoApp.Data; // <- antes: BancoApp.Data
public class BancoService
{
    private readonly BancoDbContext _context;

    public BancoService(BancoDbContext context)
    {
        _context = context;
    }

    // 1. Búsqueda ISAM B-Tree ultra-rápida por Número de Cuenta (< 100 ms)
    public async Task<(Cliente? cliente, long tiempoMs)> BuscarPorCuentaAsync(string nroCuenta)
    {
        var cronometro = Stopwatch.StartNew();

        var cliente = await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.NroCuenta == nroCuenta && c.Activo);

        cronometro.Stop();
        return (cliente, cronometro.ElapsedMilliseconds);
    }

    // 2. Búsqueda por DNI (Índice Secundario B-Tree)
    public async Task<(Cliente? cliente, long tiempoMs)> BuscarPorDniAsync(string dni)
    {
        var cronometro = Stopwatch.StartNew();

        var cliente = await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.DNI == dni && c.Activo);

        cronometro.Stop();
        return (cliente, cronometro.ElapsedMilliseconds);
    }

    // 3. Recorrido LIFO de la Lista Enlazada de Movimientos usando PtrAnt
    public async Task<List<Movimiento>> ObtenerHistorialMovimientosAsync(int clienteId)
    {
        var cliente = await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == clienteId);

        if (cliente == null || cliente.PtrUltimoMov == null)
            return new List<Movimiento>();

        var historial = new List<Movimiento>();
        int? ptrActual = cliente.PtrUltimoMov;

        // Recorrido de puntero en puntero en orden cronológico inverso (LIFO)
        while (ptrActual != null)
        {
            var mov = await _context.Movimientos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == ptrActual);

            if (mov == null) break;

            historial.Add(mov);
            ptrActual = mov.PtrAnt; // Salto al puntero anterior
        }

        return historial;
    }

    // 4. Registrar Nuevo Movimiento (Depósito / Retiro) actualizando la Lista Enlazada
    public async Task<bool> RegistrarMovimientoAsync(string nroCuenta, string tipo, decimal monto)
    {
        using var transaccion = await _context.Database.BeginTransactionAsync();
        try
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.NroCuenta == nroCuenta && c.Activo);

            if (cliente == null) return false;

            // Validar saldo suficiente para retiros ('E')
            if (tipo == "E" && cliente.Saldo < monto) return false;

            // Guardar puntero al movimiento anterior (Cabeza de la lista LIFO)
            int? ptrAnterior = cliente.PtrUltimoMov;

            // Crear el nuevo movimiento
            var nuevoMovimiento = new Movimiento
            {
                ClienteId = cliente.Id,
                Tipo = tipo,
                Monto = monto,
                FechaHora = DateTime.Now,
                PtrAnt = ptrAnterior
            };

            _context.Movimientos.Add(nuevoMovimiento);
            await _context.SaveChangesAsync();

            // Actualizar el saldo del cliente y el puntero al último movimiento
            cliente.Saldo = tipo == "I" ? cliente.Saldo + monto : cliente.Saldo - monto;
            cliente.PtrUltimoMov = nuevoMovimiento.Id;

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();

            return true;
        }
        catch
        {
            await transaccion.RollbackAsync();
            return false;
        }
    }

    // 5. Borrado Lógico de Cliente (Soft Delete - Lista URE / Inactivo)
    public async Task<bool> EliminarClienteLogicoAsync(string nroCuenta)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.NroCuenta == nroCuenta && c.Activo);

        if (cliente == null) return false;

        cliente.Activo = false; // Desactivar bandera
        await _context.SaveChangesAsync();
        return true;
    }
}