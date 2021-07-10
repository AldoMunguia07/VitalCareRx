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
    class Paciente
    {
        Conexion conexion = new Conexion();

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
                conexion.sqlConnection.Open();
                //Query para añadir un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Pacientes", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;             



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
                sqlCommand.Parameters.AddWithValue("@accion", "insertar");
              


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
        /// Metodo para actualizar un paciente.
        /// </summary>
        /// <param name="paciente"></param>
        public void ActualizarPaciente(Paciente paciente)
        {
            try
            {
                conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Pacientes", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

              

               

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
                sqlCommand.Parameters.AddWithValue("@accion", "modificar");

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
        /// Metodo para eliminar un paciente(Cambiar estado).
        /// </summary>
        /// <param name="paciente"></param>
        public void EliminarPaciente(Paciente paciente)
        {
            try
            {
                 conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Pacientes", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                       

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idPaciente", paciente.IdPaciente);
                sqlCommand.Parameters.AddWithValue("@accion", "banear");

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

        public void VerPacientes(DataGrid grid, int estado)
        {
            

            try
            {
                conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Pacientes", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@estado", estado);
                sqlCommand.Parameters.AddWithValue("@accion", "VerPaciente");

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    grid.ItemsSource = dataTable.DefaultView;

                    grid.IsReadOnly = true; // El grid es de solo lectura.

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

        public void VerUnPaciente(Paciente paciente, DataGrid grid, int estado, TextBox textBox)
        {

            try
            {
                conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Pacientes", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@estado", estado);
                sqlCommand.Parameters.AddWithValue("@nombrePaciente", textBox.Text);
                sqlCommand.Parameters.AddWithValue("@accion", "VerUnPaciente");

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    grid.ItemsSource = dataTable.DefaultView;

                    grid.IsReadOnly = true; // El grid es de solo lectura.

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

        public bool ExistePaciente(Paciente paciente, string dni)
        {
            try
            {
                conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Pacientes", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@numeroIdentidad", dni);
                    sqlCommand.Parameters.AddWithValue("@accion", "existeID");

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);


                    if (dataTable.Rows.Count == 1)  //Si existe que devuelva un true
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

       

    }
}
