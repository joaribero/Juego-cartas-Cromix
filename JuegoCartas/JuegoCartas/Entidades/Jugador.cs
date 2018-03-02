using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoCartas.Entidades
{
    /// <summary>
    /// Representa un jugador.
    /// </summary>
    public class Jugador
    {
        /// <summary>
        /// Nombre del jugador.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Codigo unico del usuario conectado.
        /// </summary>
        public string ConectionId { get; set; }

        public IList<Carta> Cartas { get; set; }

        public Jugador(string nombre) 
        {
            this.Nombre = nombre;
            this.Cartas = new List<Carta>();
        }

        public Jugador(string nombre, string Idconection)
        {
            this.Nombre = nombre;
            this.ConectionId = Idconection;
            this.Cartas = new List<Carta>();
        }

        public bool EsPerdedor()
        {
            return this.ContarCartas() == 0;
        }

        public int ContarCartas()
        {
            return this.Cartas.Count;
        }

        public Carta PerderCarta()
        {
            var cartaAfuera = this.Cartas[0];
            this.Cartas.RemoveAt(0);
            return cartaAfuera;
        }

        public void GanarCarta(Carta cartaAfuera)
        {
            this.Cartas.Add(this.PerderCarta());
            this.Cartas.Add(cartaAfuera);
        }

        
    }
}
