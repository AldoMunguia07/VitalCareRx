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

        public CitasPaciente(string paciente) //Se le pasa por parametro el id del paciente para ver las citas correspondientes a dicho paciente.
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
                    gridCitas.IsReadOnly = true; // El grid es de solo lectura.


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
            //Validar que se llenen todos los campos.
            if (dtFecha.SelectedDate != null && notas.Text != "\r\n")
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
            try
            {
                if (Validar()) //Si hay campos vacios que o deje insertar la cita.
                {

                    if (!selecionado) //El usuario no puede agregar una cita que haya seleccionado. 
                    {
                        if (dtFecha.SelectedDate > DateTime.Now.Date) //La fecha de la cita debe ser mayor a la fecha actual.
                        {
                            
                            ObtenerValoresCita();
                            cita.AgregarCita(cita);
                            LimpiarFormulario();
                            CargarCitasPaciente();
                            MessageBox.Show("¡La cita se ha agregado con exito!", "CITA", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                        }
                        else
                        {
                            MessageBox.Show("¡La fecha de la proxima cita debe ser mayor a la fecha actual!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            catch (Exception)
            {

                MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selecionado) // El usuario para modificar una cita primero debe seleccionarla.
                {
                    if (Validar()) //Para modificar una cita no debe dejar campos en blanco
                    {
                        if (dtFecha.SelectedDate > DateTime.Now.Date) //La fecha de la cita debe ser mayor a la fecha actual.
                        {
                            
                            ObtenerValoresCita();
                            cita.ModificarCita(cita);
                            LimpiarFormulario();
                            CargarCitasPaciente();
                            MessageBox.Show("¡La cita se ha modificado con exito!", "CITA", MessageBoxButton.OK, MessageBoxImage.Information);
                           
                        }
                        else
                        {
                            MessageBox.Show("¡La fecha de la proxima cita debe ser mayor a la fecha actual!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    MessageBox.Show("¡Debe seleccionar una cita!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificacion... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        //Evento para enviar la información del grid a las textBox
        private void gridCitas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender; // Instancia de objeto tipo DataaGrid.
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;
            TextRange notas = new TextRange(richTextBoxNotas.Document.ContentStart, richTextBoxNotas.Document.ContentEnd);
            
            if (rowSelected != null) // Si no esta seleccionado que no envie la información a las textBox
            {
                //Asiganamos contenido a todas las textBox segun la columna en base a la fila seleccionada

                selecionado = true;
                codigoCita = Convert.ToInt32(rowSelected.Row["Codigo de cita"]);
                dtFecha.SelectedDate = Convert.ToDateTime(rowSelected.Row["Fecha de la cita"]);
                notas.Text = rowSelected.Row["Notas"].ToString();

            }
        }


        //Al darle click que reestablesca el formulario como en un inició.
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarCitasPaciente();
        }

        //Darle formato al data grid view
        private void gridCitas_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; //Si la columna es de tipo DateTime que le cambie el formato a fecha corta.
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
    }
}
