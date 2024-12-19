using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using namasdev.Core.Validation;

namespace namasdev.Core.UnitTests
{
    public partial class ValidadorTests
    {
        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void ValidarRequeridoYThrow_ObjetoNull_Throw()
        {
            object valor = null;

            Validador.ValidarRequeridoYThrow<ArgumentNullException>(valor, NOMBRE);
        }

        [TestMethod]
        public void ValidarRequeridoYThrow_ObjetoNoNull_NoThrow()
        {
            object v1 = new { };
            object v2 = 1;
            object v3 = new DateTime();

            Validador.ValidarRequeridoYThrow<ArgumentNullException>(v1, NOMBRE);
            Validador.ValidarRequeridoYThrow<ArgumentNullException>(v2, NOMBRE);
            Validador.ValidarRequeridoYThrow<ArgumentNullException>(v3, NOMBRE);
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void ValidarRequeridoYThrow_StringNull_Throw()
        {
            string valor = null;

            Validador.ValidarRequeridoYThrow<ArgumentNullException>(valor, NOMBRE);
        }

        [TestMethod,
        DataRow(""),
        DataRow(" "),
        ExpectedException(typeof(ArgumentNullException))]
        public void ValidarRequeridoYThrow_StringVacio_Throw(string valor)
        {
            Validador.ValidarRequeridoYThrow<ArgumentNullException>(valor, NOMBRE);
        }

        [TestMethod]
        public void ValidarRequeridoYThrow_StringNoNullNiVacio_NoThrow()
        {
            string valor = "a";

            Validador.ValidarRequeridoYThrow<ArgumentNullException>(valor, NOMBRE);
        }
    }
}
