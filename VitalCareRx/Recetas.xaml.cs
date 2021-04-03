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
    /// Interaction logic for Recetas.xaml
    /// </summary>
    public partial class Recetas : Window
    {
        SqlConnection sqlConnection;
        private int codigoConsulta;
        private bool validarSeleccionado;
        private int codigoRecetaMedica;

        private Receta receta = new Receta();

        public Recetas(int idConsulta, int idRecetaMedica)
        {
            InitializeComponent();
            codigoConsulta = idConsulta;
            codigoRecetaMedica = idRecetaMedica;

            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            CargarFarmacos();
            MostrarFarmacos();

            validarSeleccionado = false;
        }

        private void MostrarFarmacos()
        {
            try
            {
                string query = @"SELECT DR.idRecetaMedica 'Receta Medica', DR.idFarmaco 'Codigo Farmaco',F.descripcionFarmaco 'Farmaco', DR.duracionTratamiento 'Duracion', DR.indicaciones 'Indicaciones'
                            FROM [Consultas].[DetalleRecetaMedica] DR INNER JOIN [Consultas].[Farmaco] F
                            ON DR.idFarmaco = F.idFarmaco
                            INNER JOIN [Consultas].[RecetaMedica] R
                            ON DR.idRecetaMedica = R.idRecetaMedica
                            INNER JOIN [Consultas].[Consulta] C
                            ON R.idConsulta = C.idConsulta
                            WHERE C.idConsulta = @idConsulta";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlCommand.Parameters.AddWithValue("@idConsulta", codigoConsulta);



                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    dgRecetas.ItemsSource = dataTable.DefaultView;
                    dgRecetas.IsReadOnly = true;


                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void CargarFarmacos()
        {
            string query = @"SELECT * FROM [Consultas].[Farmaco]";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            using (sqlDataAdapter)
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                cmbFarmacos.DisplayMemberPath = "descripcionFarmaco";
                cmbFarmacos.SelectedValuePath = "idFarmaco";
                cmbFarmacos.ItemsSource = dataTable.DefaultView;
                ;
            }
        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    if (!validarSeleccionado)
                    {
                        ObtenerValores();

                        receta.AgregarFarmacoAReceta(receta);

                        LimpiarFormulario();

                        MostrarFarmacos();

                        MessageBox.Show("Farmaco agreado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("El farmaco ya a sido agregado a la receta medica de la consulta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }



                }
                else
                {
                    MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Ha ocurrido un error al momento de realizar la insercion... Favor intentelo de nuevo mas tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (validarSeleccionado)
                {
                    
                    ObtenerValores();

                    receta.EliminarFarmacoReceta(receta);

                    MessageBox.Show("Farmaco eliminado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);

                    LimpiarFormulario();

                    MostrarFarmacos();

                }
                else
                {
                    MessageBox.Show("¡Es requerido que seleccione un farmaco!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Ha ocurrido un error al momento de realizar la elimnacion... Favor intentelo de nuevo mas tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ObtenerValores()
        {
            TextRange indicaciones = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);
            TextRange duracionTratamiento = new TextRange(rtxtDuracionTratamiento.Document.ContentStart, rtxtDuracionTratamiento.Document.ContentEnd);

            receta.IdConsulta = codigoConsulta;
            receta.Indicaciones = indicaciones.Text;
            receta.DuracionTratamiento = duracionTratamiento.Text;
            receta.IdFarmaco = Convert.ToInt32(cmbFarmacos.SelectedValue);
            receta.IdReceta = codigoRecetaMedica;
        }
        private bool ValidarCampos()
        {
            TextRange indicaciones = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);
            TextRange durationTratamiento = new TextRange(rtxtDuracionTratamiento.Document.ContentStart, rtxtDuracionTratamiento.Document.ContentEnd);
            if (cmbFarmacos.SelectedValue != null && indicaciones.Text != string.Empty && durationTratamiento.Text != string.Empty)
            {
                return true;
            }

            return false;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void LimpiarFormulario()
        {

            rtxtIndicaciones.Document.Blocks.Clear();
            rtxtDuracionTratamiento.Document.Blocks.Clear();
            cmbFarmacos.SelectedValue = null;
            validarSeleccionado = false;
        }

        private void dgRecetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;


            if (rowSelected != null)
            {
                validarSeleccionado = true;

                TextRange DuracionTratamiento = new TextRange(rtxtDuracionTratamiento.Document.ContentStart, rtxtDuracionTratamiento.Document.ContentEnd);
                TextRange Indicaciones = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);

                DuracionTratamiento.Text = rowSelected.Row["Duracion"].ToString();
                Indicaciones.Text = rowSelected.Row["Indicaciones"].ToString();
                cmbFarmacos.SelectedValue = rowSelected.Row["Codigo Farmaco"].ToString();
            }

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (validarSeleccionado)
                {
                    if (ValidarCampos())
                    {


                        ObtenerValores();

                        receta.ModificarFarmacoReceta(receta);



                        MessageBox.Show("Farmaco modificado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);

                        LimpiarFormulario();

                        MostrarFarmacos();
                    }
                    else
                    {
                        MessageBox.Show("Debe llenar todos los campus", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }



                }
                else
                {
                    MessageBox.Show("¡Es requerido que seleccione un farmaco!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificacion... Favor intentelo de nuevo mas tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }  
}
