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

            var avisos = AvisoRepo.Query().Include(x => x.Avisogeneral).Include(a => a.Avisoalumnoestado).ThenInclude(e => e.Estado).Where(a => a.TipoAvisoId == 1 && a.Avisogeneral.FechaCaducidad >= DateTime.Now);
            foreach (var a in avisos)
            {
                Console.WriteLine(a.Avisoalumnoestado.Count);
            }
            return avisos.Select(a => Mapper.Map<AvisoGeneralDetallesMaestroDTO>(a)).ToList();


        }



        public List<AvisoPersonalDetallesMaestroDTO> ObtenerAvisosPersonales(int id)
        {
            var avisos = EstadoRepo.Query().Include(x => x.Aviso).Include(x => x.Estado).Where(x => x.AlumnoId == id && x.Aviso.TipoAvisoId==2);
            return avisos.Select(a => Mapper.Map<AvisoPersonalDetallesMaestroDTO>(a)).ToList();
        }

        public List<AvisoPersonalListaAlumnoDTO> ObtenerAvisosPersonalesAlumno(int id)
        {
            var avisos = EstadoRepo.Query().Include(x => x.Aviso).ThenInclude(x => x.Avisopersonal).ThenInclude(x => x.Maestro).Include(x => x.Estado).Where(x => x.AlumnoId == id && x.Aviso.TipoAvisoId == 2);
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
                    estado.EstadoId = 2; // Cambia el estado a "Leído"
                    /*estado.FechaLeido = DateTime.Now; */// Registra la fecha de lectura
                    EstadoRepo.Update(estado);
                }

            }
        }

        public void MarcarAvisoComoLeido(int avisoId)
        {
            var estado = EstadoRepo.Query().Where(e => e.Id == avisoId).FirstOrDefault();
            if (estado != null && estado.EstadoId == 1 || estado.EstadoId == 2)
            {
                estado.EstadoId = 3; // Cambia el estado a "Leído"
                estado.FechaLeido = DateTime.Now; // Registra la fecha de lectura
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

            // 1. Obtener último aviso del alumno (solo generales)
            var ultimoAvisoAlumno = EstadoRepo.Query()
                .Where(e => e.AlumnoId == alumnoId && e.Aviso.TipoAvisoId == 1)
                .OrderByDescending(e => e.Aviso.FechaCreacion)
                .Select(e => e.Aviso.FechaCreacion)
                .FirstOrDefault();

            // 2. Query base de avisos generales vigentes
            var avisosQuery = AvisoRepo.Query()
                .Where(a => a.TipoAvisoId == 1 && a.Avisogeneral.FechaCaducidad >= ahora);

            List<Aviso> avisosNuevos;

            // 3. Decisión
            if (ultimoAvisoAlumno == null)
            {
                // No tiene avisos → traer todos
                avisosNuevos = avisosQuery.ToList();
            }
            else
            {
                // Solo los posteriores
                avisosNuevos = avisosQuery
                    .Where(a => a.FechaCreacion > ultimoAvisoAlumno)
                    .ToList();
            }

            // 4. Insertar si hay nuevos
            if (avisosNuevos.Any())
            {
                var nuevos = avisosNuevos.Select(a => new Avisoalumnoestado
                {
                    AvisoId = a.Id,
                    AlumnoId = alumnoId,
                    EstadoId = 1, // Nuevo
                    FechaLeido = null
                }).ToList();

                EstadoRepo.InsertRange(nuevos);
            }

            // 5. Obtener resultado final
            var resultado = EstadoRepo.Query().Include(x=>x.Estado).Include(x=>x.Aviso).ThenInclude(x=>x.Avisogeneral)
                .Where(e => e.AlumnoId == alumnoId && e.Aviso.TipoAvisoId == 1)
                .OrderByDescending(e => e.Aviso.FechaCreacion)
                .ToList();

            // 6. AutoMapper
            return Mapper.Map<List<AvisoGeneralListaAlumnoDTO>>(resultado);
        }
    }
}
