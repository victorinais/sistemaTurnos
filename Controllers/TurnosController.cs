using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaTurnos.Data;
using sistemaTurnos.Models;

namespace sistemaTurnos.Controllers;

public class TurnosController : Controller
{
    public readonly BaseContext _context;
    public TurnosController(BaseContext context)
    {
        _context = context;
    }

    //listar usuarios
    public async Task<IActionResult> Index()
    {
        return View(await _context.Turnos.ToListAsync());
    }

    //
}