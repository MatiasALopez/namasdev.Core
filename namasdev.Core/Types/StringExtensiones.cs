﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace namasdev.Core.Types
{
    public static class StringExtensiones
    {
        private const string REGEX_CARACTERES_NO_ALFANUMERICOS_PATTERN = "[^a-zA-Z0-9 ]";

        public static string Capitalize(this string valor,
            CultureInfo cultura = null)
        {
            return (cultura ?? CultureInfo.CurrentCulture).TextInfo.ToTitleCase(valor);
        }

        public static string RemoveAccentsAndSpecialCharacters(this string valor)
        {
            if (String.IsNullOrWhiteSpace(valor))
            {
                return String.Empty;
            }

            return new String
                (
                    valor.Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray()
                )
                .Normalize(NormalizationForm.FormC);
        }

        public static string RemoveNonAlphanumeric(this string valor)
        {
            if (String.IsNullOrWhiteSpace(valor))
            {
                return String.Empty;
            }

            return Regex.Replace(valor, REGEX_CARACTERES_NO_ALFANUMERICOS_PATTERN, String.Empty);
        }

        public static string Truncate(this string valor, ushort tamaño,
            string sufijo = null)
        {
            if (String.IsNullOrWhiteSpace(valor))
            {
                return String.Empty;
            }

            return tamaño >= valor.Length
                ? valor
                : valor.Substring(0, tamaño) + (sufijo ?? String.Empty);
        }

        public static string ValueNotEmptyOrNull(this string valor,
            string valorNull = null)
        {
            return String.IsNullOrWhiteSpace(valor)
                ? (valorNull ?? null)
                : valor;
        }

        public static string TrimEnd(this string valor, string trimValor)
        {
            return valor.EndsWith(trimValor) 
                ? valor.Substring(0, valor.Length - trimValor.Length) 
                : valor;
        }

        public static string ToFirstLetterLowercase(this string valor)
        {
            return
                !String.IsNullOrWhiteSpace(valor)
                ? valor.Substring(0, 1).ToLower() + valor.Substring(1)
                : valor;
        }
    }
}
