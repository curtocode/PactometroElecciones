using PracticaFInal.Model;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static PracticaFInal.VentanaSecundaria;

namespace PracticaFInal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Partido> partidosIzquierda = new List<Partido>();
        List<Partido> partidosDerecha = new List<Partido>();
        private Elecciones eleccionActual;
        private List<Brush> coloresDisponibles;
        private VentanaSecundaria ventanaSecundaria;
        ObservableCollection<Elecciones> MisElecciones;
        CargarDatos datos;
        private string rutaArchivo = "datos_electorales.json";
        public MainWindow()
        {
            InitializeComponent();
            MostrarDatos();
            AbrirVentanaSecundaria();
            ResetearColoresDisponibles();
            // Agrega los manejadores de eventos de arrastrar y soltar a las columnas
            this.ResizeMode = ResizeMode.NoResize;


        }
        private void MostrarDatos()
        {
            try
            {
                datos = new CargarDatos(rutaArchivo);
                var eleccionesList = datos.LeerDatosElecciones();
                MisElecciones = new ObservableCollection<Elecciones>(eleccionesList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }

        private void Pactometro()
        {
            if (eleccionActual != null)
            {

                int mayoriaAbsoluta = eleccionActual.MayoriaAbsoluta;


                // Asegurarte de que ambos gráficos utilicen la misma escala de altura máxima.
                DibujarBarras(partidosIzquierda, columnaIzquierda,mayoriaAbsoluta);
                DibujarBarras(partidosDerecha, columnaDerecha, mayoriaAbsoluta);
            }
        }
        private void DibujarBarras(List<Partido> partidos, Canvas columna,int mayoriaAbsoluta)
        {
            //scrollViewerGrafico.Visibility = Visibility.Collapsed;
            canvasGrafico.Width = Double.NaN;
            this.UpdateLayout();
            columna.Children.Clear();
            AñadirTituloElecciones();
            double margin = columna.Margin.Top; // Margen general para la izquierda y la derecha
            int totalEscaños = eleccionActual.EscañosTotales; // Suma los escaños de todos los partidos
            double scaleFactor = (columna.ActualHeight - margin * 2) / totalEscaños;


            double currentBottom = 0; // La posición de inicio para la primera barra es la parte inferior del Canvas.
            double mayoriaAbsolutaHeight = mayoriaAbsoluta * scaleFactor;

            // Crea la línea de la mayoría absoluta
            Line lineaMayoria = new Line
            {
                X1 = -30, // Inicio de la línea en el borde izquierdo del Canvas
                Y1 = columna.ActualHeight - mayoriaAbsolutaHeight, // Posición Y basada en la mayoría absoluta
                X2 = columna.ActualWidth *1.5, // Fin de la línea en el borde derecho del Canvas
                Y2 = columna.ActualHeight - mayoriaAbsolutaHeight , // Posición Y igual al inicio
                Stroke = Brushes.Black, // Color de la línea
                StrokeThickness = 2 // Grosor de la línea
            };
            columna.Children.Add(lineaMayoria);
            foreach (var partido in partidos)
            {
                double barHeight = partido.Escaños * scaleFactor;

                SolidColorBrush colorBrush;
                if (string.IsNullOrEmpty(partido.Color))
                {
                    colorBrush = (SolidColorBrush)RandomColor();
                    partido.Color = colorBrush.ToString();
                }
                else
                {
                    colorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(partido.Color);
                }

                var rect = new Rectangle
                {
                    Height = barHeight,
                    Width = 100, // Ancho fijo para la barra
                    Fill = colorBrush
                };

                // Posiciona la barra en el Canvas, justo encima de la barra anterior.
                Canvas.SetBottom(rect, currentBottom);
                Canvas.SetLeft(rect, margin); // Asume un margen para dejar espacio para el nombre del partido.

                columna.Children.Add(rect);

                // Crea el TextBlock para el nomb  if(partido.Escaños > 30)
                AgregarNombrePartidoConMenosDe30Escaños(columna, partido, currentBottom, barHeight, margin);


                rect.Name = "rect" + partido.Nombre;

                
                rect.MouseLeftButtonDown += (s, e) => {
                    btnFinalizarCoalicion.Visibility = Visibility.Visible;
                    RemoverPartidoDeColumna(partido);
                    
                };
                rect.MouseEnter += (s, e) => ResaltarPartido(rect, true);
                rect.MouseLeave += (s, e) => ResaltarPartido(rect, false);





                // Actualiza la posición inferior para la próxima barra.
                currentBottom += barHeight;
               
            }
        }
        private void AgregarNombrePartidoConMenosDe30Escaños(Canvas columna, Partido partido, double currentBottom, double barHeight, double margin)
        {
            int opacidad;
            if(eleccionActual.Tipo.Equals("ELECCIONES GENERALES"))
            {
                if (partido.Escaños <= 30)
                {
                    opacidad = 0;
                }
                else
                    opacidad = 1;
            }
            else 
            {
                if(partido.Escaños <= 10) 
                {
                    opacidad = 0;
                }
                else
                {
                    opacidad = 1;
                }
            }


            TextBlock textBlock = new TextBlock
            {
                Text = $"{partido.Nombre} - {partido.Escaños}",
                Margin = new Thickness(5, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Bottom,
                Opacity = opacidad,
               // Opacity = partido.Escaños < 30 ? 0 : 1 // Transparente si es menor a 30
            };
            textBlock.Name = "text" + partido.Nombre;

            Canvas.SetBottom(textBlock, currentBottom + (barHeight / 2) - (textBlock.FontSize / 2));
            Canvas.SetLeft(textBlock, 100 + margin * 2);


            // Crea un rectángulo transparente para recibir eventos del ratón
            Rectangle hitBox = new Rectangle
            {
                Width = 100, // El mismo ancho que el TextBlock
                Height = barHeight, // La altura de la barra
                Fill = Brushes.Transparent // Transparente para no afectar la visualización
            };

            Canvas.SetBottom(hitBox, currentBottom);
            Canvas.SetLeft(hitBox, margin);
            hitBox.MouseLeftButtonDown += (s, e) => {
                btnFinalizarCoalicion.Visibility = Visibility.Visible;
                RemoverPartidoDeColumna(partido);

            };
            hitBox.MouseEnter += (s, e) => ResaltarPartido(hitBox, true);
            hitBox.MouseLeave += (s, e) => ResaltarPartido(hitBox, false);

            // Eventos para cambiar la opacidad del TextBlock
            
            hitBox.MouseEnter += (s, e) => textBlock.Opacity = 1; // Hacer el texto opaco
            if(eleccionActual.Tipo.Equals("ELECCIONES GENERALES")) { 
                if (partido.Escaños <= 30)
                    hitBox.MouseLeave += (s, e) => textBlock.Opacity = 0;
                else
                    hitBox.MouseLeave += (s, e) => textBlock.Opacity = 1;
            }
            else
            {
                if (partido.Escaños <= 10)
                    hitBox.MouseLeave += (s, e) => textBlock.Opacity = 0;
                else
                    hitBox.MouseLeave += (s, e) => textBlock.Opacity = 1;
            }

            columna.Children.Add(hitBox);
            columna.Children.Add(textBlock);
        }

        private void ResaltarPartido(Rectangle rect, bool resaltar)
        {
            if (resaltar)
            {
                rect.Stroke = Brushes.Black; // Color del borde
                rect.StrokeThickness = 4; // Grosor del borde
         
            }
            else
            {
                rect.Stroke = null; // Quita el borde
                rect.StrokeThickness = 0;
                
            }
        }
        private void RemoverPartidoDeColumna(Partido partido)
        {
            // Remover el partido de la lista correspondiente y actualizar la interfaz
            if (partidosIzquierda.Remove(partido))
            {
                partidosDerecha.Add(partido);

            }
            else if (partidosDerecha.Remove(partido))
            {
                partidosIzquierda.Add(partido);
                
            }
            Pactometro();
        }
        private void VentanaSecundaria_EleccionesCargadas(object sender, EleccionesEventArgs e)
        {
            //Son las elecciones cargadas
            MisElecciones = new ObservableCollection<Elecciones>(e.Elecciones);
        }
        private void AbrirVentanaSecundaria()
        {
            if (ventanaSecundaria == null)
            {
                ventanaSecundaria = new VentanaSecundaria(MisElecciones);
                ventanaSecundaria.EleccionSeleccionada += VentanaSecundaria_EleccionSeleccionada;
                ventanaSecundaria.EleccionesCargadas += VentanaSecundaria_EleccionesCargadas;
                ventanaSecundaria.EleccionEliminada += VentanaSecundaria_EleccionEliminada;
                ventanaSecundaria.Closing += VentanaSecundaria_Closing; // Manejar el evento Closing
            }

            ventanaSecundaria.Show();
            ventanaSecundaria.Focus();
        }
        private void VentanaSecundaria_EleccionEliminada(object sender, EventArgs e)
        {
            canvasGrafico.Children.Clear(); // Limpia el Canvas
        }
        private void BtnAbrirVentanaSecundaria_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentanaSecundaria();
        }

        private void VentanaSecundaria_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;  // Cancela el cierre de la ventana
            ventanaSecundaria.Hide(); // Oculta la ventana en lugar de cerrarla
        }
        private void BtnFinalizarCoalicion_Click(object sender, RoutedEventArgs e)
        {
            // Asegúrate de que haya una elección actual seleccionada
            if (eleccionActual == null) return;
            // si se ha alacanzado la mayoria absoluta  se muestra un mensaje indicandolo
            if (eleccionActual.PartidoConMasEscaños().Escaños >= eleccionActual.MayoriaAbsoluta)
            {
                MessageBox.Show("Se ha alcanzado la mayoría absoluta");
            }
            else
            {
                MessageBox.Show("La coalición no ha alcanzado la mayoríoa");
            }
        }
        private void BtnPactometro_Click(object sender, RoutedEventArgs e)
        {
            btnFinalizarCoalicion.Visibility = Visibility.Visible;
            // Cambia la visibilidad del Canvas y el StackPanel del pactómetro
            LimpiarColumnas();
            canvasGrafico.Children.Clear();
            pactometroPanel.Visibility = Visibility.Visible; // Muestra el StackPanel del pactómetro
            btnComparativa.Visibility = Visibility.Collapsed;

            if (eleccionActual != null)
            {
                // Encuentra si algún partido tiene mayoría absoluta.
                this.UpdateLayout();
                var partidoConMayoria = eleccionActual.Partidos.FirstOrDefault(p => p.Escaños >= eleccionActual.MayoriaAbsoluta);

                if (partidoConMayoria != null)
                {
                    // Si un partido tiene mayoría absoluta, dibujarlo en un lado.
                    partidosDerecha.Clear();
                    partidosDerecha.Add(partidoConMayoria);

                    // Coloca el resto de los partidos en la otra columna.
                    partidosIzquierda = eleccionActual.Partidos.Where(p => p != partidoConMayoria).ToList();
                    MessageBox.Show("Se ha alcanzado la mayoría absoluta");
                }
                else
                {
                    foreach (var partido in eleccionActual.Partidos)
                    {
                        partidosIzquierda.Add(partido);
                    }
                    partidosDerecha.Clear();

                }
                Pactometro();
            }
        }
        private void VentanaSecundaria_EleccionSeleccionada(object sender, EleccionesEventArgs e)
        {
            RestablecerScrollViewer();

            // Asumiendo que 'e.Eleccion' es la instancia de 'Elecciones' que necesitas
            eleccionActual = e.Eleccion;
            DibujarGrafico(e.Eleccion);
            btnFinalizarCoalicion.Visibility = Visibility.Collapsed;
            btnComparativa.Visibility = Visibility.Visible;
        }
        private void DibujarGrafico(Elecciones eleccionSeleccionada)
        {
            RestablecerScrollViewer();
            canvasGrafico.Width = Double.NaN; // Esto restablece el ancho para que se auto ajuste.
            this.UpdateLayout();
            pactometroPanel.Visibility = Visibility.Collapsed; // Oculta el StackPanel del pactómetro
            //ResetearColoresDisponibles();
            List<Partido> partidos = eleccionSeleccionada.Partidos;

            canvasGrafico.Children.Clear(); // Limpia el lienzo antes de dibujar un nuevo gráfico

            // Ajusta estos márgenes según sea necesario para que todo quepa
            const double margin = 20; // Margen general para la izquierda y la derecha
            //Obtengo el numero total de escaños del partido mas votado para hacer la escala de mi grafico
            int maxEscaños =  eleccionSeleccionada.PartidoConMasEscaños().Escaños;

            double scaleFactor = (canvasGrafico.ActualHeight) / maxEscaños *0.8;
            double totalBarWidth = canvasGrafico.ActualWidth*0.9; // Ancho total para todas las barras
            double barWidth = totalBarWidth / partidos.Count - margin; // Ancho individual de la barra
            double requiredCanvasWidth = 0;
            // Agrega la leyenda de escaños
            for (int i = 0; i <= maxEscaños + 10; i += 10)
            {
                double yPosition = canvasGrafico.ActualHeight - (i * scaleFactor) - 30; // Ajusta la posición 'Y' para cada marca

                // Línea para la marca de escaños
                Line markLine = new Line
                {
                    X1 = 25,
                    Y1 = yPosition,
                    X2 = 35, // Ajusta la longitud de la marca de escaño
                    Y2 = yPosition,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };
                canvasGrafico.Children.Add(markLine);

                // Etiqueta de número de escaños
                TextBlock escañosLabel = new TextBlock
                {
                    Text = i.ToString(),
                    TextAlignment = TextAlignment.Right,
                    Width = 25
                };
                Canvas.SetRight(escañosLabel, canvasGrafico.ActualWidth); // Alinea a la derecha
                Canvas.SetTop(escañosLabel, yPosition - 10);
                canvasGrafico.Children.Add(escañosLabel);
            }

            // Calcula la altura de las barras y las dibuja
            for (int i = 0; i < partidos.Count; i++)
            {
                Partido partido = partidos[i];
                double barHeight = partido.Escaños * scaleFactor;

                SolidColorBrush colorBrush;
                if (string.IsNullOrEmpty(partido.Color))
                {
                    colorBrush = (SolidColorBrush)RandomColor();
                    partido.Color = colorBrush.ToString();
                }
                else
                {
                    colorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(partido.Color);
                }

                Rectangle rect = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = colorBrush
                };

                Canvas.SetLeft(rect, (i * (barWidth + 15)) + 40); // Deja espacio para la leyenda
                Canvas.SetTop(rect, canvasGrafico.ActualHeight - barHeight - 30);

                canvasGrafico.Children.Add(rect);

                TextBlock nombrePartido = new TextBlock
                {
                    Text = partido.Nombre,
                    Width = barWidth,
                    TextAlignment = TextAlignment.Center
                };
                Canvas.SetLeft(nombrePartido, (i * (barWidth + 15)) + 40);
                Canvas.SetTop(nombrePartido, canvasGrafico.ActualHeight - 20);

                canvasGrafico.Children.Add(nombrePartido);
                requiredCanvasWidth += barWidth + margin;
            }
            actualizarScroll(requiredCanvasWidth);

            // Agrega el título de la elección en la parte superior del Canvas
            AñadirTituloElecciones();
        }
        private void actualizarScroll(double requiredCanvasWidth)
        {
            if (requiredCanvasWidth > scrollViewerGrafico.ViewportWidth)
            {
                // Establece el ancho del Canvas para habilitar el desplazamiento horizontal
                canvasGrafico.Width = requiredCanvasWidth;
            }
            else
            {
                // Establece el ancho del Canvas para que coincida con el ViewportWidth para desactivar el desplazamiento innecesario
                canvasGrafico.Width = scrollViewerGrafico.ViewportWidth;
            }
        }
        private void AñadirTituloElecciones()
        {

            TextBlock tituloElecciones = new TextBlock
            {
                Text = eleccionActual.Tipo + " " + eleccionActual.Fecha,
                Width = canvasGrafico.ActualWidth - 50, // Deja espacio para la leyenda
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetTop(tituloElecciones, 0);
            Canvas.SetLeft(tituloElecciones, 25); // Alinea al inicio de las barras
            canvasGrafico.Children.Add(tituloElecciones);
        }

        private Brush RandomColor()
        {
            if (coloresDisponibles.Count == 0)
            {
                // Si todos los colores han sido usados, reinicia la lista
                ResetearColoresDisponibles();
            }

            Random rnd = new Random();
            int index = rnd.Next(coloresDisponibles.Count); // Selecciona un índice aleatorio
            Brush colorSeleccionado = coloresDisponibles[index]; // Obtiene el color
            coloresDisponibles.RemoveAt(index); // Elimina el color de la lista para no repetirlo
            return colorSeleccionado;
        }
        private void ResetearColoresDisponibles()
        {
            coloresDisponibles = new List<Brush>
    {
        Brushes.Yellow,
        Brushes.Purple,
        Brushes.Orange,
        Brushes.Brown,
        Brushes.Pink,
        Brushes.Magenta
        // Agrega más colores si es necesario
    };
        }
        private void LimpiarColumnas()
        {
            // Suponiendo que tienes listas de partidos que se están utilizando
            // para mantener los elementos en cada columna.
            partidosIzquierda.Clear();
            partidosDerecha.Clear();
            columnaIzquierda.Children.Clear();
            columnaDerecha.Children.Clear();


        }
        private List<Elecciones> FiltrarEleccionesPorTipo(string tipo)
        {
            return MisElecciones.Where(eleccion => eleccion.Tipo == tipo).ToList();
        }
        private void DibujarGraficoComparativo(string tipo)
        {
            RestablecerScrollViewer();
            double factor = 1;

            List<Elecciones> eleccionesFiltradas = FiltrarEleccionesPorTipo(tipo).OrderByDescending(e => Convert.ToInt32(e.Fecha.Substring(e.Fecha.Length - 4))).ToList();
            canvasGrafico.Children.Clear();
            ResetearColoresDisponibles();
            canvasGrafico.Width = Double.NaN;
            this.UpdateLayout();

            // Ajustamos la altura del gráfico para dejar espacio para las etiquetas de escaños
            double leftMargin = 40; // Espacio para las marcas de escaños
            double bottomMargin = 30; // Espacio para los nombres de los partidos

            int maxEscaños = eleccionActual.PartidoConMasEscaños().Escaños;
            foreach (var eleccion in eleccionesFiltradas)
            {
                var partidoConMasEscaños = eleccion.PartidoConMasEscaños();
                if (partidoConMasEscaños != null && partidoConMasEscaños.Escaños > maxEscaños)
                {
                    maxEscaños = partidoConMasEscaños.Escaños;
                }
            }
            double scaleFactor = (canvasGrafico.ActualHeight) / maxEscaños * 0.8;

            int numberOfParties = eleccionesFiltradas.SelectMany(e => e.Partidos).Select(p => p.Nombre).Distinct().Count();
            int numberOfElections = eleccionesFiltradas.Count;
            double barWidth = (canvasGrafico.ActualWidth - leftMargin) / numberOfParties / numberOfElections;
            double requiredCanvasWidth = 0;

            // Dibujar las marcas de escaños en el margen izquierdo
            for (int i = 0; i <= maxEscaños + 20; i += 10)
            {
                double yPosition = canvasGrafico.ActualHeight - (i * scaleFactor) - bottomMargin; // Ajusta la posición 'Y' para cada marca

                // Línea para la marca de escaños
                Line markLine = new Line
                {
                    X1 = leftMargin,
                    Y1 = yPosition,
                    X2 = leftMargin + 10,
                    Y2 = yPosition,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };
                canvasGrafico.Children.Add(markLine);

                // Etiqueta de número de escaños
                TextBlock escañosLabel = new TextBlock
                {
                    Text = i.ToString(),
                    TextAlignment = TextAlignment.Right,
                    Width = leftMargin
                };
                Canvas.SetLeft(escañosLabel, 0);
                Canvas.SetTop(escañosLabel, yPosition - 10);
                canvasGrafico.Children.Add(escañosLabel);
            }

            // Variables para controlar la opacidad
            double initialOpacity = 1.0;
            double minimumOpacity = 0.3; // Opacidad mínima para barras antiguas
            double opacityDecrement = (initialOpacity - minimumOpacity) / (numberOfElections - 1);


            double nombrePartidosYPosition = 20;
            for (int i = 0; i < numberOfParties; i++)
            {
                string partyName = eleccionesFiltradas.SelectMany(e => e.Partidos).Select(p => p.Nombre).Distinct().ElementAt(i);

                double partyStartingXPosition = i * (canvasGrafico.ActualWidth / numberOfParties);
                double opacity = initialOpacity;
                TextBlock nombrePartido = new TextBlock
                {
                    Text = partyName,
                    Width = (canvasGrafico.ActualWidth / numberOfParties),
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 9,
                };
                if (eleccionActual.Tipo.Equals("ELECCIONES GENERALES"))
                {
                    factor = 1;
                }else
                {
                    factor = 0.5;
                }
                Canvas.SetLeft(nombrePartido, partyStartingXPosition + ((barWidth) * 2 )*factor);
                Canvas.SetTop(nombrePartido, canvasGrafico.ActualHeight - nombrePartidosYPosition);
                canvasGrafico.Children.Add(nombrePartido);
                requiredCanvasWidth += barWidth * (eleccionesFiltradas.Count + 0.5);

                for (int j = 0; j < numberOfElections; j++)
                {

                    Elecciones election = eleccionesFiltradas[j];
                    Partido partido = election.Partidos.FirstOrDefault(p => p.Nombre == partyName);

                    if (partido != null)
                    {
                        SolidColorBrush colorBrush;
                        if (string.IsNullOrEmpty(partido.Color))
                        {
                            colorBrush = (SolidColorBrush)RandomColor();
                            partido.Color = colorBrush.ToString();
                        }
                        else
                        {
                            colorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(partido.Color);
                        }

                        Rectangle rect = new Rectangle
                        {
                            Width = barWidth - 10, // 1 pixel para el espacio entre barras
                            Height = partido.Escaños * scaleFactor,
                            Fill = colorBrush,
                            Opacity = opacity
                        };

                        Canvas.SetLeft(rect, partyStartingXPosition + (barWidth * j)+50);
                        Canvas.SetTop(rect, canvasGrafico.ActualHeight - (partido.Escaños *scaleFactor) - 30);
                        canvasGrafico.Children.Add(rect);
                    }

                    // Reducir la opacidad para la siguiente elección más antigua
                    opacity -= opacityDecrement;
                    if (opacity < minimumOpacity) opacity = minimumOpacity;
                }
            }
            requiredCanvasWidth += leftMargin;
            if (requiredCanvasWidth > scrollViewerGrafico.ViewportWidth)
            {
                // Establece el ancho del Canvas para habilitar el desplazamiento horizontal
                canvasGrafico.Width = requiredCanvasWidth;
            }
            else
            {
                // Establece el ancho del Canvas para que coincida con el ViewportWidth para desactivar el desplazamiento innecesario
                canvasGrafico.Width = scrollViewerGrafico.ViewportWidth;
            }
            TituloSinFecha();

            var yearColorMapping = AssignColorsToYears(eleccionesFiltradas);
            AgregarLeyendaAlGrafico(eleccionesFiltradas, requiredCanvasWidth, scaleFactor, yearColorMapping);

        }
        private void AgregarLeyendaAlGrafico(List<Elecciones> eleccionesFiltradas, double requiredCanvasWidth, double scaleFactor, Dictionary<int, Color> yearColorMapping)
        {
            double width = canvasGrafico.ActualWidth;
            double legendStartingXPosition = width / 1.1; // Comienza a la derecha del gráfico
            double legendYPosition = ActualHeight / 4; // Comienza en la parte superior del área de la leyenda

            for (int j = 0; j < eleccionesFiltradas.Count; j++)
            {
                Elecciones election = eleccionesFiltradas[j];
                Rectangle legendColor = new Rectangle
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(ColorFromYear(election.Fecha, yearColorMapping)) // Usa el mapeo de colores
                };
                Canvas.SetLeft(legendColor, legendStartingXPosition);
                Canvas.SetTop(legendColor, legendYPosition + (j * 20));
                canvasGrafico.Children.Add(legendColor);

                TextBlock legendText = new TextBlock
                {
                    Text = election.Fecha.Substring(election.Fecha.Length - 4),
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(legendText, legendStartingXPosition + 15);
                Canvas.SetTop(legendText, legendYPosition + (j * 20));
                canvasGrafico.Children.Add(legendText);
            }
        }
        private Dictionary<int, Color> AssignColorsToYears(List<Elecciones> elecciones)
        {
            // Extrae los años únicos y órdenalos
            var uniqueYears = elecciones.Select(e => int.Parse(e.Fecha.Substring(e.Fecha.Length - 4)))
                                         .Distinct()
                                         .OrderBy(year => year)
                                         .ToList();

            // Colores base que quieres usar en orden
            var baseColors = new List<Color> { Colors.LightGray, Colors.DarkGray, Colors.Gray };

            // Asegúrate de que hay suficientes colores para los años, repite la lista de colores si es necesario
            var colorsForYears = new List<Color>();
            while (colorsForYears.Count < uniqueYears.Count)
            {
                colorsForYears.AddRange(baseColors);
            }

            // Asigna un color a cada año
            var yearColorMapping = uniqueYears.Zip(colorsForYears, (year, color) => new { year, color })
                                              .ToDictionary(item => item.year, item => item.color);

            return yearColorMapping;
        }
        private Color ColorFromYear(string fecha, Dictionary<int, Color> yearColorMapping)
        {
            int year = int.Parse(fecha.Substring(fecha.Length - 4));
            return yearColorMapping.TryGetValue(year, out Color color) ? color : Colors.Transparent;
        }
        private void BtnDibujarComparativa_Click(object sender, RoutedEventArgs e)
        {
            if (eleccionActual == null) return;
            string tipoSeleccionado = eleccionActual.Tipo;
            DibujarGraficoComparativo(tipoSeleccionado);
        }
        private void RestablecerScrollViewer()
        {
            // Restablece cualquier propiedad del ScrollViewer que pueda haber cambiado.
            scrollViewerGrafico.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            // Ajusta otras propiedades necesarias del ScrollViewer.
        }
        private void TituloSinFecha()
        {
            TextBlock tituloElecciones = new TextBlock
            {
                Text = eleccionActual.Tipo,
                Width = canvasGrafico.ActualWidth - 50, // Deja espacio para la leyenda
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetTop(tituloElecciones, 0);
            Canvas.SetLeft(tituloElecciones, 25); // Alinea al inicio de las barras
            canvasGrafico.Children.Add(tituloElecciones);
        }


    }
}