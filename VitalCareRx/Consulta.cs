using System;
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
    class Consulta
    {
        //Variables miembro

        Conexion conexion = new Conexion();

        public int IdConsulta {get; set;}
        public string MotivoConsulta { get; set; }
        public string DiagnosticoConsulta { get; set; }
        public float Temperatura { get; set; }
        public string PresionArterial { get; set; }
        public int IdEmpleado { get; set; }
        public int IdCita { get; set; }

        public Consulta() { }

        public Consulta(string motivoConsulta, string diagnosticoConsulta, float temperatura, string presionArterial, int idEmpleado, int idCita ) 
        {
            MotivoConsulta = motivoConsulta;
            DiagnosticoConsulta = diagnosticoConsulta;
            Temperatura = temperatura;
            PresionArterial = presionArterial;
            IdEmpleado = idEmpleado;
            IdCita = idCita;

        }


        /// <summary>
        /// Trae todas las consultas de la base de datos al inicial el programa.
        /// </summary>
        public void MostrarConsultas(DataGrid dataGrid)
        {
            conexion.sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("sp_Consultas", conexion.sqlConnection);

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@accion", "Mostrar");

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            try
            {
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
        /// Metodo para crear una consulta.
        /// </summary>
        /// <param name="consulta"></param>
        public void CrearConsulta(Consulta consulta)
        {

            try
            {

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Consultas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@motivoConsulta", consulta.MotivoConsulta);
                sqlCommand.Parameters.AddWithValue("@diagnosticoConsulta", consulta.DiagnosticoConsulta);
                sqlCommand.Parameters.AddWithValue("@temperatura", consulta.Temperatura);
                sqlCommand.Parameters.AddWithValue("@presionArterial", consulta.PresionArterial);
                sqlCommand.Parameters.AddWithValue("@idEmpleado", consulta.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@idCita", consulta.IdCita);
                sqlCommand.Parameters.AddWithValue("@accion", "Insertar");

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
        /// Metodo para modificar una consulta.
        /// </summary>
        /// <param name="consulta"></param>
        public void ModificarConsulta(Consulta consulta)
        {

            try
            {

                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Consultas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@motivoConsulta", consulta.MotivoConsulta);
                sqlCommand.Parameters.AddWithValue("@diagnosticoConsulta", consulta.DiagnosticoConsulta);
                sqlCommand.Parameters.AddWithValue("@temperatura", consulta.Temperatura);
                sqlCommand.Parameters.AddWithValue("@presionArterial", consulta.PresionArterial);
                sqlCommand.Parameters.AddWithValue("@idEmpleado", consulta.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@idCita", consulta.IdCita);
                sqlCommand.Parameters.AddWithValue("@idConsulta", consulta.IdConsulta);
                sqlCommand.Parameters.AddWithValue("@accion", "Modificar");

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
        ///  Se captura el Id de la receta medica que pertenece a la consulta
        /// </summary>
        /// <returns>El id de la receta medica</returns>
        public int CapturarIdRecetaMedica(int idConsulta)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Consultas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@idConsulta", idConsulta);
                    sqlCommand.Parameters.AddWithValue("@accion", "CapturarIdRecetaMedica");

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    return Convert.ToInt32(dataTable.Rows[0]["idRecetaMedica"]); //Retorna el id de la receta medica.
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


        

        //Buscar una consulta por nombre del paciente
        public void Buscar(string buscar, DataGrid dataGrid)
        {
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Consultas", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@nombrePaciente", buscar);
                    sqlCommand.Parameters.AddWithValue("@accion", "Buscar");

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    dataGrid.ItemsSource = dataTable.DefaultView;

                    dataGrid.IsReadOnly = true;

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

        

    }
}
