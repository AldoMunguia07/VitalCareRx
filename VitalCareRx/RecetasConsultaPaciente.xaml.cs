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
    /// Lógica de interacción para RecetasConsultaPaciente.xaml
    /// </summary>
    public partial class RecetasConsultaPaciente : Window
    {

        private int consulta;
        Receta receta = new Receta();
        public RecetasConsultaPaciente(int codigoConsulta) //Se recibe por paramtero el codigo de la consulta para ver la receta que le corresponde.
        {
            InitializeComponent();
            
            consulta = codigoConsulta;
            receta.MostrarFarmacos(gridRecetas,consulta);
        }


        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("¿Desea regresar al formulario anterior?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
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
    }
}
