using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces requeridos
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace VitalCareRx
{
    class Cita
    {
        //variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        //Propiedades
        public int IdCita { get; set; }
        public DateTime FechaCita { get; set; }
        public string Notas { get; set; }
        public string NumeroIdentidad { get; set; }

        // Constructores
        public Cita() { }
        public Cita(int idCita, DateTime fechaCita, string notas, string numeroIdentidad)
        {
            IdCita = idCita;
            FechaCita = fechaCita;
            Notas = notas;
            NumeroIdentidad = numeroIdentidad;     
        }

        // Metodos
        /// <summary>
        /// Metodo para agregar citas.
        /// </summary>
        /// <param name="cita"></param>
        public void AgregarCita(Cita cita)
        {
            try
            {
                //Query para añadir una cita al paciente
                string query = @"INSERT INTO [Consultas].[Cita] VALUES (@fecha,@notas,@numeroIdentidad)";

                // Abrir la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@fecha", cita.FechaCita);
                sqlCommand.Parameters.AddWithValue("@notas", cita.Notas);
                sqlCommand.Parameters.AddWithValue("@numeroIdentidad", cita.NumeroIdentidad);



                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Metodo para modificar cita.
        /// </summary>
        /// <param name="cita"></param>
        public void ModificarCita(Cita cita)
        {
            try
            {
                //Query para añadir una cita al paciente
                string query = @"UPDATE [Consultas].[Cita]
                                SET fechaCita = @fecha, notas = @notas
                                WHERE idCita = @idCita";

                // Abrir la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idCita", cita.IdCita);
                sqlCommand.Parameters.AddWithValue("@fecha", cita.FechaCita);
                sqlCommand.Parameters.AddWithValue("@notas", cita.Notas);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
