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

namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para Inventario.xaml
    /// </summary>
    public partial class Inventario : Window
    {
        Farmaco unFarmaco = new Farmaco();
        Validaciones Validaciones = new Validaciones();
        bool seleccionado = false;
        DateTime fechaV;
        public Inventario(Farmaco farmaco)
        {
            InitializeComponent();

            unFarmaco = farmaco;
            lblFarmaco.Content = farmaco.DescripcionFarmaco;
            unFarmaco.MostrarDetalleFarmaco(gridDetalleFarmaco, unFarmaco.IdFarmaco);
            
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea regresar al formulario anterior?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private bool validar()
        {
            if(txtCantidad.Text != String.Empty && dtFecha.SelectedDate.Value != null)
            {
                return true;
            }

            return false;
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (validar())
            {
                if (!seleccionado)
                {
                    if (dtFecha.SelectedDate.Value > DateTime.Now.AddDays(7))
                    {
                        if (Convert.ToInt32(txtCantidad.Text) >= 1 && Convert.ToInt32(txtCantidad.Text) <= 10000)
                        {
                            if (unFarmaco.ExisteLote(unFarmaco.IdFarmaco, dtFecha.SelectedDate.Value))
                            {
                                unFarmaco.SumarCantidad(unFarmaco.IdFarmaco, dtFecha.SelectedDate.Value, Convert.ToInt32(txtCantidad.Text));
                                Limpiar();
                                MessageBox.Show("¡Se sumo cantidad ingresada a lote ya existente!", "Inventario", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                unFarmaco.AgregarCantidad(unFarmaco.IdFarmaco, dtFecha.SelectedDate.Value, Convert.ToInt32(txtCantidad.Text));
                                Limpiar();
                                MessageBox.Show("¡Lote agregado exitosamente!", "Inventario", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                           
                        }
                        else
                        {
                            MessageBox.Show(String.Format("¡La cantidad debe estar dada entr 1 y 10,000!"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("¡La fecha de vencimiento no puede estar dentro de los próximos 7 días!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
                else
                {
                    MessageBox.Show(String.Format("¡No puede agregar el mismo lote de farmacos!"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }               
            }
            else
            {
                MessageBox.Show("¡Debe llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }



        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            
            if (seleccionado)
            {
                if (validar())
                {
                    if (dtFecha.SelectedDate.Value > DateTime.Now.AddDays(7))
                    {
                        if (Convert.ToInt32(txtCantidad.Text) >= 1 && Convert.ToInt32(txtCantidad.Text) <= 10000)
                        {
                            unFarmaco.ModificarCantidad(unFarmaco.IdFarmaco, fechaV, dtFecha.SelectedDate.Value, Convert.ToInt32(txtCantidad.Text));
                            Limpiar();
                            MessageBox.Show("¡Lote modificado exitosamente!", "Inventario", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show(String.Format("¡La cantidad debe estar dada entr 1 y 10,000!"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show(String.Format("¡La fecha de vencimiento no puede estar dentro de los próximos 7 días!"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    MessageBox.Show("¡Debe llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(String.Format("¡Debe seleccionar un lote!"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado)
            {
                MessageBoxResult result = MessageBox.Show("¿Desea eliminar el lote?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    unFarmaco.EliminarDetalle(unFarmaco.IdFarmaco, dtFecha.SelectedDate.Value);
                    Limpiar();
                }
            }
            else
            {
                MessageBox.Show(String.Format("¡Debe seleccionar un lote!"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }


        private void Limpiar()
        {
            seleccionado = false;
            dtFecha.SelectedDate = null;
            dtFechaBuscar.SelectedDate = null;
            txtCantidad.Clear();
            fechaV = DateTime.Now;
            unFarmaco.MostrarDetalleFarmaco(gridDetalleFarmaco, unFarmaco.IdFarmaco);
          

        }


        private void gridDetalleFarmaco_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime)) //Si la columna es de tipo DateTime que le cambie el formato a fecha corta.
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        private void gridDetalleFarmaco_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender; // Instancia de objeto tipo DataaGrid
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;


            if (rowSelected != null) // Si no esta seleccionado que no envie la información a las textBox
            {
                seleccionado = true;
                dtFecha.SelectedDate = Convert.ToDateTime(rowSelected.Row["Fecha de vencimiento"]);
                fechaV = Convert.ToDateTime(rowSelected.Row["Fecha de vencimiento"]);
                txtCantidad.Text = rowSelected.Row["Cantidad"].ToString();
                
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
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

        private void dtFechaBuscar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            unFarmaco.MostrarDetalleFarmacoFiltro(gridDetalleFarmaco, unFarmaco.IdFarmaco, dtFechaBuscar);
        }

        private void txtCantidad_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Validaciones.ValidarEspacio(e);
        }

        private void txtCantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Validaciones.SoloNumeros(e);
        }
    }
}
