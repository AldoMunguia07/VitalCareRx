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
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace VitalCareRx
{
    /// <summary>
    /// Interaction logic for NuevoEmpleado.xaml
    /// </summary>
    public partial class NuevoEmpleado : Window
    {
        //variables miembro
        private Empleado empleado = new Empleado();
        SqlConnection sqlConnection;
      
        public NuevoEmpleado()
        {

            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            LlenarComboBoxSexo();
            
        }


        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validación para que no deje los campos vacios
                if (txtPrimerNombre.Text != String.Empty && txtPrimerApellido.Text !=
                  String.Empty && txtCelular.Text != String.Empty && txtUsuario.Text !=
                  String.Empty && txtContrasenia.Text != String.Empty && cmbSexo.SelectedValue != null)
                {
                    if (!ExisteUsuario()) // Si el usuario ya existe que no perimita pasar a la siguiente condición
                    {
                        if (txtUsuario.Text.Length >= 5)
                        {
                            if (txtCelular.Text.Length == 8) // el campo celular debe tener 8 caracteres
                            {
                                if (txtContrasenia.Text.Length >= 8) // el campo contraseña debe tener 8 o más caracteres
                                {

                                    ObtenerValores();
                                    empleado.CrearNuevoEmpleado(empleado); //Insertar empleado
                                    empleado.IdEmpleado = empleado.CodigoEmpleado();
                                    LimpiarFormulario();
                                    Loading loading = new Loading(String.Format("{0} {1}", empleado.PrimerNombre, empleado.PrimerApellido), empleado.IdEmpleado);
                                    loading.Show();
                                    this.Close();

                                }
                                else
                                {
                                    MessageBox.Show("¡La contraseña debe contener almenos 8 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }

                            }
                            else
                            {
                                MessageBox.Show("¡El numero de celular debe contener 8 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("¡El nombre de usuario debe contener almenos 5 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("¡El nombre de usuario ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
                else
                {
                    MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        
        // Al cerrar el formulario que regrese a el Login.
        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        /// <summary>
        /// metodo para obtener valores del formulario y seguidamente insertarlos en la base de datos
        /// </summary>
        private void ObtenerValores()
        {
            empleado.PrimerNombre = txtPrimerNombre.Text;
            empleado.SegundoNombre = txtSegundoNombre.Text;
            empleado.PrimerApellido = txtPrimerApellido.Text;
            empleado.SegundoApellido = txtSegundoApellido.Text;
            empleado.Celular = txtCelular.Text;
            empleado.IdSexo = Convert.ToInt32(cmbSexo.SelectedValue);
            empleado.NombreUsuario = txtUsuario.Text;
            empleado.Contrasenia = txtContrasenia.Text;
        }


        /// <summary>
        /// Obtener los datos de la tabla Sexo.
        /// </summary>
        public void LlenarComboBoxSexo()
        {

            // Query de selección
            string query = @"SELECT * FROM [Personas].[Sexo]";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            using (sqlDataAdapter)
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                cmbSexo.DisplayMemberPath = "descripcionSexo";
                cmbSexo.SelectedValuePath = "idSexo";
                cmbSexo.ItemsSource = dataTable.DefaultView;
            }



        }


        /// <summary>
        /// método para limpiar el formulario.
        /// </summary>
        private void LimpiarFormulario()
        {
            txtCelular.Clear();
            txtContrasenia.Clear();
            txtPrimerApellido.Clear();
            txtPrimerNombre.Clear();
            txtSegundoApellido.Clear();
            txtSegundoNombre.Clear();
            txtUsuario.Clear();
            cmbSexo.SelectedValue = null;
        }


        /// <summary>
        /// Metodo que verifica si el usuario ya existe.
        /// </summary>
        /// <returns>boolean</returns>
        public bool ExisteUsuario()
        {
            try
            {

                string query = @"SELECT nombreUsuario FROM [Personas].[Empleado] WHERE [nombreUsuario] = @user";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@user", txtUsuario.Text); //Se pasa por parametro la txtUsuario

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);



                    if (dataTable.Rows.Count == 1) //Si devuelve q fila es que existe un usuarui
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                throw;
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

        private void txtPrimerNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                    || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
         
        }

        private void txtPrimerNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtCelular_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (caracter >= 48 && caracter <= 57) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void txtCelular_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtSegundoNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                    || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }

        private void txtSegundoNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtPrimerApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                    || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtPrimerApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtSegundoApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                    || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSegundoApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        
        private void txtUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }
    }
}
