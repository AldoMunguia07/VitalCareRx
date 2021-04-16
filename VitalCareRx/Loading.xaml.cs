using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {

        
        private Timer timer;
        private int a = 0;
        private string str;
        private string usuarioActual;
        private int codigoEmpleado;

        public Loading(string empleado, int codigo)
        {
            InitializeComponent();
            usuarioActual = empleado;
            codigoEmpleado = codigo;
            timer = new Timer(35);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            /*             
             Para el funcionamiento correcto de nuestra ProgressBar Utilizamos la librería “System.Timers” esta nos permitió aplicar un tiempo determinado de carga en la ProgressBar, 
            la cual al llegar a 100% finaliza la ejecución del programa. Gracias a la librería anteriormente mencionada se creó el evento Timer_Elapsed, el cual nos permitió aplicar 
            la funcionalidad anteriormente mencionada a la ProgressBar            
             
             */
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher.invoke?view=net-5.0


        }

        /// <summary>
        /// Metodo para Visualizar el progreso de la ProgressBar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*
              invocamos el método this.Dispatcher.Invoke el cual nos permitió aplicar dichas condiciones dentro de este, y realizar dichas acciones con ayuda del timer, el cual tendrá
            un contador que finalizara hasta llegar al valor máximo establecido en  “Loading.xaml”, en caso de que termine la primera condición se procede a parar el timer, y se abrirá 
            la siguiente interfaz.             
             */

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => //Invocación de metodo dispatcher 
            {
                if (BAR.Value < 30)
                {
                    a += 1;
                    str = a + "%";
                    lbLoad.Content = str;
                    BAR.Value += 0.3;
                    
                }
                else
                {

                    timer.Stop();
                    MenuPrincipal menupincipal = new MenuPrincipal(usuarioActual, codigoEmpleado);
                    menupincipal.Show();
                    this.Close();
                }
            }));
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
