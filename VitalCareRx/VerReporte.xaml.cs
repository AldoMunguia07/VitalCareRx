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
            this.Reporte.ReportPath = String.Format(@"{0}\Omega Soft Evolution\Vital Care Rx\Reportes\{1}", validaciones.ruta,reporte);
            this.Reporte.RefreshReport();
        }

        bool right = false;
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!right)
            {
                DragMove();
            }
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            right = true;
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            right = false;
        }
    }
}
