using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;

namespace VitalCareRx
{
    class Empleado
    {

        //Variables miembro

        private static string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;

        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        public int IdEmpleado { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Celular { get; set; }

        public int IdSexo { get; set; }

        public string NombreUsuario { get; set; }

        public string Contrasenia { get; set; }

        public Empleado() { }

        public Empleado(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string celular, int idSexo, string nombreUsuario, string contrasenia )
        {
            PrimerNombre = primerNombre;
            SegundoNombre = segundoNombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Celular = celular;
            IdSexo = idSexo;
            NombreUsuario = nombreUsuario;
            Contrasenia = contrasenia;


        }

        public Empleado BuscarEmpleado(string nombreUsuario)
        {
            Empleado empleado = new Empleado();
            try
            {
                // Query de selección
                string query = @"SELECT * FROM [Personas].[Empleado] WHERE nombreUsuario = @user";


                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@user", nombreUsuario);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        // Obtener los valores del empleado si la consulta retorna valores
                        empleado.IdEmpleado = Convert.ToInt32(rdr["idEmpleado"]);
                        empleado.PrimerNombre = rdr["primerNombre"].ToString();
                        empleado.SegundoNombre = rdr["segundoNombre"].ToString();
                        empleado.PrimerApellido = rdr["primerApellido"].ToString();
                        empleado.SegundoApellido = rdr["segundoApellido"].ToString();
                        empleado.Celular = rdr["celular"].ToString();
                        empleado.IdSexo = Convert.ToInt32(rdr["idSexo"]);
                        empleado.NombreUsuario = rdr["nombreUsuario"].ToString();
                        empleado.Contrasenia = rdr["contrasenia"].ToString();


                    }
                }

                // Retornar el usuario con los valores
                return empleado;
            }
            catch (Exception ex)
            {
                throw ex;
                
                
            }
            finally
            {
                // Cerrar la conexión
                sqlConnection.Close();
            }
        }

    }
}
