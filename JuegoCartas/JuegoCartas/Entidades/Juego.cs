using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using JuegoCartas.Entidades;

namespace JuegoCartas.Entidades
{ 
    /// <summary>
    /// Representa el juego
    /// </summary>
    public class Juego
    {

        public IList<Mazo> Mazos { get; set; }

        public IList<Partida> Partidas { get; set; }

        public Juego()
        {
            this.Partidas = new List<Partida>();
            this.Mazos = new List<Mazo>();

        }

        public void CrearPartida(string nombre, Jugador creador, string mazo)
        {
            var bandera = 0; var contador = 0;

            var partida = new Partida(nombre, creador, mazo);

            //Verifico que no haya otra partida con el mismo nombre
            while (bandera == 0 && contador < Partidas.Count)
            {
                if (Partidas[contador].Creador.Nombre == partida.Nombre)
                {
                    bandera = 1;
                }
            }

            //Si la partida no esta la agrego a la lista de partidas
            if (bandera == 0)
            {
                Partidas.Add(partida);
            }


        }

        public List<string> ObtenerMazos()
        {
            var carpetas = new DirectoryInfo((HostingEnvironment.MapPath("/Mazos")));

            var mazos = new List<string>();

            foreach (var directorioMazo in carpetas.GetDirectories())
            {
                mazos.Add(directorioMazo.Name);
            }

            return mazos;
        }

        public List<Partida> ObtenerPartidasEnEspera()
        {
            var partidas = new List<Partida>();

            var contador = 0;

            while (contador < this.Partidas.Count)
            {
                if (this.Partidas[contador].Jugadores.Count == 1)
                {
                    partidas.Add(this.Partidas[contador]);
                }
                contador += 1;

            }

            return partidas;

        }

        public void InicializarMazos()
        {
            var carpetas = new DirectoryInfo((HostingEnvironment.MapPath("/Mazos")));

            foreach (var directorioMazo in carpetas.GetDirectories())
            {

                var files = directorioMazo.GetFiles("informacion.txt");

                foreach (var file in files)
                {
                    this.LeerFilas(file);
                }
            }

        }

        public Mazo buscarMazo(string nombreMazo)
        {
            var contador = 0;
            var bandera = 0;
            while (bandera == 0 && contador < this.Mazos.Count)
            {
                if (this.Mazos[contador].Nombre == nombreMazo)
                {
                    bandera = 1;
                }
                else
                {
                    contador += 1;
                }
            }

            return this.Mazos[contador];
        }

        public Partida BuscarPartida(string id)
        {
            var contador1 = 0;
            while (contador1 < this.Partidas.Count)
            {
                if (this.Partidas[contador1].Jugadores.Count == 2)
                {
                    var contador2 = 0;
                    while (contador2 < this.Partidas[contador1].Jugadores.Count)
                    {
                        if (id == this.Partidas[contador1].Jugadores[contador2].ConectionId)
                        {
                            return this.Partidas[contador1];
                        }
                        else
                            contador2 += 1;
                    }
                }
                contador1 += 1;
            }
            return null;
        }

        private void LeerFilas(FileInfo file)
        {
            var lineas = File.ReadAllLines(file.FullName);

            var nombreatributos = new List<string>();
            var listaatributos = new List<Atributo>();

            var i = 0; var indice = 0;

            var nombres = lineas[1].Split('|');

            foreach (string atributo in nombres)
            {
                if (indice > 1)
                {
                    nombreatributos.Add(nombres[indice]);
                }

                indice += 1;
            }

            var mazo = new Mazo("", nombreatributos);

            foreach (var linea in lineas)
            {

                if (i == 0)
                {
                    mazo.Nombre = lineas[0];
                    i += 1;
                }
                else
                {
                    if (i == 1)
                    {
                        string[] atributos = linea.Split('|');

                        var indice2 = 2;

                        while (indice2 < atributos.Length)
                        {
                            var nuevoatributo = new Atributo(atributos[indice2], 0.00);
                            nuevoatributo.Nombre = atributos[indice2];
                            listaatributos.Add(nuevoatributo);
                            indice2 += 1;
                        }

                        mazo.CantidadCartas = lineas.Length;

                        i += 1;
                    }
                    else
                    {
                        var listaAtribut = new List<Atributo>();
                  
                        var indice5 = 0;
                        while (indice5 < listaatributos.Count)
                        {
                            var nuevoAtributo = new Atributo(listaatributos[indice5].Nombre, 0.00);
                            listaAtribut.Add(nuevoAtributo);                           
                            indice5 += 1;
                        }

                        var arreglo = linea.Split('|');
                        var n = 0;
                        var carta = new Carta(arreglo[n], arreglo[n + 1], TipoCarta.Normal);

                        var indice4 = 0;
                        var indice3 = 2;
                        while (indice4 < listaAtribut.Count)
                        {
                            listaAtribut[indice4].Valor = Convert.ToDouble(arreglo[indice3]);
                            indice4 += 1;
                            indice3 += 1;
                        }

                        carta.ListaAtributos = listaAtribut;
                        mazo.ListaCartas.Add(carta);

                        i += 1;

                    }
                }


            }

            mazo.Comodines();

            mazo.CantidadCartas = mazo.ListaCartas.Count;

            mazo.CantidadAtributos = listaatributos.Count;

            this.Mazos.Add(mazo);
        }
    }
}
