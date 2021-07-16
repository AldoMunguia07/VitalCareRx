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
    }
}
