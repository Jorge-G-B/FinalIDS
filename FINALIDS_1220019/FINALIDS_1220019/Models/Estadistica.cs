using System;
using System.Collections.Generic;

namespace FINALIDS_1220019.Models;

public partial class Estadistica
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int CantVotos { get; set; }
}
