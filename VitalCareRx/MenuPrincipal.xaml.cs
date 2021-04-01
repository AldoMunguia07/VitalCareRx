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

namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {


        public MenuPrincipal(string usuario)
        {
            InitializeComponent();
            lbUsuario.Content = usuario; 
            
        }


        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("¿Desea cerrar sesión?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);


        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        private void ltPacientes_Selected(object sender, RoutedEventArgs e)
        {
          //  Pacientes pacientes = new Pacientes();
            //pacientes.Show();
        }

        private void ltConsultas_Selected(object sender, RoutedEventArgs e)
        {
            Consultas consultas = new Consultas();
            consultas.Show();
        }

        private void ltFarmacos_Selected(object sender, RoutedEventArgs e)
        {
            //Farmacos farmacos= new Farmacos();
            //farmacos.Show();
        }

        private void ltUsuario_Selected(object sender, RoutedEventArgs e)
        {
            //MiUsuario miusuario = new MiUsuario();
            //miusuario.Show();
        }

        private void ltSalida_Selected(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea cerrar sesión?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            } else if(result == MessageBoxResult.No)
            {
                
                
            }
           

         
        }
    }
}
