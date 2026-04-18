using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Avisopersonal
{
    public int AvisoId { get; set; }

    public int MaestroId { get; set; }

    public virtual Aviso Aviso { get; set; } = null!;

    public virtual Maestro Maestro { get; set; } = null!;
}
