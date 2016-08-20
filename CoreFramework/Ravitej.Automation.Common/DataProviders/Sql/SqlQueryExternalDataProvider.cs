using System;
using System.Data;
using System.Data.SqlClient;
using Ravitej.Automation.Common.Config.AppUnderTest;

namespace Ravitej.Automation.Common.DataProviders.Sql
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlQueryExternalDataProvider : SqlDataProviderBase, IExternalDataProvider<SqlQuery, SqlQueryResult>
    {
        private SqlQuery _query;

        private readonly string _targetDatabase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="targetDatabase"></param>
        public SqlQueryExternalDataProvider(DatabaseSettings settings, string targetDatabase) : base(settings)
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
        public SqlQueryResult Execute()
        {
            var retVal = new SqlQueryResult
                         {
                             Success = false
                         };

            var sqlDataSet = new DataSet();
            string testDataDatabase = _settings.TestDataDatabase;

            try
            {
                using (SqlConnection oSqlConnection = GetDatabaseConnection(_targetDatabase))
                {
                    oSqlConnection.Open();
                    using (var sqlda = new SqlDataAdapter(_query.Query, oSqlConnection))
                    {
                        sqlda.SelectCommand.CommandTimeout = 120;
                        sqlda.Fill(sqlDataSet);
                        retVal.ResponseData = sqlDataSet;
                        retVal.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                retVal.ExceptionMessage =
                    $"Exception occured while getting data from {testDataDatabase} database using query \"{_query.Query}\". Exception thrown is: {ex}";
                return null;
            }

            return retVal;
        }
    }
}
