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
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {

        private int codigoEmpleado;
        private string nombreEmpleado;
        

        public MenuPrincipal(string usuario, int codigo)
        {
            InitializeComponent();

            codigoEmpleado = codigo;
            nombreEmpleado = usuario;
            lbUsuario.Content = usuario;
           
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
            lbDate.Content = DateTime.Now.ToShortDateString();

        }

        void timer_Tick(object sender, EventArgs e)
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

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int index = ListViewMenu.SelectedIndex;
            if (index == 6)
            {
                ListViewMenu.SelectedIndex = 0;
                index = ListViewMenu.SelectedIndex;
                
            }
            else
            {   
                MoveCursorMenu(index);
            }


        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        private void ltPacientes_Selected(object sender, RoutedEventArgs e)
        {
            Pacientes pacientes = new Pacientes(codigoEmpleado, nombreEmpleado);
            pacientes.Show();
            this.Close();
        }

        private void ltConsultas_Selected(object sender, RoutedEventArgs e)
        {
            Consultas consultas = new Consultas(codigoEmpleado, nombreEmpleado);
            consultas.Show();
            this.Close();
        }

        private void ltFarmacos_Selected(object sender, RoutedEventArgs e)
        {
            Farmacos farmacos= new Farmacos(codigoEmpleado, nombreEmpleado);
            farmacos.Show();
            this.Close();
        }

        private void ltUsuario_Selected(object sender, RoutedEventArgs e)
        {
            MiUsuario miusuario = new MiUsuario(codigoEmpleado);
            miusuario.Show();
            this.Close();
        }

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

        private void ltInformacion_Selected(object sender, RoutedEventArgs e)
        {
           Informacion informacion= new Informacion(codigoEmpleado, nombreEmpleado);
           informacion.Show();
           this.Close();
        }
    }
}
