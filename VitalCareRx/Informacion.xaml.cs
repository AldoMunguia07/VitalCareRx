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
    /// Lógica de interacción para Informacion.xaml
    /// </summary>
    public partial class Informacion : Window
    {
       
        Empleado miEmpleado = new Empleado();
        public Informacion(Empleado empleado)// se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
                                                                      //y nombre del empleado(Se usa para volver al menu principal).
        {
            InitializeComponent();
            miEmpleado = empleado;
    }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
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
    }
}
