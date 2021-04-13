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
    /// Lógica de interacción para CitasPaciente.xaml
    /// </summary>
    public partial class CitasPaciente : Window
    {
        SqlConnection sqlConnection;

        private string codigoPaciente;
        Cita cita = new Cita();
        private bool selecionado = false;
        private int codigoCita;

        public CitasPaciente(string paciente)
        {
            
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            codigoPaciente = paciente;

            CargarCitasPaciente();
        }



        /// <summary>
        /// Carga el data grid con las citas del paciente seleccionado.
        /// </summary>
        private void CargarCitasPaciente()
        {
            string query = @"SELECT idCita 'Codigo de cita', fechaCita 'Fecha de la cita', notas Notas
                            FROM [Consultas].[Cita] WHERE numeroIdentidad = @numeroIdentidad
                            ";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlCommand.Parameters.AddWithValue("@numeroIdentidad", codigoPaciente);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    gridCitas.ItemsSource = dataTable.DefaultView;
                    gridCitas.IsReadOnly = true;


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Limpia las cajas de texto del formulario.
        /// </summary>
        private void LimpiarFormulario()
        {
            richTextBoxNotas.Document.Blocks.Clear();
            dtFecha.Text = String.Empty;
            selecionado = false;
            codigoCita = 0;
        
        }

        /// <summary>
        /// Valida que no deje espacios en blancos.
        /// </summary>
        /// <returns></returns>
        private bool Validar()
        {
            TextRange notas = new TextRange(richTextBoxNotas.Document.ContentStart, richTextBoxNotas.Document.ContentEnd);

            if (dtFecha.Text != string.Empty && notas.Text != "\r\n")
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Obtener los valores del formulario
        /// </summary>
        private void ObtenerValoresCita()
        {
            cita.IdCita = codigoCita;
            TextRange notas = new TextRange(richTextBoxNotas.Document.ContentStart, richTextBoxNotas.Document.ContentEnd);
            cita.NumeroIdentidad = codigoPaciente;
            cita.FechaCita = Convert.ToDateTime(dtFecha.SelectedDate);
            cita.Notas = notas.Text.Substring(0, notas.Text.Length - 2);
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (Validar())
            {
                
                if (!selecionado)
                {
                    try
                    {
                        ObtenerValoresCita();
                        cita.AgregarCita(cita);
                        LimpiarFormulario();
                        CargarCitasPaciente();
                        MessageBox.Show("¡La cita se ha agregado con exito!", "CITA", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("¡No se puede agregar la misma cita!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
                
            }
            else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (Validar())
            {
                if (selecionado)
                {
                    try
                    {
                        ObtenerValoresCita();
                        cita.ModificarCita(cita);
                        LimpiarFormulario();
                        CargarCitasPaciente();
                        MessageBox.Show("¡La cita se ha modificado con exito!", "CITA", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Ha ocurrido un error al momento de realizar la modificacion... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("¡Debe seleccionar una cita!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               

            }
            else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void gridCitas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;
            TextRange notas = new TextRange(richTextBoxNotas.Document.ContentStart, richTextBoxNotas.Document.ContentEnd);
            
            if (rowSelected != null)
            {
                selecionado = true;
                codigoCita = Convert.ToInt32(rowSelected.Row["Codigo de cita"]);
                dtFecha.SelectedDate = Convert.ToDateTime(rowSelected.Row["Fecha de la cita"]);
                notas.Text = rowSelected.Row["Notas"].ToString();

            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }
    }
}
