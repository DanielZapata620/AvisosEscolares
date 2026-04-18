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
