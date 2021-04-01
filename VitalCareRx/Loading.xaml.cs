﻿using System;
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
        public Loading(string empleado)
        {
            InitializeComponent();
            timer = new Timer(100);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        /// <summary>
        /// Metodo para Visualizar el progreso de la ProgressBar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                if (BAR.Value < 30)
                {
                    a += 1;
                    str = a + "%";
                    lbLoad.Content = str;
                    //lbPorciento.Visibility = Visibility;
                    BAR.Value += 0.3;

                }
                else
                {

                    timer.Stop();
                    MenuPrincipal menupincipal = new MenuPrincipal();
                    menupincipal.Show();
                    this.Close();
                }
            }));
        }


    }
}