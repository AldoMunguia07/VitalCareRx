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
        private string nombreEmpleado;


        public Consultas(int codigo, string empleado)
        {
            InitializeComponent();

            //Variables miembro

            codigoEmpleado = codigo;
            nombreEmpleado = empleado;
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            MostrarConsultas();
            CargarCodigoCita();

        }

        /// <summary>
        /// Trae todas las consultas de la base de datos al inicial el programa.
        /// </summary>
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

                    MessageBox.Show("La consulta se ha insertado con exito","CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

                    MostrarConsultas();

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
            TextRange MotivoConsulta = new TextRange(rtxtMotivoConsulta.Document.ContentStart, rtxtMotivoConsulta.Document.ContentEnd);
            TextRange DiagnosticoConsulta = new TextRange(rtxtDiagnostico.Document.ContentStart, rtxtDiagnostico.Document.ContentEnd);

            if (MotivoConsulta.Text!= "\r\n" && DiagnosticoConsulta.Text != "\r\n"
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
            TextRange MotivoConsulta = new TextRange(rtxtMotivoConsulta.Document.ContentStart, rtxtMotivoConsulta.Document.ContentEnd);
            TextRange DiagnosticoConsulta = new TextRange(rtxtDiagnostico.Document.ContentStart, rtxtDiagnostico.Document.ContentEnd);

            consulta.MotivoConsulta = MotivoConsulta.Text;
            consulta.DiagnosticoConsulta = DiagnosticoConsulta.Text;
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

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {

            rtxtMotivoConsulta.Document.Blocks.Clear();
            rtxtDiagnostico.Document.Blocks.Clear();
            txtTemperatura.Text = string.Empty;
            txtPresionArterial.Text = string.Empty;
            cmbCodigoCitas.SelectedValue = null;
        }


        /// <summary>
        /// Se encarga de traer todos los valores del data grid a los campos del formulario al darle click a una fila.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgConsultas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;

            if (rowSelected != null)
            {
                TextRange MotivoConsulta = new TextRange(rtxtMotivoConsulta.Document.ContentStart, rtxtMotivoConsulta.Document.ContentEnd);
                TextRange DiagnosticoConsulta = new TextRange(rtxtDiagnostico.Document.ContentStart, rtxtDiagnostico.Document.ContentEnd);

                MotivoConsulta.Text = rowSelected.Row["Motivo"].ToString();
                DiagnosticoConsulta.Text = rowSelected.Row["Diagnostico"].ToString();

                txtTemperatura.Text = rowSelected.Row["Temperatura"].ToString();
                txtPresionArterial.Text = rowSelected.Row["Presion arterial"].ToString();
                cmbCodigoCitas.SelectedValue = rowSelected.Row["Codigo de cita"].ToString();

                consulta.IdConsulta = Convert.ToInt32(rowSelected.Row["Codigo de consulta"]);
            }

            

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {

                try
                {
                    ObtenerValores();

                    consulta.ModificarConsulta(consulta);

                    LimpiarFormulario();

                    MessageBox.Show("La consulta se ha modificado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

                    MostrarConsultas();



                }
                catch (Exception)
                {


                    MessageBox.Show("Ha ocurrido un error al momento de realizar la modificacion... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
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
                            WHERE CONCAT(P.primerNombre, ' ', P.segundoApellido, ' ', P.primerApellido, ' ', P.segundoApellido) LIKE CONCAT('%', @nombrePaciente,'%')";


            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            using (sqlDataAdapter)
            {
                sqlCommand.Parameters.AddWithValue("@nombrePaciente", txtBuscar.Text);

                DataTable dataTable = new DataTable();

                sqlDataAdapter.Fill(dataTable);

                dgConsultas.ItemsSource = dataTable.DefaultView;

                dgConsultas.IsReadOnly = true;

            }
        }

        private void btnReceta_Click(object sender, RoutedEventArgs e)
        {
            
           // Recetas recetas = new Recetas(int idConsulta);
          //  recetas.show();
          //  this.Close();
            
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal(nombreEmpleado, codigoEmpleado);
            menu.Show();
            this.Close();
        }
    }
}
