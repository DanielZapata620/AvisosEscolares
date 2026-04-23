using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;
using AvisosEscolaresApi.Validators;
using FluentValidation;

namespace AvisosEscolaresApi.Services
{
    
    public class AlumnosServices
    {
        public AlumnosServices(Repository<Alumno> repoAlumno, Repository<Grupo> repoGrupo, AgregarAlumnoValidator agregarValidator,IMapper mapper)
        {
            RepoAlumno = repoAlumno;
            RepoGrupo = repoGrupo;
            AgregarValidator = agregarValidator;
            Mapper = mapper;
        }

        public Repository<Alumno> RepoAlumno { get; }
        public Repository<Grupo> RepoGrupo { get; }
        public AgregarAlumnoValidator AgregarValidator { get; }
        public IMapper Mapper { get; }

        public List<AlumnoDetallesListaDTO> ObtenerAlumnosByGrupo(int grupoId)
        {
            var alumnos = RepoAlumno.GetAll().Where(a => a.GrupoId == grupoId);

            if(alumnos.Any(a => a.GrupoId != grupoId))
                throw new UnauthorizedAccessException();
            return alumnos.Select(a => Mapper.Map<AlumnoDetallesListaDTO>(a)).ToList();
        }

        public void CrearAlumno(AlumnoCreateDTO dto)
        {
            // 1️⃣ VALIDAR DTO
            var result = AgregarValidator.Validate(dto);

            if (!result.IsValid)
            {

                throw new ValidationException(result.Errors);
            }

            // 2️⃣ VERIFICAR QUE EL GRUPO EXISTE
            //var grupo = _grupoRepo.Get(dto.GrupoId);
            //if (grupo == null)
            //    throw new Exception("El grupo especificado no existe.");

            // 3️⃣ CREAR ENTIDAD
            var alumno = Mapper.Map<Alumno>(dto);
           
            RepoAlumno.Insert(alumno);

           
            
        }

    }
}
