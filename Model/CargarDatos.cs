using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PracticaFInal.Model
{
    public class CargarDatos
    {
        private string rutaArchivo;

        public CargarDatos(string rutaArchivo)
        {
            this.rutaArchivo = rutaArchivo;
        }

        public List<Elecciones> LeerDatosElecciones()
        {
            try
            {
                string json = File.ReadAllText(rutaArchivo);
                var datosElecciones = JsonConvert.DeserializeObject<List<EleccionesDTO>>(json);
                return ConvertirADominio(datosElecciones);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (archivo no encontrado, JSON inválido, etc.)
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
                return new List<Elecciones>();
            }
        }

        private List<Elecciones> ConvertirADominio(List<EleccionesDTO> datosEleccionesDTO)
        {
            List<Elecciones> elecciones = new List<Elecciones>();
            foreach (var eleccionDTO in datosEleccionesDTO)
            {
                List<Partido> partidos = new List<Partido>();
                foreach (var partidoDTO in eleccionDTO.Partidos)
                {
                    partidos.Add(new Partido(partidoDTO.Escaños, partidoDTO.Nombre, partidoDTO.Color));
                }
                elecciones.Add(new Elecciones(eleccionDTO.EscañosTotales, eleccionDTO.Fecha, eleccionDTO.Tipo, partidos));
            }
            return elecciones;
        }

    }

    public class EleccionesDTO
    {
        public int EscañosTotales { get; set; }
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public List<PartidoDTO>? Partidos { get; set; }
    }

    public class PartidoDTO
    {
        public int Escaños { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
    }
}
