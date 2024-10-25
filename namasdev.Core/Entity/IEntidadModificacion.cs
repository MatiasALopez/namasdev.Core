using System;

namespace namasdev.Core.Entity
{
    public interface IEntidadModificacion
    {
        string UltimaModificacionPor { get; set; }
        DateTime UltimaModificacionFecha { get; set; }
    }
}
