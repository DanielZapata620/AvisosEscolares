using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;
using AvisosEscolaresApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AvisosEscolaresApi.Services
{
    public class AuthService
    {
        public AuthService(Repository<Alumno> repoAlumno, Repository<Maestro> repoMaestro, IMapper mapper, IConfiguration configuration)
        {
            RepoAlumno = repoAlumno;
            RepoMaestro = repoMaestro;
            Mapper = mapper;
            Configuration = configuration;
           
        }

        public Repository<Alumno> RepoAlumno { get; }
        public Repository<Maestro> RepoMaestro { get; }
        public IMapper Mapper { get; }
        public IConfiguration Configuration { get; }
        

        public LoginResponseDTO? Login(LoginDTO dto)
        {
            

            var maestro = RepoMaestro.Query().Include(g=>g.Grupo)
                .FirstOrDefault(m =>
                    m.ClaveAcceso == dto.Usuario &&
                    m.Contrasena == dto.Contrasena);

            if (maestro != null)
            {
                var m = Mapper.Map<MaestroDTO>(maestro);
                var claims = new List<Claim>() {
                    new Claim (ClaimTypes.NameIdentifier,maestro.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Maestro"),
                    new Claim("IdGrupo", m.Id.ToString())};

                return new LoginResponseDTO
                {
                    Token = GenerarJWT(claims),
                    Rol = "Maestro",
                    Maestro = m
                };
                
            }
                
            else
            {
                var alumno = RepoAlumno.Query().Include(g => g.Grupo).Include(m => m.Grupo.Maestro)
                    .FirstOrDefault(a =>
                        a.Usuario == dto.Usuario &&
                        a.Contrasena == dto.Contrasena);
                if (alumno != null)
                {

                    var claims = new List<Claim>() {
                    new Claim (ClaimTypes.NameIdentifier,alumno.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Alumno"),
                    new Claim("IdGrupo", alumno.GrupoId.ToString())};


                    return new LoginResponseDTO
                    {
                        Token= GenerarJWT(claims),
                        Rol = "Alumno",
                        Alumno = Mapper.Map<AlumnoDTO>(alumno)
                    };
                }
                    
            }
           
            return null;
        }

        public string GenerarJWT(List<Claim> claims)
        {
            var key = Configuration.GetValue<string>("Jwt:SecretKey");
            var tokenDescriptor = new JwtSecurityToken(

                issuer: Configuration.GetValue<string>("Jwt:Issuer"),
                audience: Configuration.GetValue<string>("Jwt:Audience"),
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? "")),
                    SecurityAlgorithms.HmacSha256Signature
                ));

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenDescriptor);
        }
    }
}
