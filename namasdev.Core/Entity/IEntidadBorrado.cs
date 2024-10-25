using System;

namespace namasdev.Core.Entity
{
    public interface IEntidadBorrado
    {
        string BorradoPor { get; set; }
        DateTime? BorradoFecha { get; set; }
        bool Borrado { get; set; }
    }
}
