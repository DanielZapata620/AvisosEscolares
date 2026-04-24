using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;

namespace AvisosEscolaresApi.Services
{
    public class MaestrosService
    {

        public MaestrosService(Repository<Maestro> repoMaestro, IMapper mapper)
        {
            RepoMaestro = repoMaestro;
            Mapper = mapper;
        }

        public Repository<Maestro> RepoMaestro { get; }
        public IMapper Mapper { get; }

        public MaestroDTO ObtenerMaestroById(int id)
        {
            var Maestro = RepoMaestro.Get(id);

            return Mapper.Map<MaestroDTO>(Maestro);
        }
    }
}
