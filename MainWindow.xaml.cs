using System;
using System.Windows;

namespace SplitJerkTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (sender, e) => this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnInputChanged(object sender, RoutedEventArgs e)
        {
            // Obtener los valores de los inputs
            if (double.TryParse(PressSobreCabezaInput.Text, out double pressSobreCabeza) &&
                double.TryParse(SentadillaInput.Text, out double sentadilla) &&
                double.TryParse(SplitJerkInput.Text, out double splitJerk))
            {
                // Cálculos
                double splitJerkPressSC = (pressSobreCabeza - 3.174) / 0.5751;
                double splitJerkSentadilla = (sentadilla - 6.994) / 1.262;
                double splitJerkAmbos = 0.512 + (sentadilla * 0.6329) + (pressSobreCabeza * 0.2472);

                // Mostrar los resultados
                SplitJerkPressSCResult.Text = splitJerkPressSC.ToString("F2");
                SplitJerkSentadillaResult.Text = splitJerkSentadilla.ToString("F2");
                SplitJerkAmbosResult.Text = splitJerkAmbos.ToString("F2");

                // Calcular Confianza y Debilidad Probable
                double diferencia = splitJerk - splitJerkAmbos;
                double umbral = splitJerkAmbos * 0.15;

                if (diferencia > umbral)
                {
                    // Si el split jerk real es significativamente mayor que el pronosticado
                    ConfianzaResult.Text = "Alta";

                    // Evaluar debilidad probable
                    if (splitJerkPressSC > splitJerkSentadilla)
                    {
                        DebilidadProbableResult.Text = "Fuerza en Sentadillas";
                    }
                    else if (pressSobreCabeza < sentadilla && pressSobreCabeza < splitJerk)
                    {
                        DebilidadProbableResult.Text = "Fuerza en Press de Hombros";
                    }
                    else
                    {
                        DebilidadProbableResult.Text = "Ninguna";
                    }
                }
                else if (Math.Abs(diferencia) <= umbral)
                {
                    // Si el split jerk real está dentro del umbral (±15%)
                    ConfianzaResult.Text = "Alta";

                    if (splitJerkPressSC > splitJerkSentadilla)
                    {
                        DebilidadProbableResult.Text = "Fuerza en Sentadillas";
                    }
                    else if (pressSobreCabeza < sentadilla && pressSobreCabeza < splitJerk)
                    {
                        DebilidadProbableResult.Text = "Fuerza en Press de Hombros";
                    }
                    else
                    {
                        DebilidadProbableResult.Text = "Ninguna";
                    }
                }
                else
                {
                    // Si el split jerk real es significativamente menor que el pronosticado
                    ConfianzaResult.Text = "Baja";
                    DebilidadProbableResult.Text = "Velocidad/Habilidad/Técnica";
                }
            }
            else
            {
                // Si algún campo está vacío o es incorrecto, se limpian los resultados
                SplitJerkPressSCResult.Text = "";
                SplitJerkSentadillaResult.Text = "";
                SplitJerkAmbosResult.Text = "";
                ConfianzaResult.Text = "";
                DebilidadProbableResult.Text = "";
            }
        }
    }
}