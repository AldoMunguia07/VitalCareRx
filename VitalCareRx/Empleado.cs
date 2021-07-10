﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace VitalCareRx
{
    class Empleado
    {

        //Variables miembro

        Conexion conexion = new Conexion();

        public int IdEmpleado { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Celular { get; set; }

        public int IdSexo { get; set; }

        public string NombreUsuario { get; set; }

        public string Contrasenia { get; set; }

        public Empleado() { }

        public Empleado(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string celular, int idSexo, string nombreUsuario, string contrasenia )
        {
            PrimerNombre = primerNombre;
            SegundoNombre = segundoNombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Celular = celular;
            IdSexo = idSexo;
            NombreUsuario = nombreUsuario;
            Contrasenia = contrasenia;


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
                        empleado.SegundoNombre = rdr["segundoNombre"].ToString();
                        empleado.PrimerApellido = rdr["primerApellido"].ToString();
                        empleado.SegundoApellido = rdr["segundoApellido"].ToString();
                        empleado.Celular = rdr["celular"].ToString();
                        empleado.IdSexo = Convert.ToInt32(rdr["idSexo"]);
                        empleado.NombreUsuario = rdr["nombreUsuario"].ToString();
                        empleado.Contrasenia = rdr["contrasenia"].ToString();


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
                // Query de selección
                string query = @"INSERT INTO [Personas].[Empleado] VALUES 
                                (@primerNombre,@segundoNombre,@primerApellido,
                                @segundoApellido,@celular,@idSexo,@usuario,@pass)";
                // Establecer la conexión
                conexion.sqlConnection.Open();

                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, conexion.sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@primerNombre", empleado.PrimerNombre);
                sqlCommand.Parameters.AddWithValue("@segundoNombre", empleado.SegundoNombre);
                sqlCommand.Parameters.AddWithValue("@primerApellido", empleado.PrimerApellido);
                sqlCommand.Parameters.AddWithValue("@segundoApellido", empleado.SegundoApellido);
                sqlCommand.Parameters.AddWithValue("@celular", empleado.Celular);
                sqlCommand.Parameters.AddWithValue("@idSexo", empleado.IdSexo);
                sqlCommand.Parameters.AddWithValue("@usuario", empleado.NombreUsuario);
                sqlCommand.Parameters.AddWithValue("@pass", empleado.Contrasenia);

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
        /// Metodo para midificar un empleado.
        /// </summary>
        /// <param name="empleado"></param>
        public void ModificarEmpleado(Empleado empleado)
        {
            try
            {
                //Query para modificar un empleado
                string query = @"UPDATE [Personas].[Empleado]
                                SET primerNombre = @primerNombre, segundoNombre = @segundoNombre, primerApellido = @primerApellido, segundoApellido = @segundoApellido,
                                celular = @celular, idSexo = @idSexo, nombreUsuario = @usuario, contrasenia =  @pass
                                WHERE idEmpleado = @idEmpleado";

                // Abrir la conexión
                conexion.sqlConnection.Open();
                
                // Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, conexion.sqlConnection);

                // Establecer los valores de los parámetros
                sqlCommand.Parameters.AddWithValue("@idEmpleado", empleado.IdEmpleado);
                sqlCommand.Parameters.AddWithValue("@primerNombre", empleado.PrimerNombre);
                sqlCommand.Parameters.AddWithValue("@segundoNombre", empleado.SegundoNombre);
                sqlCommand.Parameters.AddWithValue("@primerApellido", empleado.PrimerApellido);
                sqlCommand.Parameters.AddWithValue("@segundoApellido", empleado.SegundoApellido);
                sqlCommand.Parameters.AddWithValue("@celular", empleado.Celular);
                sqlCommand.Parameters.AddWithValue("@idSexo", empleado.IdSexo);
                sqlCommand.Parameters.AddWithValue("@usuario", empleado.NombreUsuario);
                sqlCommand.Parameters.AddWithValue("@pass", empleado.Contrasenia);

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
            string query = @"SELECT MAX(idEmpleado) idEmpleado FROM [Personas].[Empleado]";

            try
            {
               SqlDataAdapter adapter = new SqlDataAdapter(query, conexion.sqlConnection);

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
            
        }


    }
}
