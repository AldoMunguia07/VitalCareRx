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

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace VitalCareRx
{
    /// <summary>
    /// Interaction logic for RecuperarContrasenia.xaml
    /// </summary>
    public partial class RecuperarContrasenia : Window
    {
        RecuperarContra recuperar = new RecuperarContra();
        public RecuperarContrasenia()
        {
            InitializeComponent();
            
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea regresar al inicio de sesión?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();

            }
        }

        private void btnRecuperar_Click(object sender, RoutedEventArgs e)
        {
            recuperar.Recuperar(txtCorreo);
        }
    }
}
