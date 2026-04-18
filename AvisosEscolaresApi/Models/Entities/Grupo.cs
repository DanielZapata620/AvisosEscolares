using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Grupo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int MaestroId { get; set; }

    public virtual ICollection<Alumno> Alumno { get; set; } = new List<Alumno>();

    public virtual Maestro Maestro { get; set; } = null!;
}
