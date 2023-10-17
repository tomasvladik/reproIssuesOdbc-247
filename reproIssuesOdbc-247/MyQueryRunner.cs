using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;

namespace OdbcTest
{
    public class MyQueryRunner
    {

        public DataTable[] ExecuteQuery(string connectionString, string query, TimeSpan queryTimeout)
        {
            var stopwatch = Stopwatch.StartNew();
            var totalTimeout = (int) Math.Ceiling(queryTimeout.TotalSeconds)  * 1000;

            using (var connection = new OdbcConnection(connectionString))
            {
                connection.ConnectionTimeout = (int) Math.Ceiling(queryTimeout.TotalSeconds);
                connection.Open();

                if (stopwatch.ElapsedMilliseconds >= totalTimeout)
                {
                    throw new TimeoutException();
                }

                return LoadData(query, connection, (int) Math.Ceiling(queryTimeout.TotalSeconds), stopwatch, totalTimeout).ToArray();
            }
        }

        private IEnumerable<DataTable> LoadData(string query, OdbcConnection connection, int roundedQueryTimeout, Stopwatch stopwatch, int totalTimeout)
        {
            using (var command = new OdbcCommand(query, connection))
            {
                command.CommandTimeout = roundedQueryTimeout;

                using (var reader = command.ExecuteReader())
                {
                    do
                    {
                        var dataTable = new DataTable();
                        if (reader.HasRows)
                        {
                            var schemaTable = reader.GetSchemaTable();
                            // process schema
                            while (reader.Read())
                            {
                                var newRow = dataTable.Rows.Add();
                                //process columns ...
                            }
                        }

                        yield return dataTable;
                    }
                    while (reader.NextResult());
                }
            }
        }
    }
}