using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace namasdev.Core.Types
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static IQueryable<T> Ordenar<T>(this IQueryable<T> query, string orden)
        {
            return
                String.IsNullOrWhiteSpace(orden)
                ? query
                : query.OrderBy(orden);
        }

        public static IQueryable<T> OrdenarYPaginar<T>(this IQueryable<T> query, OrdenYPaginacionParametros ordenYPaginacion,
            string ordenDefault = null)
        {
            ordenDefault = (ordenDefault ?? ordenYPaginacion?.OrdenDefault)
                .ValueNotEmptyOrNull(valorNull: "1");

            if (ordenYPaginacion == null)
            {
                return !String.IsNullOrWhiteSpace(ordenDefault)
                    ? query.OrderBy(ordenDefault)
                    : query;
            }

            string orden =
                !String.IsNullOrWhiteSpace(ordenYPaginacion.Orden)
                ? ordenYPaginacion.Orden
                : ordenDefault;

            ordenYPaginacion.CantRegistrosTotales = query.Count();

            return query
                .Ordenar(orden)
                .Skip((Math.Max(ordenYPaginacion.Pagina, 1) - 1) * ordenYPaginacion.CantRegistrosPorPagina)
                .Take(ordenYPaginacion.CantRegistrosPorPagina);
        }

        public static IQueryable<T> Paginar<T>(this IQueryable<T> query, int pagina, int registrosPorPagina)
        {
            return query
                .Skip((Math.Max(pagina, 1) - 1) * registrosPorPagina)
                .Take(registrosPorPagina);
        }
    }
}
