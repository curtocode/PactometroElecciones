using PracticaFInal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PracticaFInal.Controller
{
    public class Controller
    {
        private string rutaArchivo;
        public CargarDatos cargarDatos;
        public Controller(string rutaArchivo)
        {
            this.rutaArchivo = rutaArchivo;
            cargarDatos = new CargarDatos(rutaArchivo);
        }
        
    }
}
