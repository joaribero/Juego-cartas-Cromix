using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JuegoCartas.Entidades;

namespace UnitTestProject1
{
    [TestClass]
    public class CartaTest
    {
        [TestMethod]
        public void CrearUnaCarta()
        {
            var carta = new Carta("T1", "MAUS", TipoCarta.Normal);
            Assert.IsNotNull(carta.Nombre);
            Assert.IsNotNull(carta.Codigo);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CrearUnaCartaSinCodigoyNombre()
        {
            var carta = new Carta(null, null, TipoCarta.Normal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CrearUnaCartaVacia()
        {
            var carta = new Carta("", "", TipoCarta.Normal);
        }

        [TestMethod]
        public void AgregarAtributosCarta()
        {
            var carta = new Carta("T1", "MAUS", TipoCarta.Normal);
            var atributo1 = new Atributo("PESO", 0);
            var atributo2 = new Atributo("VELOCIDAD", 0);

            carta.ListaAtributos.Add(atributo1);
            carta.ListaAtributos.Add(atributo2);

            carta.ListaAtributos[0].Valor = 100;
            carta.ListaAtributos[1].Valor = 20;

            Assert.AreEqual(2, carta.ListaAtributos.Count);
            Assert.AreEqual(100, carta.ListaAtributos[0].Valor);
        }
    }
}
