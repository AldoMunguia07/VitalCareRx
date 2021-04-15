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

        public MiUsuario(int empleado)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            codigoEmpleado = empleado;            
            CargarTextBox();
            LlenarComboBoxSexo();
            usuario = txtUsuario.Text;
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

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            ObtenerDatos();
            NameEmpleado = String.Format("{0} {1}", miUsuario.PrimerNombre, miUsuario.PrimerApellido);
            MenuPrincipal menu = new MenuPrincipal(NameEmpleado, codigoEmpleado);
            menu.Show();
            this.Close();
        }

        private bool Validar()
        {
            if(txtPrimerNombre.Text != String.Empty && txtSegundoNombre.Text != String.Empty && txtPrimerApellido.Text !=
                String.Empty && txtSegundoApellido.Text != String.Empty && txtCelular.Text != String.Empty && txtUsuario.Text !=
                String.Empty && txtPassword.Text != String.Empty && cmbSexo.SelectedValue != null)
            {
                return true;
            }
            return false;
        }


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



                    if (dataTable.Rows.Count == 1)
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
            if (Validar())
            {
                if(!ExisteUsuario() || usuario == txtUsuario.Text)
                {

                    if (txtCelular.Text.Length == 8)
                    {
                        if (txtPassword.Text.Length >= 8)
                        {
                            try
                            {
                                ObtenerDatos();
                                miUsuario.ModificarEmpleado(miUsuario);
                                MessageBox.Show("¡Datos actualizados correctamente!", "USUARIO", MessageBoxButton.OK, MessageBoxImage.Information);
                                usuario = txtUsuario.Text;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificación... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            }
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
                    MessageBox.Show("¡El nombre de usuario ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                

            }
            else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
