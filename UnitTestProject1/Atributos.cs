using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JuegoCartas.Entidades;

namespace UnitTestProject1
{
    [TestClass]
    public class Atributos
    {
        [TestMethod]
        public void CrearUnAtributoConNombreyValor()
        {
            var nuevoAtributo = new Atributo("PESO", 100);
            Assert.IsNotNull(nuevoAtributo.Nombre);
            Assert.IsNotNull(nuevoAtributo.Valor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CrearAtributoVacio()
        {
            var nuevoAtributo = new Atributo(null, 0);
        }
    }
}

