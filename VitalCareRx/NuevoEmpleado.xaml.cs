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
            
            if (txtPrimerNombre.Text != String.Empty && txtSegundoNombre.Text != String.Empty && txtPrimerApellido.Text != 
                String.Empty && txtSegundoApellido.Text != String.Empty && txtCelular.Text != String.Empty && txtUsuario.Text !=
                String.Empty && txtContrasenia.Text != String.Empty && cmbSexo.SelectedValue != null)
            {
                try
                {
                    ObtenerValores();
                    empleado.CrearNuevoEmpleado(empleado);
                    LimpiarFormulario();
                    Loading loading = new Loading(String.Format("{0} {1}", empleado.PrimerNombre, empleado.SegundoNombre));
                    loading.Show();
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
         

        }
        
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


    }
}
