using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace JuegoCartas.Entidades
{
    /// <summary>
    /// Representa el Mazo.
    /// </summary>
    public class Mazo
    {

        /// <summary>
        /// Nombre del mazo.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Lista de atributos
        /// </summary>
        public IList<string> NombreAtributos { get; set; }

        /// <summary>
        /// Lista de cartas existentes
        /// </summary>
        public IList<Carta> ListaCartas { get; set; }

        public int CantidadCartas { get; set; }
        public int CantidadAtributos { get; set; }

        public Mazo(string nombre, IList<string> atributos)
        {
            this.NombreAtributos = atributos;
            this.ListaCartas = new List<Carta>();
            this.Nombre = nombre;
            this.CantidadCartas = 0;
            this.CantidadAtributos = 0;
        }

        public void Mezclar()
        {

            Random variable = new Random();

            int i = this.ListaCartas.Count;
            while (i > 1)
            {
                int j = variable.Next(i);
                i--;
                Carta auxiliar = this.ListaCartas[j];
                this.ListaCartas[j] = this.ListaCartas[i];
                this.ListaCartas[i] = auxiliar;
            }

        }

        public void Comodines()
        {
            var CartaAmarilla = new Carta("amarilla", "AMARILLA", TipoCarta.Amarillo);
            var CartaRoja = new Carta("roja", "ROJA", TipoCarta.Rojo);
            this.ListaCartas.Add(CartaAmarilla);
            this.ListaCartas.Add(CartaRoja);
        }
    }
}
