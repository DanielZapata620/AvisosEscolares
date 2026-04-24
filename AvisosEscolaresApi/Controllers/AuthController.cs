using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvisosEscolaresApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(AuthService service, IValidator<LoginDTO> validator,AlumnosServices alumnosService,MaestrosService maestrosService)
        {
            Service = service;
            Validator = validator;
            AlumnosService = alumnosService;
            MaestrosService = maestrosService;
        }

        public AuthService Service { get; }
        public IValidator<LoginDTO> Validator { get; }
        public AlumnosServices AlumnosService { get; }
        public MaestrosService MaestrosService { get; }

        //[HttpGet]
        //[Authorize]
        //public IActionResult GetUsuario()
        //{
        //    int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
        //    var rol = User.FindFirst(ClaimTypes.Role)?.Value;

        //    if (idUsuario == null || rol == null)
        //        return Unauthorized();

            

        //    if (rol == "Alumno")
        //    {
        //        var alumno = AlumnosService.ObtenerAlumnoById(idUsuario);
        //        return Ok(alumno);
        //    }
        //    else if (rol == "Maestro")
        //    {
        //        var maestro = MaestrosService.ObtenerMaestroById(idUsuario);
        //        return Ok(maestro);
        //    }

        //    return Unauthorized();
        //}


        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                var validacion = Validator.Validate(dto);

                if (!validacion.IsValid)
                {
                    var errores = string.Join("\n",
                        validacion.Errors.Select(e => e.ErrorMessage));

                    return BadRequest(errores);
                }

                var result = Service.Login(dto);

                if (result == null)
                    return Unauthorized("Usuario o contraseña incorrectos.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }

        }
    }
}
