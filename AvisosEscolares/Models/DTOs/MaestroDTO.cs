namespace AvisosEscolares.Models.DTOs
{
    public class MaestroDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } =null!;
        public int? GrupoId { get; set; }
        public string? GrupoNombre { get; set; } = null!;
    }
}
