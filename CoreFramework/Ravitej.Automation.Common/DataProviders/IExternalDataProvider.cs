namespace Ravitej.Automation.Common.DataProviders
{
    /// <summary>
    /// Interface defining an object that can be queried
    /// </summary>
    public interface IExternalDataProvider<in TQuery, out TResult>
        where TQuery : IExternalDataQuery
        where TResult : IExternalDataResult
    {
        /// <summary>
        /// Defines the query to be executed against the provider
        /// </summary>
        /// <param name="query"></param>
        void DefineQuery(TQuery query);


        /// <summary>
        /// Executes the query against the data provider
        /// </summary>
        /// <returns></returns>
        TResult Execute();
    }
}
