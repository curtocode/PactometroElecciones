using PracticaFInal.Model;
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
using ControllerNamespace = PracticaFInal.Controller;

namespace PracticaFInal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Partido> partidosIzquierda = new List<Partido>();
        List<Partido> partidosDerecha = new List<Partido>();
        HashSet<Partido> partidosArrastrados = new HashSet<Partido>();
        private Elecciones eleccionActual;
        private List<Brush> coloresDisponibles;

        List<Elecciones> MisElecciones;

        public MainWindow()
        {
            InitializeComponent();
            AbrirVentanaSecundaria();
            ResetearColoresDisponibles();
            // Agrega los manejadores de eventos de arrastrar y soltar a las columnas
            MisElecciones = new List<Elecciones>();


        }
       
        private void DibujarGraficoConPartidos()
        {
            if (eleccionActual != null)
            {

                int mayoriaAbsoluta = eleccionActual.MayoriaAbsoluta;
                AñadirTituloElecciones();


                // Asegurarte de que ambos gráficos utilicen la misma escala de altura máxima.
                DibujarBarras(partidosIzquierda, columnaIzquierda,mayoriaAbsoluta);
                DibujarBarras(partidosDerecha, columnaDerecha, mayoriaAbsoluta);
            }
        }
        private void DibujarBarras(List<Partido> partidos, Canvas columna,int mayoriaAbsoluta)
        {
            //scrollViewerGrafico.Visibility = Visibility.Collapsed;
            columna.Children.Clear();
            double margin = columna.Margin.Top; // Margen general para la izquierda y la derecha
            int totalEscaños = mayoriaAbsoluta * 2 + 1; // Suma los escaños de todos los partidos
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

                // Crea el TextBlock para el nombre del partido.
                    var textBlock = new TextBlock
                    {
                        Text = $"{partido.Nombre} - {partido.Escaños}",
                        Margin = new Thickness(5, 0, 0, 0),
                        VerticalAlignment = VerticalAlignment.Bottom,
                        
                        
                    };

                // Posiciona el TextBlock a la derecha de la barra.
                Canvas.SetBottom(textBlock, currentBottom + (barHeight / 2) - (textBlock.FontSize / 2)); // Centra verticalmente respecto a la barra.
                Canvas.SetLeft(textBlock, 100 + margin * 2); // Coloca el texto a la derecha de la barra, ajusta según sea necesario.
                rect.Name = "rect" + partido.Nombre;
                textBlock.Name = "text" + partido.Nombre;

                
                rect.MouseLeftButtonDown += (s, e) => {
                    btnFinalizarCoalicion.Visibility = Visibility.Visible;
                    RemoverPartidoDeColumna(partido);
                    
                };
                textBlock.MouseLeftButtonDown += (s, e) => {
                    btnFinalizarCoalicion.Visibility = Visibility.Visible;
                    RemoverPartidoDeColumna(partido);
                };
                rect.MouseEnter += (s, e) => ResaltarPartido(rect, textBlock, true);
                rect.MouseLeave += (s, e) => ResaltarPartido(rect, textBlock, false);
                textBlock.MouseEnter += (s, e) => ResaltarPartido(rect, textBlock, true);
                textBlock.MouseLeave += (s, e) => ResaltarPartido(rect, textBlock, false);

                columna.Children.Add(textBlock);



                // Actualiza la posición inferior para la próxima barra.
                currentBottom += barHeight;
            }
        }
        private void ResaltarPartido(Rectangle rect, TextBlock textBlock, bool resaltar)
        {
            if (resaltar)
            {
                rect.Stroke = Brushes.Black; // Color del borde
                rect.StrokeThickness = 4; // Grosor del borde
                textBlock.FontWeight = FontWeights.ExtraBold;
            }
            else
            {
                rect.Stroke = null; // Quita el borde
                rect.StrokeThickness = 0;
                textBlock.FontWeight = FontWeights.Normal;
            }
        }
        private void RemoverPartidoDeColumna(Partido partido)
        {
            // Remover el partido de la lista correspondiente y actualizar la interfaz
            if (partidosIzquierda.Remove(partido))
            {
                partidosArrastrados.Remove(partido);
                partidosDerecha.Add(partido);

            }
            else if (partidosDerecha.Remove(partido))
            {
                partidosArrastrados.Remove(partido);
                partidosIzquierda.Add(partido);
                
            }
            DibujarGraficoConPartidos();
        }
        private void VentanaSecundaria_EleccionesCargadas(List<Elecciones> eleccionesCargadas)
        {
            // Aquí manejas la lista de elecciones, por ejemplo, asignándola a una propiedad de la clase MainWindow.
            // Asegúrate de tener una propiedad para almacenar las elecciones.
            MisElecciones = eleccionesCargadas;

            // Actúa en consecuencia, por ejemplo, actualizando la interfaz de usuario o lo que necesites hacer con la lista de elecciones.
        }
        private void AbrirVentanaSecundaria()
        {
            VentanaSecundaria ventanaSecundaria = new VentanaSecundaria();
            ventanaSecundaria.EleccionSeleccionada += VentanaSecundaria_EleccionSeleccionada;
            ventanaSecundaria.EleccionesCargadas += VentanaSecundaria_EleccionesCargadas;
            ventanaSecundaria.Show(); // Show significa que se abre la ventana en modo NO modal
        }
        private void BtnFinalizarCoalicion_Click(object sender, RoutedEventArgs e)
        {
            // Asegúrate de que haya una elección actual seleccionada
            if (eleccionActual == null) return;
            partidosArrastrados = eleccionActual.Partidos.ToHashSet();
            // si se ha alacanzado la mayoria absoluta  se muestra un mensaje indicandolo
            if (partidosDerecha.Sum(p => p.Escaños) >= eleccionActual.MayoriaAbsoluta)
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
                }
                else
                {
                    //dibujar todos los partidos en la columna izquierda
                    partidosArrastrados = eleccionActual.Partidos.ToHashSet();
                    //partidosIzquierda = eleccionActual.Partidos;
                    foreach (var partido in eleccionActual.Partidos)
                    {
                        partidosIzquierda.Add(partido);
                    }
                    partidosDerecha.Clear();

                }
                DibujarGraficoConPartidos();
            }
        } 
        private void VentanaSecundaria_EleccionSeleccionada(Elecciones elecciones)
        {
            RestablecerScrollViewer();
            
            eleccionActual = elecciones;
            partidosArrastrados.Clear();
            DibujarGrafico(elecciones);
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
            double maxEscaños = eleccionSeleccionada.MayoriaAbsoluta; // Usamos mayoría absoluta como el valor máximo de escaños
            double scaleFactor = (canvasGrafico.ActualHeight - margin * 2) / maxEscaños; // Escala basada en la mayoría absoluta
            double totalBarWidth = canvasGrafico.ActualWidth - margin * 2; // Ancho total para todas las barras
            double barWidth = totalBarWidth / partidos.Count - margin; // Ancho individual de la barra
            double requiredCanvasWidth = 0;
            // Agrega la leyenda de escaños
            for (int i = 0; i <= maxEscaños; i += 10)
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
                //rect.MouseEnter += (s, e) => MostrarEscaños(rect, true);
               // rect.MouseLeave += (s, e) => MostrarEscaños(rect, false);

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
            canvasGrafico.Width = requiredCanvasWidth;
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

            // Agrega el título de la elección en la parte superior del Canvas
            AñadirTituloElecciones();
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
            partidosArrastrados.Clear();


        }
        private List<Elecciones> FiltrarEleccionesPorTipo(string tipo)
        {
            return MisElecciones.Where(eleccion => eleccion.Tipo == tipo).ToList();
        }
        private void DibujarGraficoComparativo(string tipo)
        {
            RestablecerScrollViewer();
            

            List<Elecciones> eleccionesFiltradas = FiltrarEleccionesPorTipo(tipo).OrderByDescending(e => e.Fecha).ToList();

            canvasGrafico.Children.Clear();
            ResetearColoresDisponibles();
            canvasGrafico.Width = Double.NaN;
            this.UpdateLayout();

            // Ajustamos la altura del gráfico para dejar espacio para las etiquetas de escaños
            double leftMargin = 40; // Espacio para las marcas de escaños
            double bottomMargin = 50; // Espacio para los nombres de los partidos

            double maxEscaños = eleccionActual.MayoriaAbsoluta;
            double scaleFactor = (canvasGrafico.ActualHeight - bottomMargin) / maxEscaños;

            int numberOfParties = eleccionesFiltradas.SelectMany(e => e.Partidos).Select(p => p.Nombre).Distinct().Count();
            int numberOfElections = eleccionesFiltradas.Count;
            double barWidth = (canvasGrafico.ActualWidth - leftMargin) / numberOfParties / numberOfElections;
            double requiredCanvasWidth = 0;

            // Dibujar las marcas de escaños en el margen izquierdo
            for (int i = 0; i <= maxEscaños; i += 10)
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
                Canvas.SetLeft(nombrePartido, partyStartingXPosition + (barWidth) * 2);
                Canvas.SetTop(nombrePartido, canvasGrafico.ActualHeight - nombrePartidosYPosition);
                canvasGrafico.Children.Add(nombrePartido);
                requiredCanvasWidth += barWidth + leftMargin +20;

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
                        Canvas.SetBottom(rect, 50); // Margen inferior
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


    }
}