namespace sistemaTurnos.Models;

public class Servicio
{
    public int Id { get; set; }
    public string? TipoServicio { get; set; }
    public string Siglas {get; set;}
    public int CantidadTurnos { get; set; }
}