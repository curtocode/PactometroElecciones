using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace PracticaFInal.Model
{
    public class Elecciones
    {
        private int total_diputados;
        private string fecha;
        private string tipo;
        private List<Partido> partidos;
        private int mayoriaAbsoluta;

        public Elecciones(int total_diputados, string fecha, string tipo, List<Partido> partidos)
        {
            this.total_diputados = total_diputados;
            this.fecha = fecha;
            this.tipo = tipo;
            this.partidos = partidos;
            this.mayoriaAbsoluta = total_diputados / 2 + 1;
        }

        public int EscañosTotales
        {
            get { return total_diputados; }
            set { total_diputados = value; }
        }

        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public List<Partido> Partidos
        {
            get { return partidos; }
            set { partidos = value; }
        }

        public int MayoriaAbsoluta
        {
            get { return mayoriaAbsoluta; }
            set { mayoriaAbsoluta = value; }
        }
    }
}
