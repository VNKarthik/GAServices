using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security;
using GAServices.Common;

using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace GAServices.DataAccessHelper
{
    public class DataHelper
    {
        string _connectionString = "";
        //private static DataHelper _dbContext;
        private MySqlConnection _connection;
        private MySqlTransaction _transaction;

        public DataHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public static DataHelper getInstance()
        //{
        //    if (_dbContext == null)
        //    {
        //        _dbContext = new DataHelper();
        //    }

        //    return _dbContext;
        //}

        private bool OpenConnection()
        {
            try
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private void CloseConnection()
        {
            _connection.Close();
        }

        private void InitiateTransaction()
        {
            if (_connection.State != ConnectionState.Open)
            {
                OpenConnection();
            }

            if (_connection.State == ConnectionState.Open)
            {
                _transaction = _connection.BeginTransaction();
            }
            else
            {
                return;
            }
        }

        private bool CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                CloseConnection();

                return true;
            }
            else
            {
                return false;
            }
        }

        private void RollbackTransaction()
        {
            _transaction.Rollback();
            CloseConnection();
        }

        public DataTable GetData(string sqlQuery)
        {
            DataTable dt = new DataTable();

            try
            {
                if (OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sqlQuery, _connection))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }

                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// Get the data from the Stored procedure with the list of Parameters passed.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="inputParameters"></param>
        /// <returns></returns>
        public DataTable GetData(string procedureName, List<MySqlParameter> inputParameters, List<MySqlParameter> outParameters)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter();

            try
            {
                if (procedureName != null)
                {
                    if (OpenConnection())
                    {
                        cmd = new MySqlCommand(procedureName, _connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (inputParameters?.Any() == true)
                        {
                            foreach (MySqlParameter parameter in inputParameters)
                            {
                                parameter.Direction = ParameterDirection.Input;
                                cmd.Parameters.Add(parameter);
                            }
                        }

                        if (outParameters?.Any() == true)
                        {
                            foreach (MySqlParameter parameter in outParameters)
                            {
                                parameter.Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(parameter);
                            }
                        }

                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);

                        CloseConnection();
                    }
                }
            }

            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }

            return dt;
        }

        public DataTable GetData(string procedureName, List<MySqlParameter> inputParameters)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter();

            try
            {
                if (procedureName != null)
                {
                    if (OpenConnection())
                    {
                        cmd = new MySqlCommand(procedureName, _connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (inputParameters?.Any() == true)
                        {
                            foreach (MySqlParameter parameter in inputParameters)
                            {
                                parameter.Direction = ParameterDirection.Input;
                                cmd.Parameters.Add(parameter);
                            }
                        }

                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);

                        CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }

            return dt;
        }

        public DataSet GetDataSet(string procedureName, List<MySqlParameter> inputParameters)
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter();

            try
            {
                if (procedureName != null)
                {
                    if (OpenConnection())
                    {
                        cmd = new MySqlCommand(procedureName, _connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (inputParameters?.Any() == true)
                        {
                            foreach (MySqlParameter parameter in inputParameters)
                            {
                                parameter.Direction = ParameterDirection.Input;
                                cmd.Parameters.Add(parameter);
                            }
                        }

                        da = new MySqlDataAdapter(cmd);
                        da.Fill(ds);

                        CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }

            return ds;
        }


        /// <summary>
        /// Extends the ExecuteScalar of ADO.NET and gets the data in the specified datatype - T
        /// </summary>
        /// <typeparam name="T">DataType in which the result should be in</typeparam>
        /// <param name="SqlQuery">Query to execute</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string SqlQuery)
        {
            object Value = null;

            try
            {
                if (OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(SqlQuery, _connection))
                    {
                        Value = cmd.ExecuteScalar();
                    }

                    CloseConnection();
                }

                if (Convert.IsDBNull(Value) == false)
                {
                    return (T)Convert.ChangeType(Value, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppResponse Insert_UpdateData(string procedureName, MySqlParameter[] inParameters, MySqlParameter[] outParameters)
        {
            AppResponse response = new AppResponse(ResponseStatus.FAILURE);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (procedureName != null && OpenConnection())
                {
                    InitiateTransaction();

                    cmd = new MySqlCommand(procedureName, _connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (MySqlParameter param in inParameters)
                    {
                        //Create and Add Values on Parameters
                        param.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(param);
                    }

                    if (outParameters != null)
                    {
                        foreach (MySqlParameter param in outParameters)
                        {
                            //Create and Add Values on Parameters
                            param.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(param);
                        }
                    }

                    cmd.ExecuteNonQuery();

                    if (outParameters != null)
                    {
                        response.ReturnData = new Dictionary<object, object>();

                        foreach (MySqlParameter param in outParameters)
                        {
                            response.ReturnData.Add(param.ParameterName, param.Value);
                        }
                    }

                    CommitTransaction();

                    response.Status = ResponseStatus.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                if (_connection != null)
                {
                    if (_connection.State == ConnectionState.Open)
                        RollbackTransaction();
                }

                throw ex;
            }

            return response;
        }

        public List<T> GetData<T>(string procedureName, List<MySqlParameter> inputParameters)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter();

            try
            {
                if (procedureName != null)
                {
                    if (OpenConnection())
                    {
                        cmd = new MySqlCommand(procedureName, _connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (inputParameters?.Any() == true)
                        {
                            foreach (MySqlParameter parameter in inputParameters)
                            {
                                parameter.Direction = ParameterDirection.Input;
                                cmd.Parameters.Add(parameter);
                            }
                        }

                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);

                        CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }

            return Utilities.CreateListFromTable<T>(dt);
        }

    }
}
