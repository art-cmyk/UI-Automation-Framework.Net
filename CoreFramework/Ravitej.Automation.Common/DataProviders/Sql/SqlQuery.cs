namespace Ravitej.Automation.Common.DataProviders.Sql
{
    /// <summary>
    /// Defines a SQL Query to be executed
    /// </summary>
    public class SqlQuery : IExternalDataQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public string Query
        {
            get;
            set;
        }
    }
}
