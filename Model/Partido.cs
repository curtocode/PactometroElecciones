using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaFInal.Model
{
    public class Partido
    {
        private int escaños;
        private string nombre;
        private string color;

        public Partido(int escaños, string nombre, string color)
        {
            this.escaños = escaños;
            this.nombre = nombre;
            this.color = color;
        }
        public int Escaños
        {
            get { return escaños; }
            set { escaños = value; }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public string Color
        {
            get { return color; }
            set { color = value; }
        }



    }
}
