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

// Agregar los namespaces requeridos
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para MiUsuario.xaml
    /// </summary>
    public partial class MiUsuario : Window
    {
        SqlConnection sqlConnection;
        private int codigoEmpleado;
        private string NameEmpleado;

        Empleado miUsuario = new Empleado();
        private string usuario;
        Validaciones validaciones = new Validaciones();

        public MiUsuario(int empleado)// se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
                                      
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            codigoEmpleado = empleado;            
            CargarTextBox();
            LlenarComboBoxSexo();
            miUsuario.PrimerNombre = txtPrimerNombre.Text;
            miUsuario.PrimerApellido = txtPrimerApellido.Text;
           usuario = txtUsuario.Text; // se asigna el valor de la txtUsuario para la posteiror validacion (Que no permita ingresar un usuario existente).
        }

        /// <summary>
        /// Cargar las TextBox con la información del empleado actual.
        /// </summary>
        private void CargarTextBox()
        {
            try                
            {
                //Query para mostrar informacion del empleado
                string query = @"SELECT * FROM [Personas].[Empleado] WHERE idEmpleado = @idEmpleado";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@idEmpleado", codigoEmpleado);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable Empleado = new DataTable();
                    sqlDataAdapter.Fill(Empleado);

                    //Del objeto DataTable se le asigna el valor correspondiente a cada TextBox.
                    txtUsuario.Text = Empleado.Rows[0]["nombreUsuario"].ToString();
                    txtPassword.Text = Empleado.Rows[0]["contrasenia"].ToString();
                    txtPrimerNombre.Text = Empleado.Rows[0]["primerNombre"].ToString();
                    txtSegundoNombre.Text = Empleado.Rows[0]["segundoNombre"].ToString();
                    txtPrimerApellido.Text = Empleado.Rows[0]["primerApellido"].ToString();
                    txtSegundoApellido.Text = Empleado.Rows[0]["segundoApellido"].ToString();
                    txtCelular.Text = Empleado.Rows[0]["celular"].ToString();
                    cmbSexo.SelectedValue = Empleado.Rows[0]["idSexo"].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
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
        /// Metodo para obtener valores del formulario
        /// </summary>
        private void ObtenerDatos()
        {
            miUsuario.IdEmpleado = codigoEmpleado;
            miUsuario.PrimerNombre = txtPrimerNombre.Text;
            miUsuario.SegundoNombre = txtSegundoNombre.Text;
            miUsuario.PrimerApellido = txtPrimerApellido.Text;
            miUsuario.SegundoApellido = txtSegundoApellido.Text;
            miUsuario.Celular = txtCelular.Text;
            miUsuario.IdSexo = Convert.ToInt32(cmbSexo.SelectedValue);
            miUsuario.NombreUsuario = txtUsuario.Text;
            miUsuario.Contrasenia = txtPassword.Text;
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

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            
            NameEmpleado = String.Format("{0} {1}", miUsuario.PrimerNombre, miUsuario.PrimerApellido); // Se pasa por parametro el nuevo nombre de usuario actualizado o se queda con el actual en caso de
            MenuPrincipal menu = new MenuPrincipal(NameEmpleado, codigoEmpleado);                      // que no los haya actualizado
            menu.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo para validar que no se dejen campos en blancos.
        /// </summary>
        /// <returns>Boolean</returns>
        private bool Validar()
        {
            if(txtPrimerNombre.Text != String.Empty &&  txtPrimerApellido.Text !=
                String.Empty && txtCelular.Text != String.Empty && txtUsuario.Text !=
                String.Empty && txtPassword.Text != String.Empty && cmbSexo.SelectedValue != null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Metodo para validar si existe o no un usuario.
        /// </summary>
        /// <returns></returns>
        public bool ExisteUsuario()
        {
            try
            {

                string query = @"SELECT nombreUsuario FROM [Personas].[Empleado] WHERE [nombreUsuario] = @user";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@user", txtUsuario.Text);

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);



                    if (dataTable.Rows.Count == 1) //Si el usuario existe retorna un true
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





        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validar()) //El usuario no debe dejar los campos en blanco.
                {
                    if (!ExisteUsuario() || usuario == txtUsuario.Text) // Valida si el nombre usuario no existe y si es el nombre de usurio es del empleado actual, con esa condición pasa a la siguiente validación
                    {
                        if (txtUsuario.Text.Length >= 5)
                        {
                            if (txtCelular.Text.Length == 8) // El numero de telefono debe contener 8 dígitos.
                            {
                                if (txtPassword.Text.Length >= 8) // el campo contraseña debe tener 8 o más caracteres
                                {

                                    ObtenerDatos();
                                    miUsuario.ModificarEmpleado(miUsuario);
                                    MessageBox.Show("¡Datos actualizados correctamente!", "USUARIO", MessageBoxButton.OK, MessageBoxImage.Information);
                                    usuario = txtUsuario.Text;

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
                        txtUsuario.Text = usuario;
                    }


                }
                else
                {
                    MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificación... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            
        }

        private void txtPrimerNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
          
        }

        private void txtPrimerNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtSegundoNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtSegundoNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtPrimerApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtPrimerApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtSegundoApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtSegundoApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtCelular_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.SoloNumeros(e);
          
        }

        private void txtCelular_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }
    }
}
