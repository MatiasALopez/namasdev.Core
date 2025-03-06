using System;
using System.Collections.Generic;
using System.IO;

namespace namasdev.Core.IO
{
	public class ArchivoContentTypes
	{
		public class Image
		{
			public const string GIF = "image/gif";
			public const string JPG = "image/jpeg";
			public const string JPE = "image/jpeg";
			public const string JPEG = "image/jpeg";
			public const string PNG = "image/png";
			public const string TIF = "image/tiff";
			public const string TIFF = "image/tiff";
		}

		public class Video
		{
			public const string MPG = "video/mpeg";
			public const string MPE = "video/mpeg";
			public const string MPEG = "video/mpeg";
		}

		public class Text
		{
			public const string CSV = "text/plain";
			public const string HTML = "text/html";
			public const string TXT = "text/plain";
			public const string XML = "text/xml";
		}

		public class Application
		{
			public const string EXCEL = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string EXCEL_97_2003 = "application/vnd.ms-excel";
			public const string WORD = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            public const string WORD_97_2003 = "application/msword";
            public const string PDF = "application/pdf";
            public const string ZIP = "application/x-zip-compressed";
		}

		private static readonly Dictionary<string, string> _contentTypesPorExtensiones = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
		{
			{ ArchivoExtensiones.Image.GIF, "image/gif" },
			{ ArchivoExtensiones.Image.JPG, "image/jpeg" },
			{ ArchivoExtensiones.Image.JPE, "image/jpeg" },
			{ ArchivoExtensiones.Image.JPEG, "image/jpeg" },
			{ ArchivoExtensiones.Image.PNG, "image/png" },
			{ ArchivoExtensiones.Image.TIFF, "image/tiff" },
			{ ArchivoExtensiones.Image.TIF, "image/tiff" },

			{ ArchivoExtensiones.Video.MPG, "video/mpeg" },
			{ ArchivoExtensiones.Video.MPE, "video/mpeg" },
			{ ArchivoExtensiones.Video.MPEG, "video/mpeg" },

			{ ArchivoExtensiones.Text.CSV, "text/plain" },
			{ ArchivoExtensiones.Text.HTML, "text/html" },
			{ ArchivoExtensiones.Text.TXT, "text/plain" },
			{ ArchivoExtensiones.Text.XML, "text/xml" },

            { ArchivoExtensiones.Application.PDF, "application/pdf" },
			{ ArchivoExtensiones.Application.EXCEL, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ArchivoExtensiones.Application.EXCEL_97_2003, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
			{ ArchivoExtensiones.Application.WORD, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ArchivoExtensiones.Application.WORD_97_2003, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ArchivoExtensiones.Application.ZIP, "application/x-zip-compressed" },
		};

		public static string ObtenerContentType(string archivoNombre)
		{
			if (string.IsNullOrWhiteSpace(archivoNombre))
			{
				throw new ArgumentNullException(nameof(archivoNombre));
			}

			string contentType;
			return _contentTypesPorExtensiones.TryGetValue(Path.GetExtension(archivoNombre), out contentType)
				? contentType
				: "application/octet-stream";
		}
	}
}