using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaTurnos.Data;

namespace sistemaTurnos.Controllers;

public class UsuariosController : Controller
{
    public readonly BaseContext _context;
    public UsuariosController(BaseContext context)
    {
        _context = context;
    }


    public IActionResult Index(){
        return View();
    }
    [HttpPost]
    public IActionResult Index(int tipoDocumento, int? prioritario, string documento){
        var Usuario = _context.Usuarios.FirstOrDefault(e => e.IdTipoDocumento == tipoDocumento && e.IdAtencionPrioritaria == prioritario && e.Documento == documento);
        if (Usuario!= null)
        {
            HttpContext.Session.SetString("UsuarioId", Usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombres", Usuario.Nombres);
            HttpContext.Session.SetString("UsuariosApellidos", Usuario.Apellidos);
            return RedirectToAction("Categorias", "Turnos");

        }
        else {
            ViewBag.Error = "¡Alguno de los datos proporcionado es incorrecto!";
            return View();
        }
    }

     
}