using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvisosEscolaresApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AvisosController : ControllerBase
    {
        public AvisosController(AvisosService service)
        {
            Service = service;
        }

        public AvisosService Service { get; }

        [HttpGet("generales/vigentes")]
        [Authorize(Roles = "Maestro")]
        public IActionResult GetAvisosGeneralesVigentes()
        {
            var avisos = Service.ObtenerAvisosGeneralesVigentes();
            return Ok(avisos);
        }

        [HttpGet("personales/{id}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult GetAvisosPersonales(int id)
        {
            var avisos = Service.ObtenerAvisosPersonales(id);
            return Ok(avisos);
        }

        [HttpGet("personales/alumno")]
        [Authorize(Roles = "Alumno")]
        public IActionResult GetAvisosPersonalesAlumno()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
            var avisos = Service.ObtenerAvisosPersonalesAlumno(idUsuario);
            return Ok(avisos);
        }

        [HttpGet("personal/alumno/{idAviso}")]
        [Authorize(Roles = "Alumno")]
        public IActionResult GetAvisoPersonalAlumno(int idAviso)
        {
           
            var aviso = Service.ObtenerAvisoPersonalAlumno(idAviso);
            return Ok(aviso);
        }

        [HttpGet("general/alumno/{idAviso}")]
        [Authorize(Roles = "Alumno")]
        public IActionResult GetAvisoGeneralAlumno(int idAviso)
        {

            var aviso = Service.ObtenerAvisoGeneralAlumno(idAviso);
            return Ok(aviso);
        }

        [HttpGet("generales")]
        [Authorize(Roles = "Alumno")]
        public IActionResult GetAvisosGeneralesPorAlumno()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
            var avisos = Service.ObtenerAvisosGeneralesVigentesAlumno(idUsuario);
            return Ok(avisos);
        }

        [HttpPut("personal/marcarleidos")]
        [Authorize(Roles = "Alumno")]
        public IActionResult MarcarComoLeidos([FromBody] List<int> ids)
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
            Service.MarcarAvisosComoRecibido(ids, idUsuario);
            return Ok();
        }

        [HttpPut("marcarleido/{id}")]
        [Authorize(Roles = "Alumno")]
        public IActionResult MarcarComoLeido(int id)
        {
            Service.MarcarAvisoComoLeido(id);
            return Ok();
        }

        [HttpPost("general")]
        [Authorize(Roles = "Maestro")]
        public IActionResult CrearGeneral(CrearAvisoGeneralDto dto)
        {
            Service.CrearAvisoGeneral(dto);
            return Ok();
        }

        [HttpPost("personal")]
        [Authorize(Roles = "Maestro")]
        public IActionResult CrearPersonal([FromBody] CrearAvisoPersonalDto dto)
        {
            Service.CrearAvisoPersonal(dto);
            return Ok();
        }


    }
}
