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
        private string usuario;
        private string correo;
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

        private void txtPrimerNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);

        }

        private void txtPrimerNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtCelular_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.SoloNumeros(e);
        }

        private void txtCelular_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtSegundoNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtSegundoNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtPrimerApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtPrimerApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtSegundoApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtSegundoApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }


        private void txtUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (Validar())//los campos no deben de estar vacios
            {
                if (!seleccionado)//No se debe seleccionar el datagrid
                {
                    if (txtCelular.Text.Length == 8)// El numero de telefono debe contener 8 digitos. 
                    {
                        if (validaciones.Email_Correcto(txtCorreo.Text))//Valida que el correo este escrito correctamente.
                        {
                            if (dtFecha.SelectedDate <= DateTime.Now.Date)//la fecha ingresada debe ser menor a la fecha actual
                            {
                                if (!unEmpleado.ExisteUsuario(txtUsuario.Text))//Se valida que el nombre de usuario no exista.
                                {
                                    if (!unEmpleado.ExisteCorreo(txtCorreo)) 
                                    {
                                        if (txtUsuario.Text.Length >= 5)//El usuario debe contener almenos 5 caracteres.
                                        {
                                            if (txtContrasenia.Text.Length >= 8)//La contraseña debe tener almenos 8 caracteres o mas.
                                            {
                                                ObtenerDatos();
                                                unEmpleado.CrearNuevoEmpleado(unEmpleado);
                                                miEmpleado.VerEmpleados(gridEmpleados, Convert.ToInt32(cmbEstado.SelectedValue));
                                                LimpiarFormulario();
                                                MessageBox.Show("¡Empleado agregado exitosamente!", "EMPLEADO", MessageBoxButton.OK, MessageBoxImage.Information);
                                            }
                                            else
                                            {
                                                MessageBox.Show("¡La contraseña debe contener almenos 8 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("¡El nombre de usuario debe contener almenos 5 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("¡El correo ingresado ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }

                                   
                                    
                                }
                                else
                                {
                                    MessageBox.Show("¡El nombre de usuario ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("¡La fecha de nacimiento no puede ser mayor a la fecha actual!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("¡La dirección de correo electronico no es valida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡El numero de celular debe contener 8 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("¡No se puede agregar al mismo empleado!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("¡Debe llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
         
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
            usuario = string.Empty;
            correo = string.Empty;
            OcultarColumnas();

        }
        private bool Validar()
        {
            
            //Validación que no permita campos vacíos.
            if ( txtPrimerNombre.Text != String.Empty && txtPrimerApellido.Text != String.Empty
                && txtCelular.Text != String.Empty && txtCorreo.Text != String.Empty && txtUsuario.Text != String.Empty && txtContrasenia.Text != String.Empty && dtFecha.SelectedDate != null
                && cmbSexo.SelectedValue != null )
            {
                return true;
            }
            return false;
        }

        private void btnModificarr_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                if (Validar())//los campos no deben de estar vacios
                {
                    if (txtCelular.Text.Length == 8)// El numero de telefono debe contener 8 digitos. 
                    {
                        if (validaciones.Email_Correcto(txtCorreo.Text))//Valida que el correo este escrito correctamente.
                        {
                            if (dtFecha.SelectedDate <= DateTime.Now.Date)//la fecha ingresada debe ser menor a la fecha actual
                            {
                                if (!unEmpleado.ExisteUsuario(txtUsuario.Text) || usuario == txtUsuario.Text)//Se valida que el nombre de usuario no exista.
                                {
                                    if (!unEmpleado.ExisteCorreo(txtCorreo) || correo == txtCorreo.Text)
                                    {
                                        if (txtContrasenia.Text.Length >= 8)//La contraseña debe tener almenos 8 caracteres o mas.
                                        {
                                            if(txtUsuario.Text.Length >= 5)//El usuario debe contener almenos 5 caracteres.
                                            {
                                                ObtenerDatos();
                                                unEmpleado.ModificarEmpleado(unEmpleado);
                                                miEmpleado.VerEmpleados(gridEmpleados, Convert.ToInt32(cmbEstado.SelectedValue));
                                                LimpiarFormulario();
                                                MessageBox.Show("¡Empleado modificados exitosamente!", "EMPLEADO", MessageBoxButton.OK, MessageBoxImage.Information);
                                            }
                                            else
                                            {
                                                MessageBox.Show("¡La usuario debe contener almenos 5 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                           
                                        }
                                        else
                                        {
                                            MessageBox.Show("¡La contraseña debe contener almenos 8 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("¡El correo ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                   
                                }
                                else
                                {
                                    MessageBox.Show("¡El nombre de usuario ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("¡La fecha de nacimiento no puede ser mayor a la fecha actual!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                           MessageBox.Show("¡La dirección de correo electronico no es valida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡El numero de celular debe contener 8 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("¡Debe llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("¡Debe seleccionar un empleado!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
               
        
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
               
             
                idEmpleado = Convert.ToInt32(rowSelected.Row["Código empleado"].ToString());
                txtPrimerNombre.Text = rowSelected.Row["primerNombre"].ToString();
                txtSegundoNombre.Text = rowSelected.Row["segundoNombre"].ToString();
                txtPrimerApellido.Text = rowSelected.Row["primerApellido"].ToString();
                txtSegundoApellido.Text = rowSelected.Row["segundoApellido"].ToString();                
                txtCelular.Text = rowSelected.Row["Celular"].ToString();
                dtFecha.SelectedDate = Convert.ToDateTime(rowSelected.Row["Fecha de nacimiento"]);
                txtUsuario.Text = rowSelected.Row["Nombre de usuario"].ToString();
                usuario = txtUsuario.Text;
                txtCorreo.Text = rowSelected.Row["Correo"].ToString();
                correo = txtCorreo.Text;
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
