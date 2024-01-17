using PracticaFInal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PracticaFInal
{
    /// <summary>
    /// Lógica de interacción para VentanaSecundaria.xaml
    /// </summary>
    public partial class VentanaSecundaria : Window
    {
        private ObservableCollection<Elecciones> elecciones;
        public class EleccionesEventArgs : EventArgs
        {
            public List<Elecciones> Elecciones { get; private set; }
            public Elecciones Eleccion { get; private set; }

            public EleccionesEventArgs(List<Elecciones> elecciones)
            {
                Elecciones = elecciones;
            }

            public EleccionesEventArgs(Elecciones eleccion)
            {
                Eleccion = eleccion;
            }
        }
        public event EventHandler<EleccionesEventArgs> EleccionSeleccionada;
        public event EventHandler<EleccionesEventArgs> EleccionesCargadas;
        public event EventHandler EleccionEliminada;

        public VentanaSecundaria(ObservableCollection<Elecciones> elecciones)
        {
            InitializeComponent();
            this.elecciones = elecciones;
            MostrarDatos();
        }

        private void MostrarDatos()
        {
                dgElecciones.ItemsSource = elecciones;
           
        }

        private void VentanaAgregar_EleccionAgregada(Elecciones nuevaEleccion)
        {
            elecciones.Add(nuevaEleccion);
            EleccionesCargadas?.Invoke(this, new EleccionesEventArgs(elecciones.ToList()));
        }

        private void dgElecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgElecciones.SelectedItem is Elecciones eleccionSeleccionada)
            {
                OnEleccionSeleccionada(eleccionSeleccionada);
                dgDetalles.ItemsSource = eleccionSeleccionada.Partidos;
            }
        }

        protected virtual void OnEleccionSeleccionada(Elecciones eleccion)
        {
            EleccionSeleccionada?.Invoke(this, new EleccionesEventArgs(eleccion));
        }
        private void BtnAgregarPartidos_Click(object sender, RoutedEventArgs e)
        {
            VentanaAgregarElecciones ventanaAgregar = new VentanaAgregarElecciones();
            ventanaAgregar.EleccionAgregada += VentanaAgregar_EleccionAgregada;
            ventanaAgregar.ShowDialog(); // Abrir como ventana modal
        }
        private void BtnBorrarEleccion_Click(object sender, RoutedEventArgs e)
        {
            if (dgElecciones.SelectedItem is Elecciones eleccionSeleccionada)
            {
                elecciones.Remove(eleccionSeleccionada);
                dgElecciones.ItemsSource = null;
                dgElecciones.ItemsSource = elecciones;

                EleccionEliminada?.Invoke(this, EventArgs.Empty); // Invocar el evento que hace que mi canvas de la main window se quede vacio cuando se elimina una elección
                dgDetalles.ItemsSource = null;
                
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una elección para borrar.", "Elección no seleccionada", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

