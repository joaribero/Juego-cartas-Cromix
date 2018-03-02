namespace trabajo_practico.Hubs
{
    using Microsoft.AspNet.SignalR;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using JuegoCartas.Entidades;



    public class JuegoHub : Hub
    {
        
        private static Juego juego = new Juego();

        public void CrearPartida(string usuario, string partida, string mazo)
        {

            juego.InicializarMazos();

            var jugador = new Jugador(usuario);
            jugador.ConectionId = this.Context.ConnectionId;                     

            var NuevaPartida = new Partida(partida,jugador,mazo);
            Clients.Others.agregarPartida(NuevaPartida);

            NuevaPartida.MazoJuego = juego.buscarMazo(mazo);
            NuevaPartida.Jugadores.Add(jugador);

            juego.Partidas.Add(NuevaPartida);

            Clients.Caller.esperarJugador();
        }

        public void UnirsePartida(string usuario, string partida)
        {
            var jugador2 = new Jugador(usuario);
            jugador2.ConectionId = this.Context.ConnectionId;

            var partidasEnEspera = juego.Partidas;

            var contador = 0; var bandera = 0;
            while (bandera == 0 && contador < partidasEnEspera.Count)
            {
                if (partidasEnEspera[contador].Jugadores.Count == 1 && partidasEnEspera[contador].Nombre == partida)
                {
                    bandera = 1;
                }
                else
                {
                    contador += 1;
                }
            }

            //Si encuentro que la partida esta en espera y es la que ingreso el jugador lo agrego a la lista de jugadores de dicha partida.
            if (bandera == 1)
            {
                juego.Partidas[contador].Jugadores.Add(jugador2);
                //juego.Partidas[contador].MazoPartida.AgregarComodines();
                juego.Partidas[contador].Repartir();

                Clients.All.eliminarPartida(partida);
                Clients.Caller.esperarPartida();


                Clients.Client(juego.Partidas[contador].Jugadores[0].ConectionId).dibujarTablero(juego.Partidas[contador].Jugadores[0], juego.Partidas[contador].Jugadores[1], juego.Partidas[contador].MazoJuego);
                Clients.Client(juego.Partidas[contador].Jugadores[1].ConectionId).dibujarTablero(juego.Partidas[contador].Jugadores[0], juego.Partidas[contador].Jugadores[1], juego.Partidas[contador].MazoJuego);

            }
        }

        public void ObtenerPartidas()
        {
           
            Clients.Caller.agregarPartidas(juego.ObtenerPartidasEnEspera());
        }

        public void ObtenerMazos()
        {           
            Clients.Caller.agregarMazos(juego.ObtenerMazos());
        }

        public void Cantar(string idAtributo, string codigoCarta)
        {
            var partidaActual = juego.BuscarPartida(this.Context.ConnectionId);

            var jugadorQueCantaAtributo = partidaActual.Jugadores.Where(jugador => jugador.ConectionId == Context.ConnectionId).Single();

            var jugadorQueContesta = partidaActual.Jugadores.Where(jugador => jugador.ConectionId != Context.ConnectionId).Single();

            //Normal vs Normal
            if (jugadorQueCantaAtributo.Cartas[0].Tipo == TipoCarta.Normal && jugadorQueContesta.Cartas[0].Tipo == TipoCarta.Normal)
            {
                //Si gana el que canta
                if (jugadorQueCantaAtributo.Cartas[0].BuscarValorAtributo(idAtributo) >= jugadorQueContesta.Cartas[0].BuscarValorAtributo(idAtributo))
                {
                    jugadorQueCantaAtributo.GanarCarta(jugadorQueContesta.PerderCarta());

                    //Si el jugador que contesta pierde la partida.
                    if (jugadorQueContesta.EsPerdedor() == true)
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).perder();
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).ganar();
                    }
                    //Si el jugador que contesta pierde la mano.
                    else
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).perderMano(Resultado.Normal, false);
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).ganarMano(Resultado.Normal, false);
                    }
                }
                else//pierde el que canta
                {
                    jugadorQueContesta.GanarCarta(jugadorQueCantaAtributo.PerderCarta());
                    //Si el jugador que canta pierde la partida.
                    if (jugadorQueCantaAtributo.EsPerdedor() == true)
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).ganar();
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).perder();
                    }
                    //Si el jugador que canta pierde la mano.
                    else
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).ganarMano(Resultado.Normal, false);
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).perderMano(Resultado.Normal, false);
                    }
                }
            }
            //Normal vs Amarillo
            else if (jugadorQueCantaAtributo.Cartas[0].Tipo == TipoCarta.Normal && jugadorQueContesta.Cartas[0].Tipo == TipoCarta.Amarillo)
            {
                jugadorQueContesta.GanarCarta(jugadorQueCantaAtributo.PerderCarta());

                //Si el jugador que canta pierde la partida.
                if (jugadorQueCantaAtributo.EsPerdedor() == true)
                {
                    Clients.Client(jugadorQueContesta.ConectionId).ganar();
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).perder();
                }
                //Si el jugador que canta pierde la mano.
                else
                {
                    Clients.Client(jugadorQueContesta.ConectionId).ganarMano(Resultado.Amarilla, false);
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).perderMano(Resultado.Amarilla, false);
                }
                jugadorQueContesta.Cartas.RemoveAt(0);

            }
            //Amarilla vs Normal
            else if (jugadorQueCantaAtributo.Cartas[0].Tipo == TipoCarta.Amarillo && jugadorQueContesta.Cartas[0].Tipo == TipoCarta.Normal)
            {
                jugadorQueCantaAtributo.GanarCarta(jugadorQueContesta.PerderCarta());

                //Si el jugador que contesta pierde la partida.
                if (jugadorQueContesta.EsPerdedor() == true)
                {
                    Clients.Client(jugadorQueContesta.ConectionId).perder();
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).ganar();
                }
                //Si el jugador que contesta pierde la mano.
                else
                {
                    Clients.Client(jugadorQueContesta.ConectionId).perderMano(Resultado.Amarilla, false);
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).ganarMano(Resultado.Amarilla, false);
                }
                jugadorQueCantaAtributo.Cartas.RemoveAt(0);
            }
            //Normal vs Roja
            else if (jugadorQueCantaAtributo.Cartas[0].Tipo == TipoCarta.Normal && jugadorQueContesta.Cartas[0].Tipo == TipoCarta.Rojo)
            {
                jugadorQueContesta.GanarCarta(jugadorQueCantaAtributo.PerderCarta());

                //Si el jugador que canta pierde la partida.
                if (jugadorQueCantaAtributo.EsPerdedor() == true)
                {
                    Clients.Client(jugadorQueContesta.ConectionId).ganar();
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).perder();
                }
                //Si el jugador que canta pierde la mano.
                else
                {
                    jugadorQueContesta.GanarCarta(jugadorQueCantaAtributo.PerderCarta());
                    //Si el jugador que canta pierde la partida.
                    if (jugadorQueCantaAtributo.EsPerdedor() == true)
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).ganar();
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).perder();
                    }
                    else
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).ganarMano(Resultado.Roja, false);
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).perderMano(Resultado.Roja, false);
                    }
                }
                jugadorQueContesta.Cartas.RemoveAt(0);
            }
            //Rojo vs Normal       
            else if (jugadorQueCantaAtributo.Cartas[0].Tipo == TipoCarta.Rojo && jugadorQueContesta.Cartas[0].Tipo == TipoCarta.Normal)
            {
                jugadorQueCantaAtributo.GanarCarta(jugadorQueContesta.PerderCarta());

                //Si el jugador que contesta pierde la partida.
                if (jugadorQueContesta.EsPerdedor() == true)
                {
                    Clients.Client(jugadorQueContesta.ConectionId).perder();
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).ganar();
                }
                //Si el jugador que contesta pierde la mano.
                else
                {
                    jugadorQueCantaAtributo.GanarCarta(jugadorQueContesta.PerderCarta());
                    //Si el jugador que contesta pierde la partida.
                    if (jugadorQueContesta.EsPerdedor() == true)
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).perder();
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).ganar();
                    }
                    else
                    {
                        Clients.Client(jugadorQueContesta.ConectionId).perderMano(Resultado.Roja, false);
                        Clients.Client(jugadorQueCantaAtributo.ConectionId).ganarMano(Resultado.Roja, false);
                    }
                }
                jugadorQueCantaAtributo.Cartas.RemoveAt(0);
            }
            //Roja vs Amarillo
            else if (jugadorQueCantaAtributo.Cartas[0].Tipo == TipoCarta.Rojo && jugadorQueContesta.Cartas[0].Tipo == TipoCarta.Amarillo)
            {
                jugadorQueCantaAtributo.GanarCarta(jugadorQueContesta.PerderCarta());

                //Si el jugador que contesta pierde la partida.
                if (jugadorQueContesta.EsPerdedor() == true)
                {
                    Clients.Client(jugadorQueContesta.ConectionId).perder();
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).ganar();
                }
                //Si el jugador que contesta pierde la mano.
                else
                {
                    Clients.Client(jugadorQueContesta.ConectionId).perderMano(Resultado.AmarillaRoja, false);
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).ganarMano(Resultado.AmarillaRoja, false);
                }
                jugadorQueCantaAtributo.Cartas.Remove(jugadorQueCantaAtributo.Cartas[0]);
                jugadorQueContesta.Cartas.RemoveAt(0);
            }
            //Amarillo vs Rojo
            else
            {
                jugadorQueContesta.GanarCarta(jugadorQueCantaAtributo.PerderCarta());

                //Si el jugador que canta pierde la partida.
                if (jugadorQueCantaAtributo.EsPerdedor() == true)
                {
                    Clients.Client(jugadorQueContesta.ConectionId).ganar();
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).perder();
                }
                //Si el jugador que canta pierde la mano.
                else
                {
                    Clients.Client(jugadorQueContesta.ConectionId).ganarMano(Resultado.AmarillaRoja, false);
                    Clients.Client(jugadorQueCantaAtributo.ConectionId).perderMano(Resultado.AmarillaRoja, false);
                }
                jugadorQueContesta.Cartas.RemoveAt(0);
                jugadorQueCantaAtributo.Cartas.RemoveAt(0);
            }
        }
    }
}