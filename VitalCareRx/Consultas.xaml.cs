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
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : Window
    {

        
        private Consulta consulta = new Consulta();
        private Receta receta = new Receta();
        private int  idRecetaMedica;
        
        private bool seleccionado = false;
        Validaciones validaciones = new Validaciones();
        LlenarComboBox LlenarComboBox = new LlenarComboBox();
        Empleado miEmpleado = new Empleado();
        private int idConsulta;

        public Consultas(Empleado empleado) // se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
            //y nombre del empleado(Se usa para volver al menu principal).
        {
            InitializeComponent();

            //Variables miembro

            miEmpleado = empleado;
            consulta.MostrarConsultas(dgConsultas);
            LlenarComboBox.CargarPaciente(cmbPaciente);

           

        }

        /// <summary>
        /// Ocultar columnas del DataGridView
        /// </summary>
        private void OcultarColumnas()
        {
            dgConsultas.Columns[0].Visibility = Visibility.Hidden;
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

                                    

                                    consulta.MostrarConsultas(dgConsultas);
                                    LimpiarFormulario();

                                    MessageBox.Show("La consulta se ha insertado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);



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
                && txtTemperatura.Text != string.Empty && txtPresionArterial.Text != string.Empty && cmbPaciente.SelectedValue != null) 
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
            consulta.IdEmpleado = miEmpleado.IdEmpleado;
            consulta.IdPaciente = Convert.ToInt32(cmbPaciente.SelectedValue);

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
            cmbPaciente.SelectedValue = null;
            seleccionado = false;
            btnReceta.Content = "Receta";
            txtBuscar.Clear();
            OcultarColumnas();
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
                txtPresionArterial.Text = rowSelected.Row["Presión arterial"].ToString();
                cmbPaciente.SelectedValue = rowSelected.Row["idPaciente"].ToString();

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

                              

                                consulta.MostrarConsultas(dgConsultas);

                               

                                LimpiarFormulario();


                                MessageBox.Show("La consulta se ha modificado con exito", "CONSULTA", MessageBoxButton.OK, MessageBoxImage.Information);
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
            OcultarColumnas();
        }

       
        private void btnReceta_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                
                receta.RecetaMedica(idConsulta,idRecetaMedica,consulta, miEmpleado);
                idRecetaMedica = consulta.CapturarIdRecetaMedica(idConsulta);
                Recetas recetas = new Recetas(idConsulta, idRecetaMedica, miEmpleado);
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

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            
            consulta.MostrarConsultas(dgConsultas);
            LimpiarFormulario();

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
            validaciones.NumerosDecimales(e);
        }

        private void txtTemperatura_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtPresionArterial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);

        }

        private void txtPresionArterial_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (txtPresionArterial.Text.Length >= 0 && txtPresionArterial.Text.Length < 3)
                {
                    validaciones.SoloNumeros(e);
                }
                else if (txtPresionArterial.Text.Length == 3)
                {
                    validaciones.SoloPleca(e);
                }
                else if (txtPresionArterial.Text.Length >= 4 && txtPresionArterial.Text.Length < 7)
                {
                    validaciones.SoloNumeros(e);
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

        private void dgConsultas_Loaded(object sender, RoutedEventArgs e)
        {
            OcultarColumnas();
        }
    }
}
