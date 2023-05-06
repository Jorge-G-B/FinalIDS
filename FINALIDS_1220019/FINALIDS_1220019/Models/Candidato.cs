using System;
using System.Collections.Generic;

namespace FINALIDS_1220019.Models;

public partial class Candidato
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Partido { get; set; } = null!;

    public string Dpi { get; set; } = null!;
}
