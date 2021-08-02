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

namespace VitalCareRx
{
    /// <summary>
    /// Interaction logic for VerReporte.xaml
    /// </summary>
    public partial class VerReporte : Window
    {
        Validaciones validaciones = new Validaciones();
        public string reporte;
        public VerReporte(string nombreReporte)
        {
            InitializeComponent();
            reporte = nombreReporte;
            this.Loaded += new RoutedEventHandler(VerReporte_Loaded);
        }

        private void VerReporte_Loaded(object sender, RoutedEventArgs e)
        {
            this.Reporte.ReportPath = String.Format(@"{0}\Reportes\{1}", validaciones.ruta,reporte);
            this.Reporte.RefreshReport();
        }

      
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

   
    }
}
