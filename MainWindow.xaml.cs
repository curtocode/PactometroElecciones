﻿using PracticaFInal.Model;
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
        private string rutaArchivo = "C:\\Users\\Alejandro\\Desktop\\USAL\\tercerCurso\\igu\\PracticaFInal\\PracticaFInal\\datos_electorales.json";
        HashSet<Partido> partidosArrastrados = new HashSet<Partido>();
        private Elecciones eleccionActual;
        private List<Brush> coloresDisponibles;
        public MainWindow()
        {
            InitializeComponent();
            AbrirVentanaSecundaria();
            ResetearColoresDisponibles();
            // Agrega los manejadores de eventos de arrastrar y soltar a las columnas
            columnaIzquierda.Drop += Columna_Drop;
            columnaDerecha.Drop += Columna_Drop;
            // Establece los comportamientos de arrastrar y soltar
            columnaIzquierda.AllowDrop = true;
            columnaDerecha.AllowDrop = true;


        }
        private void Columna_Drop(object sender, DragEventArgs e)
        {
            var partido = e.Data.GetData(typeof(Partido)) as Partido;
            if (partido != null && !partidosArrastrados.Contains(partido)) // Si el partido no se ha arrastrado ya
            {
                // Determina a qué columna se arrastró el partido y agrégalo a la lista correspondiente
                if (sender == columnaIzquierda)
                {
                    partidosIzquierda.Add(partido);
                }
                else if (sender == columnaDerecha)
                {
                    partidosDerecha.Add(partido);
                }
                partidosArrastrados.Add(partido); // Registra el partido como arrastrado

                // Redibuja el gráfico con los partidos actualizados
                DibujarGraficoConPartidos();
            }
        }
        private void DibujarGraficoConPartidos()
        {
            if (eleccionActual != null)
            {

                int mayoriaAbsoluta = eleccionActual.MayoriaAbsoluta;
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


                // Asegurarte de que ambos gráficos utilicen la misma escala de altura máxima.
                DibujarBarras(partidosIzquierda, columnaIzquierda,mayoriaAbsoluta);
                DibujarBarras(partidosDerecha, columnaDerecha, mayoriaAbsoluta);
            }
        }
        private void DibujarBarras(List<Partido> partidos, Canvas columna,int mayoriaAbsoluta)
        {
            columna.Children.Clear();
            double margin = 20; // Margen general para la izquierda y la derecha
            double scaleFactor = (columna.ActualHeight - margin * 2) / mayoriaAbsoluta;

            double currentBottom = 0; // La posición de inicio para la primera barra es la parte inferior del Canvas.
            double mayoriaAbsolutaHeight = mayoriaAbsoluta * scaleFactor;

            // Crea la línea de la mayoría absoluta
            Line lineaMayoria = new Line
            {
                X1 = -30, // Inicio de la línea en el borde izquierdo del Canvas
                Y1 = columna.ActualHeight - mayoriaAbsolutaHeight, // Posición Y basada en la mayoría absoluta
                X2 = columna.ActualWidth +30, // Fin de la línea en el borde derecho del Canvas
                Y2 = columna.ActualHeight - mayoriaAbsolutaHeight , // Posición Y igual al inicio
                Stroke = Brushes.Black, // Color de la línea
                StrokeThickness = 2 // Grosor de la línea
            };
            columna.Children.Add(lineaMayoria);
            foreach (var partido in partidos)
            {
                double barHeight = Math.Max(partido.Escaños * scaleFactor, 0);

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
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                // Posiciona el TextBlock a la derecha de la barra.
                Canvas.SetBottom(textBlock, currentBottom + (barHeight / 2) - (textBlock.FontSize / 2)); // Centra verticalmente respecto a la barra.
                Canvas.SetLeft(textBlock, 100 + margin * 2); // Coloca el texto a la derecha de la barra, ajusta según sea necesario.

                columna.Children.Add(textBlock);

                // Actualiza la posición inferior para la próxima barra.
                currentBottom += barHeight;
            }
        }
        private void AbrirVentanaSecundaria()
        {
            VentanaSecundaria ventanaSecundaria = new VentanaSecundaria();
            ventanaSecundaria.EleccionSeleccionada += VentanaSecundaria_EleccionSeleccionada;
            ventanaSecundaria.Show(); // Show significa que se abre la ventana en modo NO modal
        }
        private void BtnPactometro_Click(object sender, RoutedEventArgs e)
        {
            // Cambia la visibilidad del Canvas y el StackPanel del pactómetro
            LimpiarColumnas();
            canvasGrafico.Children.Clear();
            pactometroPanel.Visibility = Visibility.Visible; // Muestra el StackPanel del pactómetro

            if (eleccionActual != null)
            {
                // Encuentra si algún partido tiene mayoría absoluta.
                this.UpdateLayout();
                var partidoConMayoria = eleccionActual.Partidos.FirstOrDefault(p => p.Escaños >= eleccionActual.MayoriaAbsoluta);

                if (partidoConMayoria != null)
                {
                    // Si un partido tiene mayoría absoluta, dibujarlo en un lado.
                    partidosIzquierda.Clear();
                    partidosIzquierda.Add(partidoConMayoria);

                    // Coloca el resto de los partidos en la otra columna.
                    partidosDerecha = eleccionActual.Partidos.Where(p => p != partidoConMayoria).ToList();
                }
                DibujarGraficoConPartidos();
            }


            // Inicializa las columnas si es necesario, por ejemplo, agregando títulos o espacios vacíos para arrastrar los partidos
        } 
        private void VentanaSecundaria_EleccionSeleccionada(Elecciones elecciones)
        {
            eleccionActual = elecciones;
            partidosArrastrados.Clear();
            DibujarGrafico(elecciones);

        }
        private void DibujarGrafico(Elecciones eleccionSeleccionada)
        {
            pactometroPanel.Visibility = Visibility.Collapsed; // Oculta el StackPanel del pactómetro
            ResetearColoresDisponibles();
            List<Partido> partidos = eleccionSeleccionada.Partidos;
            string nombreEleccion = eleccionSeleccionada.Tipo + " " + eleccionSeleccionada.Fecha; // Asumiendo que Fecha es un string

            canvasGrafico.Children.Clear(); // Limpia el lienzo antes de dibujar un nuevo gráfico

            // Ajusta estos márgenes según sea necesario para que todo quepa
            const double margin = 20; // Margen general para la izquierda y la derecha
            double maxEscaños = eleccionSeleccionada.MayoriaAbsoluta; // Usamos mayoría absoluta como el valor máximo de escaños
            double scaleFactor = (canvasGrafico.ActualHeight - margin * 2) / maxEscaños; // Escala basada en la mayoría absoluta
            double totalBarWidth = canvasGrafico.ActualWidth - margin * 2; // Ancho total para todas las barras
            double barWidth = totalBarWidth / partidos.Count - margin; // Ancho individual de la barra

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
                Canvas.SetRight(escañosLabel, canvasGrafico.ActualWidth - 10); // Alinea a la derecha
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
            }

            // Agrega el título de la elección en la parte superior del Canvas
            TextBlock tituloElecciones = new TextBlock
            {
                Text = nombreEleccion,
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
        Brushes.Cyan,
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


    }
}