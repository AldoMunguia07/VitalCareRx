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

        public void AgregarCantidad(int idFarmaco, DateTime fechaVencimiento, int cantidad)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;
                
                sqlCommand.Parameters.AddWithValue("@idFarmaco", idFarmaco);
                sqlCommand.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);
                sqlCommand.Parameters.AddWithValue("@cantidad", cantidad);
                sqlCommand.Parameters.AddWithValue("@accion", "InsertarDetalle");
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

        public void MostrarDetalleFarmaco(DataGrid dataGrid, int idFarmaco)
        {

            try
            {
                

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", idFarmaco);
                sqlCommand.Parameters.AddWithValue("@accion", "MostrarDetalleFarmaco");

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
           
        }

        public void MostrarDetalleFarmacoFiltro(DataGrid dataGrid, int idFarmaco, DatePicker fecha)
        {

            try
            {


                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", idFarmaco);
                sqlCommand.Parameters.AddWithValue("@fechaVencimiento", fecha.SelectedDate);

                sqlCommand.Parameters.AddWithValue("@accion", "MostrarDetalleFarmacoFiltro");

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

        }

        public void ModificarCantidad(int idFarmaco, DateTime fechaVencimiento, DateTime fechaVencimiento2, int cantidad)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idFarmaco", idFarmaco);
                sqlCommand.Parameters.AddWithValue("@fechaVencimiento2", fechaVencimiento2);
                sqlCommand.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);                
                sqlCommand.Parameters.AddWithValue("@cantidad", cantidad);
                sqlCommand.Parameters.AddWithValue("@accion", "ActualizarDetalle");
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

        public void EliminarDetalle(int idFarmaco, DateTime fechaVencimiento)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idFarmaco", idFarmaco);              
                sqlCommand.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);                
                sqlCommand.Parameters.AddWithValue("@accion", "EliminarDetalle");
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

        public void SumarCantidad(int idFarmaco, DateTime fechaVencimiento, int cantidad)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idFarmaco", idFarmaco);               
                sqlCommand.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);
                sqlCommand.Parameters.AddWithValue("@cantidad", cantidad);
                sqlCommand.Parameters.AddWithValue("@accion", "SumarCantidad");
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

        public bool ExisteLote(int idFarmaco, DateTime fechaVencimiento)
        {
            try
            {

                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Farmacos", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@idFarmaco", idFarmaco);
                    sqlCommand.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento); 
                    sqlCommand.Parameters.AddWithValue("@accion", "VerificarFecha");

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);


                    if (dataTable.Rows.Count != 0)  //Si existe que devuelva un true
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
