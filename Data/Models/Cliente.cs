namespace BancoApp.Models;

public class Cliente
{
    public int Id { get; set; }
    public string NroCuenta { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public int? PtrUltimoMov { get; set; }
    public bool Activo { get; set; } = true;
}
