using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Alumno
{
    public int Id { get; set; }

    public int GrupoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public sbyte Eliminado { get; set; }

    public virtual ICollection<Avisoalumnoestado> Avisoalumnoestado { get; set; } = new List<Avisoalumnoestado>();

    public virtual Grupo Grupo { get; set; } = null!;
}
