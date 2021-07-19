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
using System.Windows;

namespace VitalCareRx
{
    public class Empleado
    {

        //Variables miembro

        Conexion conexion = new Conexion();
        AportesControl aportes = new AportesControl();

        public int IdEmpleado { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Celular { get; set; }

        public string Correo { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public int IdSexo { get; set; }

        public int IdPuesto { get; set; }

        public string NombreUsuario { get; set; }

        public string Contrasenia { get; set; }

        public bool Estado { get; set; }

        public Empleado() { }

        public Empleado(int idEmpleado, string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string celular, string correo, DateTime fechaNacimiento, int idSexo, int idPuesto, string nombreUsuario, string contrasenia, bool estado)
        {
            IdEmpleado = idEmpleado;
            PrimerNombre = primerNombre;
            SegundoNombre = segundoNombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Celular = celular;
            Correo = correo;
            FechaNacimiento = fechaNacimiento;
            IdSexo = idSexo;
            IdPuesto = idPuesto;
            NombreUsuario = nombreUsuario;
            Contrasenia = contrasenia;
            Estado = estado;


        }

        public void VerEmpleados(DataGrid grid, int estado)
        {


            try
            {
                conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@estado", estado);
                sqlCommand.Parameters.AddWithValue("@accion", "verEmpleados");

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

        public void VerUnEmpleado(DataGrid grid, int estado, string nombreEmpleado)
        {


            try
            {
                conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@estado", estado);
                sqlCommand.Parameters.AddWithValue("@nombreEmpleado", nombreEmpleado);
                sqlCommand.Parameters.AddWithValue("@accion", "verUnEmpleado");

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

        /// <summary>
        /// Buscar un empleado en la base de datos 
        /// </summary>
        /// <param name="nombreUsuario">Es el nombre de usuario</param>
        /// <returns>Datos de usuario </returns>
        public Empleado BuscarEmpleado(string nombreUsuario)
        {
            Empleado empleado = new Empleado();
            try
            {
                conexion.sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                sqlCommand.Parameters.AddWithValue("@accion", "buscarUsuario");

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        // Obtener los valores del empleado si la consulta retorna valores
                        empleado.IdEmpleado = Convert.ToInt32(rdr["idEmpleado"]);
                        empleado.PrimerNombre = rdr["primerNombre"].ToString();
                        empleado.PrimerApellido = rdr["primerApellido"].ToString();
                        empleado.IdPuesto = Convert.ToInt32(rdr["idPuesto"]);
                        empleado.NombreUsuario = rdr["nombreUsuario"].ToString();
                        empleado.Contrasenia = rdr["contrasenia"].ToString();
                        empleado.Estado = Convert.ToBoolean(rdr["estado"]);


                    }
                }

                // Retornar el usuario con los valores
                return empleado;
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

        /// <summary>
        /// Método para crear un nuevo empleado 
        /// </summary>
        public void CrearNuevoEmpleado(Empleado empleado)
        {

            try
            {
                conexion.sqlConnection.Open();
                //Query para añadir un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@primerNombre", empleado.PrimerNombre);
                sqlCommand.Parameters.AddWithValue("@segundoNombre", empleado.SegundoNombre);
                sqlCommand.Parameters.AddWithValue("@primerApellido", empleado.PrimerApellido);
                sqlCommand.Parameters.AddWithValue("@segundoApellido", empleado.SegundoApellido);
                sqlCommand.Parameters.AddWithValue("@celular", empleado.Celular);
                sqlCommand.Parameters.AddWithValue("@correo", empleado.Correo);
                sqlCommand.Parameters.AddWithValue("@fechaNacimiento", empleado.FechaNacimiento);
                sqlCommand.Parameters.AddWithValue("@idSexo", empleado.IdSexo);
                sqlCommand.Parameters.AddWithValue("@idPuesto", empleado.IdPuesto);
                sqlCommand.Parameters.AddWithValue("@nombreUsuario", empleado.NombreUsuario);
                sqlCommand.Parameters.AddWithValue("@contrasenia", empleado.Contrasenia);
                sqlCommand.Parameters.AddWithValue("@estado", empleado.Estado);
                sqlCommand.Parameters.AddWithValue("@accion", "insertar");
                aportes.ContextoSesion(CodigoMayor(), conexion.sqlConnection);
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

        private int CodigoMayor()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "codigoMayor");

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    return Convert.ToInt32(dataTable.Rows[0]["idEmpleado"]);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Metodo para midificar un empleado.
        /// </summary>
        /// <param name="empleado"></param>
        public void ModificarEmpleado(Empleado empleado)
        {
            try
            {
                conexion.sqlConnection.Open();
                //Query para añadir un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@primerNombre", empleado.PrimerNombre);
                sqlCommand.Parameters.AddWithValue("@segundoNombre", empleado.SegundoNombre);
                sqlCommand.Parameters.AddWithValue("@primerApellido", empleado.PrimerApellido);
                sqlCommand.Parameters.AddWithValue("@segundoApellido", empleado.SegundoApellido);
                sqlCommand.Parameters.AddWithValue("@celular", empleado.Celular);
                sqlCommand.Parameters.AddWithValue("@correo", empleado.Correo);
                sqlCommand.Parameters.AddWithValue("@fechaNacimiento", empleado.FechaNacimiento);
                sqlCommand.Parameters.AddWithValue("@idSexo", empleado.IdSexo);
                sqlCommand.Parameters.AddWithValue("@idPuesto", empleado.IdPuesto);
                sqlCommand.Parameters.AddWithValue("@nombreUsuario", empleado.NombreUsuario);
                sqlCommand.Parameters.AddWithValue("@contrasenia", empleado.Contrasenia);
                sqlCommand.Parameters.AddWithValue("@estado", empleado.Estado);
                sqlCommand.Parameters.AddWithValue("@accion", "modificar");
                aportes.ContextoSesion(IdEmpleado, conexion.sqlConnection);
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

        public void EliminarEmpleado(Empleado empleado)
        {
            try
            {
                conexion.sqlConnection.Open();
                //Query para modificar un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;



                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@estado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@accion", "desactivar");
                aportes.ContextoSesion(IdEmpleado, conexion.sqlConnection);
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
        /// Metodo de obtención del codigo del último empleado ingresado.
        /// </summary>
        /// <returns>Codigo de empleado</returns>
        public int CodigoEmpleado()
        {


            try
            {
                conexion.sqlConnection.Open();
                //Query para añadir un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "codigoMayor");

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return Convert.ToInt32(dataTable.Rows[0]["idEmpleado"]);
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


        public void CargarTextBox(int codigoEmpleado, TextBox txtUsuario, TextBox txtPassword, TextBox txtPrimerNombre, TextBox txtSegundoNombre, TextBox txtPrimerApellido, TextBox txtSegundoApellido,
            TextBox txtCelular, TextBox txtCorreo, DatePicker fechaNacimiento, ComboBox cmbSexo)
        {
            try
            {

                //Query para añadir un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@accion", "buscarUsuarioID");
                sqlCommand.Parameters.AddWithValue("@idEmpleado", codigoEmpleado);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable Empleado = new DataTable();
                    sqlDataAdapter.Fill(Empleado);

                    //Del objeto DataTable se le asigna el valor correspondiente a cada TextBox.
                    txtUsuario.Text = Empleado.Rows[0]["nombreUsuario"].ToString();
                    txtPassword.Text = obtenerContraseña(codigoEmpleado);
                    txtPrimerNombre.Text = Empleado.Rows[0]["primerNombre"].ToString();
                    txtSegundoNombre.Text = Empleado.Rows[0]["segundoNombre"].ToString();
                    txtPrimerApellido.Text = Empleado.Rows[0]["primerApellido"].ToString();
                    txtSegundoApellido.Text = Empleado.Rows[0]["segundoApellido"].ToString();
                    txtCelular.Text = Empleado.Rows[0]["celular"].ToString();
                    txtCorreo.Text = Empleado.Rows[0]["correo"].ToString();
                    fechaNacimiento.SelectedDate = Convert.ToDateTime(Empleado.Rows[0]["fechaNacimiento"]).Date;
                    cmbSexo.SelectedValue = Empleado.Rows[0]["idSexo"].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public bool ExisteUsuario(string user)
        {
            try
            {

                conexion.sqlConnection.Open();
                //Query para añadir un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@nombreUsuario", user);
                    sqlCommand.Parameters.AddWithValue("@accion", "existeUsuario");

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);



                    if (dataTable.Rows.Count == 1) //Si el usuario existe retorna un true
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

        public string obtenerContraseña(int idEmpleado)
        {
            try
            {
                conexion.sqlConnection.Open();
                //Query para añadir un paciente
                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);



                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    sqlCommand.Parameters.AddWithValue("@accion", "obtenerPass");

                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable.Rows[0]["contrasenia"].ToString();

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

        public bool ExisteCorreo(TextBox txtCorreo)
        {
            try
            {

             

                SqlCommand sqlCommand = new SqlCommand("sp_Empleados", conexion.sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@correo", txtCorreo.Text);
                sqlCommand.Parameters.AddWithValue("@accion", "buscarUsuarioCorreo");


                DataTable data = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {

                    sqlDataAdapter.Fill(data);
                }
                if (data.Rows.Count != 0)
                {
                    return true;

                }   
                
                return false;
                
            }
            catch (Exception ex)
            {

                throw;
            }
          

        }
    }
}
