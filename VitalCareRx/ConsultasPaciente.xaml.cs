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
    /// Lógica de interacción para ConsultasPaciente.xaml
    /// </summary>
    public partial class ConsultasPaciente : Window
    {
        private int codigoConsulta;
        private string paciente;
        SqlConnection sqlConnection;
        private bool seleccionado = false;
        public ConsultasPaciente(string codigoPaciente)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            paciente = codigoPaciente;
            CargarCitasPaciente();
        }

        /// <summary>
        /// Carga el data grid con las citas del paciente seleccionado.
        /// </summary>
        private void CargarCitasPaciente()
        {
            string query = @"SELECT CO.idConsulta 'Codigo de consulta', CO.motivoConsulta Motico, CO.diagnosticoConsulta Diagnostico, CO.temperatura Temperatura,
                            CO.presionArterial AS 'Presion arterial',CONCAT(E.primerNombre, ' ', E.segundoNombre, ' ', E.primerApellido, ' ', E.segundoApellido) Empleado, 
                            CO.idCita 'Codigo de cita'
                            FROM [Consultas].[Consulta] CO INNER JOIN [Consultas].[Cita] C
                            ON CO.idCita = C.idCita
                            INNER JOIN [Personas].[Paciente] P
                            ON C.numeroIdentidad = P.numeroIdentidad
                            INNER JOIN [Personas].[Empleado] E
                            ON CO.idEmpleado = e.idEmpleado
                            WHERE P.numeroIdentidad = @numeroIdentidad";

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlCommand.Parameters.AddWithValue("@numeroIdentidad", paciente);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    gridConsultas.ItemsSource = dataTable.DefaultView;
                    gridConsultas.IsReadOnly = true;


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void gridConsultas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;

            if (rowSelected != null)
            {
                seleccionado = true;
                codigoConsulta = Convert.ToInt32(rowSelected.Row["Codigo de consulta"]);
            }
            else
            {
                seleccionado = false;
            }
        }

        private void btnRecetas_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                RecetasConsultaPaciente recetasConsultaPaciente = new RecetasConsultaPaciente(codigoConsulta);
                recetasConsultaPaciente.ShowDialog();
            }
            else
            {
                MessageBox.Show("¡Para ver una receta debe seleccionar una consulta!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        

        private void gridConsultas_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.Double))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "N2";
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
    }
}
