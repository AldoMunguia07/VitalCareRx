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
        private int IdEmpleado;
        private string nombreCompletoEmpleado;

        public Informacion(int codigoEmpleado,  string nombreEmpleado)// se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
                                                                      //y nombre del empleado(Se usa para volver al menu principal).
        {
            InitializeComponent();
            IdEmpleado = codigoEmpleado;
            nombreCompletoEmpleado = nombreEmpleado;
    }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuprincipal = new MenuPrincipal(nombreCompletoEmpleado, IdEmpleado); // Se regresa al menu principal con los datos del usuario actual.
            menuprincipal.Show();
            this.Close();
            
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
