using System;
using System.Collections.Generic;

namespace FINALIDS_1220019.Models;

public partial class Voto
{
    public int Id { get; set; }

    public string NombreVotante { get; set; } = null!;

    public string Dpi { get; set; } = null!;

    public DateTime? FechaVoto { get; set; }

    public string? Iporigen { get; set; }

    public int? Estado { get; set; }

    public string CandidatoVotado { get; set; } = null!;
}
