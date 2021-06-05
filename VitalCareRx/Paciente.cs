using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace VitalCareRx
{
    class Paciente
    {
        //Variables miembro

        private static string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        //Propiedades
        public int IdPaciente { get; set; }
        public string NumeroIdentidad { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Direccion { get; set; }

        public string Celular { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public float Peso { get; set; }

        public float Estatura { get; set; }

        public bool Estado { get; set; }

        public int IdTipoSangre { get; set; }

        public int IdSexo { get; set; }


        // Constructores
        public Paciente() { }

        public Paciente(int idPaciente,string identidad, string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string direccion, string celular,
                        DateTime fechaNacimeiento, float peso, float estatura, bool estado, int idTipoSangre, int idSexo)
        {
            IdPaciente = idPaciente;
            NumeroIdentidad = identidad;
            PrimerNombre = primerNombre;
            SegundoNombre = segundoNombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Direccion = direccion;
            FechaNacimiento = fechaNacimeiento;
            Peso = peso;
            Estatura = estatura;
            Estado = estado;
            IdTipoSangre = idTipoSangre;
            IdSexo = idSexo;       
        
        }

        // Metodos
        public void CrearPaciente(Paciente paciente)
        {          

            try
            {
                //Query para añadir un paciente
                string query = @"INSERT INTO [Personas].[Paciente] VALUES (@numeroIdentidad,@primerNombre,@segundoNombre,@primerApellido,@segundoApellido,@direccion, @celular,@fechaNacimiento,
						@peso,@estatura,@estado, @idTipoSangre,@idSexo)";

                // Abrir la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);


                // Establecer los valores de los parámetros

                sqlCommand.Parameters.AddWithValue("@numeroIdentidad", paciente.NumeroIdentidad);
                sqlCommand.Parameters.AddWithValue("@primerNombre", paciente.PrimerNombre);
                sqlCommand.Parameters.AddWithValue("@segundoNombre", paciente.SegundoNombre);
                sqlCommand.Parameters.AddWithValue("@primerApellido", paciente.PrimerApellido);
                sqlCommand.Parameters.AddWithValue("@segundoApellido", paciente.SegundoApellido);
                sqlCommand.Parameters.AddWithValue("@direccion", paciente.Direccion);
                sqlCommand.Parameters.AddWithValue("@celular", paciente.Celular);
                sqlCommand.Parameters.AddWithValue("@fechaNacimiento", paciente.FechaNacimiento);
                sqlCommand.Parameters.AddWithValue("@peso", paciente.Peso);
                sqlCommand.Parameters.AddWithValue("@estatura", paciente.Estatura);
                sqlCommand.Parameters.AddWithValue("@idTipoSangre", paciente.IdTipoSangre);
                sqlCommand.Parameters.AddWithValue("@idSexo", paciente.IdSexo);
                sqlCommand.Parameters.AddWithValue("@estado", paciente.Estado);



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
        /// Metodo para actualizar un paciente.
        /// </summary>
        /// <param name="paciente"></param>
        public void ActualizarPaciente(Paciente paciente)
        {
            try
            {
                //Query para modificar un paciente
                string query = @"UPDATE [Personas].[Paciente] 
                                SET numeroIdentidad = @numeroIdentidad, primerNombre = @primerNombre, segundoNombre = @segundoNombre, primerApellido = @primerApellido,
                                segundoApellido = @segundoApellido, direccion = @direccion, celular = @celular, fechaNacimiento = @fechaNacimiento, peso = @peso,
                                estatura = @estatura, idTipoSangre = @idTipoSangre, idSexo = @idSexo, estado = @estado
                                WHERE idPaciente = @idPaciente";

                // Abrir la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idPaciente", paciente.IdPaciente);
                sqlCommand.Parameters.AddWithValue("@numeroIdentidad", paciente.NumeroIdentidad);
                sqlCommand.Parameters.AddWithValue("@primerNombre", paciente.PrimerNombre);
                sqlCommand.Parameters.AddWithValue("@segundoNombre", paciente.SegundoNombre);
                sqlCommand.Parameters.AddWithValue("@primerApellido", paciente.PrimerApellido);
                sqlCommand.Parameters.AddWithValue("@segundoApellido", paciente.SegundoApellido);
                sqlCommand.Parameters.AddWithValue("@direccion", paciente.Direccion);
                sqlCommand.Parameters.AddWithValue("@celular", paciente.Celular);
                sqlCommand.Parameters.AddWithValue("@fechaNacimiento", paciente.FechaNacimiento);
                sqlCommand.Parameters.AddWithValue("@peso", paciente.Peso);
                sqlCommand.Parameters.AddWithValue("@estatura", paciente.Estatura);
                sqlCommand.Parameters.AddWithValue("@idTipoSangre", paciente.IdTipoSangre);
                sqlCommand.Parameters.AddWithValue("@idSexo", paciente.IdSexo);
                sqlCommand.Parameters.AddWithValue("@estado", paciente.Estado);

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
        /// Metodo para eliminar un paciente(Cambiar estado).
        /// </summary>
        /// <param name="paciente"></param>
        public void EliminarPaciente(Paciente paciente)
        {
            try
            {
                //Query para modificar un paciente
                string query = @"UPDATE [Personas].[Paciente] 
                                SET estado = 0
                                WHERE numeroIdentidad = @numeroIdentidad";

                // Abrir la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@numeroIdentidad", paciente.NumeroIdentidad);
               
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
