using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Agregar los namespaces requeridos
using System.Configuration;
using System.Data.SqlClient;


namespace VitalCareRx
{
    class Farmaco
    {
        //Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["VitalCareRx.Properties.Settings.VitalCareRxConnectionString"].ConnectionString;

        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        public int IdFarmaco { get; set; }
        public string DescripcionFarmaco { get; set; }
        public string InformacionPrecaucion { get; set; }



        public Farmaco() { }

        public Farmaco(string descripcionFarmaco, string informacionFarmaco)
        {
            DescripcionFarmaco = descripcionFarmaco;
            InformacionPrecaucion = informacionFarmaco;
        }


        public void CrearFarmaco(Farmaco farmaco)
        {

            try
            {

                string query = @"INSERT INTO [Consultas].[Farmaco] VALUES (@descripcionFarmaco, @informacionPrecaucion)";

                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@descripcionFarmaco", farmaco.DescripcionFarmaco);
                sqlCommand.Parameters.AddWithValue("@informacionPrecaucion", farmaco.InformacionPrecaucion);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                sqlConnection.Close();


            }
        }

        public void ModificarFarmaco(Farmaco farmaco)
        {

            try
            {

                string query = @"UPDATE [Consultas].[Farmaco]
                                SET descripcionFarmaco = @descripcionFarmaco, informacionPrecaucion = @informacionPrecaucion
                                where idFarmaco = @idFarmaco";

                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);


                sqlCommand.Parameters.AddWithValue("@descripcionFarmaco", farmaco.DescripcionFarmaco);
                sqlCommand.Parameters.AddWithValue("@informacionPrecaucion", farmaco.InformacionPrecaucion);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", farmaco.IdFarmaco);


                sqlCommand.ExecuteNonQuery();
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();

            }
        }

        public Farmaco BuscarFarmaco(string nombreFarmacos)
        {
            Farmaco farmacos = new Farmaco();
            try
            {
                // Query de selección
                string query = @"SELECT * FROM [Consultas].[Farmaco] WHERE descripcionFarmaco LIKE CONCAT('%', @nombreFarmaco, '%')";


                // Establecer la conexión
                sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@nombreFarmaco", nombreFarmacos);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        // Obtener los valores del empleado si la consulta retorna valores
                        farmacos.IdFarmaco = Convert.ToInt32(rdr["idFarmaco"]);
                        farmacos.DescripcionFarmaco = rdr["descripcionFarmaco"].ToString();
                        farmacos.InformacionPrecaucion = rdr["informacionPrecaucion"].ToString();



                    }
                }

                // Retornar el usuario con los valores
                return farmacos;
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
