using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Estado
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Avisoalumnoestado> Avisoalumnoestado { get; set; } = new List<Avisoalumnoestado>();
}
