using System;
using System.Collections.Generic;
using System.Data;
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

using System.Windows.Threading;

namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : Window
    {
        Empleado miEmpleado = new Empleado();
        LlenarComboBox LlenarComboBox = new LlenarComboBox();
        Validaciones validaciones = new Validaciones();
        Empleado unEmpleado = new Empleado();
        int estado = 1;
        int idEmpleado;
        bool seleccionado = false;
        bool cargado = false;

        public Empleados(Empleado empleado)
        {
            InitializeComponent();
            miEmpleado = empleado;
            LlenarComboBox.CargarComboBoxEstado(cmbEstado);
            LlenarComboBox.CargarComboBoxSexo(cmbSexo);
            miEmpleado.VerEmpleados(gridEmpleados, estado);
            cmbEstado.SelectedValue = estado;
            validaciones.CargarColorBoton(btnEstado, estado);

            miEmpleado.VerEmpleados(gridEmpleados, Convert.ToInt32(cmbEstado.SelectedValue));
        }

       
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Si se le da click derecho que no permita mover la ventana


        }

        //cuando se mantiene presionado click derecho
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        //cuando se suelta el click derecho
        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtPrimerNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {


        }

        private void txtPrimerNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtCelular_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {


        }

        private void txtCelular_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtSegundoNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void txtSegundoNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtPrimerApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void txtPrimerApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtSegundoApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        { 

        }

        private void txtSegundoApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }


        private void txtUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            
            ObtenerDatos();
            unEmpleado.CrearNuevoEmpleado(unEmpleado);
            miEmpleado.VerEmpleados(gridEmpleados, Convert.ToInt32(cmbEstado.SelectedValue));
            LimpiarFormulario();
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

        private void OcultarColumnas()
        {
            gridEmpleados.Columns[1].Visibility = Visibility.Hidden;
            gridEmpleados.Columns[2].Visibility = Visibility.Hidden;
            gridEmpleados.Columns[3].Visibility = Visibility.Hidden;
            gridEmpleados.Columns[4].Visibility = Visibility.Hidden;
            gridEmpleados.Columns[8].Visibility = Visibility.Hidden;
            gridEmpleados.Columns[9].Visibility = Visibility.Hidden;


        }

        private void ObtenerDatos()
        {
            bool status;
            unEmpleado.IdEmpleado= idEmpleado;
            unEmpleado.PrimerNombre = txtPrimerNombre.Text;
            unEmpleado.SegundoNombre = txtSegundoNombre.Text;
            unEmpleado.PrimerApellido = txtPrimerApellido.Text;
            unEmpleado.SegundoApellido = txtSegundoApellido.Text;
            unEmpleado.Celular = txtCelular.Text;
            unEmpleado.Correo = txtCorreo.Text;
            if (dtFecha.SelectedDate != null && cmbSexo.SelectedValue != null)
            {
                unEmpleado.FechaNacimiento = dtFecha.SelectedDate.Value;
                unEmpleado.IdSexo = Convert.ToInt32(cmbSexo.SelectedValue);
            }            
            
            unEmpleado.IdPuesto = 2;
            unEmpleado.NombreUsuario = txtUsuario.Text;
            unEmpleado.Contrasenia = txtContrasenia.Text;
            if (estado == 1)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            unEmpleado.Estado = status;
        }

        private void LimpiarFormulario()
        {
          
            txtPrimerNombre.Clear();
            txtSegundoNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();          
            txtCelular.Clear();
            txtCorreo.Clear();
            dtFecha.SelectedDate = null;           
            estado = 1;
            cmbEstado.SelectedValue = estado;
            validaciones.CargarColorBoton(btnEstado, estado);           
            cmbSexo.SelectedValue = null;
            seleccionado = false;            
            txtBuscar.Clear();
            txtUsuario.Clear();
            txtContrasenia.Clear();
            idEmpleado = 0;
            lblContra.Visibility = Visibility.Visible;
            txtContrasenia.Visibility = Visibility.Visible;
            txtBuscar.Clear();
            OcultarColumnas();

        }


        private void btnModificarr_Click(object sender, RoutedEventArgs e)
        {
            ObtenerDatos();
            unEmpleado.ModificarEmpleado(unEmpleado);
            miEmpleado.VerEmpleados(gridEmpleados, Convert.ToInt32(cmbEstado.SelectedValue));
            LimpiarFormulario();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            ObtenerDatos();
            unEmpleado.EliminarEmpleado(unEmpleado);
            miEmpleado.VerEmpleados(gridEmpleados, Convert.ToInt32(cmbEstado.SelectedValue));
            LimpiarFormulario();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            
            unEmpleado.VerEmpleados(gridEmpleados, estado);
            LimpiarFormulario();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            unEmpleado.VerUnEmpleado(gridEmpleados, estado, txtBuscar.Text);
            OcultarColumnas();
        }

        private void btnEstado_Click(object sender, RoutedEventArgs e)
        {
            if (estado == 0) // Si estado es  0(quiere decir que dio click estando el estado en 0), que cambie el botón estado a color verde.
            {
                estado = 1;
                validaciones.CargarColorBoton(btnEstado, estado);
                
            }
            else // Sino que lo pase a color rojo.
            {
                estado = 0;
                validaciones.CargarColorBoton(btnEstado, estado);
                
            }
        }

        private void gridEmpleados_Loaded(object sender, RoutedEventArgs e)
        {
            OcultarColumnas();
            cargado = true;
        }

        private void gridEmpleados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender; // Instancia de objeto tipo DataaGrid
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;
            

            if (rowSelected != null) // Si no esta seleccionado que no envie la información a las textBox
            {
                //Asiganamos contenido a todas las textBox segun la columna en base a la fila seleccionada
                seleccionado = true;
               
             
                idEmpleado = Convert.ToInt32(rowSelected.Row["Codigo empleado"].ToString());
                txtPrimerNombre.Text = rowSelected.Row["primerNombre"].ToString();
                txtSegundoNombre.Text = rowSelected.Row["segundoNombre"].ToString();
                txtPrimerApellido.Text = rowSelected.Row["primerApellido"].ToString();
                txtSegundoApellido.Text = rowSelected.Row["segundoApellido"].ToString();                
                txtCelular.Text = rowSelected.Row["Celular"].ToString();
                dtFecha.SelectedDate = Convert.ToDateTime(rowSelected.Row["Fecha de nacimiento"]);
                txtUsuario.Text = rowSelected.Row["Nombre de usuario"].ToString();
                txtCorreo.Text = rowSelected.Row["Correo"].ToString();
                txtContrasenia.Text = rowSelected.Row["contrasenia"].ToString(); ;

                if (rowSelected.Row["Estado"].ToString() == "True")
                {

                    estado = 1;
                }
                else if (rowSelected.Row["Estado"].ToString() == "False")
                {

                    estado = 0;
                }


                validaciones.CargarColorBoton(btnEstado, estado); //Cambiar color del botón de estado dependiendo del estado en que se encuentra el paciente seleccionado                
                cmbSexo.SelectedValue = rowSelected.Row["idSexo"].ToString();

                lblContra.Visibility = Visibility.Hidden;
                txtContrasenia.Visibility = Visibility.Hidden;

            }
        }

        private void gridEmpleados_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime)) //Si la columna es de tipo DateTime que le cambie el formato a fecha corta.
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.PropertyType == typeof(System.Double)) //Si la columna es de tipo Double o float que le cambie el formato o redondear a 2 cifras.
                (e.Column as DataGridTextColumn).Binding.StringFormat = "N2";
        }

        private void cmbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            unEmpleado.VerEmpleados(gridEmpleados, Convert.ToInt32(cmbEstado.SelectedValue));
            if (cargado)
            {
                OcultarColumnas();
            }
            if (Convert.ToInt32(cmbEstado.SelectedValue) == 0)
            {
                btnEliminar.IsEnabled = false;
            }
            else
            {
                btnEliminar.IsEnabled = true;
            }
        }
    }
}
