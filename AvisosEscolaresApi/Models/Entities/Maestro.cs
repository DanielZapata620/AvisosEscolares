using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Maestro
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string ClaveAcceso { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Avisopersonal> Avisopersonal { get; set; } = new List<Avisopersonal>();

    public virtual ICollection<Grupo> Grupo { get; set; } = new List<Grupo>();
}
