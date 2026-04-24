using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AvisosEscolaresApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AvisosController : ControllerBase
    {
        public AvisosController(AvisosService service,IValidator<CrearAvisoGeneralDto> crearGeneralValidator,IValidator<CrearAvisoPersonalDto> crearPersonalValidator)
        {
            Service = service;
            CrearGeneralValidator = crearGeneralValidator;
            CrearPersonalValidator = crearPersonalValidator;
        }

        public AvisosService Service { get; }
        public IValidator<CrearAvisoGeneralDto> CrearGeneralValidator { get; }
        public IValidator<CrearAvisoPersonalDto> CrearPersonalValidator { get; }

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
            var validacion = CrearGeneralValidator.Validate(dto);

            if (!validacion.IsValid)
            {
                var errores = string.Join("\n",
                    validacion.Errors.Select(e => e.ErrorMessage));

                return BadRequest(errores);
            }
            Service.CrearAvisoGeneral(dto);
            return Ok();
        }

        [HttpPost("personal")]
        [Authorize(Roles = "Maestro")]
        public IActionResult CrearPersonal(CrearAvisoPersonalDto dto)
        {
            var validacion = CrearPersonalValidator.Validate(dto);

            if (!validacion.IsValid)
            {
                var errores = string.Join("\n",
                    validacion.Errors.Select(e => e.ErrorMessage));

                return BadRequest(errores);
            }
            Service.CrearAvisoPersonal(dto);
            return Ok();
        }

        [HttpDelete("borrarpersonal/{id}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult EliminarAvisoPersonal(int id)
        {
         
            Service.EliminarAvisoPersonal(id);
            return Ok("Aviso personal eliminado correctamente");
        }
    }
}
