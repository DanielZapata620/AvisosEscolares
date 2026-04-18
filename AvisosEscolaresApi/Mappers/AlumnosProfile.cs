using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;

namespace AvisosEscolaresApi.Mappers
{
    public class AlumnosProfile:Profile
    {
        public AlumnosProfile()
        {
            CreateMap<Alumno,AlumnoDTO>()
                .ForMember(dest => dest.GrupoId, opt => opt.MapFrom(src => src.Grupo.Id))
                .ForMember(dest => dest.GrupoNombre, opt => opt.MapFrom(src => src.Grupo.Nombre))
                .ForMember(dest => dest.NombreProfesor, opt => opt.MapFrom(src => src.Grupo.Maestro.Nombre));

            CreateMap<AlumnoCreateDTO, Alumno>();

            CreateMap<Alumno, AlumnoDetallesListaDTO>();
        }
    }
}
