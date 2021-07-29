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
    /// Interaction logic for ReportesUsuario.xaml
    /// </summary>
    public partial class ReportesUsuario : Window
    {
        Empleado miEmpleado = new Empleado();
        public ReportesUsuario(Empleado empleado)
        {
            InitializeComponent();

            miEmpleado = empleado;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea regresar al menú principal?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (miEmpleado.IdPuesto == 1)
                {
                    MenuPrincipalAdmin menuPrincipalAdmin = new MenuPrincipalAdmin(miEmpleado);
                    menuPrincipalAdmin.Show();
                    this.Close();
                }
                else if (miEmpleado.IdPuesto == 2)
                {
                    MenuPrincipal menupincipal = new MenuPrincipal(miEmpleado);
                    menupincipal.Show();
                    this.Close();
                }
            }
        }

        private void btnPacientesReporte_Click(object sender, RoutedEventArgs e)
        {
            ReportePaciente reportePaciente = new ReportePaciente();
            reportePaciente.ShowDialog();
        }

        private void btnConsultasReporte_Click(object sender, RoutedEventArgs e)
        {
            ReportesConsultas reportesConsultas = new ReportesConsultas();
            reportesConsultas.ShowDialog();
        }

        private void btnFarmacosReporte_Click(object sender, RoutedEventArgs e)
        {
            ReportesFarmaco reportesFarmaco = new ReportesFarmaco();
            reportesFarmaco.ShowDialog();
        }
    }
}
