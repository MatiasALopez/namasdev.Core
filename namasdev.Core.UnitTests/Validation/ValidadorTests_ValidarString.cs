using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using namasdev.Core.Validation;

namespace namasdev.Core.UnitTests
{
    public partial class ValidadorTests
    {
        private static string NOMBRE = "Nombre";

        [TestMethod]
        public void ValidarString_ValorNull_DevuelveFalse()
        {
            string valor = null;
            string mensajeError;

            bool res = Validador.ValidarString(valor, NOMBRE, requerido: true, 
                out mensajeError);

            Assert.IsFalse(res);
            Assert.AreEqual(Validador.MensajeRequerido(NOMBRE), mensajeError);
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void ValidarStringYAgregarAListaErrores_ErroresNull_Throw()
        {
            Validador.ValidarStringYAgregarAListaErrores(null, NOMBRE, requerido: true, null);
        }

        [TestMethod]
        public void ValidarStringYAgregarAListaErrores_ValorVacio_DevuelveFalse()
        {
            string valor = "";
            var errores = new List<string>(); 

            bool res = Validador.ValidarStringYAgregarAListaErrores(valor, NOMBRE, requerido: true,
                errores);

            Assert.IsFalse(res);
            Assert.AreEqual(1, errores.Count);
            Assert.AreEqual(Validador.MensajeRequerido(NOMBRE), errores[0]);
        }
    }
}
