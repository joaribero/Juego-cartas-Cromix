using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoCartas.Entidades
{
    /// <summary>
    /// Representa juego entre 2 jugadores, manejo de turnos.
    /// </summary>
    public class Partida
    {
        public List<Jugador> Jugadores { get; set; }

        public string Nombre { get; set; }

        public Mazo MazoJuego { get; set; }

        public string Mazo { get; set; }

        public Jugador Creador { get; set; }

        public Jugador Turno { get; set; }

        public string Usuario { get { return this.Creador.Nombre; } }

        public Partida(string nombre, Jugador usuario, string nombreMazo)
        {
            this.Nombre = nombre;
            this.Creador = usuario;
            this.Mazo = nombreMazo;
            this.Jugadores = new List<Jugador>();
            this.Turno = usuario;
        }

        public void Repartir()
        {
            this.MazoJuego.Mezclar();

            var contador = 0;
            while (contador < this.MazoJuego.ListaCartas.Count)
            {
                if (contador % 2 == 0)
                {
                    Jugadores[0].Cartas.Add(this.MazoJuego.ListaCartas[contador]);
                }
                else
                {
                    Jugadores[1].Cartas.Add(this.MazoJuego.ListaCartas[contador]);
                }

                contador += 1;
            }

        }

    }
}
