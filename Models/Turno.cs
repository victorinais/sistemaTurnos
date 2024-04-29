namespace sistemaTurnos.Models;

public class Turno
{
    public int Id { get; set; }
    public int IdUsuario  { get; set; }
    public int IdServicio { get; set; }
    public int? IdModulo { get; set; }
    public string? DigitoTurno {get; set;}
    public DateTime? FechaHoraEntrada { get; set; }
    public DateTime? FechaHoraSalida { get; set; }
    public string? Estado { get; set; }
}
