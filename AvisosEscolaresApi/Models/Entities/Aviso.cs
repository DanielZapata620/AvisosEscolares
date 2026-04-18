using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Aviso
{
    public int Id { get; set; }

    public int TipoAvisoId { get; set; }

    public string Titulo { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Avisoalumnoestado> Avisoalumnoestado { get; set; } = new List<Avisoalumnoestado>();

    public virtual Avisogeneral? Avisogeneral { get; set; }

    public virtual Avisopersonal? Avisopersonal { get; set; }

    public virtual Tipoaviso TipoAviso { get; set; } = null!;
}
