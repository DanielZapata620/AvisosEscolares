using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosEscolaresApi.Services
{
    public class AuthService
    {
        public AuthService(Repository<Alumno> repoAlumno,Repository<Maestro> repoMaestro,IMapper mapper)
        {
            RepoAlumno = repoAlumno;
            RepoMaestro = repoMaestro;
            Mapper = mapper;
        }

        public Repository<Alumno> RepoAlumno { get; }
        public Repository<Maestro> RepoMaestro { get; }
        public IMapper Mapper { get; }

        public LoginResponseDTO? Login(LoginDTO dto)
        {
            
            var maestro = RepoMaestro.Query().Include(g=>g.Grupo)
                .FirstOrDefault(m =>
                    m.ClaveAcceso == dto.Usuario &&
                    m.Contrasena == dto.Contrasena);

            if (maestro != null)
            {
                return new LoginResponseDTO
                {
                    Rol = "Maestro",
                    Maestro = Mapper.Map<MaestroDTO>(maestro)
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
                    return new LoginResponseDTO
                    {
                        Rol = "Alumno",
                        Alumno = Mapper.Map<AlumnoDTO>(alumno)
                    };
                }
                    
            }
           
            return null;
        }
    }
}
