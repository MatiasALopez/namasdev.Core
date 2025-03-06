using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using namasdev.Core.Processing;

namespace namasdev.Core.IO
{
    public class ArchivoTexto
    {
        public static void ProcesarLineasEnBatchUsandoReader(
           byte[] archivoContenido, int tamañoBatch, Action<BatchArgs<string>> procesamiento,
           int? nroLineaDesde = null, int? nroLineaHasta = null,
           Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;

            int nroLinea = 0;
            string linea;
            var batchLineas = new List<string>();
            int batchLineasCount = 0;
            int batchNumero = 1;
            BatchArgs<string> batchArgs;
            using (var reader = new StreamReader(new MemoryStream(archivoContenido), encoding))
            {
                while (!reader.EndOfStream)
                {
                    linea = reader.ReadLine();
                    nroLinea++;

                    if (nroLineaHasta.HasValue && nroLinea > nroLineaHasta.Value)
                    {
                        break;
                    }

                    if (nroLineaDesde.HasValue && nroLinea < nroLineaDesde)
                    {
                        continue;
                    }

                    batchLineas.Add(linea);
                    batchLineasCount++;

                    if (batchLineasCount == tamañoBatch)
                    {
                        batchArgs = new BatchArgs<string> { Numero = batchNumero, Items = batchLineas };
                        procesamiento(batchArgs);

                        batchLineas.Clear();
                        batchLineasCount = 0;

                        if (batchArgs.CancelarProcesamiento)
                        {
                            break;
                        }

                        batchNumero++;
                    }
                }

                if (batchLineasCount > 0)
                {
                    batchArgs = new BatchArgs<string> { Numero = batchNumero, Items = batchLineas };
                    procesamiento(batchArgs);
                }
            }
        }

        public static IEnumerable<string> LeerLineasUsandoReader(
            byte[] archivoContenido,
            int? nroLineaDesde = null, int? nroLineaHasta = null,
            Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;

            int nroLinea = 1;
            string linea;
            using (var reader = new StreamReader(new MemoryStream(archivoContenido), encoding))
            {
                while (!reader.EndOfStream)
                {
                    linea = reader.ReadLine();

                    if (nroLineaDesde.HasValue && nroLinea < nroLineaDesde)
                    {
                        nroLinea++;
                        continue;
                    }

                    if (nroLineaHasta.HasValue && nroLinea > nroLineaHasta.Value)
                    {
                        yield break;
                    }

                    yield return linea;

                    nroLinea++;
                }
            }

            yield break;
        }

        public static IEnumerable<string> LeerLineas(
            byte[] archivoContenido,
            Encoding encoding = null,
            string saltoDeLinea = null)
        {
            encoding = encoding ?? Encoding.Default;
            saltoDeLinea = saltoDeLinea ?? Environment.NewLine;

            return encoding.GetString(archivoContenido)
                .Split(new[] { saltoDeLinea }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }

        public static IEnumerable<T> LeerRegistros<T>(
            byte[] archivoContenido, Func<string[], T> mapeoElemento,
            char separadorCampos = ',', bool contieneLineaEncabezados = false,
            Encoding encoding = null, string saltoDeLinea = null)
            where T : class
        {
            return LeerLineas(archivoContenido, encoding, saltoDeLinea)
                .Skip(contieneLineaEncabezados ? 1 : 0)
                .Select(linea => mapeoElemento(linea.Split(separadorCampos)))
                .ToArray();
        }

        public static Archivo GenerarDesdeElementos<T>(string nombre, IEnumerable<T> elementos, Func<T, string> mapeoElemento,
            string lineaEncabezados = null,
            Encoding encoding = null, string saltoDeLinea = null)
        {
            if (mapeoElemento == null)
            {
                mapeoElemento = (e) => Convert.ToString(e);
            }

            encoding = encoding ?? Encoding.Default;
            saltoDeLinea = saltoDeLinea ?? Environment.NewLine;

            var lineas = elementos.Select(mapeoElemento).ToList();

            if (!String.IsNullOrWhiteSpace(lineaEncabezados))
            {
                lineas.Insert(0, lineaEncabezados);
            }

            return new Archivo { Nombre = nombre, Contenido = encoding.GetBytes(String.Join(saltoDeLinea, lineas)) };
        }

        public static void GuardarDesdeElementosUsandoWriter<T>(string path, IEnumerable<T> elementos, Func<T, string> mapeoElemento,
            string lineaEncabezados = null,
            Encoding encoding = null, string saltoDeLinea = null)
        {
            if (mapeoElemento == null)
            {
                mapeoElemento = (e) => Convert.ToString(e);
            }

            encoding = encoding ?? Encoding.Default;
            saltoDeLinea = saltoDeLinea ?? Environment.NewLine;

            int lineasSinFlush = 0,
                cantLineasMaxPorFlush = 1000;

            using (var writer = new StreamWriter(path, append: false, encoding: encoding))
            {
                if (!String.IsNullOrWhiteSpace(lineaEncabezados))
                {
                    writer.Write(lineaEncabezados + saltoDeLinea);
                }

                foreach (var e in elementos)
                {
                    writer.Write(mapeoElemento(e) + saltoDeLinea);

                    lineasSinFlush++;

                    if (lineasSinFlush >= cantLineasMaxPorFlush)
                    {
                        writer.Flush();
                        lineasSinFlush = 0;
                    }
                }

                if (lineasSinFlush > 0)
                {
                    writer.Flush();
                }

                writer.Close();
            }
        }
    }
}
