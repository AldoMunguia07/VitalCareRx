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
        public ControlEmpleado( Empleado empleado)
        {
            InitializeComponent();
        }

        private void cmbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtFechaBusqueda_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnRestaurar_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
