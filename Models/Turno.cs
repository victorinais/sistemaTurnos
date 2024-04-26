using sistemaTurnos.Models;
namespace sistemaTurnos.Models;

public class Turno
{
    public int Id { get; set; }
    public int IdUsuario  { get; set; }
    public int IdServicio { get; set; }
    public DateTime FechaHoraEntrada { get; set; }
    public DateTime FechaHoraSalida { get; set; }
    public int IdModulo { get; set; }
    public string? Estado { get; set; }

    public virtual required Usuario Usuario { get; set; }
    public virtual required Servicio Servicio { get; set; }
    public virtual required Modulo Modulo { get; set; }
}
