using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvisosEscolaresApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        public AlumnosController(AlumnosServices service)
        {
            Service = service;
        }

        public AlumnosServices Service { get; }

        [HttpGet("grupo/{id}")]
        public IActionResult GetAlumnosByGrupo(int id)
        {
            try
            {
                var alumnos = Service.ObtenerAlumnosByGrupo(id);
                return Ok(alumnos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(AlumnoCreateDTO dto)
        {
            try
            {
                Service.CrearAlumno(dto);
                return Ok();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(x => x.ErrorMessage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
