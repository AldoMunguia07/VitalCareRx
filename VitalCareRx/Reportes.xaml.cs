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
    /// Interaction logic for Reportes.xaml
    /// </summary>
    public partial class Reportes : Window
    {
        Empleado miEmpleado = new Empleado();
        public Reportes(Empleado empleado)
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

        private void btnEmpleadosReporte_Click(object sender, RoutedEventArgs e)
        {
            ReportesEmpleados reportesEmpleados = new ReportesEmpleados();
            reportesEmpleados.ShowDialog();
        }

        private void btnFarmacosReporte_Click(object sender, RoutedEventArgs e)
        {
            ReportesFarmaco reportesFarmaco = new ReportesFarmaco();
            reportesFarmaco.ShowDialog();
        }

        private void btnConsultasReporte_Click(object sender, RoutedEventArgs e)
        {
            ReportesConsultas reportesConsultas = new ReportesConsultas();
            reportesConsultas.ShowDialog();
        }

        bool right = false;
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Si se le da click derecho que no permita mover la ventana

            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

     
    }
}
