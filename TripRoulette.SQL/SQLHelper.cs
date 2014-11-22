
#region Namespaces

using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Xml;
using System.Configuration;

#endregion

namespace Engine.SQL
{
    /// <summary>
    /// Summary description for Sql
    /// </summary>
    /// 
    [Serializable]
    public class SQLHelper : IDeserializationCallback
    {

        #region Fields


        private string connectionString;

        [NonSerialized]
        private SqlConnection sqlConnection;

        private bool _isOpen = false;

        #endregion

        #region Properties

        public string ConnectionString
        {

            get { return connectionString; }
            set
            {
                sqlConnection.ConnectionString = value;
                connectionString = value;
            }

        }

        #endregion

        #region Enumerators

        public enum ExecuteTypes
        {
            NonQuery = 0,
            Reader = 1,
            Scalar = 2,
            XmlReader = 3
        }

        public enum FormatTypes
        {
            HTMLTable,
            XML
        }

        public enum CommandTypes
        {
            Text,
            StoredProcedure
        }

        public enum ImportTypes
        {
            CSV,
            XML,
            TAB
        }

        #endregion

        #region Constructors

        public SQLHelper(string connectionString)
        {
            this.connectionString = connectionString;
            sqlConnection = new SqlConnection(connectionString);
        }

        public SQLHelper(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public SQLHelper()
        {
            //this.sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString);
        }

        #endregion

        #region Methods

        public object ExecuteString(string sql, ExecuteTypes executeType)
        {
            return Execute(CommandTypes.Text, sql, executeType, null);
        }

        public object ExecuteString(string sql, ExecuteTypes executeType, List<SqlParameter> parameters)
        {
            return Execute(CommandTypes.Text, sql, executeType, parameters);
        }

        public object ExecuteStoredProcedure(string storedProcedure, ExecuteTypes executeType)
        {
            return Execute(CommandTypes.StoredProcedure, storedProcedure, executeType, null);
        }

        public object ExecuteStoredProcedure(string storedProcedure, ExecuteTypes executeType, List<SqlParameter> parameters)
        {
            return Execute(CommandTypes.StoredProcedure, storedProcedure, executeType, parameters);
        }

        public T Execute<T>(CommandTypes commandType, string commandText, ExecuteTypes executeType, List<SqlParameter> parameters)
        {
            _isOpen = false;
            object value = new object();

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = sqlConnection;

            switch (commandType)
            {
                case CommandTypes.Text:

                    sqlCommand.CommandType = CommandType.Text;

                    break;

                case CommandTypes.StoredProcedure:

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    break;
            }

            sqlCommand.CommandText = commandText;

            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                    sqlCommand.Parameters.Add(param);
            }

            try
            {
                sqlConnection.Open();
                _isOpen = true;

                switch (executeType)
                {
                    case ExecuteTypes.NonQuery:

                        sqlCommand.ExecuteNonQuery();
                        value = null;
                        break;

                    case ExecuteTypes.Reader:

                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        DataTable dataTable = new DataTable();

                        dataTable.Load(sqlDataReader);

                        value = dataTable;

                        dataTable.Dispose();
                        sqlDataReader.Close();
                        sqlDataReader.Dispose();

                        break;

                    case ExecuteTypes.Scalar:

                        value = sqlCommand.ExecuteScalar();

                        if (typeof(T) == typeof(int))
                        {
                            if (value == null)
                            {
                                value = 0;
                            }
                            else
                            {
                                if (value == DBNull.Value)
                                {
                                    value = 0;
                                }
                                else
                                {
                                    value = Convert.ToInt32(value);
                                }
                            }
                        }

                        break;

                    case ExecuteTypes.XmlReader:

                        XmlReader xmlReader = sqlCommand.ExecuteXmlReader();
                        value = xmlReader;

                        break;
                }

            }
            catch (Exception ex)
            {
                if (_isOpen)
                {
                    //Engine.Data.Error.Add(ex);
                }
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                    _isOpen = false;
                }
            }

            return (T)value;
        }

        public object Execute(CommandTypes commandType, string commandText, ExecuteTypes executeType, List<SqlParameter> parameters)
        {
            object value = new object();

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = sqlConnection;

            switch (commandType)
            {
                case CommandTypes.Text:

                    sqlCommand.CommandType = CommandType.Text;

                    break;

                case CommandTypes.StoredProcedure:

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    break;
            }

            sqlCommand.CommandText = commandText;

            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                    sqlCommand.Parameters.Add(param);
            }

