//Este es el punte con mi sql Server
//Traduce tu código C# a SQL.

using Microsoft.EntityFrameworkCore;
using BancoApp.Models;

namespace BancoApp.Data;

public class BancoDbContext : DbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }
}