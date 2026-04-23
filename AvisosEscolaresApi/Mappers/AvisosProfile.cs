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

            CreateMap<Avisoalumnoestado, AvisoPersonalDetallesMaestroDTO>()
            .ForMember(d => d.AvisoId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Titulo, o => o.MapFrom(s => s.Aviso.Titulo))
            .ForMember(d => d.Mensaje, o => o.MapFrom(s => s.Aviso.Mensaje))
            .ForMember(d => d.FechaLeido, o => o.MapFrom(s => s.FechaLeido))

            // 🔥 ESTADO PERSONALIZADO
            .ForMember(d => d.Estado, o => o.MapFrom(s =>
                s.Estado.Nombre == "Nuevo" ? "Sin Ver" :
                s.Estado.Nombre == "Recibido" ? "Visto" :
                s.Estado.Nombre == "Leído" ? "Leído" :
                "Sin Ver"
            ))

            // 🔥 MAESTRO (aquí está lo que preguntaste)

            ;

            CreateMap<Avisoalumnoestado, AvisoPersonalListaAlumnoDTO>()
            .ForMember(d => d.AvisoId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Titulo, o => o.MapFrom(s => s.Aviso.Titulo))
            .ForMember(d => d.Mensaje, o => o.MapFrom(s => s.Aviso.Mensaje))
            .ForMember(d => d.FechaLeido, o => o.MapFrom(s => s.FechaLeido))

            // 🔥 ESTADO PERSONALIZADO
            .ForMember(d => d.Estado, o => o.MapFrom(s =>
                s.Estado.Nombre ))

            // 🔥 MAESTRO (aquí está lo que preguntaste)
            .ForMember(d => d.Maestro, o => o.MapFrom(s =>
                s.Aviso.Avisopersonal.Maestro.Nombre
            ));


            CreateMap<Avisoalumnoestado, AvisoPersonalAlumnoDTO>()
            .ForMember(d => d.AvisoId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Titulo, o => o.MapFrom(s => s.Aviso.Titulo))
            .ForMember(d => d.Mensaje, o => o.MapFrom(s => s.Aviso.Mensaje))
            .ForMember(d => d.FechaLeido, o => o.MapFrom(s => s.FechaLeido))

            // 🔥 ESTADO PERSONALIZADO
            .ForMember(d => d.Estado, o => o.MapFrom(s =>
                s.Estado.Nombre))

            // 🔥 MAESTRO (aquí está lo que preguntaste)
            .ForMember(d => d.Maestro, o => o.MapFrom(s =>
                s.Aviso.Avisopersonal.Maestro.Nombre
            ));

            CreateMap<Avisoalumnoestado, AvisoGeneralListaAlumnoDTO>()
            .ForMember(d => d.AvisoId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Titulo, o => o.MapFrom(s => s.Aviso.Titulo))
            .ForMember(d => d.Mensaje, o => o.MapFrom(s => s.Aviso.Mensaje))
            .ForMember(d => d.FechaLeido, o => o.MapFrom(s => s.FechaLeido))
            .ForMember(d => d.Estado, o => o.MapFrom(s =>
                s.Estado.Nombre))
            .ForMember(d => d.FechaCaducidad, o => o.MapFrom(s => s.Aviso.Avisogeneral.FechaCaducidad));

        }


    }
}
