using System;

namespace namasdev.Core.Entity
{
    public interface IEntidad<TId>
        where TId : IEquatable<TId>
    {
        TId Id { get; set; }
    }
}
