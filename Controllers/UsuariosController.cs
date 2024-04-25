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

    //listar usuarios
    public async Task<IActionResult> Index(){
        return View(await _context.Usuarios.ToListAsync());
    }

    //
}