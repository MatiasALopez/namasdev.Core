using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using namasdev.Core.Validation;

namespace namasdev.Core.UnitTests
{
    [TestClass]
    public partial class ValidadorTests
    {
        [TestMethod,
        ExpectedException(typeof(ApplicationException))]
        public void ValidarListaRequeridaYThrow_ListaNullYParametrosDefault_Throw()
        {
            IEnumerable<string> lista = null;

            Validador.ValidarListaRequeridaYThrow<ApplicationException,string>(lista, "lista");
        }

        [TestMethod,
        ExpectedException(typeof(ApplicationException))]
        public void ValidarListaRequeridaYThrow_ListaVaciaYParametrosDefault_Throw()
        {
            IEnumerable<string> lista = new string[0];

            Validador.ValidarListaRequeridaYThrow<ApplicationException, string>(lista, "lista");
        }

        [TestMethod]
        public void ValidarListaRequeridaYThrow_ListaVaciaYValidarNoVacia_NoThrow()
        {
            IEnumerable<string> lista = new string[0];

            Validador.ValidarListaRequeridaYThrow<ApplicationException, string>(lista, "lista",
                validarNoVacia: false);
        }

        [TestMethod]
        public void ValidarListaRequeridaYThrow_ListaConItemsNull_NoThrow()
        {
            IEnumerable<string> lista = new string[] { null, null };

            Validador.ValidarListaRequeridaYThrow<ApplicationException, string>(lista, "lista",
                validarNoVacia: false);
        }

        [TestMethod,
        ExpectedException(typeof(ApplicationException))]
        public void ValidarListaRequeridaYThrow_ListaConItemsNullYVaciosYValidacionItem_Throw()
        {
            IEnumerable<string> lista = new string[] { null, "", "  " };

            Validador.ValidarListaRequeridaYThrow<ApplicationException, string>(lista, "lista",
                validacionItem: i => !string.IsNullOrWhiteSpace(i));
        }
    }
}
