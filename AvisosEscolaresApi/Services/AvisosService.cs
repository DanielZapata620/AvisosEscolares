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

            var avisos = AvisoRepo.Query().Include(x => x.Avisogeneral).Include(a => a.Avisoalumnoestado).ThenInclude(e => e.Estado).Where(a => a.TipoAvisoId == 1 && a.Avisogeneral.FechaCaducidad>=DateTime.Now);
            foreach (var a in avisos)
            {
                Console.WriteLine(a.Avisoalumnoestado.Count);
            }
            return avisos.Select(a => Mapper.Map<AvisoGeneralDetallesMaestroDTO>(a)).ToList();


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

    }
}
