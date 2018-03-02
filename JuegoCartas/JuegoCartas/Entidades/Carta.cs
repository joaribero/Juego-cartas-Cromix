using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace JuegoCartas.Entidades
{
    /// <summary>
    /// Representa una carta del mazo.
    /// </summary>
    public class Carta
    {
        /// <summary>
        /// Codigo unico de la carta.
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Nombre de la carta.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Amarilla, Roja, Normal
        /// </summary>
        public TipoCarta Tipo { get; set; }

        /// <summary>
        /// Lista con atributos.
        /// </summary>
        public List<Atributo> ListaAtributos { get; set; }
  
        public Carta(string codigo, string nombre, TipoCarta tipo)
        {           
            this.Codigo = codigo;
            this.Nombre = nombre;
            this.Tipo = tipo;
            this.ListaAtributos = new List<Atributo>();
        }

        public double BuscarValorAtributo(string nombreatributo)
        {
            return this.ListaAtributos.Find(x => x.Nombre == (nombreatributo)).Valor;
        }
    }
}
