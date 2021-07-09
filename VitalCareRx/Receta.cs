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
    class Receta
    {
        //Variables miembro

        Conexion conexion = new Conexion();


        public int IdReceta {get; set;}

        public int IdConsulta { get; set; }

        public int IdFarmaco { get; set; }

        public int Cantidad { get; set; }

        public string DuracionTratamiento { get; set; }

        public string Indicaciones { get; set; }

        

        // Constructores

        public Receta() { }

        public Receta(int idReceta, int idConsulta,int idFarmaco, int cantidad, string duracionTratamiento, string indicaciones) 
        {
            IdReceta = idReceta;
            IdConsulta = idConsulta;
            IdFarmaco = idFarmaco;
            Cantidad = cantidad;
            DuracionTratamiento = duracionTratamiento;
            Indicaciones = indicaciones;
        }

        

        /// <summary>
        /// Inserta farmacos a una receta medica
        /// </summary>
        /// <param name="receta">Es un objeto de tipo receta" <param>
        public void AgregarFarmacoAReceta(Receta receta)
        {
            try
            {
                // Query de selección
                string query = @"INSERT INTO [Consultas].[DetalleRecetaMedica] VALUES (@idRecetaMedica,@idFarmaco,@cantidad,@duracion, @indicacion)";

                // Establecer la conexión
                conexion.sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, conexion.sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idRecetaMedica", receta.IdReceta);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", receta.IdFarmaco);
                sqlCommand.Parameters.AddWithValue("@cantidad", receta.Cantidad);
                sqlCommand.Parameters.AddWithValue("@duracion", receta.DuracionTratamiento);
                sqlCommand.Parameters.AddWithValue("@indicacion", receta.Indicaciones);


                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // Cerrar la conexión
                conexion.sqlConnection.Close();
            }
        }

        /// <summary>
        /// Eliminar un farmaco de una receta.
        /// </summary>
        /// <param name="receta"></param>
        public void EliminarFarmacoReceta(Receta receta)
        {
            try
            {
                string query = @"DELETE [Consultas].[DetalleRecetaMedica] WHERE idRecetaMedica = @idRecetaMedica AND idFarmaco = @idFarmaco";

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, conexion.sqlConnection);

                sqlCommand.Parameters.AddWithValue("@idRecetaMedica", receta.IdReceta);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", receta.IdFarmaco);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conexion.sqlConnection.Close();
            }
            


        }

        /// <summary>
        /// Metodo para modificar un farmaco de una receta.
        /// </summary>
        /// <param name="receta"></param>
        public void ModificarFarmacoReceta(Receta receta)
        {
            try
            {
                string query = @"UPDATE [Consultas].[DetalleRecetaMedica]
                                SET idFarmaco = @idFarmaco, cantidad = @cantidad , duracionTratamiento = @duracion, indicaciones = @indicacion
                                WHERE idRecetaMedica = @idRecetaMedica and idFarmaco = @idFarmaco
                                ";

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, conexion.sqlConnection);

                sqlCommand.Parameters.AddWithValue("@idRecetaMedica", receta.IdReceta);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", receta.IdFarmaco);
                sqlCommand.Parameters.AddWithValue("@cantidad", receta.Cantidad);
                sqlCommand.Parameters.AddWithValue("@duracion", receta.DuracionTratamiento);
                sqlCommand.Parameters.AddWithValue("@indicacion", receta.Indicaciones);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
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
