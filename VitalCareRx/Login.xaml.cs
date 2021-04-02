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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Empleado empleado = new Empleado();

        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Implementar la búsqueda del usuario desde la clase Usuario
                Empleado unEmpleado = empleado.BuscarEmpleado(txtUsuario.Text);

                // Verificar si el usuario existe
                if (unEmpleado.NombreUsuario == null)
                    MessageBox.Show("El usuario o la contraseña no es correcta. Favor verificar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    //Verificar que la contraseña ingresada es igual a la contraseña
                    // almacenada en la base de datos
                    if (unEmpleado.Contrasenia == pwbPassword.Password)
                    {
                        // Mostrar el formulario de carga


                        Loading loading = new Loading(String.Format("{0} {1}", unEmpleado.PrimerNombre, unEmpleado.PrimerApellido), unEmpleado.IdEmpleado);
                        loading.Show();
                        this.Close();

                        

              
                    }

                    else
                        MessageBox.Show("El usuario o la contraseña no es correcta. Favor verificar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ha ocurrido un error al momento de realizar la consulta... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                               
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
        private void btnNuevaCuenta_Click(object sender, RoutedEventArgs e)
        {
            NuevoEmpleado nuevoEmpleado = new NuevoEmpleado();
            nuevoEmpleado.Show();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        
        private int i = 0;
        private void btnMostrarContrasenia_Click(object sender, RoutedEventArgs e)
        {
            i++;
            if (i == 1)
            {
                txtPassword.Text = pwbPassword.Password;                
                txtPassword.Visibility = Visibility.Visible;
                pwbPassword.Visibility = Visibility.Hidden;
                txtPassword.SelectionStart = txtPassword.Text.Length;
                txtPassword.Focus();
                IconoBoton.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;

            }
            else
            {
                i = 0;
                pwbPassword.Password = txtPassword.Text;
                txtPassword.Visibility = Visibility.Hidden;
                pwbPassword.Visibility = Visibility.Visible;
                IconoBoton.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
            }
        }
    }
}
