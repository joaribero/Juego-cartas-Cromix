
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoCartas.Entidades
{
    /// <summary>
    /// Representa atributos de la carta
    /// </summary>
    public class Atributo
    {
        public double Valor { get; set; }

        public string Nombre { get; set; }       

        public Atributo (string nombre, double valor)
        {
            this.Valor = valor;
            this.Nombre = nombre;
        }

    }

}
