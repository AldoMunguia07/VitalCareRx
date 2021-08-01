using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Windows.Controls;
using System.Windows;

namespace VitalCareRx
{
    class RecuperarContra
    {
        Conexion conexion = new Conexion();
        AportesControl aportes = new AportesControl();
        public void Recuperar(TextBox txtCorreo)
        {
            try
            {

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@correo", txtCorreo.Text);
                sqlCommand.Parameters.AddWithValue( "@accion", "buscarUsuarioCorreo");
        

                DataTable data = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {

                    sqlDataAdapter.Fill(data);
                }
                if (data.Rows.Count != 0)
                {
                  
                    conexion.sqlConnection.Close();
                    GenerarNewPass(txtCorreo.Text, Convert.ToInt32(data.Rows[0]["idEmpleado"]));
                }
                else
                {
                    MessageBox.Show("Correo no encontrado");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.sqlConnection.Close();
            }
        }

        private void GenerarNewPass(string correo, int id)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            string newPass = random.Next(100000000, 999999999).ToString();

            try
            {

                conexion.sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("sp_restaurarPassword", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idEmpleado", id);
                sqlCommand.Parameters.AddWithValue("@Contrasenia", newPass);
                aportes.ContextoSesion(id, conexion.sqlConnection);
                if (sqlCommand.ExecuteNonQuery() != 0)
                {
                    EnviarEmail(newPass, correo);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.sqlConnection.Close();
            }
        }

        private void EnviarEmail(string newPass, string email)
        {
            string contrasenia = "health1234";
            string mensaje = string.Empty;

            string destinatario = email;
            string remitente = "vitalcarerx504@gmail.com";
            string asunto = "Nueva contraseña Vital Care Rx";
            string message = String.Format("Su nueva contraseña es {0}", newPass);

            MailMessage ms = new MailMessage(remitente, destinatario, asunto, message);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(remitente, contrasenia);

            try
            {
                Task.Run(() =>
                {
                    smtpClient.Send(ms);
                    ms.Dispose();
                    MessageBox.Show("¡Correo enviado exitosamente, revise su correo por favor!", "Restaurar", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                );

                MessageBox.Show("Este proceso puede tardar unos segundos, por favor espere", "Restaurar", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ha ocurrido un error al momento de enviar el correo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


    }
}
