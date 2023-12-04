using PracticaFInal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PracticaFInal
{
    /// <summary>
    /// Lógica de interacción para VentanaSecundaria.xaml
    /// </summary>
    public partial class VentanaSecundaria : Window
    {
        private string rutaArchivo = "C:\\Users\\Alejandro\\Desktop\\USAL\\tercerCurso\\igu\\PracticaFInal\\PracticaFInal\\datos_electorales.json";
        CargarDatos datos;
        List<Elecciones> elecciones;
        public delegate void EleccionSeleccionadaHandler(Elecciones eleccionSeleccionada);

        // Definir el evento basado en ese delegado
        public event EleccionSeleccionadaHandler EleccionSeleccionada;

        public VentanaSecundaria()
        {
            InitializeComponent();
            MostrarDatos();
        }
        private void MostrarDatos()
        {
            //controller = new ControllerNamespace.Controller(rutaArchivo);
            
            //List <Elecciones> elecciones = controller.cargarDatos.LeerDatosElecciones();
            datos = new CargarDatos(rutaArchivo);
            elecciones = datos.LeerDatosElecciones();
            dgElecciones.ItemsSource = elecciones;
        }
        private void dgDetalles_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && dgDetalles.SelectedItem is Partido partido)
            {
                DragDrop.DoDragDrop(dgDetalles, partido, DragDropEffects.Move);
            }
        }
        private void dgElecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgElecciones.SelectedItem is Elecciones eleccionSeleccionada)
            {
                EleccionSeleccionada?.Invoke(eleccionSeleccionada);
                // Asumiendo que 'eleccionSeleccionada' tiene una propiedad 'Partidos' que es una lista de detalles del partido
                dgDetalles.ItemsSource = eleccionSeleccionada.Partidos;
                dgDetalles.MouseMove += dgDetalles_MouseMove;
            }
        }
    }
}
