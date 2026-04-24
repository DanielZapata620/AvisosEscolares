using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosEscolaresApi.Services
{
    public class AvisosService
    {
        public AvisosService(Repository<Aviso> avisoRepo,
        Repository<Avisogeneral> avisoGeneralRepo,
        Repository<Avisopersonal> avisoPersonalRepo,
        Repository<Avisoalumnoestado> estadoRepo,
        Repository<Alumno> alumnoRepo,
        IMapper mapper)
        {
            AvisoRepo = avisoRepo;
            AvisoGeneralRepo = avisoGeneralRepo;
            AvisoPersonalRepo = avisoPersonalRepo;
            EstadoRepo = estadoRepo;
            AlumnoRepo = alumnoRepo;
            Mapper = mapper;
        }

        public Repository<Aviso> AvisoRepo { get; }
        public Repository<Avisogeneral> AvisoGeneralRepo { get; }
        public Repository<Avisopersonal> AvisoPersonalRepo { get; }
        public Repository<Avisoalumnoestado> EstadoRepo { get; }
        public Repository<Alumno> AlumnoRepo { get; }
        public IMapper Mapper { get; }


        public List<AvisoGeneralDetallesMaestroDTO> ObtenerAvisosGeneralesVigentes()
        {

            var avisos = AvisoRepo.Query().Include(x => x.Avisogeneral).Include(a => a.Avisoalumnoestado).ThenInclude(e => e.Estado).Where(a => a.TipoAvisoId == 1 && a.Avisogeneral.FechaCaducidad >= DateTime.Now).OrderByDescending(x=>x.Avisogeneral.FechaCaducidad);
            
            return avisos.Select(a => Mapper.Map<AvisoGeneralDetallesMaestroDTO>(a)).ToList();


        }




        public List<AvisoPersonalDetallesMaestroDTO> ObtenerAvisosPersonales(int id)
        {
            var avisos = EstadoRepo.Query().Include(x => x.Aviso).Include(x => x.Estado).Where(x => x.AlumnoId == id && x.Aviso.TipoAvisoId==2 && x.Aviso.Avisopersonal.Eliminado==0).OrderByDescending(x=>x.Aviso.FechaCreacion);
            return avisos.Select(a => Mapper.Map<AvisoPersonalDetallesMaestroDTO>(a)).ToList();
        }

        public List<AvisoPersonalListaAlumnoDTO> ObtenerAvisosPersonalesAlumno(int id)
        {
            var avisos = EstadoRepo.Query().Include(x => x.Aviso).ThenInclude(x => x.Avisopersonal).ThenInclude(x => x.Maestro).Include(x => x.Estado).Where(x => x.AlumnoId == id && x.Aviso.TipoAvisoId == 2).OrderBy(x=>x.EstadoId).ThenBy(x=>x.Aviso.FechaCreacion);
            return avisos.Select(a => Mapper.Map<AvisoPersonalListaAlumnoDTO>(a)).ToList();
        }

        public AvisoPersonalAlumnoDTO ObtenerAvisoPersonalAlumno(int id)
        {
            var aviso = EstadoRepo.Query().Include(x => x.Aviso).ThenInclude(x => x.Avisopersonal).ThenInclude(x => x.Maestro).Include(x => x.Estado).FirstOrDefault(x => x.Id == id );
            return Mapper.Map<AvisoPersonalAlumnoDTO>(aviso);
        }

        public void MarcarAvisosComoRecibido(List<int> avisoId, int alumnoId)
        {
            var estados = EstadoRepo.Query().Where(e =>avisoId.Contains(e.Id)).ToList();
            foreach (var estado in estados)
            {
                if (estado.EstadoId == 1)
                {
                    estado.EstadoId = 2; 
                  
                    EstadoRepo.Update(estado);
                }

            }
        }

        public void MarcarAvisoComoLeido(int avisoId)
        {
            var estado = EstadoRepo.Query().Where(e => e.Id == avisoId).FirstOrDefault();
            if (estado != null && estado.EstadoId == 1 || estado.EstadoId == 2)
            {
                estado.EstadoId = 3; 
                estado.FechaLeido = DateTime.Now; 
                EstadoRepo.Update(estado);
            }

        }




        public void CrearAvisoGeneral(CrearAvisoGeneralDto dto)
        {

            var aviso = Mapper.Map<Aviso>(dto);
            AvisoRepo.Insert(aviso);


            var avisoGeneral = Mapper.Map<Avisogeneral>(dto);
            avisoGeneral.AvisoId = aviso.Id;

            AvisoGeneralRepo.Insert(avisoGeneral);


        }

        public void CrearAvisoPersonal(CrearAvisoPersonalDto dto)
        {

            var aviso = Mapper.Map<Aviso>(dto);
            AvisoRepo.Insert(aviso);


            var avisoPersonal = Mapper.Map<Avisopersonal>(dto);
            avisoPersonal.AvisoId = aviso.Id;

            AvisoPersonalRepo.Insert(avisoPersonal);


            var estado = new Avisoalumnoestado
            {
                AvisoId = aviso.Id,
                AlumnoId = dto.AlumnoId,
                EstadoId = 1
            };

            EstadoRepo.Insert(estado);
        }

        public AvisoGeneralListaAlumnoDTO ObtenerAvisoGeneralAlumno(int avisoId)
        {
            var aviso = EstadoRepo.Query()
                .Include(x => x.Aviso)
                    .ThenInclude(x => x.Avisogeneral)
                .Include(x => x.Estado)
                .FirstOrDefault(x => x.Id == avisoId);

            return Mapper.Map<AvisoGeneralListaAlumnoDTO>(aviso);
        }
        public List<AvisoGeneralListaAlumnoDTO> ObtenerAvisosGeneralesVigentesAlumno(int alumnoId)
        {
            var ahora = DateTime.Now;

        
            var ultimoAvisoAlumno = EstadoRepo.Query()
                .Where(e => e.AlumnoId == alumnoId && e.Aviso.TipoAvisoId == 1)
                .OrderByDescending(e => e.Aviso.FechaCreacion)
                .Select(e => e.Aviso.FechaCreacion)
                .FirstOrDefault();

            
            var avisosQuery = AvisoRepo.Query()
                .Where(a => a.TipoAvisoId == 1 && a.Avisogeneral.FechaCaducidad >= ahora);

            List<Aviso> avisosNuevos;

          
            if (ultimoAvisoAlumno == null)
            {
                
                avisosNuevos = avisosQuery.ToList();
            }
            else
            {
                
                avisosNuevos = avisosQuery
                    .Where(a => a.FechaCreacion > ultimoAvisoAlumno)
                    .ToList();
            }

            
            if (avisosNuevos.Any())
            {
                var nuevos = avisosNuevos.Select(a => new Avisoalumnoestado
                {
                    AvisoId = a.Id,
                    AlumnoId = alumnoId,
                    EstadoId = 1, 
                    FechaLeido = null
                }).ToList();

                EstadoRepo.InsertRange(nuevos);
            }

            
            var resultado = EstadoRepo.Query().Include(x=>x.Estado).Include(x=>x.Aviso).ThenInclude(x=>x.Avisogeneral)
                .Where(e => e.AlumnoId == alumnoId && e.Aviso.TipoAvisoId == 1)
                .OrderBy(x=>x.EstadoId).ThenBy(x=>x.Aviso.FechaCreacion)
                .ToList();

            
            return Mapper.Map<List<AvisoGeneralListaAlumnoDTO>>(resultado);
        }


        public void EliminarAvisoPersonal(int id)
        {
            var avisoPersonal = EstadoRepo.Query().Include(x=>x.Aviso).ThenInclude(x=>x.Avisopersonal)
                .Include(x => x.Aviso)
                .FirstOrDefault(x => x.Id == id);

            if (avisoPersonal != null)
            {
                avisoPersonal.Aviso.Avisopersonal.Eliminado = 1;
                EstadoRepo.Update(avisoPersonal);

               
            }
        }

    }
}
