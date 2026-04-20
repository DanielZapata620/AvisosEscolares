namespace AvisosEscolares.Models.DTOs
{
    public class AvisosDTO
    {
    }

    public class CrearAvisoGeneralDto
    {
        public string Titulo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public DateTime FechaCaducidad { get; set; }
    }

    public class CrearAvisoPersonalDto
    {
        public string Titulo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public int MaestroId { get; set; }
        public int AlumnoId { get; set; }
    }

    public class AvisoGeneralDetallesMaestroDTO : CrearAvisoGeneralDto
    {
        public int Id { get; set; }

        public int CantLeidos { get; set; }

    }

    public class AvisoPersonalDetallesMaestroDTO
    {
        public int AvisoId { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }

        public string Estado { get; set; }
        public DateTime? FechaLeido { get; set; }


    }

    public class AvisoPersonalListaAlumnoDTO : AvisoPersonalDetallesMaestroDTO
    {
        public string Maestro { get; set; }

    }

    public class AvisoPersonalAlumnoDTO : AvisoPersonalListaAlumnoDTO
    {

    }

}
