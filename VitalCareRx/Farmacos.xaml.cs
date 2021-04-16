﻿using System;
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

        public Farmacos(int codigo, string empleado)// se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
                                                    //y nombre del empleado(Se usa para volver al menu principal).
        {
            InitializeComponent();
            //Variables miembro
            codigoEmpleado = codigo;
            nombreEmpleado = empleado;

            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            MostrarFarmaco();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal(nombreEmpleado, codigoEmpleado); // Se regresa al menu principal con los datos del usuario actual.
            menu.Show();
            this.Close();
        }


        /// <summary>
        /// Trae todas las consultas de la base de datos al inicial el programa.
        /// </summary>
        public void MostrarFarmaco()
        {
            string query = @"SELECT idFarmaco as 'Codigo Farmaco', descripcionFarmaco as 'Farmaco', informacionPrecaucion as 'Información del farmaco' FROM [Consultas].[Farmaco]";
            ;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    dgFarmacos.ItemsSource = dataTable.DefaultView;

                    dgFarmacos.IsReadOnly = true; // El grid es de solo lectura.


                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Metodo para validar que los campos no esten vacíos.
        /// </summary>
        /// <returns></returns>
        private bool ValidarCampos()
        {

            TextRange IndicacionesFarmaco = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);

            //Validación para que el usuario no deje los campos vacíos
            if (txtDescripcionFarmaco.Text != string.Empty && IndicacionesFarmaco.Text != "\r\n")
            {
                return true;
            }

            return false;

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (!seleccionado) // El usuario no puede añadir un farmaco mientras está seleccionando otro farmaco.
            {
                if (ValidarCampos()) // El usuario no puede dejar los campos en blanco.
                {

                    try
                    {
                        ObtenerValores();

                        farmaco.CrearFarmaco(farmaco);

                        LimpiarFormulario();

                        MessageBox.Show("El farmaco se ha insertado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

                        MostrarFarmaco();

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

        /// <summary>
        /// Metodo para obtener los valores de las TextBox.
        /// </summary>
        private void ObtenerValores()
        {
            //Error guarda sin necesidad de tener datos en el RichBox
            TextRange IndicacionesFarmaco = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);


            farmaco.DescripcionFarmaco = txtDescripcionFarmaco.Text;
            farmaco.InformacionPrecaucion = IndicacionesFarmaco.Text.Substring(0, IndicacionesFarmaco.Text.Length - 2);


        }

        /// <summary>
        /// Metodo para limpiar el formulario.
        /// </summary>
        private void LimpiarFormulario()
        {

            rtxtIndicaciones.Document.Blocks.Clear();
            txtDescripcionFarmaco.Text = string.Empty;
            txtBuscarFarmaco.Text = string.Empty;
            seleccionado = false;


        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado) //El usuario primero tiene que seleccionar un farmac para poder modificarlo.
            {
                if (ValidarCampos()) // El usuario no puede dejar los campos en blanco.
                {

                    try
                    {
                        ObtenerValores();

                        farmaco.ModificarFarmaco(farmaco);

                        LimpiarFormulario();

                        MessageBox.Show("El farmaco se ha modificado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

                        MostrarFarmaco();



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

        //Enviar información del grid a las textBox
        private void dgFarmacos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid dataGrid = (DataGrid)sender; // Instancia de objeto tipo DataaGrid
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;

            if (rowSelected != null)// Si no esta seleccionado que no envie la información a las textBox.
            {
                //Asiganamos contenido a todas las textBox segun la columna en base a la fila seleccionada.
                seleccionado = true;
                TextRange IndicacionesFarmaco = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);
                txtDescripcionFarmaco.Text = rowSelected.Row["Farmaco"].ToString();
                IndicacionesFarmaco.Text = rowSelected.Row["Información del farmaco"].ToString();
                farmaco.IdFarmaco = Convert.ToInt32(rowSelected.Row["Codigo Farmaco"]);


            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = @"SELECT idFarmaco as 'Codigo Farmaco', descripcionFarmaco as 'Farmaco', informacionPrecaucion as 'Información del farmaco' FROM [Consultas].[Farmaco] WHERE descripcionFarmaco LIKE CONCAT('%', @nombreFarmaco, '%')";


            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            using (sqlDataAdapter)
            {
                sqlCommand.Parameters.AddWithValue("@nombreFarmaco", txtBuscarFarmaco.Text);

                DataTable dataTable = new DataTable();

                sqlDataAdapter.Fill(dataTable);

                dgFarmacos.ItemsSource = dataTable.DefaultView;

                dgFarmacos.IsReadOnly = true; // El grid es de solo lectura.

            }
        }



       

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {

            LimpiarFormulario();
            MostrarFarmaco();
            
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
