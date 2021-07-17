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
using System.Windows.Threading;

namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipalAdmin.xaml
    /// </summary>
    public partial class MenuPrincipalAdmin : Window
    {

        Empleado miEmpleado = new Empleado();
        public MenuPrincipalAdmin(Empleado empleado)
        {
            InitializeComponent();
            miEmpleado = empleado;
            lbUsuario.Content = String.Format("{0} {1}", miEmpleado.PrimerNombre, miEmpleado.PrimerApellido);

            DispatcherTimer LiveTime = new DispatcherTimer(); //Instanciación de objeto de tipo DispatcherTimer.
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
            lbFecha.Content = DateTime.Now.ToLongTimeString(); //Inicializa la fecha y hora
            lbDate.Content = DateTime.Now.ToShortDateString(); //Inicializa la fecha
        }
        void timer_Tick(object sender, EventArgs e) //Evento para mostrar la fecha y hora en tiempo real
        {

            lbFecha.Content = DateTime.Now.ToLongTimeString();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("¿Desea cerrar sesión?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
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

        //cuando se mantiene presionado click derecho
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            right = true;
        }

        //cuando se suelta el click derecho
        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            right = false;
        }

        //Valores de listViewItem (Opciones de menu principal)
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int index = ListViewMenu.SelectedIndex;
            if (index == 9)
            {
                ListViewMenu.SelectedIndex = 0;
                index = ListViewMenu.SelectedIndex;

            }
            else
            {
                MoveCursorMenu(index);
            }


        }

        //Efecto de "seleccion" en determinado indíce de la ListView (Opciones de menu principal)
        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        //Opcion de pacientes
        private void ltPacientes_Selected(object sender, RoutedEventArgs e)
        {
            Pacientes pacientes = new Pacientes(miEmpleado);
            pacientes.Show();
            this.Close();
        }

        //Opcion de consultas
        private void ltConsultas_Selected(object sender, RoutedEventArgs e)
        {
            Consultas consultas = new Consultas(miEmpleado);
            consultas.Show();
            this.Close();
        }

        //Opcion de farmacos
        private void ltFarmacos_Selected(object sender, RoutedEventArgs e)
        {
            Farmacos farmacos = new Farmacos(miEmpleado);
            farmacos.Show();
            this.Close();
        }

        //Opcion de "Mi usuario"
        private void ltUsuario_Selected(object sender, RoutedEventArgs e)
        {
            MiUsuario miusuario = new MiUsuario(miEmpleado);
            miusuario.Show();
            this.Close();
        }

        //Opcion de salir
        private void ltSalida_Selected(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea cerrar sesión?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }



        }

        //Opcion de informacion del sistema
        private void ltInformacion_Selected(object sender, RoutedEventArgs e)
        {
            Informacion informacion = new Informacion(miEmpleado);
            informacion.Show();
            this.Close();
        }

        private void ltEmpleados_Selected(object sender, RoutedEventArgs e)
        {
            Empleados empleados = new Empleados(miEmpleado);
            empleados.Show();
            this.Close();
        }

        private void ltBitacora_Selected(object sender, RoutedEventArgs e)
        {
            Bitacora bitacora = new Bitacora(miEmpleado);
            bitacora.Show();
            this.Close();
        }

        private void ltControlEmpleado_Selected(object sender, RoutedEventArgs e)
        {
            ControlEmpleado controlempleado = new ControlEmpleado(miEmpleado);
            controlempleado.Show();
            this.Close();
        }
    }
}
