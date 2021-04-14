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
    /// Lógica de interacción para RecetasConsultaPaciente.xaml
    /// </summary>
    public partial class RecetasConsultaPaciente : Window
    {
        SqlConnection sqlConnection;
        private int consulta;
        public RecetasConsultaPaciente(int codigoConsulta)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            consulta = codigoConsulta;
            CargarRecetasAConsulta();
        }

        /// <summary>
        /// Carga el data grid con las citas del paciente seleccionado.
        /// </summary>
        private void CargarRecetasAConsulta()
        {
            string query = @"SELECT DR.idRecetaMedica 'Codigo de receta', DR.idFarmaco 'Codigo de farmaco', DR.cantidad 'Cantidad',F.descripcionFarmaco 'Farmaco', 
                            DR.duracionTratamiento 'Duracion', DR.indicaciones 'Indicacion'
                            FROM [Consultas].[DetalleRecetaMedica] DR INNER JOIN [Consultas].[Farmaco] F
                            ON DR.idFarmaco = F.idFarmaco
                            INNER JOIN [Consultas].[RecetaMedica] R
                            ON DR.idRecetaMedica = R.idRecetaMedica
                            INNER JOIN [Consultas].[Consulta] C
                            ON R.idConsulta = C.idConsulta
                            WHERE C.idConsulta = @idConsulta";

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlCommand.Parameters.AddWithValue("@idConsulta", consulta);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    gridRecetas.ItemsSource = dataTable.DefaultView;
                    gridRecetas.IsReadOnly = true;


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
