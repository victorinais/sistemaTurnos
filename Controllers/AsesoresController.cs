using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaTurnos.Data;
using sistemaTurnos.Models;

namespace sistemaTurnos.Controllers{

    public class AsesoresController: Controller
    {
        public readonly BaseContext _context;
        public AsesoresController(BaseContext context){
            _context = context;
        }


    /* INICIO DE SESION DE LOS ASESORES */
        public IActionResult Login(){
        return View();
        }
        [HttpPost]
        public IActionResult Login(string correo, string documento){
            //Verificar si los datos que se dan coinciden con los de algun asesor.
            var Asesor = _context.Asesores.FirstOrDefault(a => a.Correo == correo && a.Documento == documento);

            // Si se encuentra al asesor ...
            if (Asesor != null){

                //Guardar los datos que se necesitan del asesor en la sesion
                HttpContext.Session.SetString("AsesorId", Asesor.Id.ToString());
                HttpContext.Session.SetString("AsesorNombres", Asesor.Nombres);
                HttpContext.Session.SetString("AsesorApellidos", Asesor.Apellidos);
                HttpContext.Session.SetString("AsesorServicio", Asesor.IdServicio.ToString());
                HttpContext.Session.SetString("AsesorModulo", Asesor.IdModulo.ToString());
                return RedirectToAction("AtenderTurnos", "Asesores");

            }
            else {
                ViewBag.Error = "¡Correo o documento incorrectos!";
                return View();
            }
        }


        public IActionResult CambiarServicio(){
            return View();
        }


    /* LISTAR LOS TURNOS PENDIENTES DE ACUERDO AL SERVICIO */
        public async Task<IActionResult> AtenderTurnos(){
            // Traer el servicio al que se dedica el asesor
            var asesorCategoria = HttpContext.Session.GetString("AsesorServicio");
            var nombresAsesor = HttpContext.Session.GetString("AsesorNombres");
            var apellidossAsesor = HttpContext.Session.GetString("AsesorApellidos");
            ViewBag.NombreAsesor = $"{nombresAsesor} {apellidossAsesor}";

            // Buscar el servicio mediante el IdServicio del asesor.
            var buscarServicio = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == Convert.ToInt32(asesorCategoria));
            if(buscarServicio != null){
                ViewBag.ServicioAsesor = buscarServicio.TipoServicio;
            }

            // Traer los turnos que estan pendientes y que eligieron el servicio que atiende el asesor
            var historialTurnos = await _context.Turnos.Where(u => u.Estado != "Completado" && u.Estado != "Ausente" && u.IdServicio == Convert.ToInt32(asesorCategoria)).ToListAsync();
            return View(historialTurnos);
        }



    /* LLAMAR TURNO */
        [HttpPost]
        public async Task<IActionResult> LlamarTurno(int id){
            // Traer el modulo en el que esta el asesor.
            var moduloTurno = HttpContext.Session.GetString("AsesorModulo");

            // Buscar al el turno que se llamo
            var turnoLlamado = await _context.Turnos.FirstOrDefaultAsync(m => m.Id == id);

            // Si se encuentra el turno...
            if (turnoLlamado != null){
                // Guardar el modulo del que se llamo el turno y cambiar estado a "Llamado" y guardar cambios.
                turnoLlamado.IdModulo = Convert.ToInt32(moduloTurno);
                turnoLlamado.Estado = "Llamado";
                _context.SaveChanges();

                // Eliminar el id del turno que se llamo anteriormente.
                HttpContext.Session.Remove("TurnoLlamadoId");

                // Guardar el id del turno que se llamo actualmente.
                HttpContext.Session.SetInt32("TurnoLlamadoId", id);
                TempData["confirmacionLlamado"] = $"Se está llamando al turno {turnoLlamado.DigitoTurno}";
            }

            return RedirectToAction("AtenderTurnos", "Asesores");
        }


    /* EL USUARIO DEL TURNO SE HIZO PRESENTE */
        [HttpPost]
        public async Task<IActionResult> TurnoPresente(int id){
            // Buscar el id del turno del usuario que se presento
            var turnoPresente = await _context.Turnos.FirstOrDefaultAsync(m => m.Id == id);
            // Si se encontro el id del turno, indicar que el estado esta en "En proceso".
            if(turnoPresente != null){
                turnoPresente.Estado = "En proceso";
                _context.SaveChanges();
                TempData["TurnoPresente"] = $"El turno {turnoPresente.DigitoTurno} se esta atendiendo";
                return RedirectToAction("AtenderTurnos", "Asesores");
            }
            return RedirectToAction("AtenderTurnos", "Asesores");
        }

    /* FINALIZAR O COMPLETAR TURNO */
        [HttpPost]
        public async Task<IActionResult> TurnoFinalizado(int id){
            var fechaHoraActual = DateTime.Now;

            // Buscar el id del turno que se completo
            var finalizarTurno = await _context.Turnos.FirstOrDefaultAsync(m => m.Id == id);
            // Si se encontro el id del turno guardar la hora en que finalizo y cambiar el estado a "Completado". 
            if(finalizarTurno != null){
                finalizarTurno.FechaHoraSalida = fechaHoraActual;
                finalizarTurno.Estado = "Completado";
                _context.SaveChanges();

                TempData["TurnoCompletado"] = $"El turno {finalizarTurno.DigitoTurno} se completó a las {finalizarTurno.FechaHoraSalida}";
                return RedirectToAction("AtenderTurnos", "Asesores");
            }
            return RedirectToAction("AtenderTurnos", "Asesores");
        }

    /* SI EL USUARIO NO SE PRESENTO AL LLAMADO SE SU TURNO */
        [HttpPost]
        public async Task<IActionResult> TurnoAusente(int id){
            // Buscar el id del turno que no se presento.
            var turnoAusente = await _context.Turnos.FirstOrDefaultAsync(m => m.Id == id);
            // Si se encontro el id del turno, cambiar su estado a "Ausente".
            if(turnoAusente != null){
                turnoAusente.Estado = "Ausente";
                _context.SaveChanges();

                TempData["TurnoAusente"] = $"El turno {turnoAusente.DigitoTurno} No se presento";
                return RedirectToAction("AtenderTurnos", "Asesores");
            }
            return RedirectToAction("AtenderTurnos", "Asesores");
        }


        /* BORRAR LOS DATOS DEL ASESOR EN LA SESSION AL CERRAR CESION*/
        [HttpPost]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Asesores");            
        }




       


    }
}