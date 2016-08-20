using System.Data;

namespace Ravitej.Automation.Common.DataProviders.Sql
{
    /// <summary>
    /// Result object of a Sql Query
    /// </summary>
    public class SqlQueryResult : IExternalDataResult
    {
        /// <summary>
        /// Data set of the results
        /// </summary>
        public DataSet ResponseData
        {
            get;
            set;
        }

        /// <summary>
        /// Any exception message returned
        /// </summary>
        public string ExceptionMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Was the query successful
        /// </summary>
        public bool Success
        {
            get;
            set;
        }
    }
}
