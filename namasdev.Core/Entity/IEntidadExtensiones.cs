using System;
using System.Collections.Generic;

using namasdev.Core.Validation;

namespace namasdev.Core.Entity
{
    public static class IEntidadExtensiones
    {
        /// <summary>
        /// Establece los valores de auditoría en base al usuario y fecha/hora especificados.
        /// Esta operación solo establece los valores en el objeto, luego es necesario aplicar los cambios de manera persistente.
        /// </summary>
        public static void EstablecerDatosCreado(this IEnumerable<IEntidadCreado> entidades, string usuario, DateTime fechaHora)
        {
            Validador.ValidarArgumentRequeridoYThrow(entidades, "entidades");

            foreach (var entidad in entidades)
            {
                entidad.EstablecerDatosCreado(usuario, fechaHora);
            }
        }

        /// <summary>
        /// Establece los valores de auditoría en base al usuario y fecha/hora especificados.
        /// Esta operación solo establece los valores en el objeto, luego es necesario aplicar los cambios de manera persistente.
        /// </summary>
        public static void EstablecerDatosCreado(this IEntidadCreado entidad, string usuario, DateTime fechaHora)
        {
            Validador.ValidarArgumentRequeridoYThrow(entidad, "entidad");

            entidad.CreadoPor = usuario;
            entidad.CreadoFecha = fechaHora;

            var entidadModificacion = entidad as IEntidadModificacion;
            if (entidadModificacion != null)
            {
                entidadModificacion.UltimaModificacionPor = usuario;
                entidadModificacion.UltimaModificacionFecha = fechaHora;
            }
        }

        /// <summary>
        /// Establece los valores de auditoría en base al usuario y fecha/hora especificados.
        /// Esta operación solo establece los valores en el objeto, luego es necesario aplicar los cambios de manera persistente.
        /// </summary>
        public static void EstablecerDatosModificacion(this IEnumerable<IEntidadModificacion> entidades, string usuario, DateTime fechaHora)
        {
            Validador.ValidarArgumentRequeridoYThrow(entidades, "entidades");

            foreach (var entidad in entidades)
            {
                entidad.EstablecerDatosModificacion(usuario, fechaHora);
            }
        }

        /// <summary>
        /// Establece los valores de auditoría en base al usuario y fecha/hora especificados.
        /// Esta operación solo establece los valores en el objeto, luego es necesario aplicar los cambios de manera persistente.
        /// </summary>
        public static void EstablecerDatosModificacion(this IEntidadModificacion entidad, string usuario, DateTime fechaHora)
        {
            Validador.ValidarArgumentRequeridoYThrow(entidad, "entidad");

            entidad.UltimaModificacionPor = usuario;
            entidad.UltimaModificacionFecha = fechaHora;
        }

        /// <summary>
        /// Establece los valores de auditoría de borrado en base al usuario y fecha/hora especificados.
        /// Esta operación solo establece los valores en el objeto, luego es necesario aplicar los cambios de manera persistente.
        /// </summary>
        public static void EstablecerDatosBorrado(this IList<IEntidadBorrado> entidades, string usuario, DateTime fechaHora)
        {
            Validador.ValidarArgumentRequeridoYThrow(entidades, "entidades");

            foreach (var entidad in entidades)
            {
                entidad.EstablecerDatosBorrado(usuario, fechaHora);
            }
        }

        /// <summary>
        /// Establece los valores de auditoría de borrado en base al usuario y fecha/hora especificados.
        /// Esta operación solo establece los valores en el objeto, luego es necesario aplicar los cambios de manera persistente.
        /// </summary>
        public static void EstablecerDatosBorrado(this IEntidadBorrado entidad, string usuario, DateTime fechaHora)
        {
            Validador.ValidarArgumentRequeridoYThrow(entidad, "entidad");

            entidad.BorradoPor = usuario;
            entidad.BorradoFecha = fechaHora;
        }
    }
}
