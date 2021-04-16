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

        private int estado = 0;
        bool cargado = false;
        bool seleccionado = false;
        private string dni;


        SqlConnection sqlConnection;

        Paciente paciente = new Paciente();

        public Pacientes(int codigo, string empleado)
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            btnEstado.Background = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0)); // Por defecto el botón de estado se inicializa en color rojo
            nombreEmpleado = empleado;
            codigoEmpleado = codigo;

            MostrarPacientes();
            CargarComboBoxSexo();
            CargarComboBoxEstado();
            CargarComboBoxTipoSangre();
        }



        /// <summary>
        /// Trae todos los clientes de la base de datos al iniciar el programa.
        /// </summary>
        public void MostrarPacientes()
        {
            string query = @"SELECT P.numeroIdentidad Identidad,primerNombre,segundoNombre,PrimerApellido,segundoApellido, P.idTipoSangre, P.idSexo,
                            CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido) Paciente,
                            P.direccion Direccion, P.celular Celular, P.fechaNacimiento 'Fecha de nacimiento', P.peso 'Peso (lbs)', P.estatura 'Estatura (cm)', P.estado Estado,
                            T.descripcionTipoSangre 'Tipo de sangre', S.descripcionSexo Sexo
                            FROM [Personas].[Paciente] P INNER JOIN [Personas].[TipoSangre] T
                            ON P.idTipoSangre = T.idTipoSangre
                            INNER JOIN [Personas].[Sexo] S
                            ON P.idSexo = S.idSexo
                            WHERE P.estado = @estado
                            ";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlCommand.Parameters.AddWithValue("@estado", cmbEstado.SelectedValue);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    gridPacientes.ItemsSource = dataTable.DefaultView;

                    gridPacientes.IsReadOnly = true; // El grid es de solo lectura.


                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Metodo para buscar un paciente
        /// </summary>
        public void MostrarUnPaciente()
        {
            string query = @"SELECT P.numeroIdentidad Identidad,primerNombre,segundoNombre,PrimerApellido,segundoApellido, P.idTipoSangre, P.idSexo,
                            CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido) Paciente,
                            P.direccion Direccion, P.celular Celular, P.fechaNacimiento 'Fecha de nacimiento', P.peso 'Peso (lbs)', P.estatura 'Estatura (cm)', P.estado Estado,
                            T.descripcionTipoSangre 'Tipo de sangre', S.descripcionSexo Sexo
                            FROM [Personas].[Paciente] P INNER JOIN [Personas].[TipoSangre] T
                            ON P.idTipoSangre = T.idTipoSangre
                            INNER JOIN [Personas].[Sexo] S
                            ON P.idSexo = S.idSexo
                            WHERE P.estado = @estado and CONCAT(P.primerNombre, ' ', P.segundoNombre, ' ', P.primerApellido, ' ', P.segundoApellido) LIKE CONCAT('%', @nombrePaciente,'%')
                           
                            ";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlCommand.Parameters.AddWithValue("@estado", cmbEstado.SelectedValue);
            sqlCommand.Parameters.AddWithValue("@nombrePaciente", txtBuscar.Text); // Filtro de nombre del paciente

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    gridPacientes.ItemsSource = dataTable.DefaultView;

                    gridPacientes.IsReadOnly = true; //El grid es de solo lectura, el usuario no podrá modificar lo que este contenga.


                }

            }
            catch (Exception)
            {

                throw;
            }
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

        }

        private void gridPacientes_Loaded(object sender, RoutedEventArgs e)
        {
            OcultarColumnas();
            cargado = true;
        }



        private void cmbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MostrarPacientes();
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
            estado = 0;
            CargarColorBoton();
            cmbTipoSangre.SelectedValue = null;
            cmbSexo.SelectedValue = null;
            seleccionado = false;
            dni = string.Empty;
            txtBuscar.Clear();

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
            if (seleccionado) // Para modificar un paciente primero debe seleccionarlo
            {
                
                if (Validar()) // Los campos no deben estar vacios
                {

                    if (!ExistePaciente() || dni == txtDni.Text) // No puede asignar el DNI o identidad de un paciente ya registrado
                    {
                        if(txtCelular.Text.Length == 8) // El numero de telefno debe contener 8 dígitos.
                        {
                            if(txtDni.Text.Length == 13) // El DNI debe contener 13  dígitos.
                            {
                                if (dtFechaNacimiento.SelectedDate <= DateTime.Now.Date) // La fecha de nacimiento debe ser mayor o igual a la fecha actual.
                                {
                                    if (float.Parse(txtEstatura.Text) > 0) // La estatura debe ser mayor 0.
                                    {
                                        if (float.Parse(txtPeso.Text) > 0) // El peso debe ser mayor a 0.
                                        {
                                            try
                                            {
                                                ObtenerDatos();

                                                paciente.ActualizarPaciente(paciente);
                                                MostrarPacientes();
                                                LimpiarFormulario();
                                                OcultarColumnas();
                                                MessageBox.Show("El paciente se ha modificado con exito", "PACIENTE", MessageBoxButton.OK, MessageBoxImage.Information);
                                            }
                                            catch (Exception)
                                            {

                                                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificación... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("¡No puede tener una peso menor o igual a 0 libras!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("¡No puede tener una estatura menor o igual a 0 centimetros!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        /// <summary>
        /// Valdar que ingrese todos los datos.
        /// </summary>
        /// <returns></returns>
        private bool Validar()
        {
            TextRange direccion = new TextRange(richTxtDireccion.Document.ContentStart, richTxtDireccion.Document.ContentEnd);
            //Validación que no permita campos vacíos.
            if (txtDni.Text != String.Empty && txtPrimerNombre.Text != String.Empty && txtSegundoNombre.Text != String.Empty && txtPrimerApellido.Text !=
                String.Empty && txtSegundoApellido.Text != String.Empty && direccion.Text != "\r\n" && txtCelular.Text != String.Empty && dtFechaNacimiento.SelectedDate != null
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
            if (Validar()) // Los campos no deben estar vacios
            {
                if (!seleccionado) // No puede agregar a un paciente existente.
                {
                    if (!ExistePaciente()) // No puede asignar el DNI o identidad de un paciente ya registrado
                    {
                        if (txtCelular.Text.Length == 8) // El numero de telefno debe contener 8 dígitos.
                        {
                            if (txtDni.Text.Length == 13) // El DNI debe contener 13  dígitos.
                            {
                                if (dtFechaNacimiento.SelectedDate <= DateTime.Now.Date) // La fecha de nacimiento debe ser mayor o igual a la fecha actual.
                                {
                                    if (float.Parse(txtEstatura.Text) > 0) // La estatura debe ser mayor 0.
                                    {
                                        if (float.Parse(txtPeso.Text) > 0) // El peso debe ser mayor a 0.
                                        {

                                            try
                                            {
                                                ObtenerDatos();

                                                paciente.CrearPaciente(paciente);
                                                MessageBox.Show("El paciente se ha insertado con exito", "PACIENTE", MessageBoxButton.OK, MessageBoxImage.Information);
                                                MostrarPacientes();
                                                LimpiarFormulario();
                                                OcultarColumnas();
                                            }
                                            catch (Exception)
                                            {

                                                MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                            }

                                        }
                                        else
                                        {
                                            MessageBox.Show("¡No puede tener una peso menor o igual a 0 libras!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }


                                    }
                                    else
                                    {
                                        MessageBox.Show("¡No puede tener una estatura menor o igual a 0 centimetros!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("¡Numero de identidad existente!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado) //Para poder eliminar un paciente de seleccinarlo primero.
            {
                ObtenerDatos();
                paciente.EliminarPaciente(paciente);
                MostrarPacientes();
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
            MostrarUnPaciente();
            OcultarColumnas();
        }

        private void btnCitas_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado) //Para ver las citas de un paciente de seleccinarlo primero.
            {
                CitasPaciente citasPaciente = new CitasPaciente(dni); //Se envia el DNI del paciente para visualizar las citas correspondientes a dicho paciente.
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
                ConsultasPaciente consultasPaciente = new ConsultasPaciente(dni); //Se envia el DNI del paciente para visualizar las consultas correspondientes a dicho paciente.
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
            MostrarPacientes();
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

        /// <summary>
        /// Metodo para verificar si existe o no un paciente.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool ExistePaciente()
        {
            try
            {

                string query = @"SELECT numeroIdentidad FROM [Personas].[Paciente] WHERE [numeroIdentidad] = @identidad";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@identidad", txtDni.Text);

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);



                    if (dataTable.Rows.Count == 1)  //Si existe que devuelva un true
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
    }
}
