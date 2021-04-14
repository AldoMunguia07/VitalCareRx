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

            btnEstado.Background = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0));
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

                    gridPacientes.IsReadOnly = true;


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
            sqlCommand.Parameters.AddWithValue("@nombrePaciente", txtBuscar.Text);

            try
            {
                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    gridPacientes.ItemsSource = dataTable.DefaultView;

                    gridPacientes.IsReadOnly = true;


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal(nombreEmpleado, codigoEmpleado);
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

        private void gridPacientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;
            TextRange direccion = new TextRange(richTxtDireccion.Document.ContentStart, richTxtDireccion.Document.ContentEnd);
            
           if (rowSelected != null)
            {
                seleccionado = true;
                dni = rowSelected.Row["Identidad"].ToString(); ;
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
                

                CargarColorBoton();
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
           


        }

        /// <summary>
        /// Cargar el color al boton del estado.
        /// </summary>
        private void CargarColorBoton()
        {
            if (estado == 1)
            {
                btnEstado.Background = new SolidColorBrush(Color.FromArgb(165, 42, 165, 42));
            }
            else
            {
                btnEstado.Background = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0));
            }
        }

        private void btnEstado_Click(object sender, RoutedEventArgs e)
        {
            
            if (estado == 0)
            {
                btnEstado.Background = new SolidColorBrush(Color.FromArgb(165, 42, 165, 42));
                estado = 1;
            }
            else
            {
                
                btnEstado.Background = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0));
                estado = 0;
            }

        }

        private void btnModificarr_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                if (Validar())
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
                    catch (Exception )
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

            if (txtDni.Text != String.Empty && txtPrimerNombre.Text != String.Empty && txtSegundoNombre.Text != String.Empty && txtPrimerApellido.Text !=
                String.Empty && txtSegundoApellido.Text != String.Empty && direccion.Text != "\r\n" && txtCelular.Text != String.Empty && dtFechaNacimiento.SelectedDate != null
                && txtPeso.Text != String.Empty && txtEstatura.Text != String.Empty && cmbSexo.SelectedValue != null && cmbTipoSangre.SelectedValue != null )
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
            if (Validar())
            {
                if (!seleccionado)
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
                    MessageBox.Show("¡No se puede agregar el mismo paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
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
            if (seleccionado)
            {
                CitasPaciente citasPaciente = new CitasPaciente(dni);
                citasPaciente.ShowDialog();
            }
            else
            {
                MessageBox.Show("¡Para ver las citas debes seleccionar un paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void btnConsultas_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                ConsultasPaciente consultasPaciente = new ConsultasPaciente(dni);
                consultasPaciente.ShowDialog();
            }
            else
            {
                MessageBox.Show("¡Para ver las consultas debes seleccionar un paciente!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            MostrarPacientes();
            OcultarColumnas();
        }

        private void gridPacientes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            if (e.PropertyType == typeof(System.Double))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "N2";

        }
    }
}
