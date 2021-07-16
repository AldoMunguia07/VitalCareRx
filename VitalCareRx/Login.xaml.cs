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
            // Validar que txtPassword sea distinto de vacio para que el psbPassword tome su valor
            if (txtPassword.Text != String.Empty)
            {
                pwbPassword.Password = txtPassword.Text;
            }
            
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


                        Loading loading = new Loading(String.Format("{0} {1}", unEmpleado.PrimerNombre, unEmpleado.PrimerApellido), unEmpleado.IdEmpleado, unEmpleado.IdPuesto);
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
            MessageBoxResult result = MessageBox.Show("¿Desea cerrar el programa?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
            
        }

        
      

        

        //Proceso para ocultar y mostrar la contraseña
        private int i = 0; // variable miembro que sirve como bandera
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

        private void btnRecuperarPass_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
