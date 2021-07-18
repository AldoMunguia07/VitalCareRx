using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions; // Para validar qeu el correo este correcto.

namespace VitalCareRx
{
     class Validaciones
    {

        public void SoloNumeros(TextCompositionEventArgs e)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(e.Text));

                if (caracter >= 48 && caracter <= 57) // Codigo ASCII 
                    e.Handled = false;  // Permite 
                else
                    e.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }


        public void NumerosDecimales(TextCompositionEventArgs a)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(a.Text));

                if (caracter >= 48 && caracter <= 57 || caracter == 46) // Codigo ASCII 
                    a.Handled = false;  // Permite 
                else
                    a.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void ValidarEspacio(KeyEventArgs a)
        {
                if (a.Key == Key.Space)
                {
                    a.Handled = true; 
                }   
        }
       
        public void Sololetras(TextCompositionEventArgs a)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(a.Text));


                if ((caracter >= 65 && caracter <= 90) || (caracter >= 97 && caracter <= 122)
                   || (caracter == 241 || caracter == 209)) // Codigo ASCII 
                    a.Handled = false;  // Permite 
                else
                    a.Handled = true; // Bloquea
            }
            catch (Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SoloPleca(TextCompositionEventArgs a)
        {
            int caracter = Convert.ToInt32(Convert.ToChar(a.Text));
            try
            {
                if (caracter == 47) // Codigo ASCII 
                    a.Handled = false;  // Permite
                else
                    a.Handled = true; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        public void CaracteresInecesarios(TextCompositionEventArgs a)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(a.Text));
                if (caracter >= 33 && caracter <= 47 || caracter == 64)
                {
                    a.Handled = true;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NoNegativo(TextCompositionEventArgs a)
        {
            try
            {
                int caracter = Convert.ToInt32(Convert.ToChar(a.Text));

                if (caracter == 45) // Codigo ASCII 
                    a.Handled = true;  // No Permite 
                else
                    a.Handled = false; // Bloquea
            }
            catch (Exception)
            {

                MessageBox.Show("El caracter Ingresado no es correcto!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Verifica que el correo electronico estebe bien escrito o no
        /// </summary>
        /// <param name="correo">El correo a evaluar</param>
        /// <returns></returns>
        public Boolean Email_Correcto(String correo)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(correo, expresion))
            {
                if (Regex.Replace(correo, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
