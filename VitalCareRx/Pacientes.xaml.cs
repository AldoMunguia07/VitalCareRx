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
    /// Lógica de interacción para Pacientes.xaml
    /// </summary>
    public partial class Pacientes : Window
    {
        private string nombreEmpleado;
        private int codigoEmpleado;

        private int estado = 1;
        bool cargado = false;
        bool seleccionado = false;
        private string dni;
        private int idPaciente;

        SqlConnection sqlConnection;

        Paciente paciente = new Paciente();

        public Pacientes(int codigo, string empleado)// se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
                                                     //y nombre del empleado(Se usa para volver al menu principal).
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            btnEstado.Background = new SolidColorBrush(Color.FromArgb(165, 42, 165, 42)); // Por defecto el botón de estado se inicializa en color verde
            nombreEmpleado = empleado;
            codigoEmpleado = codigo;

            CargarComboBoxSexo();
            CargarComboBoxEstado();
            CargarComboBoxTipoSangre();
            paciente.VerPacientes(paciente, gridPacientes, Convert.ToInt32(cmbEstado.SelectedValue));
            
        }



      
        

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal(nombreEmpleado, codigoEmpleado); // Se regresa al menu principal con los datos del usuario actual.
            menu.Show();
            this.Close();
        }

        /// <summary>
        /// Carga el ComboBox de estado
        /// </summary>
        private void CargarComboBoxEstado()
        {
            string query = @"SELECT '1' id, 'Activos' estado
                            UNION
                            SELECT '0', 'Inactivos'";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            using (sqlDataAdapter)
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                cmbEstado.DisplayMemberPath = "estado";
                cmbEstado.SelectedValuePath = "id";
                cmbEstado.ItemsSource = dataTable.DefaultView;

            }
        }

        /// <summary>
        /// Obtener los datos de la tabla Sexo.
        /// </summary>
        public void CargarComboBoxSexo()
        {

            // Query de selección
            string query = @"SELECT * FROM [Personas].[Sexo]";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            using (sqlDataAdapter)
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                cmbSexo.DisplayMemberPath = "descripcionSexo";
                cmbSexo.SelectedValuePath = "idSexo";
                cmbSexo.ItemsSource = dataTable.DefaultView;
            }



        }

        /// <summary>
        /// Obtener los datos de la tabla TipoSangre.
        /// </summary>
        public void CargarComboBoxTipoSangre()
        {

            // Query de selección
            string query = @"SELECT * FROM [Personas].[TipoSangre]";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            using (sqlDataAdapter)
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                cmbTipoSangre.DisplayMemberPath = "descripcionTipoSangre";
                cmbTipoSangre.SelectedValuePath = "idTipoSangre";
                cmbTipoSangre.ItemsSource = dataTable.DefaultView;
            }



        }

        /// <summary>
        /// Ocultar columnas del DataGridView
        /// </summary>
        private void OcultarColumnas()
        {
            gridPacientes.Columns[1].Visibility = Visibility.Hidden;
            gridPacientes.Columns[2].Visibility = Visibility.Hidden;
            gridPacientes.Columns[3].Visibility = Visibility.Hidden;
            gridPacientes.Columns[4].Visibility = Visibility.Hidden;
            gridPacientes.Columns[5].Visibility = Visibility.Hidden;
            gridPacientes.Columns[6].Visibility = Visibility.Hidden;
            gridPacientes.Columns[7].Visibility = Visibility.Hidden;

        }

        private void gridPacientes_Loaded(object sender, RoutedEventArgs e)
        {
            OcultarColumnas();
            cargado = true;
        }



        private void cmbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paciente.VerPacientes(paciente, gridPacientes, Convert.ToInt32(cmbEstado.SelectedValue));
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

        //Enviar información del grid a las textBox
        private void gridPacientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender; // Instancia de objeto tipo DataaGrid
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;
            TextRange direccion = new TextRange(richTxtDireccion.Document.ContentStart, richTxtDireccion.Document.ContentEnd);

            if (rowSelected != null) // Si no esta seleccionado que no envie la información a las textBox
            {
                //Asiganamos contenido a todas las textBox segun la columna en base a la fila seleccionada
                seleccionado = true;
                dni = rowSelected.Row["Identidad"].ToString(); 
                txtDni.Text = rowSelected.Row["Identidad"].ToString();
                idPaciente = Convert.ToInt32(rowSelected.Row["idPaciente"].ToString());
                txtPrimerNombre.Text = rowSelected.Row["primerNombre"].ToString();
                txtSegundoNombre.Text = rowSelected.Row["segundoNombre"].ToString();
                txtPrimerApellido.Text = rowSelected.Row["primerApellido"].ToString();
                txtSegundoApellido.Text = rowSelected.Row["segundoApellido"].ToString();
                direccion.Text = rowSelected.Row["Direccion"].ToString();
                txtCelular.Text = rowSelected.Row["Celular"].ToString();
                dtFechaNacimiento.SelectedDate = Convert.ToDateTime(rowSelected.Row["Fecha de nacimiento"]);
                txtPeso.Text = rowSelected.Row["Peso (lbs)"].ToString();
                txtEstatura.Text = rowSelected.Row["Estatura (cm)"].ToString();

                if (rowSelected.Row["Estado"].ToString() == "True")
                {

                    estado = 1;
                }
                else if (rowSelected.Row["Estado"].ToString() == "False")
                {

                    estado = 0;
                }


                CargarColorBoton(); //Cambiar color del botón de estado dependiendo del estado en que se encuentra el paciente seleccionado
                cmbTipoSangre.SelectedValue = rowSelected.Row["idTipoSangre"].ToString();
                cmbSexo.SelectedValue = rowSelected.Row["idSexo"].ToString();

            }
        }

        /// <summary>
        /// Limpiar las cajas de texto del formulario.
        /// </summary>
        private void LimpiarFormulario()
        {
            txtDni.Clear();
            txtPrimerNombre.Clear();
            txtSegundoNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            richTxtDireccion.Document.Blocks.Clear();
            txtCelular.Clear();
            dtFechaNacimiento.SelectedDate = null;
            txtPeso.Clear();
            txtEstatura.Clear();
            estado = 1;
            CargarColorBoton();
            cmbTipoSangre.SelectedValue = null;
            cmbSexo.SelectedValue = null;
            seleccionado = false;
            dni = string.Empty;
            txtBuscar.Clear();
            idPaciente = 0;

        }

        /// <summary>
        /// Cargar el color al boton del estado.
        /// </summary>
        private void CargarColorBoton()
        {
            if (estado == 1) // Si el estado es un que lo pase a color verde
            {
                btnEstado.Background = new SolidColorBrush(Color.FromArgb(165, 42, 165, 42));
            }
            else // Sino que lo pase a color rojo.
            {
                btnEstado.Background = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0));
            }
        }

        private void btnEstado_Click(object sender, RoutedEventArgs e)
        {

            if (estado == 0) // Si estado es  0(quiere decir que dio click estando el estado en 0), que cambie el botón estado a color verde.
            {
                btnEstado.Background = new SolidColorBrush(Color.FromArgb(165, 42, 165, 42));
                estado = 1;
            }
            else // Sino que lo pase a color rojo.
            {

                btnEstado.Background = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0));
                estado = 0;
            }

        }

        private void btnModificarr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (seleccionado) // Para modificar un paciente primero debe seleccionarlo
                {

                    if (Validar()) // Los campos no deben estar vacios
                    {

                        if (!paciente.ExistePaciente(paciente, txtDni.Text) || dni == txtDni.Text) // No puede asignar el DNI o identidad de un paciente ya registrado
                        {
                            if (txtCelular.Text.Length == 8) // El numero de telefono debe contener 8 dígitos.
                            {
                                if (txtDni.Text.Length == 13) // El DNI debe contener 13  dígitos.
                                {
                                    if (dtFechaNacimiento.SelectedDate <= DateTime.Now.Date) // La fecha de nacimiento debe ser mayor o igual a la fecha actual.
                                    {
                                        if (float.Parse(txtEstatura.Text) >= 45 && float.Parse(txtEstatura.Text) <= 250)  // La estatura debe ser mayor o igual a 45, y menor o igual a 250.
                                        {
                                            if (float.Parse(txtPeso.Text) >= 5 && float.Parse(txtPeso.Text) <= 800) // El peso debe ser mayor o igual a 5, y menor o igual a 800.
                                            {
                                                
                                                ObtenerDatos();
                                                
                                                paciente.ActualizarPaciente(paciente);
                                                paciente.VerPacientes(paciente, gridPacientes, Convert.ToInt32(cmbEstado.SelectedValue));
                                                LimpiarFormulario();
                                                OcultarColumnas();
                                                MessageBox.Show("El paciente se ha modificado con exito", "PACIENTE", MessageBoxButton.OK, MessageBoxImage.Information);
                                                
                                            }
                                            else
                                            {
                                                MessageBox.Show("¡El peso debe estar dado entre 5 y 800 libras!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("¡La estatura debe estar dada entre 45 y 250 centimetros!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("¡La fecha de nacimiento no puede ser mayor a la fecha actual!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("¡Numero de identidad incompleto, debe de contener 13 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("¡El numero de celular debe contener 8 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("¡Numero de identidad existente!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }


                    }
                    else
                    {
                        MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("¡Debe seleccionar un paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificación... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           


        }

        /// <summary>
        /// Valdar que ingrese todos los datos.
        /// </summary>
        /// <returns></returns>
        private bool Validar()
        {
            TextRange direccion = new TextRange(richTxtDireccion.Document.ContentStart, richTxtDireccion.Document.ContentEnd);
            //Validación que no permita campos vacíos.
            if (txtDni.Text != String.Empty && txtPrimerNombre.Text != String.Empty && txtPrimerApellido.Text != String.Empty 
                && direccion.Text != "\r\n" && txtCelular.Text != String.Empty && dtFechaNacimiento.SelectedDate != null
                && txtPeso.Text != String.Empty && txtEstatura.Text != String.Empty && cmbSexo.SelectedValue != null && cmbTipoSangre.SelectedValue != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que se encarga de la obtencion de valores
        /// </summary>
        private void ObtenerDatos()
        {
            TextRange direccion = new TextRange(richTxtDireccion.Document.ContentStart, richTxtDireccion.Document.ContentEnd);
            bool status;
            paciente.IdPaciente = idPaciente;
            paciente.NumeroIdentidad = txtDni.Text;
            paciente.PrimerNombre = txtPrimerNombre.Text;
            paciente.SegundoNombre = txtSegundoNombre.Text;
            paciente.PrimerApellido = txtPrimerApellido.Text;
            paciente.SegundoApellido = txtSegundoApellido.Text;
            paciente.Direccion = direccion.Text.Substring(0, direccion.Text.Length - 2);
            paciente.Celular = txtCelular.Text;
            paciente.FechaNacimiento = dtFechaNacimiento.SelectedDate.Value;
            paciente.Peso = float.Parse(Math.Round(float.Parse(txtPeso.Text), 2).ToString());
            paciente.Estatura = float.Parse(Math.Round(float.Parse(txtEstatura.Text), 2).ToString());
            if (estado == 1)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            paciente.Estado = status;
            paciente.IdTipoSangre = Convert.ToInt32(cmbTipoSangre.SelectedValue);
            paciente.IdSexo = Convert.ToInt32(cmbSexo.SelectedValue);

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validar()) // Los campos no deben estar vacios
                {
                    if (!seleccionado) // No puede agregar a un paciente existente.
                    {
                        if (!paciente.ExistePaciente(paciente, txtDni.Text)) // No puede asignar el DNI o identidad de un paciente ya registrado
                        {
                            if (txtCelular.Text.Length == 8) // El numero de telefno debe contener 8 dígitos.
                            {
                                if (txtDni.Text.Length == 13) // El DNI debe contener 13  dígitos.
                                {
                                    if (dtFechaNacimiento.SelectedDate <= DateTime.Now.Date) // La fecha de nacimiento debe ser mayor o igual a la fecha actual.
                                    {
                                        if (float.Parse(txtEstatura.Text) >= 45 && float.Parse(txtEstatura.Text) <= 250 ) // La estatura debe ser mayor o igual a 45, y menor o igual a 250.
                                        {
                                            if (float.Parse(txtPeso.Text) >= 5 && float.Parse(txtPeso.Text) <= 800) // El peso debe ser mayor o igual a 5, y menor o igual a 800.
                                            {

                                                
                                                ObtenerDatos();
                                                
                                                paciente.CrearPaciente(paciente);
                                                MessageBox.Show("El paciente se ha insertado con exito", "PACIENTE", MessageBoxButton.OK, MessageBoxImage.Information);
                                                paciente.VerPacientes(paciente, gridPacientes, Convert.ToInt32(cmbEstado.SelectedValue));
                                                LimpiarFormulario();
                                                OcultarColumnas();
                                                
                                                

                                            }
                                            else
                                            {
                                                MessageBox.Show("¡El peso debe estar dado entre 5 y 800 libras!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }


                                        }
                                        else
                                        {
                                            MessageBox.Show("¡La estatura debe estar dada entre 45 y 250 centimetros!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("¡La fecha de nacimiento no puede ser mayor a la fecha actual!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }


                                }
                                else
                                {
                                    MessageBox.Show("¡Numero de identidad incompleto, debe de contener 13 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("¡El numero de celular debe contener 8 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("¡El numero de identidad ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡No se puede agregar al mismo paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado) //Para poder eliminar un paciente de seleccinarlo primero.
            {
                ObtenerDatos();
                
                paciente.EliminarPaciente(paciente);
                paciente.VerPacientes(paciente, gridPacientes, Convert.ToInt32(cmbEstado.SelectedValue));
                LimpiarFormulario();
                OcultarColumnas();
                MessageBox.Show("El paciente se ha eliminado con exito", "PACIENTE", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("¡Debe seleccionar un paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            paciente.VerUnPaciente(paciente, gridPacientes, Convert.ToInt32(cmbEstado.SelectedValue), txtBuscar);
            OcultarColumnas();
        }

        private void btnCitas_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado) //Para ver las citas de un paciente de seleccinarlo primero.
            {
                CitasPaciente citasPaciente = new CitasPaciente(idPaciente); //Se envia el DNI del paciente para visualizar las citas correspondientes a dicho paciente.
                citasPaciente.ShowDialog(); //ShowDialog perimte volver a ventana de paciente una vez se cierre la ventana de CitasPaciente.
            }
            else
            {
                MessageBox.Show("¡Para ver las citas debes seleccionar un paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnConsultas_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado) //Para ver las consultas de un paciente de seleccinarlo primero.
            {
                ConsultasPaciente consultasPaciente = new ConsultasPaciente(idPaciente); //Se envia el DNI del paciente para visualizar las consultas correspondientes a dicho paciente.
                consultasPaciente.ShowDialog(); //ShowDialog perimte volver a ventana de paciente una vez se cierre la ventana de ConsultasPaciente.
            }
            else
            {
                MessageBox.Show("¡Para ver las consultas debes seleccionar un paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        //Al darle click que reestablesca el formulario como en un inició.
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            paciente.VerPacientes(paciente, gridPacientes, Convert.ToInt32(cmbEstado.SelectedValue));
            OcultarColumnas();
        }

        //Darle formato al data grid view
        private void gridPacientes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime)) //Si la columna es de tipo DateTime que le cambie el formato a fecha corta.
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.PropertyType == typeof(System.Double)) //Si la columna es de tipo Double o float que le cambie el formato o redondear a 2 cifras.
                (e.Column as DataGridTextColumn).Binding.StringFormat = "N2";

        }

     

        private void txtDni_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space) // No permite espacios
                    e.Handled = true; // Bloquea
                base.OnPreviewKeyDown(e);
            }
            catch
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void txtDni_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (caracter >= 48 && caracter <= 57) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch(Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void txtPrimerNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtPrimerNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                   || (caracter == 241 || caracter == 209)) // Codigo ASCII
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }

        private void txtSegundoNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtSegundoNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                   || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }

        private void txtPrimerApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtPrimerApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                    || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void txtSegundoApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtSegundoApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                    || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtCelular_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (caracter >= 48 && caracter <= 57) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }

        private void txtCelular_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

  

        private void txtPeso_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (caracter >= 48 && caracter <= 57 || caracter == 46) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

         
        }

        private void txtEstatura_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtEstatura_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (caracter >= 48 && caracter <= 57 || caracter == 46) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
     
        }

        private void txtPeso_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }
    }
}
