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
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : Window
    {

        SqlConnection sqlConnection;
        private Consulta consulta = new Consulta();
        private int codigoEmpleado;

        public Consultas(int codigo)
        {
            InitializeComponent();

            //Variables miembro

            codigoEmpleado = codigo;

            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            MostrarConsultas();
            CargarCodigoCita();

        }

        public void MostrarConsultas()
        {
            string query = @"SELECT CO.idConsulta 'Codigo de consulta',CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido)
                            Paciente, CO.motivoConsulta Motivo, CO.diagnosticoConsulta Diagnostico, CO.temperatura Temperatura, CO.presionArterial AS 'Presion arterial',
                            CONCAT(E.primerNombre, ' ', E.segundoNombre, ' ', E.primerApellido, ' ', E.segundoApellido) Empleado, CO.idCita 'Codigo de cita'
                            FROM[Consultas].[Consulta] CO INNER JOIN[Consultas].[Cita] C
                            ON CO.idCita = C.idCita
                            INNER JOIN[Personas].[Paciente] P
                            ON C.numeroIdentidad = P.numeroIdentidad
                            INNER JOIN[Personas].[Empleado] E
                            ON CO.idEmpleado = e.idEmpleado
                            ";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    dgConsultas.ItemsSource = dataTable.DefaultView;

                    dgConsultas.IsReadOnly = true;


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            
            if (ValidarCampos())
            {
                
                try
                {
                    ObtenerValores();

                    consulta.CrearConsulta(consulta);

                    LimpiarFormulario();

                }
                catch (Exception)
                {

                    MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

            
        }

        /// <summary>
        /// Se encarga de validar que NO se dejen vacios los campos del formulario
        /// </summary>
        /// <returns></returns>
        private bool ValidarCampos()
        {
            if(rtxtMotivoConsulta.Selection.ToString() != string.Empty && rtxtDiagnostico.Selection.ToString() != string.Empty 
                && txtTemperatura.Text != string.Empty && txtPresionArterial.Text != string.Empty && cmbCodigoCitas.SelectedValue != null) 
            {
                return true;
            }

            return false;
            
        }

        /// <summary>
        /// Metodo que se encarga de la obtencion de valores
        /// </summary>
        private void ObtenerValores()
        {
            consulta.MotivoConsulta = rtxtMotivoConsulta.Selection.ToString();
            consulta.DiagnosticoConsulta = rtxtDiagnostico.Selection.ToString();
            consulta.Temperatura = float.Parse(txtTemperatura.Text);
            consulta.PresionArterial = txtPresionArterial.Text;
            consulta.IdEmpleado = codigoEmpleado;
            consulta.IdCita = Convert.ToInt32(cmbCodigoCitas.SelectedValue);

        }

        /// <summary>
        /// Carga los codigos de las citas del dia de hoy.
        /// </summary>
        private void CargarCodigoCita()
        {
            string query = @"SELECT idCita FROM [Consultas].[Cita]";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            using (sqlDataAdapter)
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                cmbCodigoCitas.DisplayMemberPath = "idCita";
                cmbCodigoCitas.SelectedValuePath = "idCita";
                cmbCodigoCitas.ItemsSource = dataTable.DefaultView;
                ;
            }
        }

        private void LimpiarFormulario()
        {
            //rtxtDiagnostico.Selection.ToString() = string.Empty;
            //rtxtMotivoConsulta.Selection.ToString() = string.Empty;
            txtTemperatura.Text = string.Empty;
            txtPresionArterial.Text = string.Empty;
            cmbCodigoCitas.SelectedValue = null;
        }
    }
}
