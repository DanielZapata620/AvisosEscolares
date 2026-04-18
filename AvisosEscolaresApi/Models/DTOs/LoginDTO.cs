namespace AvisosEscolaresApi.Models.DTOs
{
    public class LoginDTO
    {
      
            public string Usuario{ get; set; }
            public string Contrasena { get; set; }
        
    }

    public class LoginResponseDTO
    {
        public string Rol { get; set; } = null!;
        public AlumnoDTO? Alumno { get; set; }
        public MaestroDTO? Maestro { get; set; }
    }
}
