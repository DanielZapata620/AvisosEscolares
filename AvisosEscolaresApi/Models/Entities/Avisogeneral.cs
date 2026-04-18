using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Avisogeneral
{
    public int AvisoId { get; set; }

    public DateTime FechaCaducidad { get; set; }

    public virtual Aviso Aviso { get; set; } = null!;
}
