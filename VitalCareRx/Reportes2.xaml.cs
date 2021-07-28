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
    /// Interaction logic for Reportes2.xaml
    /// </summary>
    public partial class Reportes2 : Window
    {
        Validaciones Validaciones = new Validaciones();
        public Reportes2()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Reportes2_Loaded);

        }

        private void Reportes2_Loaded(object sender, RoutedEventArgs e)
        {
            this.Report.ReportPath = String.Format(@"{0}\Omega Soft Evolution\Vital Care Rx\Reportes\ControlPlanilla.rdl", Validaciones.ruta);
           
            this.Report.RefreshReport();
        }
    }
}
