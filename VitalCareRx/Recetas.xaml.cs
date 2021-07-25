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
       
        private int codigoConsulta;
        private bool validarSeleccionado;
        private int codigoRecetaMedica;
        private int codigoFarmaco;
        Validaciones validaciones = new Validaciones();
        LlenarComboBox LlenarComboBox = new LlenarComboBox();
        private Empleado miEmpleado = new Empleado();
        private Receta receta = new Receta();

       
        List<DetalleReceta> farmacos = new List<DetalleReceta>();

        public Recetas(int idConsulta, int idRecetaMedica, Empleado empleado) // Recibe como paramtero el id de la consulta y el id de la receta (La informacion de este formulario se guarda en una tabla detalle)
        {
            InitializeComponent();
            codigoConsulta = idConsulta;
            codigoRecetaMedica = idRecetaMedica;
            miEmpleado = empleado;
            LlenarComboBox.CargarFarmacos(cmbFarmacos);
            //receta.MostrarFarmacos(dgRecetas, codigoConsulta);
            receta.IdEmpleado = miEmpleado.IdEmpleado;
            validarSeleccionado = false;
            OcultarColumnas();
        }

        private void OcultarColumnas()
        {
            dgRecetas.Columns[0].Visibility = Visibility.Hidden;
            dgRecetas.Columns[1].Visibility = Visibility.Hidden;
        }

        private void AgregarDetalle()
        {
            var detalleReceta = new DetalleReceta
            {
                IdReceta = receta.IdReceta,
                IdFarmaco = receta.IdFarmaco,
                Farmaco = cmbFarmacos.Text,
                Cantidad = int.Parse(txtCantidad.Text),
                DuracionTratamiento = receta.DuracionTratamiento,
                Indicaciones = receta.Indicaciones
            };

            

            farmacos.Add(new DetalleReceta
            {
                IdReceta = receta.IdReceta,
                IdFarmaco = receta.IdFarmaco,
                Farmaco = cmbFarmacos.Text,
                Cantidad = int.Parse(txtCantidad.Text),
                DuracionTratamiento = receta.DuracionTratamiento,
                Indicaciones = receta.Indicaciones
            });

            dgRecetas.Items.Add(detalleReceta);
            LimpiarFormulario();
            OcultarColumnas();
        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (ValidarCampos()) // El usuario no tiene que dejar los campos vacios.
                {
                    if (!validarSeleccionado) // El usuario no tiene que agregar un farmaco que este seleccionando.
                    {
                       
                        if (int.Parse(txtCantidad.Text) > 0 && int.Parse(txtCantidad.Text) <= 25) // La cantidad de farmacos debe ser mayor a 0 y menor a 25
                        {
                            if (receta.VerificarCantidad(Convert.ToInt32(cmbFarmacos.SelectedValue)) >= int.Parse(txtCantidad.Text))
                            {
                                try
                                {

                                    if (farmacos.Count == 0)
                                    {
                                        ObtenerValores();
                                        AgregarDetalle();
                                        MessageBox.Show("Farmaco agreado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        bool encontrado = false;
                                        foreach (DetalleReceta detalle in farmacos)
                                        {

                                            if (Convert.ToInt32(cmbFarmacos.SelectedValue) == detalle.IdFarmaco)
                                            {
                                                encontrado = true;
                                                break;

                                            }

                                        }

                                        if (!encontrado)
                                        {
                                            ObtenerValores();
                                            AgregarDetalle();
                                            MessageBox.Show("Farmaco agreado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show("El farmaco ya a sido agregado a la receta medica de la consulta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                            LimpiarFormulario();
                                        }
                                    }


                                    LimpiarFormulario();

                                }
                                catch (InvalidOperationException ex)
                                {

                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Ha ocurrido un error al momento de realizar la insercción... Favor intentelo de nuevo mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format("¡Cantidad insuficiente del farmaco {0}\nactualmente se cuenta con {1} unidades!", cmbFarmacos.Text,
                                receta.VerificarCantidad(Convert.ToInt32(cmbFarmacos.SelectedValue))),  "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                    int indice = dgRecetas.SelectedIndex;

                    dgRecetas.Items.RemoveAt(indice);
                    farmacos.RemoveAt(indice);


                    LimpiarFormulario();


                    MessageBox.Show("Farmaco eliminado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("¡Es requerido que seleccione un farmaco!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (validarSeleccionado)
            {
                receta.IdFarmaco = Convert.ToInt32(cmbFarmacos.SelectedValue);


            }
            else 
            {
                receta.IdFarmaco = Convert.ToInt32(cmbFarmacos.SelectedValue);
            }
            
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
            MessageBoxResult result = MessageBox.Show("¿Desea regresar al formulario anterior?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
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
            dgRecetas.SelectedValue = false;
            
        }

        //Evento para enviar la información del grid a las textBox
        private void dgRecetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           
            int indice = dgRecetas.SelectedIndex;
            TextRange indicaciones = new TextRange(rtxtIndicaciones.Document.ContentStart, rtxtIndicaciones.Document.ContentEnd);
            TextRange durationTratamiento = new TextRange(rtxtDuracionTratamiento.Document.ContentStart, rtxtDuracionTratamiento.Document.ContentEnd);

            if (indice > -1) // Si no esta seleccionado que no capture el id
            {
                validarSeleccionado = true;

                codigoFarmaco = farmacos[indice].IdFarmaco;
                cmbFarmacos.SelectedValue = farmacos[indice].IdFarmaco;
                txtCantidad.Text = farmacos[indice].Cantidad.ToString();
                durationTratamiento.Text = farmacos[indice].DuracionTratamiento;
                indicaciones.Text = farmacos[indice].Indicaciones;




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

                        bool encontrado = false;
                        foreach (DetalleReceta detalle in farmacos)
                        {

                            if (Convert.ToInt32(cmbFarmacos.SelectedValue) == detalle.IdFarmaco)
                            {
                                encontrado = true;
                                break;

                            }

                        }

                        if (!encontrado || codigoFarmaco == Convert.ToInt32(cmbFarmacos.SelectedValue)) // Valida si el farmaco no esta en la receta actual y si es el codigo de farmaco seleccionado, con esa
                        {                                                                                             // condición pasara a la siguiente validación
                            if (int.Parse(txtCantidad.Text) > 0 && int.Parse(txtCantidad.Text) <= 25) // La cantidad de farmacos debe ser mayor a 0 y menor a 25
                            {
                                if (receta.VerificarCantidad(Convert.ToInt32(cmbFarmacos.SelectedValue)) >= int.Parse(txtCantidad.Text))
                                {
                                    int indice = dgRecetas.SelectedIndex;
                                    ObtenerValores();
                                    dgRecetas.Items.RemoveAt(indice);
                                    farmacos.RemoveAt(indice);


                                    AgregarDetalle();

                                    LimpiarFormulario();



                                    MessageBox.Show("Farmaco modificado exitosamente", "Farmaco", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("¡Cantidad insuficiente del farmaco {0}\nactualmente se cuenta con {1} unidades!", cmbFarmacos.Text,
                                    receta.VerificarCantidad(Convert.ToInt32(cmbFarmacos.SelectedValue))), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        MessageBox.Show("Debe llenar todos los campus", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }



                }
                else
                {
                    MessageBox.Show("¡Es requerido que seleccione un farmaco!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            //receta.MostrarFarmacos(dgRecetas, codigoConsulta);
        }

   

        private void rtxtDuracionTratamiento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.NoNegativo(e);
           
        }

        private void txtCantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.SoloNumeros(e);
        }

        private void txtCantidad_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void btnGenerarReceta_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea generar la receta?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (dgRecetas.Items.Count > 0)
                {
                    //Ciclo que recorre la lista producto para ingresar cada producto del detalle
                    foreach (var detalleR in farmacos)
                    {

                        receta.IdReceta = detalleR.IdReceta;
                        receta.IdFarmaco = detalleR.IdFarmaco;
                        receta.Cantidad = detalleR.Cantidad;
                        receta.DuracionTratamiento = detalleR.DuracionTratamiento;
                        receta.Indicaciones = detalleR.Indicaciones;
                        receta.AgregarFarmacoAReceta(receta);
                        receta.retirarInventario(receta.IdFarmaco, receta.Cantidad);
                    }

                    MessageBox.Show("Receta generada exitosamente", "Receta", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }

            
        }

        private void dgRecetas_CurrentCellChanged(object sender, EventArgs e)
        {

        }
    }  
}
