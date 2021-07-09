using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces requeridos
using System.Configuration;
using System.Data.SqlClient;

namespace VitalCareRx
{
    class Consulta
    {
        //Variables miembro

        Conexion conexion = new Conexion();

        public int IdConsulta {get; set;}
        public string MotivoConsulta { get; set; }
        public string DiagnosticoConsulta { get; set; }
        public float Temperatura { get; set; }
        public string PresionArterial { get; set; }
        public int IdEmpleado { get; set; }
        public int IdCita { get; set; }

        public Consulta() { }

        public Consulta(string motivoConsulta, string diagnosticoConsulta, float temperatura, string presionArterial, int idEmpleado, int idCita ) 
        {
            MotivoConsulta = motivoConsulta;
            DiagnosticoConsulta = diagnosticoConsulta;
            Temperatura = temperatura;
            PresionArterial = presionArterial;
            IdEmpleado = idEmpleado;
            IdCita = idCita;

        }

        /// <summary>
        /// Metodo para crear una consulta.
        /// </summary>
        /// <param name="consulta"></param>
        public void CrearConsulta(Consulta consulta)
        {

            try
            {

                string query = @"INSERT INTO [Consultas].[Consulta] VALUES (@motivoConsulta,@diagnosticoConsulta,@temperatura,@presionArterial,@idEmpleado,@idCita)";

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, conexion.sqlConnection);

                sqlCommand.Parameters.AddWithValue("@motivoConsulta", consulta.MotivoConsulta);
                sqlCommand.Parameters.AddWithValue("@diagnosticoConsulta", consulta.DiagnosticoConsulta);
                sqlCommand.Parameters.AddWithValue("@temperatura", consulta.Temperatura);
                sqlCommand.Parameters.AddWithValue("@presionArterial", consulta.PresionArterial);
                sqlCommand.Parameters.AddWithValue("@idEmpleado", consulta.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@idCita", consulta.IdCita);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                conexion.sqlConnection.Close();

                
            }
        }

        /// <summary>
        /// Metodo para modificar una consulta.
        /// </summary>
        /// <param name="consulta"></param>
        public void ModificarConsulta(Consulta consulta)
        {

            try
            {

                string query = @"UPDATE [Consultas].[Consulta]
                                SET motivoConsulta = @motivoConsulta, diagnosticoConsulta = @diagnosticoConsulta,temperatura = @temperatura, 
                                presionArterial = @presionArterial, idEmpleado = @idEmpleado, idCita = @idCita
                                WHERE idConsulta = @idConsulta";

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, conexion.sqlConnection);

                sqlCommand.Parameters.AddWithValue("@motivoConsulta", consulta.MotivoConsulta);
                sqlCommand.Parameters.AddWithValue("@diagnosticoConsulta", consulta.DiagnosticoConsulta);
                sqlCommand.Parameters.AddWithValue("@temperatura", consulta.Temperatura);
                sqlCommand.Parameters.AddWithValue("@presionArterial", consulta.PresionArterial);
                sqlCommand.Parameters.AddWithValue("@idEmpleado", consulta.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@idCita", consulta.IdCita);
                sqlCommand.Parameters.AddWithValue("@idConsulta", consulta.IdConsulta);

                sqlCommand.ExecuteNonQuery();
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conexion.sqlConnection.Close();

            }
        }






    }
}
