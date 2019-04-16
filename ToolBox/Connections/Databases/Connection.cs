
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Connections.Databases
{
    public class Connection : IConnection
    {
        private string _ConnectionString;

        public Connection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException();

            _ConnectionString = connectionString;

            using (SqlConnection c = CreateConnection())
            {
                c.Open();
            }
        }

        public int ExecuteNonQuery(Command command)
        {
            using (SqlConnection c = CreateConnection())
            {
                using (SqlCommand cmd = CreateCommand(command, c))
                {
                    c.Open();
                    return cmd.ExecuteNonQuery(); 
                }

            }
        }

        public object ExecuteScalar(Command command)
        {
            using (SqlConnection c = CreateConnection())
            {
                using (SqlCommand cmd = CreateCommand(command, c))
                {
                    c.Open();
                    object o = cmd.ExecuteScalar();
                    return (o is DBNull) ? null : o;
                }

            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector)
        {
            using (SqlConnection c = CreateConnection())
            {
                using (SqlCommand cmd = CreateCommand(command, c))
                {
                    c.Open();
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                            yield return selector(sqlDataReader);
                    }
                }
            }
        }

        public DataTable GetDataTable(Command command)
        {
            using (SqlConnection c = CreateConnection())
            {
                using (SqlCommand cmd = CreateCommand(command, c))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;

                        DataTable dataTable = new DataTable();
                        da.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
        }
        //Possible que vous devriez ajouter des méthodes privées

        private SqlConnection CreateConnection()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _ConnectionString;

            return sqlConnection;
        }

        private SqlCommand CreateCommand(Command command, SqlConnection connection)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = command.Query;
            cmd.CommandType = (command.IsStoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;

            foreach (KeyValuePair<string, object> kvp in command.Parameters)
            {
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = kvp.Key;
                sqlParameter.Value = kvp.Value ?? DBNull.Value;

                cmd.Parameters.Add(sqlParameter);
            }

            return cmd;
        }
    }
}
