using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using FluentValidation;
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
        public AlumnosController(AlumnosServices service,IValidator<AlumnoCreateDTO> crearValidator)
        {
            Service = service;
            CrearValidator = crearValidator;
        }

        public AlumnosServices Service { get; }
        public IValidator<AlumnoCreateDTO> CrearValidator { get; }

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
                var validacion = CrearValidator.Validate(dto);
                if (!validacion.IsValid)
                {
                    var errores = string.Join("\n",
                        validacion.Errors.Select(e => e.ErrorMessage));

                    return BadRequest(errores);
                }
                Service.CrearAlumno(dto);
                return Ok();
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("alumno/{id}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult EliminarAlumno(int id)
        {
            Service.EliminarAlumno(id);
            return Ok(new { mensaje = "Alumno eliminado correctamente" });
        }

    }
}
