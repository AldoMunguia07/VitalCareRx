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
        /// Metodo para ver los farmacos de la consulta actual.
        /// </summary>
        public void MostrarFarmacos(DataGrid dataGrid, int codigoConsulta)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_RecetasMedicas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idConsulta", codigoConsulta);

                sqlCommand.Parameters.AddWithValue("@accion", "MostrarFarmacos");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

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
                // Cerrar la conexión
                conexion.sqlConnection.Close();
            }
        }


        /// <summary>
        /// Inserta farmacos a una receta medica
        /// </summary>
        /// <param name="receta">Es un objeto de tipo receta" <param>
        public void AgregarFarmacoAReceta(Receta receta)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_RecetasMedicas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idRecetaMedica", receta.IdReceta);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", receta.IdFarmaco);
                sqlCommand.Parameters.AddWithValue("@cantidad", receta.Cantidad);
                sqlCommand.Parameters.AddWithValue("@duracion", receta.DuracionTratamiento);
                sqlCommand.Parameters.AddWithValue("@indicacion", receta.Indicaciones);
                sqlCommand.Parameters.AddWithValue("@accion", "InsertarFarmaco");


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
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_RecetasMedicas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idRecetaMedica", receta.IdReceta);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", receta.IdFarmaco);
                sqlCommand.Parameters.AddWithValue("@accion", "EliminarFarmaco");

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
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_RecetasMedicas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idRecetaMedica", receta.IdReceta);
                sqlCommand.Parameters.AddWithValue("@idFarmaco", receta.IdFarmaco);
                sqlCommand.Parameters.AddWithValue("@cantidad", receta.Cantidad);
                sqlCommand.Parameters.AddWithValue("@duracion", receta.DuracionTratamiento);
                sqlCommand.Parameters.AddWithValue("@indicacion", receta.Indicaciones);
                sqlCommand.Parameters.AddWithValue("@accion", "ModificarFarmaco");

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
        ///Metodo que valida si la consulta tiene o no una receta.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool ValidarCrearRecetaMedica(int idConsulta)
        {
            try
            {

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_RecetasMedicas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@idConsulta", idConsulta);
                    sqlCommand.Parameters.AddWithValue("@accion", "ValidarCrearRecetaMedica");

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 1) //Si devuelve una fila, es decir tiene una receta que retorne un true.
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
            finally
            {
                conexion.sqlConnection.Close();
            }

        }

        /// <summary>
        /// Metodo para crear o visualizar una receta medica
        /// </summary>
        public void RecetaMedica(int idConsulta, int idRecetaMedica, Consulta consulta)
        {
            if (!ValidarCrearRecetaMedica(idConsulta)) //Si la consulta no tiene una receta procede a crearle una.
            {
                try
                {


                    conexion.sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("sp_RecetasMedicas", conexion.sqlConnection);

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Establecer los valores de los parámetros
                    sqlCommand.Parameters.AddWithValue("@idConsulta", idConsulta);
                    sqlCommand.Parameters.AddWithValue("@accion", "InsertarReceta");
                    sqlCommand.ExecuteNonQuery();

                    idRecetaMedica = consulta.CapturarIdRecetaMedica(idConsulta);

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

        /// <summary>
        /// Metodo para validar si el farmaco ya esta en la receta.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool ValidarFarmacoEnReceta(ComboBox comboBox, int codigoRecetaMedica)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_RecetasMedicas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@idFarmaco", comboBox.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@idRecetaMedica", codigoRecetaMedica);
                    sqlCommand.Parameters.AddWithValue("@accion", "ValidarFarmacoEnReceta");

                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 1) //Si el farmaco existe en la receta actual, devuelve un true
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
            finally
            {
                // Cerrar la conexión
                conexion.sqlConnection.Close();
            }

        }
    }
}
