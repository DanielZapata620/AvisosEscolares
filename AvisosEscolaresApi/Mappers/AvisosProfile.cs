using AutoMapper;
using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;

namespace AvisosEscolaresApi.Mappers
{
    public class AvisosProfile : Profile
    {
        public AvisosProfile()
        {
            CreateMap<CrearAvisoGeneralDto, Aviso>()
           .ForMember(dest => dest.TipoAvisoId, opt => opt.MapFrom(src => 1))
           .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CrearAvisoGeneralDto, Avisogeneral>()
            .ForMember(dest => dest.AvisoId, opt => opt.Ignore());

            CreateMap<CrearAvisoPersonalDto, Aviso>()
            .ForMember(dest => dest.TipoAvisoId, opt => opt.MapFrom(src => 2))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CrearAvisoPersonalDto, Avisopersonal>()
           .ForMember(dest => dest.AvisoId, opt => opt.Ignore());

            CreateMap<Aviso, AvisoGeneralDetallesMaestroDTO>()
            .ForMember(dest=>dest.FechaCaducidad,opt=>opt.MapFrom(src=>src.Avisogeneral.FechaCaducidad))
            .ForMember(dest => dest.CantLeidos, opt => opt.MapFrom(src =>src.Avisoalumnoestado.Count(e => e.EstadoId==3)));

            CreateMap<Aviso, AvisoPersonalDetallesMaestroDTO>()
            .ForMember(dest => dest.Estado,opt => opt.MapFrom(src => src.Avisoalumnoestado.Select(e=>e.EstadoId).First() == 1 ? "Sin ver" :
                                                                     src.Avisoalumnoestado.Select(e=>e.EstadoId).First() == 2 ? "Visto" :
                                                                      "Leído"));

        }


    }
}
