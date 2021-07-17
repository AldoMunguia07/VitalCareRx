using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Controls;

namespace VitalCareRx
{
    class LlenarComboBox
    {
        Conexion conexion = new Conexion();

        public void CargarComboBoxEstado(ComboBox comboBox)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_LlenarComboBox", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "CargarEstado");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    comboBox.DisplayMemberPath = "estado";
                    comboBox.SelectedValuePath = "id";
                    comboBox.ItemsSource = dataTable.DefaultView;

                }
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

        public void CargarComboBoxSexo(ComboBox comboBox)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_LlenarComboBox", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "CargarSexo");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    comboBox.DisplayMemberPath = "descripcionSexo";
                    comboBox.SelectedValuePath = "idSexo";
                    comboBox.ItemsSource = dataTable.DefaultView;

                }
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

        public void CargarComboBoxTipoSangre(ComboBox comboBox)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_LlenarComboBox", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "CargarTipoSangre");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    comboBox.DisplayMemberPath = "descripcionTipoSangre";
                    comboBox.SelectedValuePath = "idTipoSangre";
                    comboBox.ItemsSource = dataTable.DefaultView;

                }
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
        /// Metodo para cargar el ComboBox farmacos con información.
        /// </summary>
        public void CargarFarmacos(ComboBox comboBox)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_LlenarComboBox", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "CargarFarmacos");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    comboBox.DisplayMemberPath = "descripcionFarmaco";
                    comboBox.SelectedValuePath = "idFarmaco";
                    comboBox.ItemsSource = dataTable.DefaultView;

                }
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

        public void CargarEmpleado(ComboBox comboBox)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_LlenarComboBox", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "cargarEmpleados");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    comboBox.DisplayMemberPath = "Empleado";
                    comboBox.SelectedValuePath = "idEmpleado";
                    comboBox.ItemsSource = dataTable.DefaultView;

                }
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

        public void CargarPaciente(ComboBox comboBox)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_LlenarComboBox", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "cargarPacientes");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    comboBox.DisplayMemberPath = "Paciente";
                    comboBox.SelectedValuePath = "idPaciente";
                    comboBox.ItemsSource = dataTable.DefaultView;

                }
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
    }
}
