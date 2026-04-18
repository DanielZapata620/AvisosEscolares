using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;

namespace AvisosEscolaresApi.Mappers
{
    public class MaestroProfile: Profile
    {
        public MaestroProfile()
        {
            CreateMap<Maestro, MaestroDTO>()
    .ForMember(dest => dest.GrupoId,
               opt => opt.MapFrom(src => src.Grupo.FirstOrDefault().Id))
    .ForMember(dest => dest.GrupoNombre,
               opt => opt.MapFrom(src => src.Grupo.FirstOrDefault().Nombre));

        }
        
    }
}
