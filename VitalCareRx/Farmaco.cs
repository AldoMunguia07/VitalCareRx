using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Agregar los namespaces requeridos
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;

namespace VitalCareRx
{
    public class Farmaco
    {
        //Variables miembro
        Conexion conexion = new Conexion();
        AportesControl aportes = new AportesControl();
       

        public int IdFarmaco { get; set; }
        public int IdEmpleado { get; set; }
        public string DescripcionFarmaco { get; set; }
        public string InformacionPrecaucion { get; set; }



        public Farmaco() { }

        public Farmaco(string descripcionFarmaco, string informacionFarmaco)
        {
            DescripcionFarmaco = descripcionFarmaco;
            InformacionPrecaucion = informacionFarmaco;
        }



        /// <summary>
        /// Trae todas las consultas de la base de datos al inicial el programa.
        /// </summary>
        public void MostrarFarmaco(DataGrid dataGrid)
        {

            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlCommand.Parameters.AddWithValue("@accion", "Mostrar");

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    dataGrid.ItemsSource = dataTable.DefaultView;

                    dataGrid.IsReadOnly = true; // El grid es de solo lectura.
                }

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
        /// Metodo para crear un farmaco.
        /// </summary>
        /// <param name="farmaco"></param>
        public void CrearFarmaco(Farmaco farmaco)
        {

            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@descripcionFarmaco", farmaco.DescripcionFarmaco);
                sqlCommand.Parameters.AddWithValue("@informacionPrecaucion", farmaco.InformacionPrecaucion);
                sqlCommand.Parameters.AddWithValue("@accion", "Insertar");
                aportes.ContextoSesion(IdEmpleado, conexion.sqlConnection);
                
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
        /// Metodo para modificar un farmaco.
        /// </summary>
        /// <param name="farmaco"></param>
        public void ModificarFarmaco(Farmaco farmaco)
        {

            try
            {

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;


                sqlCommand.Parameters.AddWithValue("@descripcionFarmaco", farmaco.DescripcionFarmaco);
                sqlCommand.Parameters.AddWithValue("@informacionPrecaucion", farmaco.InformacionPrecaucion);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", farmaco.IdFarmaco);
                sqlCommand.Parameters.AddWithValue("@accion", "Modificar");
                aportes.ContextoSesion(IdEmpleado, conexion.sqlConnection);
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

        /// <summary>
        /// Metodo para buscar un farmaco.
        /// </summary>
        /// <param name="nombreFarmacos"></param>
        /// <returns></returns>
        public Farmaco BuscarFarmaco(string nombreFarmacos, DataGrid datagrid)
        {
            Farmaco farmacos = new Farmaco();
            try
            {

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@descripcionFarmaco", nombreFarmacos);
                sqlCommand.Parameters.AddWithValue("@accion", "Buscar");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    datagrid.ItemsSource = dataTable.DefaultView;

                    datagrid.IsReadOnly = true; // El grid es de solo lectura.

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
                conexion.sqlConnection.Close();
            }
        }

    }
}
