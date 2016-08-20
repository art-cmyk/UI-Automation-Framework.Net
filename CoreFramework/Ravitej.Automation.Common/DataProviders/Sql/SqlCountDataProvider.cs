using System;
using System.Data.SqlClient;
using Ravitej.Automation.Common.Config.AppUnderTest;

namespace Ravitej.Automation.Common.DataProviders.Sql
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlCountExternalDataProvider : SqlDataProviderBase, IExternalDataProvider<SqlQuery, SqlCountResult>
    {
        private SqlQuery _query;

        private readonly string _targetDatabase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="targetDatabase"></param>
        public SqlCountExternalDataProvider(DatabaseSettings settings, string targetDatabase)
            : base(settings)
        {
            _targetDatabase = targetDatabase;
        }

        /// <summary>
        /// Defines the query to be executed against the provider
        /// </summary>
        /// <param name="query"></param>
        public void DefineQuery(SqlQuery query)
        {
            _query = query;
        }

        /// <summary>
        /// Executes the query against the data provider
        /// </summary>
        /// <returns></returns>
        public SqlCountResult Execute()
        {
            var retVal = new SqlCountResult
            {
                Success = false
            };

            int nCount = -1;

            using (SqlConnection oSqlConnection = GetDatabaseConnection(_targetDatabase))
            {
                var command = new SqlCommand(_query.Query, oSqlConnection);

                try
                {
                    oSqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        nCount = Convert.ToInt16(reader[0]);
                    }
                    reader.Close();
                    retVal.ResultCount = nCount;
                    retVal.Success = true;
                }
                catch (Exception ex)
                {
                    retVal.ExceptionMessage =
                        $"Exception occured while getting data from {_targetDatabase} database using count query \"{_query.Query}\". Exception thrown is: {ex}";
                }
            }

            return retVal;
        }
    }
}
