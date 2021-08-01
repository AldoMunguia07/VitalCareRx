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
    /// Lógica de interacción para Bitacora.xaml
    /// </summary>
    public partial class Bitacora : Window
    {

        Empleado miEmpleado = new Empleado();
        LlenarComboBox LlenarComboBox = new LlenarComboBox();
        AportesControl AportesControl = new AportesControl();
        public Bitacora(Empleado empleado)
        {
            InitializeComponent();
            miEmpleado = empleado;
            AportesControl.MostrarBitacora(gridBitacora);
            LlenarComboBox.CargarEmpleado(cmbEmpleado);
           

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

        private void btnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            cmbEmpleado.SelectedValue = null;
            dtFechaAccion.SelectedDate = null;
            AportesControl.MostrarBitacora(gridBitacora);
        }

        private void cmbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtFechaAccion.SelectedDate == null) 
            {
                AportesControl.MostrarBitacoraFiltro(gridBitacora, dtFechaAccion, cmbEmpleado);
            }
            else
            {
                AportesControl.MostrarBitacoraFiltroAmbos(gridBitacora, dtFechaAccion, cmbEmpleado);
            }
            
        }

        private void dtFechaAccion_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEmpleado.SelectedValue == null)
            {
                AportesControl.MostrarBitacoraFiltro(gridBitacora, dtFechaAccion, cmbEmpleado);
            }
            else
            {
                AportesControl.MostrarBitacoraFiltroAmbos(gridBitacora, dtFechaAccion, cmbEmpleado);
            }
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
