using PracticaFInal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PracticaFInal
{
    public partial class VentanaAgregarElecciones : Window
    {
        public ObservableCollection<Partido> partidos { get; set; }
        public delegate void EleccionAgregadaHandler(Elecciones nuevaEleccion);
        public event EleccionAgregadaHandler EleccionAgregada;
        private Dictionary<string, int> escañosPorTipoEleccion;

        public VentanaAgregarElecciones()
        {
            InitializeComponent();
            partidos = new ObservableCollection<Partido>();
            dataGridPartidos.ItemsSource = partidos;
            InicializarEscañosPorTipoEleccion();
        }
        private void InicializarEscañosPorTipoEleccion()
        {
            escañosPorTipoEleccion = new Dictionary<string, int>
        {
            { "ELECCIONES GENERALES", 350 },
            // Agrega aquí los tipos de elección y sus escaños correspondientes
            { "ELECCIONES AUTONÓMICAS CyL", 81 },
            { "Comunidad Autónoma 2", 30 },
            // y así sucesivamente para cada comunidad autónoma
        };
        }
        private void AgregarEleccion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obteniendo los datos de la elección
                int totalEscaños = partidos.Sum(p => p.Escaños);
                DateTime? fechaSeleccionada = dpFecha.SelectedDate;
                //pasar la fecha seleccionada a string 
                string fecha = fechaSeleccionada.Value.ToString("dd/MM/yyyy");
                string tipoEleccion = (cmbTipoEleccion.SelectedItem as ComboBoxItem).Content.ToString();
                int escañosRequeridos = tipoEleccion == "ELECCIONES GENERALES" ? 350 : 81;
                if (totalEscaños != escañosRequeridos)
                {
                    MessageBox.Show($"El número total de escaños para {tipoEleccion} debe ser {escañosRequeridos}.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(tipoEleccion))
                {
                    MessageBox.Show("El nombre de la elección no es válido.");
                    return;
                }
                List<Partido> listaPartidos = partidos.ToList(); // Conversión a List<Partido>


                Elecciones nuevaEleccion = new Elecciones(
                    totalEscaños,
                    fecha, // Considera usar un control de fecha o validar este campo
                    tipoEleccion,
                    listaPartidos
                );
                //si el numero de escaños de todos los partidos no es igual a los escaños totales error
                if (nuevaEleccion.EscañosTotales != nuevaEleccion.Partidos.Sum(p => p.Escaños))
                {
                    MessageBox.Show("El número de escaños de los partidos no coincide con el total de escaños.");
                }
                else
                {
                    EleccionAgregada?.Invoke(nuevaEleccion);
                }
                

            }
            catch (FormatException)
            {
                // Manejo de excepción si txtTotalEscaños.Text no es un número válido
                MessageBox.Show("El total de escaños debe ser un número.");
            }
            catch (Exception ex)
            {
                // Manejo de cualquier otra excepción inesperada
                MessageBox.Show("Ha ocurrido un error: " + ex.Message);
            }

            this.Close();
        }

        private void AgregarPartido_Click(object sender, RoutedEventArgs e)
        {
            // Validar nombre del partido
            string mensajeError = ValidarNombrePartido(txtNombrePartido.Text);
            if (mensajeError != null)
            {
                MessageBox.Show(mensajeError);
                return;
            }

            // Obtener color del partido
            SolidColorBrush colorBrush = ObtenerColorPartido(cmbColorPartido.SelectedItem);

            // Validar escaños del partido
            var (isValid, errorMessage) = ValidarEscañosPartido(txtEscañosPartido.Text);
            if (!isValid)
            {
                MessageBox.Show(errorMessage);
                return;
            }

            // Crear instancia del partido y añadir a la lista
            Partido nuevoPartido = new Partido(int.Parse(txtEscañosPartido.Text), txtNombrePartido.Text, colorBrush.ToString());
            partidos.Add(nuevoPartido);
            //comprobar que si hay menos de 2 partidos no se puede agregar
            if (ValidarEscañosTotales())
            {
                //poner visible boton agregar eleccion
                AgregarEleccion.Visibility = Visibility.Visible;
            }
            else { AgregarEleccion.Visibility = Visibility.Hidden;}
            
            // Limpieza de campos
            LimpiarCampos();
        }
        public string ValidarNombrePartido(string nombre)
        {
            return string.IsNullOrWhiteSpace(nombre) ? "El nombre del partido no es válido." : null;
        }
        //hacer una funcion que compruebe la suma total de los escaños de los partidos
        private bool ValidarEscañosTotales()
        {
            int totalEscaños = partidos.Sum(p => p.Escaños);
            string tipoEleccion = (cmbTipoEleccion.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (escañosPorTipoEleccion.TryGetValue(tipoEleccion, out int escañosRequeridos))
            {
                return totalEscaños == escañosRequeridos;
            }
            return false;
        }
        public SolidColorBrush ObtenerColorPartido(object selectedItem)
        {
            if (selectedItem != null)
            {
                string selectedColor = (selectedItem as ComboBoxItem).Tag.ToString();
                return (SolidColorBrush)new BrushConverter().ConvertFromString(selectedColor);
            }
            return GenerarColorAleatorio();
        }
        public (bool isValid, string message) ValidarEscañosPartido(string escañosTexto)
        {
            if (!int.TryParse(escañosTexto, out int escañosPartido) || escañosPartido <= 0)
            {
                return (false, "Número de escaños no válido. Por favor, ingresa un número entero positivo.");
            }
            return (true, null);
        }
        public SolidColorBrush GenerarColorAleatorio()
        {
            Random random = new Random();
            byte[] colorBytes = new byte[3];
            random.NextBytes(colorBytes);
            return new SolidColorBrush(Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]));
        }

        private void LimpiarCampos()
        {
            txtNombrePartido.Text = "";
            txtEscañosPartido.Text = "";
            cmbColorPartido.SelectedIndex = -1;
        }

    }
}