            try
            {
                sqlConnection.Open();
                _isOpen = true;

                switch (executeType)
                {
                    case ExecuteTypes.NonQuery:

                        sqlCommand.ExecuteNonQuery();
                        value = null;
                        break;

                    case ExecuteTypes.Reader:

                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        DataTable dataTable = new DataTable();

                        dataTable.Load(sqlDataReader);

                        value = dataTable;

                        dataTable.Dispose();
                        sqlDataReader.Close();
                        sqlDataReader.Dispose();

                        break;

                    case ExecuteTypes.Scalar:

                        value = sqlCommand.ExecuteScalar();

                        break;

                    case ExecuteTypes.XmlReader:

                        XmlReader xmlReader = sqlCommand.ExecuteXmlReader();
                        value = xmlReader;

                        break;
                }

            }
            catch (Exception ex)
            {
                if (_isOpen)
                {
                    //Engine.Data.Error.Add(ex);
                }
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                    _isOpen = false;
                }
            }

            return value;

        }

        public object Scalar(string sql, string column)
        {
            object value = null;

            DataTable dataTable = (DataTable)Execute(CommandTypes.Text, sql, ExecuteTypes.Reader, null);

            if (dataTable.Rows.Count > 0)
            {
                value = dataTable.Rows[0][column];
            }

            dataTable.Dispose();

            return value;
        }

        public object Scalar(string sql, List<SqlParameter> parameters, string column)
        {
            object value = null;

            DataTable dataTable = (DataTable)Execute(CommandTypes.Text, sql, ExecuteTypes.Reader, parameters);

            if (dataTable.Rows.Count > 0)
            {
                value = dataTable.Rows[0][column];
            }

            dataTable.Dispose();

            return value;
        }

        public static string SerializeRow(DataRow row)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                sb.Append(row.Table.Columns[i].ColumnName + ":" + row[i] + ";");
            }

            return sb.ToString();

        }

        public T First<T>(string ColumnName, string TableName, string WhereClause)
        {

            object value = new object();

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = sqlConnection;

            sqlCommand.CommandText = "SELECT TOP 1 [" + ColumnName + "] AS FirstOF_" + ColumnName + " FROM [" + TableName + "]";

            if (WhereClause != "")
            {
                sqlCommand.CommandText += " WHERE " + WhereClause;
            }

            sqlCommand.CommandText += " ORDER BY 1 ASC;";

            try
            {
                sqlConnection.Open();
                value = sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //Engine.Data.Error.Add(ex);
                value = new object();
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return (T)value;
        }

        public T Last<T>(string columnName, string tableName, string where, string orderBy = "")
        {

            object value = new object();

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = sqlConnection;

            sqlCommand.CommandText = "SELECT TOP 1 [" + columnName + "] AS LastOF_" + columnName + " FROM [" + tableName + "]";

            if (where != "")
            {
                sqlCommand.CommandText += " WHERE " + where;
            }

            if (orderBy == "")
            {
                sqlCommand.CommandText += " ORDER BY 1 DESC;";
            }
            else
            {
                sqlCommand.CommandText += " " + orderBy;

            }
            try
            {
                sqlConnection.Open();
                value = sqlCommand.ExecuteScalar();

                if (typeof(T) == typeof(int))
                {
                    if (value == null)
                    {
                        value = 0;
                    }
                    else
                    {
                        if (value == DBNull.Value)
                        {
                            value = 0;
                        }
                        else
                        {
                            value = Convert.ToInt32(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Engine.Data.Error.Add(ex);
                value = new object();
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return (T)value;
        }

        public T Count<T>(string ColumnName, string TableName, string WhereClause)
        {

            object value = new object();

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = sqlConnection;

            sqlCommand.CommandText = "SELECT COUNT([" + ColumnName + "]) AS CountOF_" + ColumnName + " FROM [" + TableName + "]";

            if (WhereClause != "")
            {
                sqlCommand.CommandText += " WHERE " + WhereClause;
            }

            try
            {
                sqlConnection.Open();
                value = sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //Engine.Data.Error.Add(ex);
                value = new object();
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return (T)value;
        }

        public T Lookup<T>(string ColumnName, string TableName, string WhereClause)
        {

            object value = new object();

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = sqlConnection;

            sqlCommand.CommandText = "SELECT TOP 1 [" + ColumnName + "] AS " + ColumnName + " FROM [" + TableName + "]";

            if (WhereClause != "")
            {
                sqlCommand.CommandText += " WHERE " + WhereClause;
            }

            try
            {
                sqlConnection.Open();
                value = sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //Engine.Data.Error.Add(ex);
                value = new object();
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return (T)value;
        }

        public T SUM<T>(string ColumnName, string TableName, string WhereClause)
        {

            object value = new object();

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = sqlConnection;

            sqlCommand.CommandText = "SELECT SUM([" + ColumnName + "]) AS SUMOF_" + ColumnName + " FROM [" + TableName + "]";

            if (WhereClause != "")
            {
                sqlCommand.CommandText += " WHERE " + WhereClause;
            }

            try
            {
                sqlConnection.Open();
                value = sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //Engine.Data.Error.Add(ex);
                value = new object();
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return (T)value;
        }

        public DataTable StoredProcedures()
        {
            return Execute<DataTable>(CommandTypes.Text, "select specific_name AS SPName from information_schema.routines where routine_type='PROCEDURE' ORDER BY SPECIFIC_NAME ASC", ExecuteTypes.Reader, null);
        }

        public enum ParseSteps
        {
            CommandType
        }

       
        #endregion


        public void OnDeserialization(object sender)
        {
            this.sqlConnection = new SqlConnection(connectionString);

        }
    }

}