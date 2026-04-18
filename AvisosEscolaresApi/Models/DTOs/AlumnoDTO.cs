namespace AvisosEscolaresApi.Models.DTOs
{
    public class AlumnoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int GrupoId { get; set; }
        public string GrupoNombre { get; set; } = null!;
        public string NombreProfesor { get; set; } = null!; 
    }

    public class AlumnoDetallesListaDTO
    {
        public int Id { get; set; }

        public string Usuario { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        
        

    }
    public class AlumnoCreateDTO
    {
        public string Nombre { get; set; } = null!;

        public int GrupoId { get; set; }
        public string Usuario { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
    // public class AlumnoUpdateDTO
    //{
    //    public string Nombre { get; set; } = null!;
    //    public int GrupoId { get; set; }
    //    public string Usuario { get; set; } = null!;
    //    public string Contrasena { get; set; } = null!;
    //}
}
