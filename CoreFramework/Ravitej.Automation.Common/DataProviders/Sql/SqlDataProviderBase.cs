using System.Data.SqlClient;
using Ravitej.Automation.Common.Config.AppUnderTest;

namespace Ravitej.Automation.Common.DataProviders.Sql
{
    public abstract class SqlDataProviderBase
    {
        protected readonly DatabaseSettings _settings;

        protected SqlDataProviderBase(DatabaseSettings settings)
        {
            _settings = settings;
        }

        protected SqlConnection GetDatabaseConnection(string sDatabase)
        {
            string sConnectionString = string.Empty;
            if (sDatabase.Equals("iFarm"))
            {
                sConnectionString =
                    $"Data Source={_settings.DatabaseServerInstance};Initial Catalog={sDatabase};User Id=tcdatauser;Password=tcdatauser;";
            }
            else
            {
                sConnectionString =
                    $"Data Source={_settings.DatabaseServerInstance};Initial Catalog={sDatabase};User Id=automation;Password=automation;";
            }

            return new SqlConnection(sConnectionString);

        }
    }
}
