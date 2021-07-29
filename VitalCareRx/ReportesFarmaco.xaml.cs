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
    /// Interaction logic for ReportesFarmaco.xaml
    /// </summary>
    public partial class ReportesFarmaco : Window
    {
        public ReportesFarmaco()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ltProductoVencimiento_Selected(object sender, RoutedEventArgs e)
        {
            VerReporte verReporte = new VerReporte("ProductoXVencimiento.rdl");
            verReporte.ShowDialog();
        }

        private void ltFarmacosIngresados_Selected(object sender, RoutedEventArgs e)
        {
       
        }
    }
}
