namespace sistemaTurnos.Models;

public class Usuario
{
    public int Id { get; set; }
    public int Id_Tipo_Documento { get; set; }
    public string? Documento { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Email { get; set; }
    public int? Id_AtencionPrioritario { get; set; }

}