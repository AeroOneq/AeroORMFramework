using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    internal class DataBaseSettings
    {
        #region Connection Parameters
        private static string ServerName { get; } = "odmainserver.database.windows.net";
        private static string InitialCatalog { get; } = "ODdb";
        private static string IntegratedSecurity { get; } = "True";
        private static string UserID { get; } = "Aero";
        private static string Password { get; } = "OdWyitBuodKidV6";
        private static string TrustedConnection { get; } = "False";
        private static string Encrypt { get; } = "True";
        #endregion
        public static string ConnectionString { get; } =
            $"Data Source={ServerName}; Initial Catalog={InitialCatalog}; " +
            $"Integrated Security={IntegratedSecurity}; User ID={UserID}; " +
            $"Password={Password}; Trusted_Connection={TrustedConnection}; Encrypt={Encrypt};";
    }
}
