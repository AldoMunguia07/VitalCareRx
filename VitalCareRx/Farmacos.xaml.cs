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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para Farmacos.xaml
    /// </summary>
    public partial class Farmacos : Window
    {
        SqlConnection sqlConnection;
        private Farmaco farmaco = new Farmaco();
        private int codigoEmpleado;
        private string nombreEmpleado;
        private bool seleccionado = false;

        public Farmacos(int codigo, string empleado)
        {
            InitializeComponent();
            //Variables miembro
            codigoEmpleado = codigo;
            nombreEmpleado = empleado;

            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            MostrarConsultas();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal(nombreEmpleado, codigoEmpleado);
            menu.Show();
            this.Close();
        }


        /// <summary>
        /// Trae todas las consultas de la base de datos al inicial el programa.
        /// </summary>
        public void MostrarConsultas()
        {
            string query = @"SELECT * FROM [Consultas].[Farmaco]";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    dgFarmacos.ItemsSource = dataTable.DefaultView;

                    dgFarmacos.IsReadOnly = true;


                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        private bool ValidarCampos()
        {

            TextRange IndicacionesFarmaco = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);

            if (txtDescripcionFarmaco.Text != string.Empty && IndicacionesFarmaco.Text != "\r\n")
            {
                return true;
            }

            return false;

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (!seleccionado)
            {
                if (ValidarCampos())
                {

                    try
                    {
                        ObtenerValores();

                        farmaco.CrearFarmaco(farmaco);

                        LimpiarFormulario();

                        MessageBox.Show("El farmaco se ha insertado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

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
            else
            {
                MessageBox.Show("¡El Farmaco ya se encuentra ingresado!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void ObtenerValores()
        {
            //Error guarda sin necesidad de tener datos en el RichBox
            TextRange IndicacionesFarmaco = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);


            farmaco.DescripcionFarmaco = txtDescripcionFarmaco.Text;
            farmaco.InformacionPrecaucion = IndicacionesFarmaco.Text;


        }

        private void LimpiarFormulario()
        {

            rtxtIndicaciones.Document.Blocks.Clear();
            txtDescripcionFarmaco.Text = string.Empty;
            seleccionado = false;


        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                if (ValidarCampos())
                {

                    try
                    {
                        ObtenerValores();

                        farmaco.ModificarFarmaco(farmaco);

                        LimpiarFormulario();

                        MessageBox.Show("El farmaco se ha modificado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

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
            else
            {
                MessageBox.Show("¡Debe seleccionar un Farmaco!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void dgFarmacos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;

            if (rowSelected != null)
            {
                seleccionado = true;
                TextRange IndicacionesFarmaco = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);
                txtDescripcionFarmaco.Text = rowSelected.Row["descripcionFarmaco"].ToString();
                IndicacionesFarmaco.Text = rowSelected.Row["informacionPrecaucion"].ToString();
                farmaco.IdFarmaco = Convert.ToInt32(rowSelected.Row["idFarmaco"]);


            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = @"SELECT * FROM [Consultas].[Farmaco] WHERE descripcionFarmaco LIKE CONCAT('%', @nombreFarmaco, '%')";


            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            using (sqlDataAdapter)
            {
                sqlCommand.Parameters.AddWithValue("@nombreFarmaco", txtBuscarFarmaco.Text);

                DataTable dataTable = new DataTable();

                sqlDataAdapter.Fill(dataTable);

                dgFarmacos.ItemsSource = dataTable.DefaultView;

                dgFarmacos.IsReadOnly = true;

            }
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
