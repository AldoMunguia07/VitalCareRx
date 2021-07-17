using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Windows;

namespace VitalCareRx
{
    class AportesControl
    {
        //Variables miembro
        Conexion conexion = new Conexion();
        
        public void MostrarBitacora(DataGrid dataGrid)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlCommand.Parameters.AddWithValue("@accion", "MostrarBitacora");

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
        
        public void MostrarBitacoraFiltro(DataGrid dataGrid, DatePicker date, ComboBox cmbEmpleado )
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlCommand.Parameters.AddWithValue("@accion", "MostrarBitacoraParametros");
                sqlCommand.Parameters.AddWithValue("@fecha", date.SelectedDate);
                sqlCommand.Parameters.AddWithValue("@idEmpleado", Convert.ToInt32(cmbEmpleado.SelectedValue));

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
        public void MostrarBitacoraFiltroAmbos(DataGrid dataGrid, DatePicker date, ComboBox cmbEmpleado)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlCommand.Parameters.AddWithValue("@accion", "MostrarBitacoraParametrosAND");
                sqlCommand.Parameters.AddWithValue("@fecha", date.SelectedDate);
                sqlCommand.Parameters.AddWithValue("@idEmpleado", Convert.ToInt32(cmbEmpleado.SelectedValue));

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


        public void ContextoSesion( int idEmpleado, SqlConnection connection)
        {
            try
            {
                
                SqlCommand sqlCommand = new SqlCommand("sp_usuarioActual", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@value", idEmpleado);
                sqlCommand.Parameters.AddWithValue("@key", "user_id");
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
         
        }


        public void RegistrarEntrada(Empleado empleado)
        {

            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@HoraEntrada", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@accion", "InsertarEntrada");
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

        public void RegistrarSalida(Empleado empleado)
        {

            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@HoraSalida", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@idControlEmpleado", CapturarControl(empleado));
                sqlCommand.Parameters.AddWithValue("@accion", "InsertarSalida");
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

        public int CapturarControl(Empleado empleado)
        {

            try
            {
               

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@accion", "CapturarIdControl");
              

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable data = new DataTable();
                    sqlDataAdapter.Fill(data);
                    return (Convert.ToInt32(data.Rows[0]["idControlEmpleado"]));
                }

            }
            catch (Exception e)
            {

                throw e;
            }
           

        }

        public string MensajeHorasTrabajadas(Empleado empleado)
        {

            try
            {

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;


                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@accion", "mensajeFecha");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable data = new DataTable();
                    sqlDataAdapter.Fill(data);
                    return data.Rows[0]["mensaje"].ToString();
                }
               
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public bool validarEntrada(Empleado empleado)
        {
            try
            {

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;


                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@accion", "validarEntradaSalida");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable data = new DataTable();
                    sqlDataAdapter.Fill(data);

                    if (data.Rows.Count != 0)
                    {
                        return false;
                    }

                    return true;
                    
                }

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool validarSalida(Empleado empleado)
        {
            try
            {

                SqlCommand sqlCommand = new SqlCommand("sp_Aportes", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;


                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@accion", "validarEntradaSalida");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable data = new DataTable();
                    sqlDataAdapter.Fill(data);

                    
                    if (data.Rows[0]["fechaSalida"].ToString() == String.Empty)
                    {
                        return true;
                    }

                    return false;
                }

            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
