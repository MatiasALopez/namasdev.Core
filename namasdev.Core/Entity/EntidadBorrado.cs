using System;

namespace namasdev.Core.Entity
{
    public abstract class EntidadBorrado<TId> : Entidad<TId>, IEntidadBorrado
        where TId : IEquatable<TId>
    {
        public string BorradoPor { get; set; }
        public DateTime? BorradoFecha { get; set; }
        public bool Borrado { get; set; }
    }
}
