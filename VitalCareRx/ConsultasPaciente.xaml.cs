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
    /// Lógica de interacción para ConsultasPaciente.xaml
    /// </summary>
    public partial class ConsultasPaciente : Window
    {
        private int codigoConsulta;
        private int idPaciente;        
        private bool seleccionado = false;

        Consulta consulta = new Consulta();
        public ConsultasPaciente(int codigoPaciente) //Se le pasa por parametro el id del paciente para ver las consultas correspondientes a dicho paciente.
        {
            InitializeComponent();
            
            idPaciente = codigoPaciente;
            consulta.MostrarConsultasPaciente(gridConsultas, idPaciente);
            
        }


        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("¿Desea regresar al formulario anterior?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        //Evento para capturar el ide la consulta
        private void gridConsultas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;

            if (rowSelected != null) // Si no esta seleccionado que no capture el id
            {
                seleccionado = true;
                codigoConsulta = Convert.ToInt32(rowSelected.Row["Codigo de consulta"]); // Capturamos el id de consulta para enviarlo como parametro al llamar a ventana de recetas.
            }
            else
            {
                seleccionado = false;
            }
        }

        private void btnRecetas_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionado) //Para ver las recetas de una consulta primero debe seleccionarla.
            {
                RecetasConsultaPaciente recetasConsultaPaciente = new RecetasConsultaPaciente(codigoConsulta); //Se envia el codigo de consulta para llamar ala receta de dicha consulta.
                recetasConsultaPaciente.ShowDialog();  //ShowDialog perimte volver a ventana de ConsultasPaciente una vez se cierre la ventana de RecetasConsultaPaciente.
            }
            else
            {
                MessageBox.Show("¡Para ver una receta debe seleccionar una consulta!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        //Darle formato al data grid view
        private void gridConsultas_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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

        //cuando se suelta el click derecho
        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            right = false;
        }
    }
}
