using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvisosEscolaresApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(AuthService service)
        {
            Service = service;
        }

        public AuthService Service { get; }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            var result = Service.Login(dto);

            if (result == null)
                return Unauthorized("Usuario o contraseña incorrectos.");
            
            return Ok(result);
        }
    }
}
