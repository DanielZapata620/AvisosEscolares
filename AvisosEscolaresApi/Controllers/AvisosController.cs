using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvisosEscolaresApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvisosController : ControllerBase
    {
        public AvisosController(AvisosService service)
        {
            Service = service;
        }

        public AvisosService Service { get; }

        [HttpGet("generales/vigentes")]
        public IActionResult GetAvisosGeneralesVigentes()
        {
            var avisos = Service.ObtenerAvisosGeneralesVigentes();
            return Ok(avisos);
        }

        [HttpGet("personales/{id}")]
        public IActionResult GetAvisosPersonales(int id)
        {
            var avisos = Service.ObtenerAvisosPersonales(id);
            return Ok(avisos);
        }

        [HttpGet("personales/alumno/{id}")]
        public IActionResult GetAvisosPersonalesAlumno(int id)
        {
            var avisos = Service.ObtenerAvisosPersonalesAlumno(id);
            return Ok(avisos);
        }

        [HttpGet("personal/alumno/{id}")]
        public IActionResult GetAvisoPersonalAlumno(int id)
        {
            var aviso = Service.ObtenerAvisoPersonalAlumno(id);
            return Ok(aviso);
        }


        [HttpPut("personal/marcarleidos/{alumnoId}")]
        public IActionResult MarcarComoLeidos([FromBody] List<int> ids, int alumnoId)
        {
            Service.MarcarAvisosComoLeido(ids, alumnoId);
            return Ok();
        }

        [HttpPut("marcarleido/{id}")]
        public IActionResult MarcarComoLeido(int id)
        {
            Service.MarcarAvisoComoLeido(id);
            return Ok();
        }

        [HttpPost("general")]
        public IActionResult CrearGeneral(CrearAvisoGeneralDto dto)
        {
            Service.CrearAvisoGeneral(dto);
            return Ok();
        }

        [HttpPost("personal")]
        public IActionResult CrearPersonal([FromBody] CrearAvisoPersonalDto dto)
        {
            Service.CrearAvisoPersonal(dto);
            return Ok();
        }


    }
}
