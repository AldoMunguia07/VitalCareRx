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

// Agregar los namespaces requeridos
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace VitalCareRx
{
    /// <summary>
    /// Lógica de interacción para MiUsuario.xaml
    /// </summary>
    public partial class MiUsuario : Window
    {
        
       

        Empleado miUsuario = new Empleado();
        private string usuario;
        Validaciones validaciones = new Validaciones();
        LlenarComboBox LlenarComboBox = new LlenarComboBox();
        

        public MiUsuario(Empleado empleado)// se recibe por parametro el codigo (Para ver que empleado realizo esa consulta y tambien se usa para volver al menu principal) 
                                      
        {
            InitializeComponent();
            miUsuario = empleado;
            miUsuario.CargarTextBox(miUsuario.IdEmpleado, txtUsuario, txtPassword, txtPrimerNombre, txtSegundoNombre, txtPrimerApellido, txtSegundoApellido, txtCelular, txtCorreo, dtpFechaNacimiento, cmbSexo);
            LlenarComboBox.CargarComboBoxSexo(cmbSexo);
            miUsuario.PrimerNombre = txtPrimerNombre.Text;
            miUsuario.PrimerApellido = txtPrimerApellido.Text;
            usuario = txtUsuario.Text; // se asigna el valor de la txtUsuario para la posteiror validacion (Que no permita ingresar un usuario existente).
        }


        /// <summary>
        /// Metodo para obtener valores del formulario
        /// </summary>
        private void ObtenerDatos()
        {
            miUsuario.IdEmpleado = miUsuario.IdEmpleado;
            miUsuario.PrimerNombre = txtPrimerNombre.Text;
            miUsuario.SegundoNombre = txtSegundoNombre.Text;
            miUsuario.PrimerApellido = txtPrimerApellido.Text;
            miUsuario.SegundoApellido = txtSegundoApellido.Text;
            miUsuario.Celular = txtCelular.Text;
            miUsuario.Correo = txtCorreo.Text;
            miUsuario.FechaNacimiento = dtpFechaNacimiento.SelectedDate.Value;
            miUsuario.IdSexo = Convert.ToInt32(cmbSexo.SelectedValue);
            miUsuario.NombreUsuario = txtUsuario.Text;
            miUsuario.Contrasenia = txtPassword.Text;
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

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea regresar al menú principal?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (miUsuario.IdPuesto == 1)
                {
                    MenuPrincipalAdmin menuPrincipalAdmin = new MenuPrincipalAdmin(miUsuario);
                    menuPrincipalAdmin.Show();
                    this.Close();
                }
                else if (miUsuario.IdPuesto == 2)
                {
                    MenuPrincipal menupincipal = new MenuPrincipal(miUsuario);
                    menupincipal.Show();
                    this.Close();
                }
            }
            
        }

        /// <summary>
        /// Metodo para validar que no se dejen campos en blancos.
        /// </summary>
        /// <returns>Boolean</returns>
        private bool Validar()
        {
            if(txtPrimerNombre.Text != String.Empty &&  txtPrimerApellido.Text !=
                String.Empty && txtCelular.Text != String.Empty && txtUsuario.Text !=
                String.Empty && txtPassword.Text != String.Empty && cmbSexo.SelectedValue != null && txtCorreo.Text != String.Empty
                && dtpFechaNacimiento.SelectedDate != null)
            {
                return true;
            }
            return false;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validar()) //El usuario no debe dejar los campos en blanco.
                {
                    if (!miUsuario.ExisteUsuario(txtUsuario.Text) || usuario == txtUsuario.Text) // Valida si el nombre usuario no existe y si es el nombre de usurio es del empleado actual, con esa condición pasa a la siguiente validación
                    {
                        if (txtUsuario.Text.Length >= 5)
                        {
                            if (txtCelular.Text.Length == 8) // El numero de telefono debe contener 8 dígitos.
                            {
                                if (txtPassword.Text.Length >= 8) // el campo contraseña debe tener 8 o más caracteres
                                {

                                    ObtenerDatos();
                                    miUsuario.ModificarEmpleado(miUsuario);
                                    MessageBox.Show("¡Datos actualizados correctamente!", "USUARIO", MessageBoxButton.OK, MessageBoxImage.Information);
                                    usuario = txtUsuario.Text;

                                }
                                else
                                {
                                    MessageBox.Show("¡La contraseña debe contener almenos 8 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("¡El numero de celular debe contener 8 digitos!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("¡El nombre de usuario debe contener almenos 5 caracteres!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡El nombre de usuario ya existe!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtUsuario.Text = usuario;
                    }


                }
                else
                {
                    MessageBox.Show("¡Es requerido llenar todos los campos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            
        }

        private void txtPrimerNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
          
        }

        private void txtPrimerNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtSegundoNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtSegundoNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtPrimerApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtPrimerApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtSegundoApellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.Sololetras(e);
        }

        private void txtSegundoApellido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtCelular_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaciones.SoloNumeros(e);
          
        }

        private void txtCelular_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

        private void txtUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            validaciones.ValidarEspacio(e);
        }

   
    }
}
