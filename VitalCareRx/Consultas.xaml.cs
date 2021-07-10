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
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace VitalCareRx
{


    /// <summary>
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : Window
    {

        SqlConnection sqlConnection;
        private Consulta consulta = new Consulta();
        private Receta receta = new Receta();
        private int codigoEmpleado, idRecetaMedica;
        private string nombreEmpleado;
        private bool seleccionado = false;

        private int idConsulta;

        public Consultas(int codigo, string empleado) // se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
            //y nombre del empleado(Se usa para volver al menu principal).
        {
            InitializeComponent();

            //Variables miembro

            codigoEmpleado = codigo;
            nombreEmpleado = empleado;
            string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            consulta.MostrarConsultas(dgConsultas);
            consulta.CargarCodigoCita(cmbCodigoCitas);

        }


        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidarCampos()) //El usuario no puede dejar campos vacios.
                {
                    if (!seleccionado)// El usuario no puede agregar una consulta que este seleccionando.
                    {
                        if (float.Parse(txtTemperatura.Text) >= 35 && float.Parse(txtTemperatura.Text) <= 40) //La temperatura tiene que ser mayor a 0.
                        {
                            if ((Convert.ToInt32(txtPresionArterial.Text.Substring(0, 3)) >= 100 && Convert.ToInt32(txtPresionArterial.Text.Substring(0, 3)) <= 250)
                               && (Convert.ToInt32(txtPresionArterial.Text.Substring(4)) >= 40 && Convert.ToInt32(txtPresionArterial.Text.Substring(4)) <= 150))
                            {
                                try
                                {
                                    ObtenerValores();

                                    consulta.CrearConsulta(consulta);

                                    LimpiarFormulario();

                                    MessageBox.Show("La consulta se ha insertado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

                                    consulta.MostrarConsultas(dgConsultas);

                                }
                                catch (Exception)
                                {


                                    MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El valor maximo de la presion sistolica es 250 y el valor minimo es 100\nEl valor maximo de la presion diastolica es 150 y el valor minimo es 40", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("¡La temperatura debe estar entre 35-40 C°!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                        MessageBox.Show("¡No se puede agregar la misma consulta!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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


        /// <summary>
        /// Se encarga de validar que NO se dejen vacios los campos del formulario
        /// </summary>
        /// <returns></returns>
        private bool ValidarCampos()
        {
            //Validar que todos los campos esten llenos.
            TextRange MotivoConsulta = new TextRange(rtxtMotivoConsulta.Document.ContentStart, rtxtMotivoConsulta.Document.ContentEnd);
            TextRange DiagnosticoConsulta = new TextRange(rtxtDiagnostico.Document.ContentStart, rtxtDiagnostico.Document.ContentEnd);

            if (MotivoConsulta.Text!= "\r\n" && DiagnosticoConsulta.Text != "\r\n"
                && txtTemperatura.Text != string.Empty && txtPresionArterial.Text != string.Empty && cmbCodigoCitas.SelectedValue != null) 
            {
                return true;
            }

            return false;
            
        }

        /// <summary>
        /// Metodo que se encarga de la obtencion de valores
        /// </summary>
        private void ObtenerValores()
        {
            TextRange MotivoConsulta = new TextRange(rtxtMotivoConsulta.Document.ContentStart, rtxtMotivoConsulta.Document.ContentEnd);
            TextRange DiagnosticoConsulta = new TextRange(rtxtDiagnostico.Document.ContentStart, rtxtDiagnostico.Document.ContentEnd);

            consulta.MotivoConsulta = MotivoConsulta.Text.Substring(0, MotivoConsulta.Text.Length - 2);
            consulta.DiagnosticoConsulta = DiagnosticoConsulta.Text.Substring(0, DiagnosticoConsulta.Text.Length - 2);
            consulta.Temperatura = float.Parse(txtTemperatura.Text);
            consulta.PresionArterial = txtPresionArterial.Text;
            consulta.IdEmpleado = codigoEmpleado;
            consulta.IdCita = Convert.ToInt32(cmbCodigoCitas.SelectedValue);

        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {

            rtxtMotivoConsulta.Document.Blocks.Clear();
            rtxtDiagnostico.Document.Blocks.Clear();
            txtTemperatura.Text = string.Empty;
            txtPresionArterial.Text = string.Empty;
            cmbCodigoCitas.SelectedValue = null;
            seleccionado = false;
            btnReceta.Content = "Receta";
            consulta.CargarCodigoCita(cmbCodigoCitas);
            txtBuscar.Clear();
        }


        /// <summary>
        /// Se encarga de traer todos los valores del data grid a los campos del formulario al darle click a una fila.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgConsultas_SelectionChanged(object sender, SelectionChangedEventArgs e) //Enviar información del grid a las textBox
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;

            if (rowSelected != null)// Si no esta seleccionado que no envie la información a las textBox
            {
                //Asiganamos contenido a todas las textBox segun la columna en base a la fila seleccionada
                seleccionado = true;
                TextRange MotivoConsulta = new TextRange(rtxtMotivoConsulta.Document.ContentStart, rtxtMotivoConsulta.Document.ContentEnd);
                TextRange DiagnosticoConsulta = new TextRange(rtxtDiagnostico.Document.ContentStart, rtxtDiagnostico.Document.ContentEnd);

                idConsulta = Convert.ToInt32(rowSelected.Row["Codigo de consulta"]); //Capturamos el id de la consulta para pasarlo por parametro al ver o crear una receta.

                MotivoConsulta.Text = rowSelected.Row["Motivo"].ToString();
                DiagnosticoConsulta.Text = rowSelected.Row["Diagnostico"].ToString();

                txtTemperatura.Text = rowSelected.Row["Temperatura"].ToString();
                txtPresionArterial.Text = rowSelected.Row["Presion arterial"].ToString();
                consulta.CargarCodigoCitaSeleccionar(Convert.ToInt32(rowSelected.Row["Codigo de consulta"]),cmbCodigoCitas);
                cmbCodigoCitas.SelectedValue = rowSelected.Row["Codigo de cita"].ToString();

                consulta.IdConsulta = Convert.ToInt32(rowSelected.Row["Codigo de consulta"]);

                if (receta.ValidarCrearRecetaMedica(idConsulta)) // si la consulta ya tiene una receta que capture el id de esa receta para posteriormente poder visualizar la receta de dicha consulta.
                {
                    btnReceta.Content = "Ver receta";
                    idRecetaMedica = consulta.CapturarIdRecetaMedica(idConsulta);

                }
                else
                {
                    btnReceta.Content = "   Crear receta";
                }

                

            }

            

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (seleccionado) // El usuario tiene que seleccionar una consulta para poder modificarla.  120/80
                {
                    if (ValidarCampos()) //El usuario no puede dejar campos vacios.
                    {
                        if (float.Parse(txtTemperatura.Text) >= 35 && float.Parse(txtTemperatura.Text) <= 40 ) //La temperatura tiene que estar entre 35-40 grados.
                        {
                            if ( (Convert.ToInt32(txtPresionArterial.Text.Substring(0, 3)) >= 100 && Convert.ToInt32(txtPresionArterial.Text.Substring(0,3)) <= 250 )
                               && (Convert.ToInt32(txtPresionArterial.Text.Substring(4)) >= 40 && Convert.ToInt32(txtPresionArterial.Text.Substring(4)) <= 150))
                            {
                                
                                ObtenerValores();

                                consulta.ModificarConsulta(consulta);

                                LimpiarFormulario();

                                MessageBox.Show("La consulta se ha modificado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);

                                consulta.MostrarConsultas(dgConsultas);
                            }
                            else
                            {
                                MessageBox.Show("El valor maximo de la presion sistolica es 250 y el valor minimo es 100\nEl valor maximo de la presion diastolica es 150 y el valor minimo es 40", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                           

                            
                        }
                        else
                        {
                            MessageBox.Show("¡La temperatura debe estar entre 35-40 C°!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("¡Debe seleccionar una consulta!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Ha ocurrido un error al momento de realizar la modificacion... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
            
        }

        //Buscar una consulta por nombre del paciente
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            consulta.Buscar(txtBuscar.Text, dgConsultas);
        }

       
        private void btnReceta_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                receta.RecetaMedica(idConsulta,idRecetaMedica,consulta);
                idRecetaMedica = consulta.CapturarIdRecetaMedica(idConsulta);
                Recetas recetas = new Recetas(idConsulta,idRecetaMedica);
                recetas.ShowDialog(); //ShowDialog perimte volver a ventana de consulta una vez se cierre la ventana de Recetas.
                btnReceta.Content = "Ver receta";
                
            }
            else
            {
                MessageBox.Show("Por favor seleccione una celda", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal(nombreEmpleado, codigoEmpleado); // Se regresa al menu principal con los datos del usuario actual.
            menu.Show();
            this.Close();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            consulta.MostrarConsultas(dgConsultas);

        }

        //Darle formato al data grid view
        private void dgConsultas_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.Double)) //Si la columna es de tipo Double o float que le cambie el formato o redondear a 2 cifras.
                (e.Column as DataGridTextColumn).Binding.StringFormat = "N2";
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

        private void txtTemperatura_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void txtTemperatura_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);
        }

        private void txtPresionArterial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // No permite espacios
                e.Handled = true; // Bloquea
            base.OnPreviewKeyDown(e);

        }

        private void txtPresionArterial_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (txtPresionArterial.Text.Length >= 0 && txtPresionArterial.Text.Length < 3)
                {
                    if (caracter >= 48 && caracter <= 57) // Codigo ASCII 
                        e.Handled = false;  // Permite 
                    else
                        e.Handled = true; // Bloquea
                }
                else if (txtPresionArterial.Text.Length == 3)
                {
                    if (caracter == 47) // Codigo ASCII 
                        e.Handled = false;  // Permite
                    else
                        e.Handled = true; // Bloquea
                }
                else if (txtPresionArterial.Text.Length >= 4 && txtPresionArterial.Text.Length < 7)
                {
                    if (caracter >= 48 && caracter <= 57) // Codigo ASCII 
                        e.Handled = false;  // Permite 
                    else
                        e.Handled = true; // Bloquea
                }
                else if (txtPresionArterial.Text.Length == 7)
                {
                    e.Handled = true; // Bloquea
                }
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }

        //cuando se suelta el click derecho
        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            right = false;
        }
    }
}
