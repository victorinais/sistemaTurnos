namespace sistemaTurnos.Models{
    public class Asesor{
        public  int Id {get; set;}
        public string Documento  {get; set;}
        public string Correo  {get; set;}
        public string Nombres  {get; set;}
        public string Apellidos  {get; set;}
        public int IdServicio {get; set;}
        public  int IdModulo  {get; set;}
    }
}