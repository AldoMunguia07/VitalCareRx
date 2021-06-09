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
        private int codigoFarmaco;
        

        private Receta receta = new Receta();

        public Recetas(int idConsulta, int idRecetaMedica) // Recibe como paramtero el id de la consulta y el id de la receta (La informacion de este formulario se guarda en una tabla detalle)
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

        /// <summary>
        /// Metodo para ver los farmacos de la consulta actual.
        /// </summary>
        private void MostrarFarmacos()
        {
            try
            {
                string query = @"SELECT DR.idRecetaMedica 'Receta Medica', DR.idFarmaco 'Codigo Farmaco', F.descripcionFarmaco 'Farmaco', DR.cantidad 'Cantidad', DR.duracionTratamiento 'Duracion', DR.indicaciones 'Indicaciones'
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
                    dgRecetas.IsReadOnly = true; // El grid es de solo lectura.


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Metodo para cargar el ComboBox farmacos con información.
        /// </summary>
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
                if (ValidarCampos()) // El usuario no tiene que dejar los campos vacios.
                {
                    if (!validarSeleccionado) // El usuario no tiene que agregar un farmaco que este seleccionando.
                    {
                        if (!ValidarFarmacoEnReceta()) // El usuario no tiene que añadir un farmaco 2 veces a la receta
                        {
                            if (int.Parse(txtCantidad.Text) > 0 && int.Parse(txtCantidad.Text) <= 25) // La cantidad de farmacos debe ser mayor a 0 y menor a 25
                            {
                                try
                                {
                                    ObtenerValores();

                                    receta.AgregarFarmacoAReceta(receta);

                                    LimpiarFormulario();

                                    MostrarFarmacos();

                                    MessageBox.Show("Farmaco agreado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("¡La cantidad del farmaco debe estar entre 1 y 25!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("El farmaco ya a sido agregado a la receta medica de la consulta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

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
                MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        //Evento click del botón eliminar para eliminar un farmaco de la receta.
        //Cabe recalcar que aquí si es posible eliminar porque un farmaco erroneo en una receta no es de mucha utilidad como historico.
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (validarSeleccionado) // Para eliminar un farmaco de una receta, primero el usuario debe seleccionar dicha receta.
                {
                    
                    ObtenerValores();

                    receta.EliminarFarmacoReceta(receta);

                    LimpiarFormulario();

                    MostrarFarmacos();

                    MessageBox.Show("Farmaco eliminado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);

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

        /// <summary>
        /// Metodo para obtener los valores de receta.
        /// </summary>
        private void ObtenerValores()
        {
            TextRange indicaciones = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);
            TextRange duracionTratamiento = new TextRange(rtxtDuracionTratamiento.Document.ContentStart, rtxtDuracionTratamiento.Document.ContentEnd);

            receta.IdConsulta = codigoConsulta;
            receta.Indicaciones = indicaciones.Text.Substring(0, indicaciones.Text.Length - 2);
            receta.DuracionTratamiento = duracionTratamiento.Text.Substring(0, duracionTratamiento.Text.Length - 2);
            receta.Cantidad = Convert.ToInt32(txtCantidad.Text);
            receta.IdFarmaco = Convert.ToInt32(cmbFarmacos.SelectedValue);
            receta.IdReceta = codigoRecetaMedica;
            
        }

        /// <summary>
        /// Metodo para validar que llene todos los campos.
        /// </summary>
        /// <returns></returns>
        private bool ValidarCampos()
        {
            TextRange indicaciones = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);
            TextRange durationTratamiento = new TextRange(rtxtDuracionTratamiento.Document.ContentStart, rtxtDuracionTratamiento.Document.ContentEnd);
            if (cmbFarmacos.SelectedValue != null && txtCantidad.Text != string.Empty && indicaciones.Text != "\r\n" && durationTratamiento.Text != "\r\n")
            {
                return true;
            }

            return false;
        }


        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        /// <summary>
        /// Metedo para limpiar el formulario.
        /// </summary>
        private void LimpiarFormulario()
        {
            txtCantidad.Text = string.Empty;
            rtxtIndicaciones.Document.Blocks.Clear();
            rtxtDuracionTratamiento.Document.Blocks.Clear();
            cmbFarmacos.SelectedValue = null;
            validarSeleccionado = false;
        }

        //Evento para enviar la información del grid a las textBox
        private void dgRecetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;


            if (rowSelected != null) // Si no esta seleccionado que no capture el id
            {
                validarSeleccionado = true;

                //Asiganamos contenido a todas las textBox segun la columna en base a la fila seleccionada

                TextRange DuracionTratamiento = new TextRange(rtxtDuracionTratamiento.Document.ContentStart, rtxtDuracionTratamiento.Document.ContentEnd);
                TextRange Indicaciones = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);

                cmbFarmacos.SelectedValue = rowSelected.Row["Codigo Farmaco"].ToString();
                txtCantidad.Text = rowSelected.Row["Cantidad"].ToString();
                DuracionTratamiento.Text = rowSelected.Row["Duracion"].ToString();
                Indicaciones.Text = rowSelected.Row["Indicaciones"].ToString();

                codigoFarmaco = Convert.ToInt32(rowSelected.Row["Codigo Farmaco"].ToString()); // Se pasa id de farmaco para la correspondinete validacion en el proceso de modificar un farmaco.

            }

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validarSeleccionado) // El usuario no puede modificar un farmaco en a receta si antes no lo ha seleccionado.
                {
                    if (ValidarCampos()) // El usuario no tiene que dejar los campos vacios.
                    {
                        if (!ValidarFarmacoEnReceta() || codigoFarmaco == Convert.ToInt32(cmbFarmacos.SelectedValue)) // Valida si el farmaco no esta en la receta actual y si es el codigo de farmaco seleccionado, con esa
                        {                                                                                             // condición pasara a la siguiente validación
                            if (int.Parse(txtCantidad.Text) > 0) // La cantidad de farmacos debe ser mayor a 0
                            {
                                
                                ObtenerValores();

                                receta.ModificarFarmacoReceta(receta);

                                LimpiarFormulario();

                                MostrarFarmacos();

                                MessageBox.Show("Farmaco modificado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);
                                
                            }
                            else
                            {
                                MessageBox.Show("¡No puede recetar 0 o una cantidad menor de farmacos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("El farmaco ya a sido agregado a la receta medica de la consulta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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

                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificación... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            MostrarFarmacos();
        }

        /// <summary>
        /// Metodo para validar si el farmaco ya esta en la receta.
        /// </summary>
        /// <returns>Boolean</returns>
        private bool ValidarFarmacoEnReceta()
        {
            try
            {
                string query = @"SELECT idFarmaco FROM [Consultas].[DetalleRecetaMedica] WHERE idRecetaMedica = @idReceta and idFarmaco = @idFarmaco";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@idFarmaco", cmbFarmacos.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@idReceta", codigoRecetaMedica);

                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 1) //Si el farmaco existe en la receta actual, devuelve un true
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

        private void rtxtDuracionTratamiento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (caracter == 45) // Codigo ASCII 
                    e.Handled = true;  // Permite 
                else
                    e.Handled = false; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

       
    }  
}
