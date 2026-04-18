using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Avisoalumnoestado
{
    public int Id { get; set; }

    public int AvisoId { get; set; }

    public int EstadoId { get; set; }

    public DateTime? FechaLeido { get; set; }

    public int AlumnoId { get; set; }

    public virtual Alumno Alumno { get; set; } = null!;

    public virtual Aviso Aviso { get; set; } = null!;

    public virtual Estado Estado { get; set; } = null!;
}
