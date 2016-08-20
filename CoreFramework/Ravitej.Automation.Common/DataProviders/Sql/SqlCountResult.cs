namespace Ravitej.Automation.Common.DataProviders.Sql
{
    /// <summary>
    /// Result object for a Sql Count query
    /// </summary>
    public class SqlCountResult : IExternalDataResult
    {
        /// <summary>
        /// How many rows were returned
        /// </summary>
        public int ResultCount
        {
            get;
            set;
        }

        /// <summary>
        /// Any exception message displayed
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
