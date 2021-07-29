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
    /// Interaction logic for ReportesEmpleados.xaml
    /// </summary>
    public partial class ReportesEmpleados : Window
    {
        public ReportesEmpleados()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lt_Selected(object sender, RoutedEventArgs e)
        {
            VerReporte verReporte = new VerReporte("ControlPlanilla.rdl");
            verReporte.ShowDialog();
        }

        private void ltAccionPorEmpleado_Selected(object sender, RoutedEventArgs e)
        {
            VerReporte verReporte = new VerReporte("AccionesXEmpleado.rdl");
            verReporte.ShowDialog();
        }

        private void ltFarmacoXEmpleado_Selected(object sender, RoutedEventArgs e)
        {
            VerReporte verReporte = new VerReporte("FarmacosXEmpleado.rdl");
            verReporte.ShowDialog();
        }
    }
}
