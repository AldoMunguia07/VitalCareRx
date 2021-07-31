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
    /// Interaction logic for ControlEmpleado.xaml
    /// </summary>
    
   
    public partial class ControlEmpleado : Window
    {
        Empleado miEmpleado = new Empleado();
        LlenarComboBox llenarComboBox = new LlenarComboBox();
        AportesControl aportesControl = new AportesControl();
        bool bandera = true;
        public ControlEmpleado( Empleado empleado)
        {
            InitializeComponent();
            miEmpleado = empleado;
            llenarComboBox.CargarAnios(cmbAnio);
            llenarComboBox.CargarMes(cmbMes);
            llenarComboBox.CargarEmpleado(cmbEmpleado);
            aportesControl.MostrarControlEmpleados(gridControlEmpleado);
            
        }

        private void cmbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAnio.SelectedValue == null && cmbMes.SelectedValue == null && bandera)
            {

                aportesControl.MostrarControlPorEmpleados(gridControlEmpleado, cmbEmpleado);
            }
            else 
            {
                aportesControl.MostrarControlEmpleadoTodos(gridControlEmpleado,cmbAnio,cmbMes,cmbEmpleado);
            }
        }

        private void btnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            bandera = false;
            cmbEmpleado.SelectedValue = null;
            cmbAnio.SelectedValue = null;
            cmbMes.SelectedValue = null;
            aportesControl.MostrarControlEmpleados(gridControlEmpleado);
            bandera = true;
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

        private void cmbAnio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEmpleado.SelectedValue == null && cmbMes.SelectedValue != null)
            {
                aportesControl.MostrarControlEmpleadoFiltro(gridControlEmpleado, cmbEmpleado, cmbAnio, cmbMes);
            }
            else if (cmbMes.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el mes", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                aportesControl.MostrarControlEmpleadoTodos(gridControlEmpleado,cmbAnio,cmbMes,cmbEmpleado);
            }
            
        }

        private void cmbMes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEmpleado.SelectedValue == null && cmbAnio.SelectedValue != null)
            {
                aportesControl.MostrarControlEmpleadoFiltro(gridControlEmpleado, cmbEmpleado, cmbAnio, cmbMes);
            }
            else if (cmbAnio.SelectedValue == null && bandera)
            {
                MessageBox.Show("Seleccione el año", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                aportesControl.MostrarControlEmpleadoTodos(gridControlEmpleado, cmbAnio, cmbMes, cmbEmpleado);
            }
        }

        bool right = false;
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Si se le da click derecho que no permita mover la ventana
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
