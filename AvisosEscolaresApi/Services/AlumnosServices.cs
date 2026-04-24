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
        public AlumnosServices(Repository<Alumno> repoAlumno, Repository<Grupo> repoGrupo, AgregarAlumnoValidator agregarValidator,IMapper mapper, Repository<Maestro> repoMaestro)
        {
            RepoAlumno = repoAlumno;
            RepoGrupo = repoGrupo;
            AgregarValidator = agregarValidator;
            Mapper = mapper;
            RepoMaestro = repoMaestro;
        }

        public Repository<Alumno> RepoAlumno { get; }
        public Repository<Grupo> RepoGrupo { get; }
        public AgregarAlumnoValidator AgregarValidator { get; }
        public IMapper Mapper { get; }
        public Repository<Maestro> RepoMaestro { get; }

        public List<AlumnoDetallesListaDTO> ObtenerAlumnosByGrupo(int grupoId)
        {
            var alumnos = RepoAlumno.GetAll().Where(a => a.GrupoId == grupoId && a.Eliminado == 0).OrderBy(x=>x.Nombre);

            if(alumnos.Any(a => a.GrupoId != grupoId))
                throw new UnauthorizedAccessException();
            return alumnos.Select(a => Mapper.Map<AlumnoDetallesListaDTO>(a)).ToList();
        }

        
        public AlumnoDTO ObtenerAlumnoById(int id)
        {
            var alumno = RepoAlumno.Get(id);
           
            return Mapper.Map<AlumnoDTO>(alumno);
        }

        public void CrearAlumno(AlumnoCreateDTO dto)
        {
            
            var alumno = Mapper.Map<Alumno>(dto);
           
            RepoAlumno.Insert(alumno);

           
            
        }

        public void EliminarAlumno(int id)
        {
            var alumno = RepoAlumno.Query()
                .FirstOrDefault(a => a.Id == id);

            if (alumno != null)
            {
                alumno.Eliminado = 1;
                RepoAlumno.Update(alumno);
            }
        }

    }
}
