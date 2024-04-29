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

    public IActionResult Categorias(){
        return View();
    }
/* MOSTRAR LAS CATEGORIAS Y GUARDAR DATOS(TURNO, SERVICIO) */
    [HttpPost]
    public IActionResult Categorias(int servicio){
        // Traer el Id del usuario
        var usuarioRegistrado = HttpContext.Session.GetString("UsuarioId");
        
        // Agregar el Id del usuario y el id del servicio que eligio en la tabla turnos.
        Turno turno = new Turno{
            IdUsuario = Convert.ToInt32(usuarioRegistrado),
            IdServicio = servicio
        };

       // Añadir a la tabla los cu¡ambis y guardarlos.
        _context.Turnos.Add(turno);
        _context.SaveChanges();

        // Guardar en la session el id del turno que se creo y el id del servicio que se eligio.
        HttpContext.Session.SetString("TurnoId", turno.Id.ToString());
        HttpContext.Session.SetString("TurnoServicio", turno.IdServicio.ToString());

        //Con el idServicio de la tabla turnos encontrar el servicio correspondiente
        var encontrarServicio = _context.Servicios.FirstOrDefault(s => s.Id == turno.IdServicio);
         if (encontrarServicio != null)
        {
            // Traer siglas del servicio que el usuario eligio.
            string siglasDelServicio = encontrarServicio.Siglas;
            // Traer la cantidad de turnos que tiene ese servicio.
            int siglasNumero = encontrarServicio.CantidadTurnos;
            //Sumarle 1 a la cantidad de turnos.
            int numeroActual = siglasNumero+1;

            // Generar el número de turno final con el formato adecuado.
            string turnoMostrado = $"{siglasDelServicio}-{numeroActual:D3}";
            HttpContext.Session.SetString("TurnoMostrado", turnoMostrado);
            
            // Actualizar la cantidad de turnos por el ultimo numero que se entrego y guardaar cambios.
            encontrarServicio.CantidadTurnos = numeroActual;
            _context.SaveChanges();
            return RedirectToAction("MostrarTurno");
        }
        else
        {
            // Verificar si hay un error y no se encontro algun dato necesario.
            ViewBag.Error = "No se encontro el servicio";
            return View();
        }

        return RedirectToAction("MostrarTurno");
    }



/* DARLE UN TURNO AL USUSARIO */
    public IActionResult MostrarTurno(){
        // Traer la Fecha de entrega del turno para hacer la condicional del boton en la vista.
        var btnFechaEntregaTurno = HttpContext.Session.GetString("HoraEntregaTurno");

        // Traer el turno para mostrarlo en la vista
        var turnoMostrado = HttpContext.Session.GetString("TurnoMostrado");
        ViewBag.TurnoFinal = turnoMostrado;
        return View();
    }


/* ENTREGAR TURNO, GUARDAR SU FECHA Y HORA DE ENTREGA Y VOLVER AL INICIO */
    public IActionResult EntregarTurno(){
        // Traer el id del turno que se creo y el tueno que se creo.
        var filaTurno = HttpContext.Session.GetString("TurnoId");
        var fechaHoraActual = DateTime.Now;
        var TurnoFinal = HttpContext.Session.GetString("TurnoMostrado");

        // Buscar la columna en que esta el Id del turno que se creo.
        var turno = _context.Turnos.FirstOrDefault(u => u.Id == Convert.ToInt32(filaTurno));

        // Si se encuentra ese Id...
        if(turno != null){
            // Guardar el Digito en la tabla turnos y la fecha en que se entrego y guardar cambios.
            turno.DigitoTurno = TurnoFinal;
            turno.FechaHoraEntrada = fechaHoraActual;
            _context.SaveChanges();
            
            // Si el boton se oprimio, mostrar confirmacion
            HttpContext.Session.SetString("HoraEntregaTurno", turno.FechaHoraEntrada.ToString());
            if (Request.Form["RecogerTurno"] == "turnoEntrega"){
                // El botón se ha presionado
                TempData["MensajeConfirmacion"]= $"El ficho fue entregado a las {turno.FechaHoraEntrada}";
                return RedirectToAction("MostrarTurno", "Turnos");
            }

            return RedirectToAction("MostrarTurno", "Turnos");

        }else{
            ViewBag.NullUsuario = "No se encontro el usuario";
            return RedirectToAction("MostrarTurno", "Turnos");
        }
    }
    

/* BORRAR LOS DATOS DEL PACIENTE EN LA SESSION AL VOLVER AL INICIO*/
    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Usuarios");            
    }


/* --------------------------------------------------------------------------------------------------- */

    public async Task<IActionResult> TurnosEnEspera(){
        var historialTurnos = await _context.Turnos.Where(u => u.Estado == null || u.Estado == "Llamado").ToListAsync();
        var turnoLlamado = HttpContext.Session.GetInt32("TurnoLlamadoId");
        TempData["ConfirmarLlamado"] = $"El turno llamado es: {turnoLlamado}";

        return View(historialTurnos);
    }

}