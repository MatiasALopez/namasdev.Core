using System;

namespace namasdev.Core.Entity
{
    public interface IEntidadCreado
    {
        string CreadoPor { get; set; }
        DateTime CreadoFecha { get; set; }
    }
}
