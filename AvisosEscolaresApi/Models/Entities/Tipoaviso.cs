using System;
using System.Collections.Generic;

namespace AvisosEscolaresApi.Models.Entities;

public partial class Tipoaviso
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Aviso> Aviso { get; set; } = new List<Aviso>();
}
