using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaTurnos.Data;
using sistemaTurnos.Models;

namespace sistemaTurnos.Controllers;

public class ServiciosController : Controller
{
    public readonly BaseContext _context;
    public ServiciosController(BaseContext context)
    {
        _context = context;
    }


    public IActionResult Index(){
        return View();
    }

    /* REINICIAR CANTIDAD DE TURNOS */
    public async Task<IActionResult> ReiniciarCantidadTurnos(){
        // Obtener todos los servicios
        var servicios = await _context.Servicios.ToListAsync();

        // En cada servicio establecer la cantidad de turnos
        foreach (var servicio in servicios)
        {
            servicio.CantidadTurnos = 0;
        }
        
        // Guardar los cambios en la base de datos
        await _context.SaveChangesAsync();
        return View(); 
    }
 



   
}