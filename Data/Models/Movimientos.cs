namespace BancoApp.Models
{
public class Movimiento
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Tipo { get; set; } = "I"; // 'I': Ingreso/Depósito, 'E': Egreso/Retiro
    public decimal Monto { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public int? PtrAnt { get; set; } // Puntero de Lista Enlazada al movimiento anterior
    }
}