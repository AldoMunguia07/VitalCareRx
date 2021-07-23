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
       
        private Farmaco farmaco = new Farmaco();
        
        private bool seleccionado = false;
        Validaciones validaciones = new Validaciones();
        Empleado miEmpleado = new Empleado();

        public Farmacos(Empleado empleado)// se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
                                                    //y nombre del empleado(Se usa para volver al menu principal).
        {
            InitializeComponent();
            //Variables miembro
            miEmpleado = empleado;       
            farmaco.MostrarFarmaco(dgFarmacos);
            farmaco.IdEmpleado = miEmpleado.IdEmpleado;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea regresar al menú principal?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (miEmpleado.IdPuesto == 1)
                {
                    MenuPrincipalAdmin menuPrincipalAdmin = new MenuPrincipalAdmin(miEmpleado);
                    menuPrincipalAdmin.Show();
                    this.Close();
                }
                else if (miEmpleado.IdPuesto == 2)
                {
                    MenuPrincipal menupincipal = new MenuPrincipal(miEmpleado);
                    menupincipal.Show();
                    this.Close();
                }
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
            try
            {
                if (!seleccionado) // El usuario no puede añadir un farmaco mientras está seleccionando otro farmaco.
                {
                    if (ValidarCampos()) // El usuario no puede dejar los campos en blanco.
                    {

                      
                        ObtenerValores();

                        farmaco.CrearFarmaco(farmaco);

                        LimpiarFormulario();

                        MessageBox.Show("El farmaco se ha insertado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

                        farmaco.MostrarFarmaco(dgFarmacos);



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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            try
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

                            farmaco.MostrarFarmaco(dgFarmacos);



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
            catch (Exception)
            {


                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificacion... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                IndicacionesFarmaco.Text = rowSelected.Row["Informacion del farmaco"].ToString();
                farmaco.IdFarmaco = Convert.ToInt32(rowSelected.Row["Codigo Farmaco"]);
                ObtenerValores();


            }

        }

        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            Inventario inventario = new Inventario(farmaco);
            inventario.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            farmaco.BuscarFarmaco(txtBuscarFarmaco.Text, dgFarmacos);
        }


        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {

            LimpiarFormulario();
            farmaco.MostrarFarmaco(dgFarmacos);

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

        private void txtDescripcionFarmaco_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.CaracteresInecesarios(e);
            
        }

        private void rtxtIndicaciones_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.CaracteresInecesarios(e);
        }

       
    }
}
