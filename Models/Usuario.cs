namespace sistemaTurnos.Models;

public class Usuario
{
    public int Id { get; set; }
    public int IdTipoDocumento { get; set; }
    public string? Documento { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Correo { get; set; }
    public int? IdServicio { get; set; }
    public int? IdAtencionPrioritaria { get; set; }

}