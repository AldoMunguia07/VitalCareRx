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

namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para Informacion.xaml
    /// </summary>
    public partial class Informacion : Window
    {
        private int IdEmpleado;
        private string nombreCompletoEmpleado;

        public Informacion(int codigoEmpleado,  string nombreEmpleado)
        {
            InitializeComponent();
            IdEmpleado = codigoEmpleado;
            nombreCompletoEmpleado = nombreEmpleado;
    }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuprincipal = new MenuPrincipal(nombreCompletoEmpleado, IdEmpleado);
            menuprincipal.Show();
            this.Close();
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
