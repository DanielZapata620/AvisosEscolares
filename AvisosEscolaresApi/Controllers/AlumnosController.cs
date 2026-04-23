using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvisosEscolaresApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        public AlumnosController(AlumnosServices service)
        {
            Service = service;
        }

        public AlumnosServices Service { get; }
        [Authorize(Roles = "Maestro")]
        [HttpGet("grupo")]
        public IActionResult GetAlumnosByGrupo()
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "IdGrupo")?.Value, out var grupoId);
            try
            {
                var alumnos = Service.ObtenerAlumnosByGrupo(grupoId);
                return Ok(alumnos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Maestro")]
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
